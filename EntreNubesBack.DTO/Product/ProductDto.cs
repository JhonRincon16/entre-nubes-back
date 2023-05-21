namespace EntreNubesBack.DTO.Product;

public class ProductDto
{
    public int IdProduct { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductCategory { get; set; } = null!;
    public double ProductPrice { get; set; }
    public int ProductStock { get; set; }
    public bool State { get; set; }
}