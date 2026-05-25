using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Enums;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class FinancialGoalService(IFinancialGoalRepository repo)
{
    public async Task<GoalResponse> CreateAsync(CreateGoalRequest request, CancellationToken ct = default)
    {
        var goal = new FinancialGoal
        {
            Id = Guid.NewGuid(),
            UserProfileId = request.UserId,
            Type = request.Type,
            Description = request.Description,
            TargetAmount = request.TargetAmount,
            CurrentAmount = 0,
            DeadlineMonths = request.DeadlineMonths,
            Status = GoalStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(goal, ct);
        return MapToResponse(goal);
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

    private static GoalResponse MapToResponse(FinancialGoal g) =>
        new(g.Id, g.Type, g.Description, g.TargetAmount, g.CurrentAmount,
            g.RemainingAmount, g.ProgressPercentage, g.MonthlyContributionNeeded,
            g.DeadlineMonths, g.Status);
}
