(Simple README, generated with a LLM)

# DataAggregator.Processor

Prediction service for the DataAggregator project that enriches collected data with predictions based on ONNX models.

## Features

- **Real-time predictions** : Executes ONNX models to predict values based on sensor data
- **Per-machine configuration** : Each machine can have its own model and configuration
- **InfluxDB integration** : Stores predictions in the same database as raw data
- **Robust error handling** : Automatically stops predictions for a machine in case of error
- **Health checks** : Monitoring endpoints to verify service status
- **Connection optimization** : Reuses InfluxDB connections to avoid unnecessary reinitializations
- **Preprocessing strategies** : Strategy pattern for different data preparation methods

## Configuration

### appsettings.json

```json
{
  "PredictionService": {
    "RegistrationServiceUrl": "http://localhost:5001",
    "GlobalCycleIntervalSeconds": 30,
    "Machines": [
      {
        "MachineName": "OpenCN-Machine-001",
        "Enabled": true,
        "ModelPath": "/app/models/opencn-machine-001.onnx",
        "PreprocessingStrategy": "ActuatorCurrentFeatureExtractor",
        "InputSensors": ["V1", "V2", "V3", "V4", "V5", "V6"],
        "PredictionSensorName": "PredictedActuatorCurrent",
        "WindowSizeSeconds": 60,
        "CycleIntervalSeconds": 30
      }
    ]
  }
}
```

### Configuration parameters

- **MachineName** : Machine name (must match the one registered in the Registration Service)
- **Enabled** : Enables/disables predictions for this machine
- **ModelPath** : Path to the ONNX model file (in Docker volume `/app/models`)
- **PreprocessingStrategy** : Name of the preprocessing strategy to use
- **InputSensors** : List of input sensors required by the model
- **PredictionSensorName** : Name of the prediction sensor (will be stored in InfluxDB)
- **WindowSizeSeconds** : Data window size in seconds
- **CycleIntervalSeconds** : Prediction execution frequency in seconds

## Usage with Docker

### 1. Prepare ONNX models

Place your ONNX models in the `models/` folder at the project root:

```
models/
├── opencn-machine-001.onnx
├── machine-002.onnx
└── ...
```

### 2. Launch the service

```bash
# Launch all services
docker-compose up

# Launch only the prediction service
docker-compose up processor
```

### 3. Check service status

```bash
# Health check
curl http://localhost:5002/api/healthcheck

# Swagger UI (in development)
http://localhost:5002/swagger
```

## Architecture

### Main components

1. **PredictionBackgroundService** : Background service that manages predictions for all machines
2. **MachinePredictionProcessor** : Processes predictions for a specific machine
3. **OnnxPredictionEngine** : ONNX prediction engine with model caching
4. **InfluxV3Repository** : Interface with InfluxDB for reading/writing data
5. **RegistrationServiceClient** : Client to retrieve machine information
6. **PreprocessingStrategyFactory** : Factory to create preprocessing strategies
7. **ActuatorCurrentFeatureExtractor** : Preprocessing strategy for current sensors

### Data flow

1. The service retrieves machine configuration from `appsettings.json`
2. For each enabled machine, it retrieves its information from the Registration Service
3. It reads a data window from InfluxDB for input sensors
4. It prepares data using the configured preprocessing strategy
5. It executes the prediction with the ONNX model
6. It stores the result in InfluxDB with the "Prediction" tag

### Optimizations

- **InfluxDB connection cache** : Avoids unnecessary client reinitializations
- **ONNX model cache** : Loads models only once in memory
- **Error handling** : Continues processing even if one machine fails

## Error handling

- **Missing model** : Service stops at startup if an ONNX model is not found
- **Insufficient data** : Predictions are stopped for a machine if input data doesn't match expected format
- **Prediction errors** : Errors are logged and predictions are temporarily stopped for the concerned machine
- **Connection issues** : Automatic InfluxDB reconnection handling

## Monitoring

### Logs

The service uses Serilog for logging with the following levels:
- **Information** : Service start/stop, successful predictions
- **Warning** : Missing data, invalid configurations
- **Error** : Prediction errors, connection issues
- **Debug** : Operation details (in development mode)

### Health Check

The `/api/healthcheck` endpoint returns service status:
- **200 OK** : Service functional
- **500 Internal Server Error** : Service issue

## Development

### Project structure

```
src/DataAggregator.Processor/
├── Configuration/
│   ├── MachinePredictionConfig.cs
│   └── PredictionServiceConfiguration.cs
├── Controllers/
│   └── HealthCheckController.cs
├── Services/
│   ├── DataAccess/
│   │   ├── IInfluxV3Repository.cs
│   │   ├── InfluxV3Repository.cs
│   │   ├── IRegistrationServiceClient.cs
│   │   └── RegistrationServiceClient.cs
│   ├── Prediction/
│   │   ├── IOnnxPredictionEngine.cs
│   │   ├── OnnxPredictionEngine.cs
│   │   └── MachinePredictionProcessor.cs
│   ├── Preprocessing/
│   │   ├── IPreprocessingStrategy.cs
│   │   ├── IPreprocessingStrategyFactory.cs
│   │   ├── PreprocessingStrategyFactory.cs
│   │   ├── ActuatorCurrentFeatureExtractor.cs
│   │   └── MathUtils.cs
│   └── Background/
│       └── PredictionBackgroundService.cs
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── Dockerfile
```

### Testing

To test the service locally:

1. Ensure Registration and InfluxDB services are started
2. Place a test ONNX model in the `models/` folder
3. Configure `appsettings.json` with correct values
4. Launch the service: `dotnet run`

## Preprocessing strategies

### ActuatorCurrentFeatureExtractor

This strategy extracts 14 features from current sensor data:

1. **Statistical features** : Mean, standard deviation, min, max, etc.
2. **Correlation features** : Correlation between axes
3. **Activity features** : Global activity ratio
4. **Stability features** : Temporal stability of signals

Features are normalized with Z-score and formatted for ONNX model input.

## Integration with DataAggregator ecosystem

The prediction service integrates perfectly with the existing architecture:

- **Reuses** shared data models (`IMeasurementData`, `DeviceRegistrationResponse`)
- **Follows** the same configuration and logging patterns
- **Uses** the same technologies (Serilog, ASP.NET Core, Docker)
- **Integrates** with existing services without modifying them
- **Optimizes** performance with caches and reused connections 