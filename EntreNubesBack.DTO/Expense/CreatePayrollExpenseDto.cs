namespace EntreNubesBack.DTO.Expense;

public class CreatePayrollExpenseDto
{
    public int EmployeeId { get; set; }
    public string? Description { get; set; }
    public int PaymentMethodId { get; set; }
    public double Total { get; set; }
    public int LastIncomeId { get; set; }
}