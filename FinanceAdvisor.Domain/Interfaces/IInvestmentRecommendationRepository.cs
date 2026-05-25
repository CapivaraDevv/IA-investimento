using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IInvestmentRecommendationRepository
{
    Task<InvestmentRecommendation?> GetLatestByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<InvestmentRecommendation>> GetHistoryByUserIdAsync(Guid userId, int limit = 6, CancellationToken ct = default);
    Task AddAsync(InvestmentRecommendation recommendation, CancellationToken ct = default);
}
