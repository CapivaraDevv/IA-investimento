using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Application.DTOs;

public record CreateGoalRequest(
    Guid UserId,
    GoalType Type,
    string Description,
    decimal TargetAmount,
    int DeadlineMonths
);

public record UpdateGoalProgressRequest(
    decimal Amount
);

public record GoalResponse(
    Guid Id,
    GoalType Type,
    string Description,
    decimal TargetAmount,
    decimal CurrentAmount,
    decimal RemainingAmount,
    double ProgressPercentage,
    decimal MonthlyContributionNeeded,
    int DeadlineMonths,
    GoalStatus Status
);
