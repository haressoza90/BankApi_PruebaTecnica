using BankApi.Dtos;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(ICustomerService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        try
        {
            var customer = new Customer
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Sex = dto.Sex,
                Income = dto.Income
            };

            var created = await _service.CreateAsync(customer);
            var resp = new CustomerResponse(created.Id, created.Name, created.BirthDate, created.Sex, created.Income);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, resp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await _service.GetAsync(id);
        if (c is null) return NotFound();

        var resp = new CustomerResponse(c.Id, c.Name, c.BirthDate, c.Sex, c.Income);
        return Ok(resp);
    }

    //para eliminar cliente por ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id); //tiene que xistir en ICustomerService
            if (!deleted)
                return NotFound(new { error = "Cliente no encontrado" });

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
