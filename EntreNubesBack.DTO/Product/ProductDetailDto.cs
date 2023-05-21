using EntreNubesBack.DTO.Account;

namespace EntreNubesBack.DTO.Product;

public class ProductDetailDto
{
    public int IdDetail { get; set; }
    public int IdAccount { get; set; }
    public int IdProduct { get; set; }
    public int ProductQuantity { get; set; }
    public double? TotalPrice { get; set; }
    public double ProductPrice { get; set; }
    public ProductDto IdProductNavigation { get; set; } = null!;
    public List<AddProductUnitsDetailDto> AddProductUnitsDetails { get; set; } = null!;
}