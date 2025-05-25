FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DataAggregator.Collector/DataAggregator.Collector.csproj", "src/DataAggregator.Collector/"]
COPY ["src/DataAggregator.Shared/DataAggregator.Shared.csproj", "src/DataAggregator.Shared/"]
RUN dotnet restore "src/DataAggregator.Collector/DataAggregator.Collector.csproj"
COPY . .
WORKDIR "/src/src/DataAggregator.Collector"
RUN dotnet build "DataAggregator.Collector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataAggregator.Collector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataAggregator.Collector.dll"]
