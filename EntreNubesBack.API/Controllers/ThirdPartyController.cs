using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ThirdPartyController : ControllerBase
{
    private readonly IThirdPartyService _thirdPartyService;

    public ThirdPartyController(IThirdPartyService thirdPartyService)
    {
        _thirdPartyService = thirdPartyService;
    }
    
    /// <summary>
    /// Metodo para obtener el listado de terceros
    /// </summary>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetThirdPartiesList()
    {
        var response = new Response<List<ThirdPartyDto>>();
        try
        {
            response.Value = await _thirdPartyService.List();
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
    /// Metodo para crear un tercero
    /// </summary>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateThirdParty([FromBody] ThirdPartyDto data)
    {
        var response = new Response<ThirdPartyDto>();
        try
        {
            response.Value = await _thirdPartyService.Create(data);
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
    /// Metodo para editar los datos de un tercero
    /// </summary>
    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditThirdParty([FromBody] ThirdPartyDto dataToEdit)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _thirdPartyService.Edit(dataToEdit);
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
    /// Metodo para eliminar un tercero (cambiar estado a false)
    /// </summary>
    [HttpPatch]
    [Route("Delete")]
    public async Task<IActionResult> DeleteThirdParty(int idThirdParty)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _thirdPartyService.Delete(idThirdParty);
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
    /// Metodo para agregar un asesor a un tercero
    /// </summary>
    [HttpPost]
    [Route("AddAdvisor")]
    public async Task<IActionResult> AddAdvisorToThirdParty([FromBody] AddAdvisorToThirdPartyDto data)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _thirdPartyService.AddAdvisorToThirdParty(data);
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
    /// Metodo para editar los datos de un asesor
    /// </summary>
    [HttpPut]
    [Route("EditAdvisor")]
    public async Task<IActionResult> EditAdvisor([FromBody] PersonDto data)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _thirdPartyService.EditAdvisorInfo(data);
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
    /// Metodo para eliminar un asesor de un tercero
    /// </summary>
    [HttpPut]
    [Route("RemoveAdvisor")]
    public async Task<IActionResult> DeleteAdvisor(int idThirdParty, int advisorId)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _thirdPartyService.DeleteAdvisorFromThirdParty(idThirdParty, advisorId);
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