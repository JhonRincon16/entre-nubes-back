using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO.Report;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class ReportService : IReportService
{
    private IGenericRepository<Payment> _paymentRepository;
    private IExpenseRepository _expenseRepository;
    
    public ReportService(IGenericRepository<Payment> paymentRepository, IExpenseRepository expenseRepository)
    {
        _paymentRepository = paymentRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<List<ReportPerMonthReport>> GenerateSalesReport(DateTime startDate, DateTime endDate)
    {
        var payments = await _paymentRepository.Consult(p => p.State && p.AmountToPay > 0);
        try
        {
            var list = await payments.Where(p => p.Date >= startDate.Date && p.Date <= endDate.Date).ToListAsync();
            List<ReportPerMonthReport> report = new List<ReportPerMonthReport>();
            for (int i = 1; i <= endDate.Day; i++)
            {
                var total = list.Where(p => p.Date.Day == i).Sum(p => p.AmountToPay);
                report.Add(new ReportPerMonthReport(){ Day = i, Total = total});
            }
            return report;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<ReportPerMonthReport>> GenerateExpensesReport(DateTime startDate, DateTime endDate)
    {
        try
        {
            var expenses =
                await _expenseRepository.Consult(e => e.State && e.CreationDate >= startDate && e.CreationDate <= endDate);
            List<ReportPerMonthReport> report = new List<ReportPerMonthReport>();
            for (int i = 1; i <= endDate.Day; i++)
            {
                var total = expenses.Where(e => e.CreationDate.Value.Day == i).Sum(e => e.ExpenseTotal);
                report.Add(new ReportPerMonthReport(){ Day = i, Total = total});
            }
            return report;
        }
        catch
        {
            throw;
        }
    }

    public async Task<SalesVsExpensesReport> GenerateSalesVsExpensesReport(DateTime startDate, DateTime endDate)
    {
        var salesReport = await GenerateSalesReport(startDate, endDate);
        var expensesReport = await GenerateExpensesReport(startDate, endDate);
        return new SalesVsExpensesReport()
        {
            SalesReport = salesReport,
            ExpensesReport = expensesReport
        };
    }
}