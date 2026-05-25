using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class InvestmentProfileRepository(AppDbContext db) : IInvestmentProfileRepository
{
    public Task<InvestmentProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        db.InvestmentProfiles.FirstOrDefaultAsync(x => x.UserId == userId, ct);

    public async Task UpsertAsync(InvestmentProfile profile, CancellationToken ct = default)
    {
        var existing = await GetByUserIdAsync(profile.UserId, ct);
        if (existing is null)
            db.InvestmentProfiles.Add(profile);
        else
        {
            existing.RiskTolerance = profile.RiskTolerance;
            existing.KnowledgeLevel = profile.KnowledgeLevel;
            existing.InvestmentHorizonMonths = profile.InvestmentHorizonMonths;
            existing.UpdatedAt = profile.UpdatedAt;
            db.InvestmentProfiles.Update(existing);
        }

        await db.SaveChangesAsync(ct);
    }
}
