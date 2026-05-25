using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class UserProfileService(IUserProfileRepository repo)
{
    public async Task<UserProfileResponse> CreateAsync(CreateUserProfileRequest request, CancellationToken ct = default)
    {
        var profile = new UserProfile
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Salary = request.Salary,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(profile, ct);
        return MapToResponse(profile);
    }

    public async Task<UserProfileResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repo.GetByIdAsync(id, ct);
        return profile is null ? null : MapToResponse(profile);
    }

    public async Task<IEnumerable<UserProfileResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var profiles = await repo.GetAllAsync(ct);
        return profiles.Select(MapToResponse);
    }

    public async Task<UserProfileResponse?> UpdateAsync(Guid id, UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        var profile = await repo.GetByIdAsync(id, ct);
        if (profile is null) return null;

        profile.Name = request.Name;
        profile.Salary = request.Salary;
        profile.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(profile, ct);
        return MapToResponse(profile);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var profile = await repo.GetByIdAsync(id, ct);
        if (profile is null) return false;
        await repo.DeleteAsync(id, ct);
        return true;
    }

    private static UserProfileResponse MapToResponse(UserProfile p) =>
        new(p.Id, p.Name, p.Salary, p.CreatedAt);
}
