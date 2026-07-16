# Backend build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first to leverage Docker layer caching
COPY Warehouse.sln .
COPY Warehouse.Api/Warehouse.Api.csproj Warehouse.Api/
COPY Warehouse.Application/Warehouse.Application.csproj Warehouse.Application/
COPY Warehouse.Contracts/Warehouse.Contracts.csproj Warehouse.Contracts/
COPY Warehouse.Domain/Warehouse.Domain.csproj Warehouse.Domain/
COPY Warehouse.Infrastructure/Warehouse.Infrastructure.csproj Warehouse.Infrastructure/
COPY Warehouse.Tests/Warehouse.Tests.csproj Warehouse.Tests/

RUN dotnet restore Warehouse.sln

# Copy the rest of the source code and publish
# We do NOT use --no-restore here because local obj/ folders copied from Windows
# may contain stale project.assets.json with Windows paths.
COPY . .
RUN dotnet publish Warehouse.Api/Warehouse.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for the health check
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Run as the non-root app user provided by the base image
USER app

ENTRYPOINT ["dotnet", "Warehouse.Api.dll"]
