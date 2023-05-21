using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Expense;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IExpenseService
{
    Task<List<TypeExpenseDto>> ExpenseTypesList();
    Task<List<PaymentTypeDto>> PaymentMethodsList();
    Task<List<ExpenseDto>> ExpensesList();
    Task<ExpenseDto> CreateExpense(CreateExpenseDto info);
    Task<bool> EditExpense(EditExpenseDto info);
    Task<bool> DeleteExpense(int id);
    Task<ExpenseDto> CreatePayrollExpense(CreatePayrollExpenseDto info);
}