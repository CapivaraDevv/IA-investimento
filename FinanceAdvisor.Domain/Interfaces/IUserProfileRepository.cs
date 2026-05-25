using FinanceAdvisor.Domain.Entities;

namespace FinanceAdvisor.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<UserProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(UserProfile profile, CancellationToken ct = default);
    Task UpdateAsync(UserProfile profile, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
