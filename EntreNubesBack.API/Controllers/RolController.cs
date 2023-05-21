using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

/// <summary>
/// Controlador de la entidad Roles
/// </summary>

[Authorize]
[ApiController]
[Route("[controller]")]
public class RolController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolController(IRolService rolService)
    {
        _rolService = rolService;
    }

    /// <summary>
    /// Listado de Roles 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var response = new Response<List<RolDto>>();
        try
        {
            response.Status = true;
            response.Value = await _rolService.List();
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpGet]
    [Route("Actions")]
    public async Task<IActionResult> GetActions()
    {
        var response = new Response<List<ActionDto>>();
        try
        {
            response.Status = true;
            response.Value = await _rolService.Actions();
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRol([FromBody] CreateRolDto newRolInfo)
    {
        var response = new Response<RolDto>();
        try
        {
            response.Status = true;
            response.Value = await _rolService.Create(newRolInfo);
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> EditRol([FromBody] EditRolDto rolDto)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _rolService.Edit(rolDto);
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        return Ok(response);
    }
    
    [HttpPatch]
    [Route("Delete")]
    public async Task<IActionResult> DeleteRol([FromQuery] int rolId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _rolService.Delete(rolId);
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