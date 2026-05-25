using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class FinancialGoal
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public GoalType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public int DeadlineMonths { get; set; }
    public GoalStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserProfile UserProfile { get; set; } = null!;

    public decimal RemainingAmount => TargetAmount - CurrentAmount;
    public double ProgressPercentage => TargetAmount == 0 ? 0 : (double)(CurrentAmount / TargetAmount) * 100;
    public decimal MonthlyContributionNeeded => DeadlineMonths == 0 ? RemainingAmount : RemainingAmount / DeadlineMonths;
}
