namespace FinanceAdvisor.Application.DTOs;

public record AssetAllocationResponse(
    string Asset,
    decimal Amount,
    decimal Percentage,
    string Reason
);

public record RecommendationResponse(
    Guid RecommendationId,
    decimal TotalIncome,
    decimal TotalFixedExpenses,
    decimal Surplus,
    decimal AmountToInvest,
    decimal EmergencyFundTarget,
    decimal EmergencyFundCurrent,
    bool EmergencyFundComplete,
    string Priority,
    IReadOnlyList<AssetAllocationResponse> Allocations,
    IReadOnlyList<string> Insights
);
