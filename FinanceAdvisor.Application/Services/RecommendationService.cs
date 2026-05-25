using FinanceAdvisor.Application.DTOs;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Enums;
using FinanceAdvisor.Domain.Interfaces;

namespace FinanceAdvisor.Application.Services;

public class RecommendationService(
    IUserProfileRepository profileRepo,
    IInvestmentProfileRepository investmentProfileRepo,
    IInvestmentRecommendationRepository recommendationRepo)
{
    private const decimal InvestmentRate = 0.20m;
    private const decimal EmergencyFundMonths = 6m;

    public async Task<RecommendationResponse> GenerateAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await profileRepo.GetByIdWithDetailsAsync(userId, ct)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        var investmentProfile = await investmentProfileRepo.GetByUserIdAsync(userId, ct);

        var totalIncome = profile.Incomes
            .Where(i => i.Month == DateTime.UtcNow.Month && i.Year == DateTime.UtcNow.Year)
            .Sum(i => i.Amount);

        if (totalIncome == 0) totalIncome = profile.Salary;

        var totalExpenses = profile.FixedExpenses.Where(e => e.IsActive).Sum(e => e.Amount);
        var surplus = totalIncome - totalExpenses;
        var amountToInvest = surplus > 0 ? surplus * InvestmentRate : 0m;

        var emergencyGoal = profile.Goals
            .FirstOrDefault(g => g.Type == GoalType.EmergencyFund && g.Status == GoalStatus.Active);

        var emergencyTarget = totalIncome * EmergencyFundMonths;
        var emergencyCurrent = emergencyGoal?.CurrentAmount ?? 0m;
        var emergencyComplete = emergencyCurrent >= emergencyTarget;

        var risk = investmentProfile?.RiskTolerance ?? RiskTolerance.Low;
        var knowledge = investmentProfile?.KnowledgeLevel ?? InvestmentKnowledgeLevel.Beginner;

        var allocations = BuildAllocations(amountToInvest, risk, emergencyComplete);
        var insights = BuildInsights(surplus, amountToInvest, emergencyComplete, emergencyCurrent, emergencyTarget, risk, knowledge);

        var priority = emergencyComplete
            ? "Diversificar investimentos conforme perfil de risco"
            : $"Construir reserva de emergência (faltam R${emergencyTarget - emergencyCurrent:N2})";

        var recommendation = new InvestmentRecommendation
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Surplus = surplus,
            AmountToInvest = amountToInvest,
            Priority = priority,
            EmergencyFundComplete = emergencyComplete,
            GeneratedAt = DateTime.UtcNow,
            Allocations = allocations.Select(a => new AssetAllocation
            {
                Id = Guid.NewGuid(),
                Asset = a.Asset,
                Percentage = a.Percentage,
                Amount = a.Amount,
                Reason = a.Reason
            }).ToList()
        };

        await recommendationRepo.AddAsync(recommendation, ct);

        return new RecommendationResponse(
            RecommendationId: recommendation.Id,
            TotalIncome: totalIncome,
            TotalFixedExpenses: totalExpenses,
            Surplus: surplus,
            AmountToInvest: amountToInvest,
            EmergencyFundTarget: emergencyTarget,
            EmergencyFundCurrent: emergencyCurrent,
            EmergencyFundComplete: emergencyComplete,
            Priority: priority,
            Allocations: allocations,
            Insights: insights
        );
    }

    public async Task<NextStepResponse> GetNextStepAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await profileRepo.GetByIdWithDetailsAsync(userId, ct)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        var investmentProfile = await investmentProfileRepo.GetByUserIdAsync(userId, ct);

        var totalIncome = profile.Salary;
        var emergencyGoal = profile.Goals
            .FirstOrDefault(g => g.Type == GoalType.EmergencyFund && g.Status == GoalStatus.Active);

        var emergencyTarget = totalIncome * EmergencyFundMonths;
        var emergencyCurrent = emergencyGoal?.CurrentAmount ?? 0m;
        var emergencyComplete = emergencyCurrent >= emergencyTarget;

        if (!emergencyComplete)
        {
            var remaining = emergencyTarget - emergencyCurrent;
            var monthlyInvest = totalIncome * InvestmentRate;
            var monthsToComplete = monthlyInvest > 0
                ? (int)Math.Ceiling((double)(remaining / monthlyInvest))
                : 36;

            return new NextStepResponse(
                Message: "Sua reserva de emergência ainda está incompleta.",
                Action: "Direcionar 100% dos investimentos para CDB Liquidez Diária",
                SuggestedDate: DateTime.UtcNow.AddMonths(monthsToComplete)
            );
        }

        var urgentGoal = profile.Goals
            .Where(g => g.Status == GoalStatus.Active && g.Type != GoalType.EmergencyFund && g.DeadlineMonths <= 6)
            .OrderBy(g => g.DeadlineMonths)
            .FirstOrDefault();

        if (urgentGoal is not null)
        {
            return new NextStepResponse(
                Message: $"Objetivo '{urgentGoal.Description}' próximo do prazo ({urgentGoal.DeadlineMonths} meses).",
                Action: $"Aumentar aporte para R${urgentGoal.MonthlyContributionNeeded:N2}/mês e priorizar renda fixa de curto prazo",
                SuggestedDate: DateTime.UtcNow.AddMonths(urgentGoal.DeadlineMonths)
            );
        }

        var horizonMonths = investmentProfile?.InvestmentHorizonMonths ?? 12;
        var action = (investmentProfile?.RiskTolerance ?? RiskTolerance.Low) switch
        {
            RiskTolerance.Low => "Mantenha alocação em Tesouro Selic e CDB. Revise taxas a cada 3 meses.",
            RiskTolerance.Medium => "Diversifique com ETFs (IVVB11/BOVA11). Aporte mensal consistente supera timing de mercado.",
            RiskTolerance.High => "Com reserva completa e horizonte longo, considere aumentar exposição a ações e ETFs internacionais.",
            _ => "Mantenha os aportes regulares."
        };

        return new NextStepResponse(
            Message: "Reserva de emergência completa. Foco em crescimento patrimonial.",
            Action: action,
            SuggestedDate: DateTime.UtcNow.AddMonths(3)
        );
    }

    private static IReadOnlyList<AssetAllocationResponse> BuildAllocations(
        decimal total, RiskTolerance risk, bool emergencyComplete)
    {
        if (total <= 0) return [];

        if (!emergencyComplete)
            return [new("CDB Liquidez Diária", total, 100m, "Prioridade: reserva de emergência ainda incompleta")];

        return risk switch
        {
            RiskTolerance.Low => [
                new("Tesouro Selic / CDB", Pct(total, 0.70m), 70m, "Segurança e liquidez garantida"),
                new("LCI/LCA", Pct(total, 0.30m), 30m, "Isenção de IR com boa rentabilidade")
            ],
            RiskTolerance.Medium => [
                new("Renda Fixa (CDB/Tesouro)", Pct(total, 0.50m), 50m, "Base estável da carteira"),
                new("ETF (BOVA11/IVVB11)", Pct(total, 0.30m), 30m, "Exposição à bolsa com diversificação automática"),
                new("FIIs", Pct(total, 0.20m), 20m, "Renda passiva mensal isenta de IR para PF")
            ],
            RiskTolerance.High => [
                new("Renda Fixa", Pct(total, 0.30m), 30m, "Âncora de segurança da carteira"),
                new("ETF (BOVA11/IVVB11)", Pct(total, 0.30m), 30m, "Crescimento de longo prazo diversificado"),
                new("Ações", Pct(total, 0.20m), 20m, "Maior potencial de retorno com seleção ativa"),
                new("Bitcoin", Pct(total, 0.20m), 20m, "Exposição a ativo alternativo de alto risco/retorno")
            ],
            _ => []
        };
    }

    private static IReadOnlyList<string> BuildInsights(
        decimal surplus, decimal toInvest, bool emergencyComplete,
        decimal emergencyCurrent, decimal emergencyTarget,
        RiskTolerance risk, InvestmentKnowledgeLevel knowledge)
    {
        var insights = new List<string>();

        if (surplus <= 0)
        {
            insights.Add("Suas despesas fixas superam a renda. Priorize cortar gastos antes de investir.");
            return insights;
        }

        var investRate = (double)(toInvest / surplus) * 100;
        insights.Add($"Você está destinando {investRate:F0}% do excedente para investimentos (meta recomendada: 20%).");

        if (!emergencyComplete)
        {
            var months = toInvest > 0
                ? Math.Ceiling((double)((emergencyTarget - emergencyCurrent) / toInvest))
                : 0;
            insights.Add($"Reserva de emergência completa em ~{months} meses no ritmo atual.");
        }
        else
        {
            insights.Add("Reserva de emergência completa. Você está pronto para diversificar.");
        }

        if (knowledge == InvestmentKnowledgeLevel.Beginner)
            insights.Add("Como iniciante, prefira ETFs a ações individuais: menos risco e sem necessidade de analisar empresas.");

        if (risk == RiskTolerance.High && knowledge == InvestmentKnowledgeLevel.Beginner)
            insights.Add("Atenção: seu perfil agressivo com conhecimento iniciante pode gerar perdas inesperadas. Considere estudar antes de aumentar risco.");

        return insights;
    }

    private static decimal Pct(decimal total, decimal pct) => Math.Round(total * pct, 2);
}
