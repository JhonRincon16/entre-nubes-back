using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

/// <summary>
/// Controlador de la entidad Productos
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    /// <summary>
    /// Listado de Productos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetProductsList()
    {
        var response = new Response<List<ProductDto>>();
        try
        {
            response.Status = true;
            response.Value = await _productService.List();
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
    /// Metodo para crear un producto
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductDto productInfo)
    {
        var response = new Response<ProductDto>();
        try
        {
            response.Status = true;
            response.Value = await _productService.CreateProduct(productInfo);
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
    /// Metodo para editar los datos de un producto
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditProduct([FromBody]ProductDto productInfo)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _productService.EditProduct(productInfo);
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
    /// Metodo para eliminar producto (cambiar estado)
    /// </summary>
    /// <returns></returns>
    [HttpPatch]
    [Route("Delete")]
    public async Task<IActionResult> DeleteProduct([FromQuery]int productId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _productService.DeleteProduct(productId);
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