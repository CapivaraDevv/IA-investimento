using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpenseController(ExpenseService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetByUser), new { userId = request.UserId }, result);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct) =>
        Ok(await service.GetByUserAsync(userId, ct));

    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}
