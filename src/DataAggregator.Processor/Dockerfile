FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/DataAggregator.Processor/DataAggregator.Processor.csproj", "src/DataAggregator.Processor/"]
COPY ["src/DataAggregator.Shared/DataAggregator.Shared.csproj", "src/DataAggregator.Shared/"]
COPY ["src/DataAggregator.Collector.Shared/DataAggregator.Collector.Shared.csproj", "src/DataAggregator.Collector.Shared/"]
RUN dotnet restore "src/DataAggregator.Processor/DataAggregator.Processor.csproj"
COPY . .
WORKDIR "/src/src/DataAggregator.Processor"
RUN dotnet build "DataAggregator.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataAggregator.Processor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create models directory for ONNX models
RUN mkdir -p /app/models

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "DataAggregator.Processor.dll"] 