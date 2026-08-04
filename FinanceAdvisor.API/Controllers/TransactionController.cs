using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Application.Services;
using FinanceAdvisor.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(TransactionService service) : ControllerBase
{
    [HttpPost("{userProfileId:guid}")]
    public async Task<ActionResult<TransactionResponse>> Create(
        Guid userProfileId,
        CreateTransactionRequest request,
        CancellationToken ct)
    {
        var transaction = await service.CreateAsync(request, userProfileId, ct);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpGet("user/{userProfileId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetByUser(
        Guid userProfileId,
        CancellationToken ct)
    {
        var transactions = await service.GetByUserAsync(userProfileId, ct);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> GetById(
        Guid id,
        CancellationToken ct)
    {
        var transaction = await service.GetByIdAsync(id, ct);

        if (transaction is null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> Update(
        Guid id,
        UpdateTransactionRequest request,
        CancellationToken ct)
    {
        var transaction = await service.UpdateAsync(id, request, ct);

        if (transaction is null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var deleted = await service.DeleteAsync(id, ct);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

}
