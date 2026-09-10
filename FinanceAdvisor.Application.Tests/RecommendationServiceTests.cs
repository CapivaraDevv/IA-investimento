using FinanceAdvisor.Application.Services;
using FinanceAdvisor.Domain.Entities;
using FinanceAdvisor.Domain.Enums;
using FinanceAdvisor.Domain.Interfaces;
using Moq;

namespace FinanceAdvisor.Application.Tests;

public class RecommendationServiceTests
{

    [Fact]
    public async Task GenerateAsync_WithEssentialExpenses_CalculatesSixMonthsEmergencyFund()
    {
        var userId = Guid.NewGuid();

        var profile = new UserProfile 
        {
            Id = userId,
            Salary = 5000m,
            FixedExpenses = new List<FixedExpense> 
            {
                new() 
                {
                    Category = ExpenseCategory.Housing,
                    Amount = 1000m, 
                    IsActive = true
                },
                new()
                {
                    Category = ExpenseCategory.Food,
                    Amount = 1000m,
                    IsActive = true
                },
                new()
                {
                    Category = ExpenseCategory.Leisure,
                    Amount = 500m,
                    IsActive = true
                },
            }
        };

        var profileRepository = new Mock<IUserProfileRepository>();

        profileRepository
            .Setup(repository => repository.GetByIdWithDetailsAsync(
                userId,
                It.IsAny<CancellationToken>())).ReturnsAsync(profile);

        var investmentProfileRepository = new Mock<IInvestmentProfileRepository>();

        investmentProfileRepository
            .Setup(repository => repository.GetByUserIdAsync(
                userId,
                It.IsAny<CancellationToken>())).ReturnsAsync((InvestmentProfile?)null); 
        
        var recommendationRepository = new Mock<IInvestmentRecommendationRepository>();

        recommendationRepository
            .Setup(repository => repository.AddAsync(
                It.IsAny<InvestmentRecommendation>(),
                It.IsAny<CancellationToken>()
            )).Returns(Task.CompletedTask);

        var service = new RecommendationService(
            profileRepository.Object,
            investmentProfileRepository.Object,
            recommendationRepository.Object);

        var result = await service.GenerateAsync(userId); 

        Assert.Equal(12000m, result.EmergencyFundTarget); 

    }
}