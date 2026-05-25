using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class MonthlyIncome
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public IncomeType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
}
