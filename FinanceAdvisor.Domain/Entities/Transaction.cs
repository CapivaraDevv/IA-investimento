using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;

    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; } = null!;

    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    public TransactionCategory Category { get; private set; }
    
    public DateTime Date { get; private set; }

    private Transaction() { }

    public Transaction(
        Guid userProfileId,
        string description,
        decimal amount,
        TransactionType type,
        TransactionCategory category,
        DateTime date)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");
        if (amount<=0)
            throw new ArgumentException("Amount must be greater than zero");
            
        Id = Guid.NewGuid();
        UserProfileId = userProfileId;
        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
    }
}