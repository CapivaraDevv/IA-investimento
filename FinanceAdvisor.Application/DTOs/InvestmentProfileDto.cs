using FinanceAdvisor.Domain.Enums;

namespace FinanceAdvisor.Application.DTOs;

public record UpsertInvestmentProfileRequest(
    Guid UserId,
    RiskTolerance RiskTolerance,
    InvestmentKnowledgeLevel KnowledgeLevel,
    int InvestmentHorizonMonths
);

public record InvestmentProfileResponse(
    Guid UserId,
    RiskTolerance RiskTolerance,
    InvestmentKnowledgeLevel KnowledgeLevel,
    int InvestmentHorizonMonths,
    DateTime UpdatedAt
);
