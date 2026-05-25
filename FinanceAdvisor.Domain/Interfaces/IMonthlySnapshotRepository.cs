using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IMonthlySnapshotRepository
{
    Task<MonthlySnapshot?> GetAsync(Guid userId, int month, int year, CancellationToken ct = default);
    Task<IEnumerable<MonthlySnapshot>> GetByUserIdAsync(Guid userId, int limit = 12, CancellationToken ct = default);
    Task UpsertAsync(MonthlySnapshot snapshot, CancellationToken ct = default);
}
