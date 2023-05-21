namespace EntreNubesBack.DTO.Report;

public class SalesVsExpensesReport
{
    public List<ReportPerMonthReport>? SalesReport { get; set; }
    public List<ReportPerMonthReport>? ExpensesReport { get; set; }
}