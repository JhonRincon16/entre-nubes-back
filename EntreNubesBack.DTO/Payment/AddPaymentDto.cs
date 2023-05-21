namespace EntreNubesBack.DTO.Sale;

public class AddPaymentDto
{
    public int? SaleId { get; set; }
    public int ProductDetailId { get; set; }
    public int PaymentTypeId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double AmountToPay { get; set; }
}