# 🚀 DataAggregator

A modular application for collecting and registering data from devices, using InfluxDB and PostgreSQL.

---

## 🧱 Project Structure

```

DataAggregator/
├── Shared         # Common DTOs and models
├── Collector      # Data collection service
└── Registration   # Device registration service

````

---

## ⚡ Quick Start

### 🔁 All services

```bash
docker-compose up
````

### 🧩 Individual services

```bash
docker-compose up registration
docker-compose up collector
docker-compose up influxdb
docker-compose up postgres
```

---

## 🛠️ First-Time InfluxDB Setup

InfluxDB v3 requires an admin token for access.

### 1. Start the container

```bash
docker-compose up influxdb
```

### 2. Get the container name

```bash
docker ps
```

### 3. Create an admin token

Replace `<your_influxdb_container>` with the actual name from the previous step:

```bash
docker exec -it <your_influxdb_container> influxdb3 create token --admin
```

### 4. Add the token to your configuration

Open `appsettings.json` in the appropriate project (e.g., `Registration`) and add your token:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "Influx": {
    "Endpoints": [
      {
        "Name": "DefaultEndpoint",
        "Endpoint": "http://localhost:8181",
        "Token": "your-token"
      }
    ]
  }
}
```

---

## 🌐 Service Endpoints

| Service      | URL                                            |
| ------------ | ---------------------------------------------- |
| Collector    | [http://localhost:5000](http://localhost:5000) |
| Registration | [http://localhost:5001](http://localhost:5001) |
| InfluxDB UI  | [http://localhost:8086](http://localhost:8181) |
| PostgreSQL   | `localhost:5432`                               |

---

## 🧰 Tech Stack

* **.NET 8** – Backend services
* **Docker / Docker Compose** – Container orchestration
* **InfluxDB v3** – Time-series database
* **PostgreSQL** – Relational data storage

---

