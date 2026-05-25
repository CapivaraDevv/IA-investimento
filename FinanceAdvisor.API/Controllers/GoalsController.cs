using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/goals")]
public class GoalsController(FinancialGoalService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateGoalRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetByUser), new { userId = request.UserId }, result);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct) =>
        Ok(await service.GetByUserAsync(userId, ct));

    [HttpPatch("{goalId:guid}/progress")]
    public async Task<IActionResult> UpdateProgress(Guid goalId, UpdateGoalProgressRequest request, CancellationToken ct)
    {
        var result = await service.UpdateProgressAsync(goalId, request, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
