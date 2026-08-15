# Etapa 1: restauración y compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Cesardd.Api.slnx", "./"]
COPY ["Cesardd.Api/Cesardd.Api.csproj", "Cesardd.Api/"]
COPY ["Cesardd.Core/Cesardd.Core.csproj", "Cesardd.Core/"]
COPY ["Cesardd.Infrastructure/Cesardd.Infrastructure.csproj", "Cesardd.Infrastructure/"]
COPY ["Cesardd.Shared/Cesardd.Shared.csproj", "Cesardd.Shared/"]
RUN dotnet restore "Cesardd.Api.slnx"

COPY . .
RUN dotnet publish "Cesardd.Api/Cesardd.Api.csproj" -c Release -o /app/out --no-restore

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app

ENTRYPOINT ["dotnet", "Cesardd.Api.dll"]
