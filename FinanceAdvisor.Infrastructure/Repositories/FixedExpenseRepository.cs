using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class FixedExpenseRepository(AppDbContext db) : IFixedExpenseRepository {
    public async Task<IEnumerable<FixedExpense>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await db.FixedExpenses.Where(x => x.UserProfileId == userId).ToListAsync(ct);

    public async Task AddAsync(FixedExpense expense, CancellationToken ct = default)
    {
        db.FixedExpenses.Add(expense);
        await db.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var expense = await db.FixedExpenses.FindAsync([id], ct);
        if (expense is not null)
        {
            db.FixedExpenses.Remove(expense);
            await db.SaveChangesAsync(ct);
        }
    }
}