FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 7290

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/Adoroid.CarService.API/Adoroid.CarService.API.csproj Adoroid.CarService.API/
COPY src/Adoroid.CarService.Application/Adoroid.CarService.Application.csproj Adoroid.CarService.Application/
COPY src/Adoroid.CarService.Persistence/Adoroid.CarService.Persistence.csproj Adoroid.CarService.Persistence/
COPY src/Adoroid.CarService.Domain/Adoroid.CarService.Domain.csproj Adoroid.CarService.Domain/
COPY src/Adoroid.CarService.Shared/Adoroid.CarService.Shared.csproj Adoroid.CarService.Shared/
COPY src/Adoroid.CarService.Infrastructure/Adoroid.CarService.Infrastructure.csproj Adoroid.CarService.Infrastructure/

RUN dotnet restore Adoroid.CarService.API/Adoroid.CarService.API.csproj
COPY src/ .
WORKDIR /src/Adoroid.CarService.API
RUN dotnet build Adoroid.CarService.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Adoroid.CarService.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN apt-get update && apt-get install -y netcat-openbsd dos2unix && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .
COPY wait-for-postgres.sh /app/wait-for-postgres.sh
RUN dos2unix /app/wait-for-postgres.sh && chmod +x /app/wait-for-postgres.sh

RUN adduser --disabled-password --gecos '' appuser
USER appuser

ENTRYPOINT ["/app/wait-for-postgres.sh", "postgres-db:5432", "--", "dotnet", "Adoroid.CarService.API.dll"]
