# API Documentation

## Overview

This documentation describes the available API endpoints in the different DataAggregator services.

## Services

### 1. DataAggregator.Collector

Service responsible for collecting data from various sources.

#### Endpoints

##### Health Check

**GET** `/api/HealthCheck`

Retrieves the health status of the collector.

**Responses:**
- **200 OK** : Returns a `HealthStatus` object
- **500 Internal Server Error** : Error while retrieving status

**Response Model:**
```json
{
  "status": "Healthy|Degraded|Unhealthy",
  "message": "Description of current status",
  "lastDataSent": "2024-01-01T00:00:00Z",
  "bufferSize": 0,
  "databaseConnected": true,
  "timestamp": "2024-01-01T00:00:00Z"
}
```

**Status Codes:**
- `Healthy` : Collector is running normally
- `Degraded` : Minor issues (full buffer, no recent data)
- `Unhealthy` : Critical issues (no database connection)

---

### 2. DataAggregator.Registration

Service responsible for registering and managing collector devices.

#### Base URL
```
http://localhost:5001/api
```

#### Endpoints

##### Device Registration

**POST** `/api/DeviceRegistration/register`

Registers a new collector device.

**Request Body:**
```json
{
  "deviceName": "Device name",
  "healthCheckEndpoint": "http://endpoint:port/api/HealthCheck",
  "additionalConfiguration": {}
}
```

**Responses:**
- **200 OK** : Returns a `DeviceRegistrationResponse` object
- **400 Bad Request** : Invalid request data
- **500 Internal Server Error** : Server error

##### Get Device Information

**GET** `/api/DeviceRegistration/{deviceName}`

Retrieves information about a specific device by its name.

**Parameters:**
- `deviceName` (string) : Device name

**Responses:**
- **200 OK** : Returns a `CollectorInfoDto` object
- **400 Bad Request** : Missing device name
- **404 Not Found** : Device not found
- **500 Internal Server Error** : Server error

##### List All Devices

**GET** `/api/DeviceRegistration`

Retrieves the list of all registered devices.

**Responses:**
- **200 OK** : Returns a list of `CollectorInfoDto` objects
- **500 Internal Server Error** : Server error

---

### 3. DataAggregator.Processor

Service responsible for processing and analyzing collected data.

**Note:** This service currently does not have public API endpoints. It operates as a background processing service.
