namespace EntreNubesBack.DTO;

public class PaymentTypeDto
{
    public int IdPaymentType { get; set; }

    public string PaymentTypeName { get; set; } = null!;

    public bool State { get; set; }
}