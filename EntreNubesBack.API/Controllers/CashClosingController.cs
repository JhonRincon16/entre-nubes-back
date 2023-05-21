using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO.CashClosing;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CashClosingController : ControllerBase
{
    private readonly ICashClosingService _cashClosingService;

    public CashClosingController(ICashClosingService cashClosingService)
    {
        _cashClosingService = cashClosingService;
    }
    
    /// <summary>
    /// Metodo para obtener la informacion del cierre de caja actual
    /// </summary>
    [HttpGet]
    [Route("GetCashClosingInfo")]
    public async Task<IActionResult> GetCashClosingInfo()
    {
        var response = new Response<CashClosingInfoDto>();
        try
        {
            response.Value = await _cashClosingService.GetCashClosingInfo();
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
    /// Metodo para obtener el listado de cierres de caja
    /// </summary>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetCashClosingList()
    {
        var response = new Response<List<CashClosingInfoDto>>();
        try
        {
            response.Value = await _cashClosingService.GetCashClosingList();
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
    /// Metodo para cerrar un cuadre de caja
    /// </summary>
    [HttpPost]
    [Route("CloseCashClosing")]
    public async Task<IActionResult> CloseCashClosing([FromBody] CloseCashClosing info)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _cashClosingService.CloseCashClosing(info);
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
    /// Metodo para actualizar el efectivo base de un ciere de caja
    /// </summary>
    [HttpPut]
    [Route("UpdateBaseCash")]
    public async Task<IActionResult> UpdateBaseCash([FromQuery] int cashClosingId, double baseCash)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _cashClosingService.UpdateBaseCash(cashClosingId, baseCash);
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