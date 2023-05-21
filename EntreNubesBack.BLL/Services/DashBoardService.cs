using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.DashBoard;
using EntreNubesBack.Models;

namespace EntreNubesBack.BLL.Services;

public class DashBoardService : IDashBoardService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICashClosingRepository _cashClosingRepository;
    private readonly IGenericRepository<Payment> _paymentRepository;

    public DashBoardService(IExpenseRepository expenseRepository, 
                            IAccountRepository accountRepository, 
                            ICashClosingRepository cashClosingRepository, 
                            IGenericRepository<Payment> paymentRepository)
    {
        _expenseRepository = expenseRepository;
        _accountRepository = accountRepository;
        _cashClosingRepository = cashClosingRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<DashBoardInfoDto> GetDashBoardInfo()
    {
        var lastCashClosing = await _cashClosingRepository.GetLastCashClosing();
        var expenses = lastCashClosing == null
            ? await _expenseRepository.ConsultAsNoTacking(e => e.State)
            : await _expenseRepository.ConsultAsNoTacking(e => e.State && e.CreationDate >= lastCashClosing.StartDate);
        var payments = lastCashClosing == null 
            ? await _paymentRepository.ConsultAsNoTacking(p => p.State && p.AmountToPay > 0)
            : await _paymentRepository.ConsultAsNoTacking(p => p.State && p.AmountToPay > 0 && p.Date >= lastCashClosing.StartDate);
        
        var unclosedAccounts = _accountRepository.Consult(a => a.State && !a.IsClosed).Result.Count();
        var totalExpenses = expenses.ToList().Sum(e => e.ExpenseTotal);
        var totalSales = payments.ToList().Sum(p => p.AmountToPay);
        DashBoardInfoDto info = new DashBoardInfoDto()
        {
            TotalExpenses = totalExpenses,
            TotalSales = totalSales,
            Cash = totalSales - totalExpenses,
            UnclosedAccounts = unclosedAccounts
        };
        return info;
    }
}