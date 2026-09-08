using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class FinancialGoal
{
    public Guid Id { get; private set; }
    public Guid UserProfileId { get; private set; }
    public GoalType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal TargetAmount { get; private set; }
    public decimal CurrentAmount { get; private set; }
    public int DeadlineMonths { get; private set; }
    public GoalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserProfile UserProfile { get; set; } = null!;

    public decimal RemainingAmount => TargetAmount - CurrentAmount;
    public double ProgressPercentage => TargetAmount == 0 ? 0 : (double)(CurrentAmount / TargetAmount) * 100;
    public decimal MonthlyContributionNeeded => DeadlineMonths == 0 ? RemainingAmount : RemainingAmount / DeadlineMonths;

    private FinancialGoal() { }


    public FinancialGoal(
        Guid userProfileId,
        GoalType type,
        string description,
        decimal targetAmount,
        int deadlineMonths)
    {
        if (targetAmount <= 0)
            throw new ArgumentException("Amount must be greater than zero");
        if (deadlineMonths <= 0)
            throw new ArgumentException("DeadLine must be greater than zero");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");
        if (UserProfileId == Guid.Empty)
            throw new ArgumentException("User profile is required");


        Id = Guid.NewGuid();
        UserProfileId = userProfileId;
        Type = type;
        Description = description;
        TargetAmount = targetAmount;
        CurrentAmount = 0;
        DeadlineMonths = deadlineMonths;
        Status = GoalStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddProgress(decimal amount)
{
    if (amount <= 0)
        throw new ArgumentException("Amount must be greater than zero");

    if (Status == GoalStatus.Completed)
        throw new InvalidOperationException("Completed goals cannot receive contributions");

    CurrentAmount += amount;


    if (CurrentAmount >= TargetAmount)
    {
        CurrentAmount = TargetAmount;
        Status = GoalStatus.Completed;
    }
}
}
