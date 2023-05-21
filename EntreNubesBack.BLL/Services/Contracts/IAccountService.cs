using EntreNubesBack.DTO.Account;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Product;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IAccountService
{
    Task<List<AccountDto>> GetUnclosedAccounts();
    Task<List<ProductDetailDto>> GetProductsFromAnAccount(int accountId);
    Task<AccountDto> CreateAccount(string accountName);
    Task<bool> AddProductsToAccount(AddProductsToAccountDto info);
    Task<bool> AddUnitsToProductDetail(AddUnitsToProductDetailDto info);
    Task<bool> EditAccount(EditAccountDto info);
    Task<List<PaymentDto>> GetPaymentsFromAccount(int accountId);
}