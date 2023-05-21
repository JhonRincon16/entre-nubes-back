using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Sale;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IGenericRepository<ProductsDetail> _productsDetailRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICashClosingRepository _cashClosingRepository;
    private readonly IMapper _mapper;

    public SaleService(ISaleRepository saleRepository, 
                       IMapper mapper, 
                       IAccountRepository accountRepository, 
                       IGenericRepository<ProductsDetail> productsDetailRepository, 
                       IGenericRepository<Payment> paymentRepository, 
                       ICashClosingRepository cashClosingRepository)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _accountRepository = accountRepository;
        _productsDetailRepository = productsDetailRepository;
        _paymentRepository = paymentRepository;
        _cashClosingRepository = cashClosingRepository;
    }

    public async Task<List<SaleDto>> GetSales()
    {
        var sales = await _saleRepository.Consult(s => s.State);
        var salesDetails = sales.Include(s => s.Accounts)
            .Include(s => s.Payments)
            .ThenInclude(p => p.IdPaymentTypeNavigation)
            .Include(s => s.Payments)
            .ThenInclude(p => p.IdProductNavigation).ToList();
        foreach (var sale in salesDetails)
        {
            sale.TotalSale = salesDetails.FirstOrDefault(sd => sd.IdSale == sale.IdSale)?.Payments.Sum(p => p.AmountToPay);
        }
        return _mapper.Map<List<SaleDto>>(salesDetails);
    }

    public async Task<PaymentDto> AddPayment(AddPaymentDto info)
    {
        var productDetail = await _productsDetailRepository.Get(pd => pd.IdDetail == info.ProductDetailId);
        if (productDetail == null)
            throw new TaskCanceledException("El detalle no existe");
        var account = await _accountRepository.Get(a => a.State && !a.IsClosed && a.IdAccount == productDetail.IdAccount);
        if (account == null)
            throw new TaskCanceledException("La cuenta no existe");
        productDetail.ProductQuantity -= info.Quantity;
        productDetail.TotalPrice = productDetail.ProductQuantity * productDetail.ProductPrice;
        var actualCashClosing = await _cashClosingRepository.GetLastCashClosing();
        Payment createdPayment = null;
        if (info.SaleId == null)
        {
            var sale = await _saleRepository.Create(new Sale()
            {
                State = true,
                IdCashClosing = actualCashClosing.IdCashClosing
            });
            account.IdSale = sale.IdSale;
            await _accountRepository.Edit(account);
            createdPayment = await _paymentRepository.Create(new Payment()
            {
                Quantity = info.Quantity,
                AmountToPay = info.AmountToPay,
                IdSale = sale.IdSale,
                IdProduct = info.ProductId,
                IdProductDetail = info.ProductDetailId,
                IdPaymentType = info.PaymentTypeId,
                State = true,
                Date = DateTime.Now
            });
        }
        else
        {
            var sale = await _saleRepository.Get(s =>s.State && s.IdSale == info.SaleId);
            createdPayment = await _paymentRepository.Create(new Payment()
            {
                Quantity = info.Quantity,
                AmountToPay = info.AmountToPay,
                IdSale = sale.IdSale,
                IdProduct = info.ProductId,
                IdProductDetail = info.ProductDetailId,
                IdPaymentType = info.PaymentTypeId,
                State = true,
                Date = DateTime.Now
            });
            if (createdPayment == null)
                throw new TaskCanceledException("Error al agregar el pago");
        }
        var payment = await _paymentRepository.Consult(p => p.State && p.IdPayment == createdPayment.IdPayment);
        var paymentDetails = payment.Include(p => p.IdProductNavigation)
            .Include(p => p.IdPaymentTypeNavigation).FirstOrDefault();
        return _mapper.Map<PaymentDto>(paymentDetails);
    }

    public async Task<bool> RemovePayment(int paymentId)
    {
        var payment = await _paymentRepository.Get(p => p.State && p.IdPayment == paymentId);
        if (payment == null)
            throw new TaskCanceledException("No existe el pago");
        var productDetail = await _productsDetailRepository.Get(pd => pd.IdDetail == payment.IdProductDetail);
        if(productDetail == null)
            throw new TaskCanceledException("No existe el detalle");
        productDetail.ProductQuantity += payment.Quantity;
        productDetail.TotalPrice += payment.AmountToPay;
        bool resultProductDetail = await _productsDetailRepository.Edit(productDetail);
        if (!resultProductDetail)
            throw new TaskCanceledException("Error al agregar las unidades");

        payment.State = false;
        bool resultPayment = await _paymentRepository.Edit(payment);
        if (!resultPayment)
            throw new TaskCanceledException("Error al eliminar el pago");
        return resultPayment;
    }

    public async Task<bool> CloseSale(CloseSaleDto info)
    {
        var productDetail = await _productsDetailRepository.Get(pd => pd.IdDetail == info.ProductDetailId);
        if (productDetail == null)
            throw new TaskCanceledException("El detalle no existe");
        var account = await _accountRepository.Get(a => a.State && !a.IsClosed && a.IdAccount == productDetail.IdAccount);
        if (account == null)
            throw new TaskCanceledException("La cuenta no existe");
        productDetail.ProductQuantity -= info.Quantity;
        productDetail.TotalPrice = productDetail.ProductQuantity * productDetail.ProductPrice;
        account.IsClosed = true;
        
        bool resultAccount = await _accountRepository.Edit(account);
        if (!resultAccount)
            throw new TaskCanceledException("Error al cerrar la cuenta");

        var sale = await _saleRepository.Get(s => s.State && s.IdSale == info.SaleId);
        if (sale == null)
            throw new TaskCanceledException("Error no existe la venta");
        sale.SaleDate = DateTime.Now;
        var payment = await _paymentRepository.Create(new Payment()
        {
            Quantity = info.Quantity,
            AmountToPay = info.AmountToPay,
            IdSale = sale.IdSale,
            IdProduct = info.ProductId,
            IdProductDetail = info.ProductDetailId,
            IdPaymentType = info.PaymentTypeId,
            State = true,
            Date = DateTime.Now
        });
        if (payment == null)
            throw new TaskCanceledException("Error al agregar el pago");
        bool saleResult = await _saleRepository.Edit(sale);
        if (!saleResult)
            throw new TaskCanceledException("Error al cerrar la venta");
        return true;
    }
}