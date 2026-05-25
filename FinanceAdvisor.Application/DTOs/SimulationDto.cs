namespace FinanceAdvisor.Application.DTOs;

public record SimulationRequest(
    decimal InitialAmount,
    decimal MonthlyContribution,
    double AnnualRatePercent,
    int Months
);

public record SimulationResponse(
    decimal InitialAmount,
    decimal Invested,
    decimal Interest,
    decimal Total,
    IReadOnlyList<SimulationPoint> Points
);

public record SimulationPoint(
    int Month,
    decimal TotalInvested,
    decimal TotalWithInterest
);
