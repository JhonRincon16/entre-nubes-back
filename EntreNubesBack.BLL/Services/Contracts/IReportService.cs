using EntreNubesBack.DTO.Report;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IReportService
{
    Task<List<ReportPerMonthReport>> GenerateSalesReport(DateTime startDate, DateTime endDate);
    Task<List<ReportPerMonthReport>> GenerateExpensesReport(DateTime startDate, DateTime endDate);
    Task<SalesVsExpensesReport> GenerateSalesVsExpensesReport(DateTime startDate, DateTime endDate);
}