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

    public async Task<IEnumerable<TransactionResponse>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var transactions = await repo.GetByUserIdAsync(userId, ct);
        return transactions.Select(MapToResponse);
    }

    public async Task<TransactionResponse?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var transaction = await repo.GetByIdAsync(id, ct);

        if(transaction is null) return null;

        return MapToResponse(transaction);
    }

    public async Task<TransactionResponse?> UpdateAsync(
        Guid id,
        UpdateTransactionRequest request,
        CancellationToken ct = default)
    {
        var transaction = await repo.GetByIdAsync(id, ct);

        if(transaction is null) return null;

        transaction.Update(
            request.Description,
            request.Amount,
            request.Type,
            request.Category,
            request.Date
        );

        await repo.UpdateAsync(transaction, ct);

        return MapToResponse(transaction);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var transaction = await repo.GetByIdAsync(id, ct);

        if(transaction is null) return false;

        await repo.DeleteAsync(id, ct);

        return true;
    }

    private static TransactionResponse MapToResponse(Transaction t) =>
        new(t.Id, t.Description, t.Amount, t.Type, t.Category, t.Date);
}
