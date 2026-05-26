FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY FinanceAdvisor.Domain/FinanceAdvisor.Domain.csproj               FinanceAdvisor.Domain/
COPY FinanceAdvisor.Application/FinanceAdvisor.Application.csproj     FinanceAdvisor.Application/
COPY FinanceAdvisor.Infrastructure/FinanceAdvisor.Infrastructure.csproj FinanceAdvisor.Infrastructure/
COPY FinanceAdvisor.API/FinanceAdvisor.API.csproj                     FinanceAdvisor.API/

RUN dotnet restore FinanceAdvisor.API/FinanceAdvisor.API.csproj

COPY . .
RUN dotnet publish FinanceAdvisor.API/FinanceAdvisor.API.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceAdvisor.API.dll"]
