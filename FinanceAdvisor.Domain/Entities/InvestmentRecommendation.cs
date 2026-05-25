namespace FinanceAdvisor.Domain.Entities;

public class InvestmentRecommendation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Surplus { get; set; }
    public decimal AmountToInvest { get; set; }
    public string Priority { get; set; } = string.Empty;
    public bool EmergencyFundComplete { get; set; }
    public DateTime GeneratedAt { get; set; }

    public UserProfile User { get; set; } = null!;
    public ICollection<AssetAllocation> Allocations { get; set; } = [];
}
