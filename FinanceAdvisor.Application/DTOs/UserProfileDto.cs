namespace FinanceAdvisor.Application.DTOs;

public record CreateUserProfileRequest(
    string Name,
    decimal Salary
);

public record UpdateUserProfileRequest(
    string Name,
    decimal Salary
);

public record UserProfileResponse(
    Guid Id,
    string Name,
    decimal Salary,
    DateTime CreatedAt
);
