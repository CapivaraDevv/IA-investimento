using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IFinancialGoalRepository
{
    Task<FinancialGoal?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<FinancialGoal>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(FinancialGoal goal, CancellationToken ct = default);
    Task UpdateAsync(FinancialGoal goal, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
