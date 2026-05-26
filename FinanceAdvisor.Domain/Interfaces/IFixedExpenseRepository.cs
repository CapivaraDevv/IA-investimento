using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IFixedExpenseRepository{

    Task<IEnumerable<FixedExpense>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(FixedExpense expense, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);


}