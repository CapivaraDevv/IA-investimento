namespace FinanceAdvisor.Application.DTOs;

public record NextStepResponse(
    string Message,
    string Action,
    DateTime SuggestedDate
);
