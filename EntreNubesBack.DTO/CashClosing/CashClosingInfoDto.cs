using EntreNubesBack.DTO.Payment;

namespace EntreNubesBack.DTO.CashClosing;

public class CashClosingInfoDto
{
    public int CashClosingId { get; set; }
    public double? TotalExpenses { get; set; }
    public double? TotalSales { get; set; }
    public double? TotalPurchases { get; set; }
    public List<PaymentTypeTotalDto> TotalByPaymentType { get; set; } = null!;
}