using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.DTO.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    /// <summary>
    /// Metodo para obtener el listado de metodos de pago
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("PaymentMethods")]
    public async Task<IActionResult> GetPaymentMethods()
    {
        var response = new Response<List<PaymentTypeDto>>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.PaymentMethodsList();
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
    /// Metodo para obtener el listado de tipos de gastos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("ExpenseTypes")]
    public async Task<IActionResult> GetExpenseTypes()
    {
        var response = new Response<List<TypeExpenseDto>>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.ExpenseTypesList();
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
    /// Metodo para obtener el listado de gastos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> GetExpenses()
    {
        var response = new Response<List<ExpenseDto>>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.ExpensesList();
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
    /// Metodo para crear un gasto
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseDto expenseInfo)
    {
        var response = new Response<ExpenseDto>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.CreateExpense(expenseInfo);
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
    /// Metodo para crear un gasto o pago de nomina
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("CreatePayrollExpense")]
    public async Task<IActionResult> CreatePayrollExpense([FromBody]CreatePayrollExpenseDto expenseInfo)
    {
        var response = new Response<ExpenseDto>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.CreatePayrollExpense(expenseInfo);
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
    /// Metodo para editar los datos de un gasto
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditExpense([FromBody]EditExpenseDto expenseInfo)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.EditExpense(expenseInfo);
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
    /// Metodo para eliminar un gasto
    /// </summary>
    /// <returns></returns>
    [HttpPatch]
    [Route("Delete")]
    public async Task<IActionResult> DeleteExpense([FromQuery]int expenseId)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _expenseService.DeleteExpense(expenseId);
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