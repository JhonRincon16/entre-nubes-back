using EntreNubesBack.DTO.Product;

namespace EntreNubesBack.DTO.Purchase;

public class CreatePurchaseDto
{
    public string EmployeeEmail { get; set; }
    public string Description { get; set; }
    public List<PurchaseProductDto> products { get; set; }
    public int PaymentTypeId { get; set; }
    public double Total { get; set; }
}