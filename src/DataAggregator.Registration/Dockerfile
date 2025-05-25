FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DataAggregator.Registration/DataAggregator.Registration.csproj", "src/DataAggregator.Registration/"]
COPY ["src/DataAggregator.Shared/DataAggregator.Shared.csproj", "src/DataAggregator.Shared/"]
RUN dotnet restore "src/DataAggregator.Registration/DataAggregator.Registration.csproj"
COPY . .
WORKDIR "/src/src/DataAggregator.Registration"
RUN dotnet build "DataAggregator.Registration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataAggregator.Registration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataAggregator.Registration.dll"]
