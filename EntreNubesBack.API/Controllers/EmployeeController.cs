using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    /// <summary>
    /// Listado de Empleados con datos personales y de acceso
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetEmployeeList()
    {
        var response = new Response<List<EmployeeDto>>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.EmployeeList();
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
    /// Metodo para crear un empleado y su correspondiente usuario
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeDto employeeInfo)
    {
        var response = new Response<EmployeeDto>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.CreateEmployee(employeeInfo);
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
    /// Metodo para editar los datos de un empleado
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditEmployee([FromBody]EmployeeDto employeeInfo)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.EditEmployee(employeeInfo);
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
    /// Metodo para eliminar un empleado
    /// </summary>
    /// <returns></returns>
    [HttpPatch]
    [Route("Delete")]
    public async Task<IActionResult> DeleteEmployee([FromQuery]int employeeId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.DeleteEmployee(employeeId);
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
    /// Metodo para registrar la entrada de un empleado que gana por horas
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("RegisterIncome")]
    public async Task<IActionResult> RegisterIncome([FromQuery]int userId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.RegisterEntrance(userId);
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
    /// Metodo para registrar la salida de un empleado que gana por horas
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("RegisterExit")]
    public async Task<IActionResult> RegisterExit([FromQuery]int userId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.RegisterExit(userId);
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
    /// Metodo para calcular el numero de horas trabajadas por un empleado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("CalculateHoursWorked")]
    public async Task<IActionResult> CalculateHoursWorked([FromQuery]int employeeId)
    {
        var response = new Response<CalculateHoursWorkedDto>();
        try
        {
            response.Status = true;
            response.Value = await _employeeService.CalculateHoursWorked(employeeId);
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