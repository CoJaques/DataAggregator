# 🚀 DataAggregator

[![.NET 9](https://img.shields.io/badge/.NET-9.0-blueviolet?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)  
[![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)  
[![InfluxDB 3](https://img.shields.io/badge/InfluxDB-3.0-22ADF6?logo=influxdb&logoColor=white)](https://www.influxdata.com/)  
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/)  

**DataAggregator** is a modular infrastructure for collecting, registering, and processing industrial data.  
It provides a flexible backbone to acquire machine signals, store them in time-series and relational databases, and run ML-based processing pipelines.

<img width="2212" height="1333" alt="example-architecture" src="https://github.com/user-attachments/assets/d19e435a-cc7b-4c99-80b5-ba42c6701573" />

---

## 🧱 Project Structure

```
DataAggregator/
├─ DataAggregator.sln                 # Visual Studio solution entry point
├─ .github/workflows/                 # CI/CD pipelines (build & test)
│  └─ buildAndTest.yml
├─ docker/                            # Dockerfiles for each service
│  ├─ Dockerfile.Collector
│  ├─ Dockerfile.Processor
│  └─ Dockerfile.Registration
├─ env/                               # Developement environment (docker-compose, volumes)
│  ├─ docker-compose.yml
│  └─ volumes/
│     └─ grafana/
├─ lib/                               # External local libraries
│  └─ Capnp.Net.Runtime/              # Cap’n Proto .NET runtime
├─ src/                               # Main application source code
│  ├─ DataAggregator.Shared/                  # Common DTOs, models, helpers
│  ├─ DataAggregator.Collector/               # Collector host (ASP.NET Core service)
│  ├─ DataAggregator.Collector.Shared/        # Collector abstractions & config
│  ├─ DataAggregator.Collector.OpenCNCapnProtoConnector/ # OpenCN connector
│  ├─ DataAggregator.Collector.FileCollector/ # File-based collector for demo/testing
│  ├─ DataAggregator.Registration/            # Registration service (ASP.NET Core)
│  └─ DataAggregator.Processor/               # Processing service (ONNX models, ML logic)
├─ tests/                            
│  ├─ DataAggregator.Registration.Tests/      # Unit tests for Registration
│  ├─ DataAggregator.Collector.Tests/      # Unit tests for Registration
│  └─ DataAggregator.Processor.Tests/         # Unit tests for Processor
```

---

## 🌐 Related Repositories

- 🔧 **Deployment example (full stack with configs, Grafana, etc.)**  
  👉 [DataAggregator-demo](https://github.com/CoJaques/DataAggregator-demo)

- 📊 **Model training & analysis pipeline**  
  👉 [Data-analysis](https://github.com/CoJaques/Data-analysis)

- 🛠️ **OpenCN fork with connector support** (work in progress)  
  👉 [OpenCN branch `cmctl-bufferization`](https://gitlab.com/CoJaques/opencn/-/tree/cmctl-bufferization?ref_type=heads)

---

## 🧰 Tech Stack

- **.NET 9** → Backend services (Collector, Registration, Processor)
- **ML.NET** -> Data processing
- **Docker / Docker Compose** → Deployment & orchestration  
- **InfluxDB v3** → Time-series data storage  
- **PostgreSQL** → Relational storage & metadata  
- **Grafana** (in demo repo) → Data visualization  

---

## ⚡ Overview

DataAggregator is built around a **modular and distributed architecture**:

- **Collector** → Acquires machine signals (e.g., currents from CNC drives) via OpenCN or file input.  
- **Registration** → Manages device registration and configuration in PostgreSQL.  
- **Processor** → Extracts features and runs ML inference (ONNX models) to classify machine states.  
- **Databases** → InfluxDB v3 stores raw and processed signals; PostgreSQL stores device metadata.  

This design ensures scalability across multiple machines and flexibility for integration with existing industrial setups.  

---

## 🚀 Getting Started

👉 For a ready-to-use deployment example (including Docker setup, Grafana dashboards, and configs), check out:  
📦 [**DataAggregator-demo**](https://github.com/CoJaques/DataAggregator-demo)

---

`this readme was enhanced by AI to clean the formatting and structure the content`
