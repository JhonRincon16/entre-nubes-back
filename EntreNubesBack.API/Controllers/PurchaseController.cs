using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    
    /// <summary>
    /// Metodo para obtener el listado de compras
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> List()
    {
        var response = new Response<List<PurchaseDto>>();
        try
        {
            response.Status = true;
            response.Value = await _purchaseService.PurchaseList();
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    /// <summary>
    /// Metodo para crear una compra de productos
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseDto info)
    {
        var response = new Response<PurchaseDto>();
        try
        {
            response.Status = true;
            response.Value = await _purchaseService.CreatePurchase(info);
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    /// <summary>
    /// Metodo para eliminar una compra
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("Delete")]
    public async Task<IActionResult> DeletePurchase([FromQuery] int purchaseId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _purchaseService.DeletePurchase(purchaseId);
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
}