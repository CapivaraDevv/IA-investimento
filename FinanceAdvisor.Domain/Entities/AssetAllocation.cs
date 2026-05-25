namespace FinanceAdvisor.Domain.Entities;

public class AssetAllocation
{
    public Guid Id { get; set; }
    public Guid RecommendationId { get; set; }
    public string Asset { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;

    public InvestmentRecommendation Recommendation { get; set; } = null!;
}
