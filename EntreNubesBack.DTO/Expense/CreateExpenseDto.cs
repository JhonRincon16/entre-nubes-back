namespace EntreNubesBack.DTO.Expense;

public class CreateExpenseDto
{
    public int ProviderId { get; set; }
    public string ExpenseType { get; set; }
    public string? Description { get; set; }
    public int PaymentMethodId { get; set; }
    public double Total { get; set; }
}