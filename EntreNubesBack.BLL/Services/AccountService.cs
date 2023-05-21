using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Account;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Product;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<ProductsDetail> _productsDetailRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, 
                          IMapper mapper, 
                          IProductRepository productRepository, 
                          IGenericRepository<ProductsDetail> productsDetailRepository, 
                          IGenericRepository<Payment> paymentRepository)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _productsDetailRepository = productsDetailRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<List<AccountDto>> GetUnclosedAccounts()
    {
        var accounts = await _accountRepository.Consult(a => a.State && !a.IsClosed);
        return _mapper.Map<List<AccountDto>>(accounts.ToList());
    }

    public async Task<List<ProductDetailDto>> GetProductsFromAnAccount(int accountId)
    {
        var account = await _accountRepository.Consult(a => a.State && a.IdAccount == accountId);
        var accountDetails = account.Include(a => a.ProductsDetails)
                                    .ThenInclude(pd => pd.IdProductNavigation)
                                    .Include(a => a.ProductsDetails)
                                    .ThenInclude(pd => pd.AddProductUnitsDetails).FirstOrDefault();
        if (accountDetails == null)
            throw new TaskCanceledException("La cuenta no existe o ya esta cerrada");
        return _mapper.Map<List<ProductDetailDto>>(accountDetails.ProductsDetails);
    }

    public async Task<AccountDto> CreateAccount(string accountName)
    {
        var newAccount = await _accountRepository.Create(new Account()
        {
            AccountName = accountName,
            State = true,
            IsClosed = false,
            CreationDate = DateTime.Now
        });
        if (newAccount == null)
            throw new TaskCanceledException("Error al crear la cuenta");
        return _mapper.Map<AccountDto>(newAccount);
    }

    public async Task<bool> AddProductsToAccount(AddProductsToAccountDto info)
    {
        try
        {
            var account = await _accountRepository.Get(a => a.State && !a.IsClosed && a.IdAccount == info.AccountId);
            if (account == null)
                throw new TaskCanceledException("La cuenta no existe o ya esta cerrada");
            foreach (var productAccount in info.Products)
            {
                var product = await _productRepository.Get(p => p.State && p.IdProduct == productAccount.ProductId);
                if (product == null)
                    throw new TaskCanceledException("El producto con id " + productAccount.ProductId + " no existe");
                if (product.ProductStock < productAccount.ProductUnits)
                    throw new TaskCanceledException("No hay unidades suficientes de " + product.ProductName);
                ProductsDetail productDetail = new ProductsDetail()
                {
                    IdAccount = account.IdAccount,
                    IdProduct = productAccount.ProductId,
                    ProductQuantity = productAccount.ProductUnits,
                    ProductPrice = productAccount.ProductPrice,
                    TotalPrice = (productAccount.ProductUnits * productAccount.ProductPrice),
                    State = true
                };
                productDetail.AddProductUnitsDetails.Add(new AddProductUnitsDetail()
                {
                    Description = "Primera(s) unidades agregadas",
                    IdDetail = productDetail.IdDetail,
                    CreationDate = DateTime.Now,
                    ProductQuantity = productAccount.ProductUnits
                });
                account.ProductsDetails.Add(productDetail);
                product.ProductStock -= productAccount.ProductUnits;
                await _productRepository.Edit(product);
            }
            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> AddUnitsToProductDetail(AddUnitsToProductDetailDto info)
    {
        var productDetail = await _productsDetailRepository.Get(pd => pd.IdDetail == info.ProductDetailId);
        if (productDetail == null)
            throw new TaskCanceledException("No existe el detalle o fue eliminado");
        var product = await _productRepository.Get(p => p.State && p.IdProduct == productDetail.IdProduct);
        if (product == null)
            throw new TaskCanceledException("El producto no existe");
        if (product.ProductStock < info.ProductQuantity)
            throw new TaskCanceledException("No hay unidades suficientes de " + product.ProductName);
        productDetail.AddProductUnitsDetails.Add(new AddProductUnitsDetail()
        {
            Description = info.Description,
            IdDetail = productDetail.IdDetail,
            CreationDate = DateTime.Now,
            ProductQuantity = info.ProductQuantity
        });
        product.ProductStock -= info.ProductQuantity;
        productDetail.ProductQuantity += info.ProductQuantity;
        productDetail.TotalPrice = info.ProductQuantity > 0 ? (productDetail.TotalPrice + productDetail.ProductPrice * info.ProductQuantity)
                                                            : (productDetail.TotalPrice - productDetail.ProductPrice * Int32.Abs(info.ProductQuantity));
        var result = await _productsDetailRepository.Edit(productDetail);
        return result;
    }

    public async Task<bool> EditAccount(EditAccountDto info)
    {
        var validateName =
            await _accountRepository.Get(a => a.State && !a.IsClosed && a.AccountName == info.AccountName);
        if (validateName != null)
            throw new TaskCanceledException("El nombre de cuenta ya esta asignado a una cuenta sin cerrar");
        var account = await _accountRepository.Get(a => a.State && !a.IsClosed && a.IdAccount == info.AccountId);
        if (account == null)
            throw new TaskCanceledException("La cuenta no existe o ya fue cerrada");
        account.AccountName = info.AccountName;
        bool result = await _accountRepository.Edit(account);
        if (!result)
            throw new TaskCanceledException("Error al editar la cuenta");
        return result;
    }

    public async Task<List<PaymentDto>> GetPaymentsFromAccount(int accountId)
    {
        var account = await _accountRepository.Consult(a => a.State && !a.IsClosed && a.IdAccount == accountId);
        var accountDetails = account.Include(a => a.ProductsDetails)
            .ThenInclude(pd => pd.Payments)
            .ThenInclude(p => p.IdProductNavigation)
            .ThenInclude(pd => pd.Payments)
            .ThenInclude(p => p.IdPaymentTypeNavigation).FirstOrDefault();
        var payments = new List<PaymentDto>();
        foreach (var productDetail in accountDetails.ProductsDetails)
        {
            var productPayments = _mapper.Map<List<PaymentDto>>(productDetail.Payments.Where(p => p.State));
            payments.AddRange(productPayments);
        }
        return payments;
    }
}