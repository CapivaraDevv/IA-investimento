using FinanceAdvisor.Application.DTOs;

namespace FinanceAdvisor.Application.Services;

public class SimulationService
{
    // FV = P*(1+r)^n + PMT * ((1+r)^n - 1) / r
    // P   = initial amount
    // PMT = monthly contribution
    // r   = monthly rate
    // n   = months
    public SimulationResponse Simulate(SimulationRequest request)
    {
        var monthlyRate = Math.Pow(1 + request.AnnualRatePercent / 100.0, 1.0 / 12) - 1;
        var points = new List<SimulationPoint>();

        decimal totalInvested = request.InitialAmount;
        decimal totalWithInterest = request.InitialAmount;

        for (int month = 1; month <= request.Months; month++)
        {
            totalInvested += request.MonthlyContribution;
            totalWithInterest = (totalWithInterest + request.MonthlyContribution) * (decimal)(1 + monthlyRate);
            points.Add(new SimulationPoint(month, totalInvested, Math.Round(totalWithInterest, 2)));
        }

        var finalAmount = Math.Round(totalWithInterest, 2);
        var interest = finalAmount - totalInvested;

        return new SimulationResponse(
            InitialAmount: request.InitialAmount,
            Invested: totalInvested,
            Interest: Math.Round(interest, 2),
            Total: finalAmount,
            Points: points
        );
    }
}
