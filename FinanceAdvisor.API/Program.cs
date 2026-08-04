using FinanceAdvisor.Application.Services;
using FinanceAdvisor.Domain.Interfaces;
using FinanceAdvisor.Infrastructure.Data;
using FinanceAdvisor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
builder.Services.AddScoped<IMonthlySnapshotRepository, MonthlySnapshotRepository>();
builder.Services.AddScoped<IInvestmentProfileRepository, InvestmentProfileRepository>();
builder.Services.AddScoped<IInvestmentRecommendationRepository, InvestmentRecommendationRepository>();
builder.Services.AddScoped<IFixedExpenseRepository, FixedExpenseRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<TransactionService>();


// Services
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<FinancialGoalService>();
builder.Services.AddScoped<InvestmentProfileService>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<SimulationService>();
builder.Services.AddScoped<ExpenseService>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
