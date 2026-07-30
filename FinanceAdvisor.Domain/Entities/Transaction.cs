using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }

    private Transaction() { }

    public Transaction(
        string description,
        decimal amount,
        TransactionType type,
        string category,
        DateTime date)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");
        if (amount<=0)
            throw new ArgumentException("Amount must be greater than zero");
            
        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
    }
}