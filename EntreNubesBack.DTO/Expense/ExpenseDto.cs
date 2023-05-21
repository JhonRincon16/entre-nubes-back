using EntreNubesBack.DTO.Employee;

namespace EntreNubesBack.DTO.Expense;

public class ExpenseDto
{
    public int IdExpense { get; set; }
    
    public string ExpenseDescription { get; set; } = null!;

    public DateTime Date { get; set; }

    public double ExpenseTotal { get; set; }

    public bool State { get; set; }

    public int IdPaymentType { get; set; }
    
    public int? LastIncomeId { get; set; }

    public virtual PaymentTypeDto IdPaymentTypeNavigation { get; set; } = null!;

    public virtual ThirdPartyDto? IdThirdPartyNavigation { get; set; }
    
    public virtual EmployeeDto? IdEmployeeNavigation { get; set; }

    public virtual TypeExpenseDto IdTypeExpenseNavigation { get; set; } = null!;
}