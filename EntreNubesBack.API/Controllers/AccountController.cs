using EntreNubesBack.API.Util;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DTO.Account;
using EntreNubesBack.DTO.Payment;
using EntreNubesBack.DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntreNubesBack.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet]
    [Route("UnclosedAccounts")]
    public async Task<IActionResult> GetUnclosedAccounts()
    {
        var response = new Response<List<AccountDto>>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.GetUnclosedAccounts();
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
    [Route("GetProductsFromAccount")]
    public async Task<IActionResult> GetProductsFromAnAccount(int accountId)
    {
        var response = new Response<List<ProductDetailDto>>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.GetProductsFromAnAccount(accountId);
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
    [Route("Create")]
    public async Task<IActionResult> CreateAccount([FromQuery] string accountName)
    {
        var response = new Response<AccountDto>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.CreateAccount(accountName);
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
    [Route("AddProducts")]
    public async Task<IActionResult> AddProductToAccount([FromBody] AddProductsToAccountDto info)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.AddProductsToAccount(info);
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
    [Route("AddUnits")]
    public async Task<IActionResult> AddUnitsToProductDetail([FromBody] AddUnitsToProductDetailDto info)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.AddUnitsToProductDetail(info);
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
    [Route("Edit")]
    public async Task<IActionResult> EditAccount([FromBody] EditAccountDto info)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.EditAccount(info);
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
    [Route("GetPaymentsFromAccount")]
    public async Task<IActionResult> GetPaymentsFromAccount(int accountId)
    {
        var response = new Response<List<PaymentDto>>();
        try
        {
            response.Status = true;
            response.Value = await _accountService.GetPaymentsFromAccount(accountId);
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