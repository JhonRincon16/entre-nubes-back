namespace EntreNubesBack.DTO.Account;

public class AddProductUnitsDetailDto
{
    public int IdAddUnits { get; set; }

    public int IdDetail { get; set; }

    public string? Description { get; set; }

    public int ProductQuantity { get; set; }

    public DateTime CreationDate { get; set; }
}