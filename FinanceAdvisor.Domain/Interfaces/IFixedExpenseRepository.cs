using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IFixedExpenseRepository
{
    Task<FixedExpense?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<FixedExpense>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(FixedExpense expense, CancellationToken ct = default);
    Task UpdateAsync(FixedExpense expense, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
