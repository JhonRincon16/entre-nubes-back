using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class DashBoardController : ControllerBase
{
    private readonly IDashBoardService _dashBoardService;

    public DashBoardController(IDashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }
    
    /// <summary>
    /// Metodo para obtener los datos del dashboard
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetDashBoardInfo")]
    public async Task<IActionResult> GetDashBoardInfo()
    {
        var response = new Response<DashBoardInfoDto>();
        try
        {
            response.Status = true;
            response.Value = await _dashBoardService.GetDashBoardInfo();
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