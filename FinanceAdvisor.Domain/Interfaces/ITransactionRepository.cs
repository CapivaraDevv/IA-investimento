using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<Transaction>> GetByUserIdAsync(
        Guid userProfileId,
        CancellationToken ct = default);
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    Task UpdateAsync(Transaction transaction, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}