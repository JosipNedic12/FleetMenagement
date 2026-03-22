FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FleetManagement.API/FleetManagement.API.csproj", "FleetManagement.API/"]
COPY ["FleetManagement.Application/FleetManagement.Application.csproj", "FleetManagement.Application/"]
COPY ["FleetManagement.Domain/FleetManagement.Domain.csproj", "FleetManagement.Domain/"]
COPY ["FleetManagement.Infrastructure/FleetManagement.Infrastructure.csproj", "FleetManagement.Infrastructure/"]
RUN dotnet restore "FleetManagement.API/FleetManagement.API.csproj"
COPY . .
WORKDIR "/src/FleetManagement.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FleetManagement.API.dll"]
