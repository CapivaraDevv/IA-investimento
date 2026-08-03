using FinanceAdvisor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceAdvisor.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<MonthlyIncome> MonthlyIncomes => Set<MonthlyIncome>();
    public DbSet<FixedExpense> FixedExpenses => Set<FixedExpense>();
    public DbSet<FinancialGoal> FinancialGoals => Set<FinancialGoal>();
    public DbSet<MonthlySnapshot> MonthlySnapshots => Set<MonthlySnapshot>();
    public DbSet<InvestmentProfile> InvestmentProfiles => Set<InvestmentProfile>();
    public DbSet<InvestmentRecommendation> InvestmentRecommendations => Set<InvestmentRecommendation>();
    public DbSet<AssetAllocation> AssetAllocations => Set<AssetAllocation>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Salary).HasPrecision(18, 2);
        });

        modelBuilder.Entity<MonthlyIncome>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.HasOne(x => x.UserProfile).WithMany(x => x.Incomes).HasForeignKey(x => x.UserProfileId);
        });

        modelBuilder.Entity<FixedExpense>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(200);
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.HasOne(x => x.UserProfile).WithMany(x => x.FixedExpenses).HasForeignKey(x => x.UserProfileId);
        });

        modelBuilder.Entity<FinancialGoal>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(300);
            e.Property(x => x.TargetAmount).HasPrecision(18, 2);
            e.Property(x => x.CurrentAmount).HasPrecision(18, 2);
            e.Ignore(x => x.RemainingAmount);
            e.Ignore(x => x.ProgressPercentage);
            e.Ignore(x => x.MonthlyContributionNeeded);
            e.HasOne(x => x.UserProfile).WithMany(x => x.Goals).HasForeignKey(x => x.UserProfileId);
        });

        modelBuilder.Entity<MonthlySnapshot>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TotalIncome).HasPrecision(18, 2);
            e.Property(x => x.TotalFixedExpenses).HasPrecision(18, 2);
            e.Property(x => x.AvailableToInvest).HasPrecision(18, 2);
            e.Property(x => x.ActualInvested).HasPrecision(18, 2);
            e.Ignore(x => x.SurplusOrDeficit);
            e.HasIndex(x => new { x.UserProfileId, x.Year, x.Month }).IsUnique();
            e.HasOne(x => x.UserProfile).WithMany(x => x.Snapshots).HasForeignKey(x => x.UserProfileId);
        });

        modelBuilder.Entity<InvestmentProfile>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId).IsUnique();
            e.HasOne(x => x.User).WithOne(x => x.InvestmentProfile).HasForeignKey<InvestmentProfile>(x => x.UserId);
        });

        modelBuilder.Entity<InvestmentRecommendation>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TotalIncome).HasPrecision(18, 2);
            e.Property(x => x.TotalExpenses).HasPrecision(18, 2);
            e.Property(x => x.Surplus).HasPrecision(18, 2);
            e.Property(x => x.AmountToInvest).HasPrecision(18, 2);
            e.Property(x => x.Priority).HasMaxLength(500);
            e.HasOne(x => x.User).WithMany(x => x.Recommendations).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<AssetAllocation>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Asset).HasMaxLength(100).IsRequired();
            e.Property(x => x.Percentage).HasPrecision(5, 2);
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.Property(x => x.Reason).HasMaxLength(300);
            e.HasOne(x => x.Recommendation).WithMany(x => x.Allocations).HasForeignKey(x => x.RecommendationId);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Description).HasMaxLength(200).IsRequired();

            e.Property(x => x.Amount).HasPrecision(18,2);

            e.Property(x => x.Category).HasConversion<int>();

            e.Property(x => x.Type).HasConversion<int>();

            e.HasOne(x => x.UserProfile).WithMany(x => x.Transactions).HasForeignKey(x => x.UserProfileId);
        }
        );
    }
}
