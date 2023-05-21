using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Metodo para obtener el listado de usuarios
    /// </summary>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetUsers()
    {
        var response = new Response<List<UserDto>>();
        try
        {
            response.Value = await _userService.List();
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
    /// Metodo para la creacion de un usuario
    /// </summary>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        var response = new Response<UserDto>();
        try
        {
            response.Value = await _userService.Create(userDto);
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
    /// Metodo para la editar un usuario
    /// </summary>
    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditUser([FromBody] EditUserDto userDto)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _userService.Edit(userDto);
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
    /// Metodo para cambiar el estado a un usuario
    /// </summary>
    [HttpPut]
    [Route("ChangeStatus")]
    public async Task<IActionResult> DeactivateUser([FromQuery] int userId)
    {
        var response = new Response<bool>();
        try
        {
            response.Value = await _userService.ChangeStatus(userId);
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