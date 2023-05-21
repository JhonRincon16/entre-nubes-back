using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Metodo para la autenticacion de usuarios
    /// </summary>
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var response = new Response<SessionDto>();
        try
        {
            response.Value = await _authService.ValidateCredentials(loginDto.Email, loginDto.Password);
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