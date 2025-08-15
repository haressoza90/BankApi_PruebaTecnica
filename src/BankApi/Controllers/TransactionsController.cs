using BankApi.Dtos;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;
    public TransactionsController(ITransactionService service) => _service = service;

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] MovementRequest req)
    {
        try { await _service.DepositAsync(req.AccountNumber, req.Amount, TransactionType.Deposit); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] MovementRequest req)
    {
        try { await _service.WithdrawAsync(req.AccountNumber, req.Amount); return NoContent(); }
        catch (Exception ex) { return BadRequest(new { error = ex.Message }); }
    }
}
