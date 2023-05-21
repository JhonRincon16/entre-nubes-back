namespace EntreNubesBack.DTO.Payment;

public class PaymentTypeTotalDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double? TotalSales { get; set; }
    public double? TotalExpenses { get; set; }
    public double? TotalPurchases { get; set; }
}