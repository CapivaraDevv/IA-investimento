namespace FinanceAdvisor.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public InvestmentProfile? InvestmentProfile { get; set; }
    public ICollection<MonthlyIncome> Incomes { get; set; } = [];
    public ICollection<FixedExpense> FixedExpenses { get; set; } = [];
    public ICollection<FinancialGoal> Goals { get; set; } = [];
    public ICollection<MonthlySnapshot> Snapshots { get; set; } = [];
    public ICollection<InvestmentRecommendation> Recommendations { get; set; } = [];

    public ICollection<Transaction> Transactions { get; private set; } = [];
}
