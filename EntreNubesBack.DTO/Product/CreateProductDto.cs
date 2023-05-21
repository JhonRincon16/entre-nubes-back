namespace EntreNubesBack.DTO.Product;

public class CreateProductDto
{
    public string ProductName { get; set; } = null!;
    public string ProductCategory { get; set; } = null!;
    public double ProductPrice { get; set; }
    public int ProductStock { get; set; }
}