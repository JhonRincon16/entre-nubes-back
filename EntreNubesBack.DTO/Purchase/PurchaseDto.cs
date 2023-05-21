namespace EntreNubesBack.DTO.Purchase;

public class PurchaseDto
{
    public int IdPurchase { get; set; }

    public string? PurchaseDescription { get; set; }

    public bool State { get; set; }

    public DateTime CreationDate { get; set; }

    public PersonDto? Person { get; set; }

    public List<PurchaseDetailDto> Products { get; } = new List<PurchaseDetailDto>();
}