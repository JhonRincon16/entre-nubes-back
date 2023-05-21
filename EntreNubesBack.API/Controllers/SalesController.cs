using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }
    
    /// <summary>
    /// Metodo para obtener el listado de ventas
    /// </summary>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetSales()
    {
        var response = new Response<List<SaleDto>>();
        try
        {
            response.Value = await _saleService.GetSales();
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
    /// Metodo para registrar un pago
    /// </summary>
    [HttpPost]
    [Route("AddPayment")]
    public async Task<IActionResult> AddPayment(AddPaymentDto info)
    {
        var response = new Response<PaymentDto>();
        try
        {
            response.Value = await _saleService.AddPayment(info);
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
    /// Metodo para registrar un pago
    /// </summary>
    [HttpPatch]
    [Route("RemovePayment")]
    public async Task<IActionResult> RemovePayment(int paymentId)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _saleService.RemovePayment(paymentId);
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
    /// Metodo para para registrar ultimo pago y cierre de venta.
    /// </summary>
    [HttpPost]
    [Route("CloseSale")]
    public async Task<IActionResult> CreateSale(CloseSaleDto info)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _saleService.CloseSale(info);
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