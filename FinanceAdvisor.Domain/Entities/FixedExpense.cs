using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class FixedExpense
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsActive { get; set; } = true;

    public UserProfile UserProfile { get; set; } = null!;
}
