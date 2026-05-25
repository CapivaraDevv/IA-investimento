using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class InvestmentRecommendationRepository(AppDbContext db) : IInvestmentRecommendationRepository
{
    public Task<InvestmentRecommendation?> GetLatestByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        db.InvestmentRecommendations
            .Include(x => x.Allocations)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.GeneratedAt)
            .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<InvestmentRecommendation>> GetHistoryByUserIdAsync(Guid userId, int limit = 6, CancellationToken ct = default) =>
        await db.InvestmentRecommendations
            .Include(x => x.Allocations)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.GeneratedAt)
            .Take(limit)
            .ToListAsync(ct);

    public async Task AddAsync(InvestmentRecommendation recommendation, CancellationToken ct = default)
    {
        db.InvestmentRecommendations.Add(recommendation);
        await db.SaveChangesAsync(ct);
    }
}
