using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class InvestmentProfileService(IInvestmentProfileRepository repo)
{
    public async Task<InvestmentProfileResponse> UpsertAsync(UpsertInvestmentProfileRequest request, CancellationToken ct = default)
    {
        var profile = new InvestmentProfile
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RiskTolerance = request.RiskTolerance,
            KnowledgeLevel = request.KnowledgeLevel,
            InvestmentHorizonMonths = request.InvestmentHorizonMonths,
            UpdatedAt = DateTime.UtcNow
        };

        await repo.UpsertAsync(profile, ct);
        return MapToResponse(profile);
    }

    public async Task<InvestmentProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await repo.GetByUserIdAsync(userId, ct);
        return profile is null ? null : MapToResponse(profile);
    }

    private static InvestmentProfileResponse MapToResponse(InvestmentProfile p) =>
        new(p.UserId, p.RiskTolerance, p.KnowledgeLevel, p.InvestmentHorizonMonths, p.UpdatedAt);
}
