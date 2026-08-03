using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Application.DTOs;

public record CreateTransactionRequest(
    decimal Amount,
    string Description,
    TransactionType Type,
    TransactionCategory Category
);

public record TransactionResponse
(
    Guid Id,
    decimal Amount,
    string Description,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt
);