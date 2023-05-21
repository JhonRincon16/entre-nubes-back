using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    /// <summary>
    /// Metodo para obtener el reporte de ventas por mes
    /// </summary>
    [HttpGet]
    [Route("SalesPerMonth")]
    public async Task<IActionResult> GetSales([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new Response<List<ReportPerMonthReport>>();
        try
        {
            response.Value = await _reportService.GenerateSalesReport(startDate, endDate);
            response.Status = true;
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }
    
    /// <summary>
    /// Metodo para obtener el reporte de gastos por mes
    /// </summary>
    [HttpGet]
    [Route("ExpensesPerMonth")]
    public async Task<IActionResult> GetExpenses([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new Response<List<ReportPerMonthReport>>();
        try
        {
            response.Value = await _reportService.GenerateExpensesReport(startDate, endDate);
            response.Status = true;
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }
    
    /// <summary>
    /// Metodo para obtener el reporte de Ventas vs Gastos
    /// </summary>
    [HttpGet]
    [Route("GetSalesVsExpenses")]
    public async Task<IActionResult> GetExpensesVsSales([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new Response<SalesVsExpensesReport>();
        try
        {
            response.Value = await _reportService.GenerateSalesVsExpensesReport(startDate, endDate);
            response.Status = true;
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }
}