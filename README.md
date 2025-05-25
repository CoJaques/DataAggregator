# DataAggregator

## Structure
- **DataAggregator.Shared** - Models et DTOs communs
- **DataAggregator.Collector** - Application de collecte de données
- **DataAggregator.Registration** - Service d'enregistrement des devices

## Démarrage rapide
```bash
# Lancer avec Docker
docker-compose up

# Ou individuellement
dotnet run --project src/DataAggregator.Registration
dotnet run --project src/DataAggregator.Collector
```

## URLs
- Collector: http://localhost:5000
- Registration: http://localhost:5001
- InfluxDB: http://localhost:8086
- PostgreSQL: localhost:5432
