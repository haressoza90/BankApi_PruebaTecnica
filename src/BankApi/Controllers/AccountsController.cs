using BankApi.Dtos;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accounts;
    private readonly ITransactionService _transactions;

    public AccountsController(IAccountService accounts, ITransactionService transactions)
    {
        _accounts = accounts;
        _transactions = transactions;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto req)
    {
        try
        {
            var acc = await _accounts.CreateAccountAsync(req.CustomerId, req.AccountNumber, req.InitialBalance);
            return CreatedAtAction(nameof(GetBalance), new { accountNumber = acc.AccountNumber }, acc);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{accountNumber}/balance")]
    public async Task<IActionResult> GetBalance(string accountNumber)
    {
        try
        {
            var bal = await _accounts.GetBalanceAsync(accountNumber);
            return Ok(new BalanceResponse(accountNumber, bal));
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("{accountNumber}/transactions")]
    public async Task<IActionResult> GetTransactions(string accountNumber)
    {
        try
        {
            var txs = await _transactions.GetTransactionsAsync(accountNumber);
            return Ok(txs);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{accountNumber}/apply-interest")]
    public async Task<IActionResult> ApplyInterest(string accountNumber, [FromBody] ApplyInterestDto req)
    {
        try
        {
            await _accounts.ApplyInterestAsync(accountNumber, req.Rate);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // NUEVO: Eliminar cuenta
    [HttpDelete("{accountNumber}")]
    public async Task<IActionResult> DeleteAccount(string accountNumber)
    {
        try
        {
            var deleted = await _accounts.DeleteAccountAsync(accountNumber);
            if (!deleted)
                return NotFound(new { error = "Account not found" });

            return NoContent(); // 204
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
