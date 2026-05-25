using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class InvestmentProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public RiskTolerance RiskTolerance { get; set; }
    public InvestmentKnowledgeLevel KnowledgeLevel { get; set; }
    public int InvestmentHorizonMonths { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserProfile User { get; set; } = null!;
}
