using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class FinancialGoalRepository(AppDbContext db) : IFinancialGoalRepository
{
    public Task<FinancialGoal?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.FinancialGoals.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IEnumerable<FinancialGoal>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await db.FinancialGoals.Where(x => x.UserProfileId == userId).ToListAsync(ct);

    public async Task AddAsync(FinancialGoal goal, CancellationToken ct = default)
    {
        db.FinancialGoals.Add(goal);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(FinancialGoal goal, CancellationToken ct = default)
    {
        db.FinancialGoals.Update(goal);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var goal = await db.FinancialGoals.FindAsync([id], ct);
        if (goal is not null)
        {
            db.FinancialGoals.Remove(goal);
            await db.SaveChangesAsync(ct);
        }
    }
}
