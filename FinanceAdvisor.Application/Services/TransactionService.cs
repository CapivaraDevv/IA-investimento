using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Enums;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class TransactionService(ITransactionRepository repo)
{
    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request, Guid userProfileId, CancellationToken ct = default)
    {
        var transaction = new Transaction
        (
            userProfileId,
            request.Description,
            request.Amount,
            request.Type,
            request.Category,
            DateTime.UtcNow
        );

        await repo.AddAsync(transaction, ct);

        return MapToResponse(transaction);
    }

    public async Task<IEnumerable<GoalResponse>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var goals = await repo.GetByUserIdAsync(userId, ct);
        return goals.Select(MapToResponse);
    }

    public async Task<GoalResponse?> UpdateProgressAsync(Guid goalId, UpdateGoalProgressRequest request, CancellationToken ct = default)
    {
        var goal = await repo.GetByIdAsync(goalId, ct);
        if (goal is null) return null;

        goal.CurrentAmount += request.Amount;

        if (goal.CurrentAmount >= goal.TargetAmount)
        {
            goal.CurrentAmount = goal.TargetAmount;
            goal.Status = GoalStatus.Completed;
        }

        await repo.UpdateAsync(goal, ct);
        return MapToResponse(goal);
    }

    private static TransactionResponse MapToResponse(Transaction t) =>
        new(t.Id, t.Description, t.Amount, t.Type, t.Category, t.Date);
}
