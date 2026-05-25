using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Repositories;

public class UserProfileRepository(AppDbContext db) : IUserProfileRepository
{
    public Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.UserProfiles.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<UserProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default) =>
        db.UserProfiles
            .Include(x => x.Incomes)
            .Include(x => x.FixedExpenses)
            .Include(x => x.Goals)
            .Include(x => x.InvestmentProfile)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken ct = default) =>
        await db.UserProfiles.ToListAsync(ct);

    public async Task AddAsync(UserProfile profile, CancellationToken ct = default)
    {
        db.UserProfiles.Add(profile);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(UserProfile profile, CancellationToken ct = default)
    {
        db.UserProfiles.Update(profile);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await db.UserProfiles.FindAsync([id], ct);
        if (profile is not null)
        {
            db.UserProfiles.Remove(profile);
            await db.SaveChangesAsync(ct);
        }
    }
}
