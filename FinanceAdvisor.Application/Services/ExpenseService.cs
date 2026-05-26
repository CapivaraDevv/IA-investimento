using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class ExpenseService(IFixedExpenseRepository repo)
{
    public async Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request, CancellationToken ct = default)
    {
        var expense = new FixedExpense
        {
            Id = Guid.NewGuid(),
            UserProfileId = request.UserId,
            Category = request.Category,
            Description = request.Description,
            Amount = request.Amount
        };

        await repo.AddAsync(expense, ct);
        return MapToResponse(expense);
    }

    public async Task<IEnumerable<ExpenseResponse>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var expenses = await repo.GetByUserIdAsync(userId, ct);
         return expenses.Select(MapToResponse);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repo.GetByIdAsync(id, ct);
        if (profile is null) return false;
        await repo.DeleteAsync(id, ct);
        return true;
    }

    private static ExpenseResponse MapToResponse(FixedExpense f) =>
        new(f.Id, f.Category, f.Description, f.Amount, f.IsActive);
    
}
