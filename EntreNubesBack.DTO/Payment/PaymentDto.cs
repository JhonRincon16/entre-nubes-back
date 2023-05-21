using EntreNubesBack.DTO.Product;

namespace EntreNubesBack.DTO.Payment;

public class PaymentDto
{
    public int IdPayment { get; set; }
    public int IdSale { get; set; }
    public int IdPaymentType { get; set; }
    public int IdProduct { get; set; }
    public int IdProductDetail { get; set; }
    public int Quantity { get; set; }
    public double AmountToPay { get; set; }
    public bool State { get; set; }
    public DateTime Date { get; set; }
    public PaymentTypeDto IdPaymentTypeNavigation { get; set; } = null!;
    public ProductDto IdProductNavigation { get; set; } = null!;
}