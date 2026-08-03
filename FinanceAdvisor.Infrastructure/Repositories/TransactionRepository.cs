using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Transactions.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        await db.Transactions.Where(x => x.UserProfileId == userId).ToListAsync(ct);

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
    {
        db.Transactions.Add(transaction);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken ct = default)
    {
        db.Transactions.Update(transaction);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var transaction = await db.Transactions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (transaction is not null)
        {
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync(ct);
        }
    }
}
