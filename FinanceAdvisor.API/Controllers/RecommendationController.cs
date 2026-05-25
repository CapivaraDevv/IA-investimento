using FinanceAdvisor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/recommendations")]
public class RecommendationController(RecommendationService service) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> Generate(Guid userId, CancellationToken ct)
    {
        try
        {
            return Ok(await service.GenerateAsync(userId, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{userId:guid}/next-step")]
    public async Task<IActionResult> NextStep(Guid userId, CancellationToken ct)
    {
        try
        {
            return Ok(await service.GetNextStepAsync(userId, ct));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
