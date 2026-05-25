namespace FinanceAdvisor.Domain.Entities;

public class MonthlySnapshot
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalFixedExpenses { get; set; }
    public decimal AvailableToInvest { get; set; }
    public decimal ActualInvested { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserProfile UserProfile { get; set; } = null!;

    public decimal SurplusOrDeficit => AvailableToInvest - ActualInvested;
}
