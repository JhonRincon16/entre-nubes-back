namespace EntreNubesBack.DTO.Account;

public class AddProductsToAccountDto
{
    public int AccountId { get; set; }
    public List<ProductAccountDto> Products { get; set; } = null!;
}