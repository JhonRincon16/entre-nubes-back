using EntreNubesBack.DTO.Product;

namespace EntreNubesBack.DTO.Purchase;

public class PurchaseDetailDto
{
    public int IdPurchase { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public double TotalPrice { get; set; }

    public ProductDto IdProductNavigation { get; set; } = null!;
}