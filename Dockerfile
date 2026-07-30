FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Ensure NuGet packages folder is inside the container (avoid host Windows fallback paths)
ENV NUGET_PACKAGES=/root/.nuget/packages
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

COPY NuGet.Config .

COPY FinanceAdvisor.Domain/FinanceAdvisor.Domain.csproj               FinanceAdvisor.Domain/
COPY FinanceAdvisor.Application/FinanceAdvisor.Application.csproj     FinanceAdvisor.Application/
COPY FinanceAdvisor.Infrastructure/FinanceAdvisor.Infrastructure.csproj FinanceAdvisor.Infrastructure/
COPY FinanceAdvisor.API/FinanceAdvisor.API.csproj                     FinanceAdvisor.API/

RUN dotnet restore FinanceAdvisor.API/FinanceAdvisor.API.csproj

COPY . .
RUN dotnet publish FinanceAdvisor.API/FinanceAdvisor.API.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinanceAdvisor.API.dll"]
