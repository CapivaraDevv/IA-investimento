using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Application.DTOs;

public record CreateTransactionRequest(
    decimal Amount,
    string Description,
    TransactionType Type,
    TransactionCategory Category
);

public record UpdateTransactionRequest(
    string Description,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime Date
);

public record TransactionResponse
(
    Guid Id,
    string Description,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt
);