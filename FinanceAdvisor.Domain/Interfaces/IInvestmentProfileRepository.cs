using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IInvestmentProfileRepository
{
    Task<InvestmentProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task UpsertAsync(InvestmentProfile profile, CancellationToken ct = default);
}
