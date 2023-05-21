using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Sale;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface ISaleService
{
    Task<List<SaleDto>> GetSales();
    Task<PaymentDto> AddPayment(AddPaymentDto info);
    Task<bool> CloseSale(CloseSaleDto info);
    Task<bool> RemovePayment(int paymentId);
}