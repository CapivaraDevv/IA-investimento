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

    public async Task<TransactionResponse?> GetByIdAsync(Guid transactionId, CancellationToken ct = default)
    {
        var transaction = await repo.GetByIdAsync(transactionId, ct);
        return transaction is null ? null : MapToResponse(transaction);
    }

    public async Task DeleteAsync(Guid transactionId, CancellationToken ct = default)
    {
        await repo.DeleteAsync(transactionId, ct);
    }

    private static TransactionResponse MapToResponse(Transaction t) =>
        new(t.Id, t.Description, t.Amount, t.Type, t.Category, t.Date);
}
