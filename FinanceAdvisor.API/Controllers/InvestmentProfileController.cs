using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAdvisor.API.Controllers;

[ApiController]
[Route("api/investment-profile")]
public class InvestmentProfileController(InvestmentProfileService service) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Upsert(UpsertInvestmentProfileRequest request, CancellationToken ct) =>
        Ok(await service.UpsertAsync(request, ct));

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
    {
        var result = await service.GetByUserIdAsync(userId, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
