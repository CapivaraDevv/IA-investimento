using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Application.DTOs;

public record CreateExpenseRequest(
    Guid UserId,
    ExpenseCategory Category,
    string Description,
    decimal Amount
);

public record ExpenseResponse(
    Guid Id,
    ExpenseCategory Category,
    string Description,
    decimal Amount,
    bool IsActive
);
