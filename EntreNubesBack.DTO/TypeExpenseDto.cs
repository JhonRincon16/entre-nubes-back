namespace EntreNubesBack.DTO;

public class TypeExpenseDto
{
    public int IdTypeExpense { get; set; }

    public string TypeExpenseName { get; set; } = null!;

    public bool State { get; set; }
}