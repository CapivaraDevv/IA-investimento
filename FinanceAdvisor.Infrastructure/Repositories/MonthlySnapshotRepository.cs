using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class MonthlySnapshotRepository(AppDbContext db) : IMonthlySnapshotRepository
{
    public Task<MonthlySnapshot?> GetAsync(Guid userId, int month, int year, CancellationToken ct = default) =>
        db.MonthlySnapshots.FirstOrDefaultAsync(x => x.UserProfileId == userId && x.Month == month && x.Year == year, ct);

    public async Task<IEnumerable<MonthlySnapshot>> GetByUserIdAsync(Guid userId, int limit = 12, CancellationToken ct = default) =>
        await db.MonthlySnapshots
            .Where(x => x.UserProfileId == userId)
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .Take(limit)
            .ToListAsync(ct);

    public async Task UpsertAsync(MonthlySnapshot snapshot, CancellationToken ct = default)
    {
        var existing = await GetAsync(snapshot.UserProfileId, snapshot.Month, snapshot.Year, ct);
        if (existing is null)
            db.MonthlySnapshots.Add(snapshot);
        else
        {
            existing.TotalIncome = snapshot.TotalIncome;
            existing.TotalFixedExpenses = snapshot.TotalFixedExpenses;
            existing.AvailableToInvest = snapshot.AvailableToInvest;
            existing.ActualInvested = snapshot.ActualInvested;
            db.MonthlySnapshots.Update(existing);
        }

        await db.SaveChangesAsync(ct);
    }
}
