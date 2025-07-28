using DataAggregator.Collector.Shared.Models;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.Abstraction;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Registration;
using DataAggregator.Shared.Configuration.TimeSeries;
using DataAggregator.Shared.Domain.DataType;
using DataAggregator.Shared.DTOs;
using Moq;

namespace DataAggregator.Processor.Tests.Services.Prediction;

public class MachinePredictionProcessorTests
{
    private readonly Mock<IDataRepository> _mockInfluxRepository;
    private readonly Mock<IRegistrationServiceClient> _mockRegistrationClient;
    private readonly Mock<IDataProcessorFactory> _mockProcessorFactory;
    private readonly MachinePredictionProcessor _processor;
    private readonly Mock<IDataProcessor> _mockProcessor;

    public MachinePredictionProcessorTests()
    {
        _mockInfluxRepository = new Mock<IDataRepository>();
        _mockRegistrationClient = new Mock<IRegistrationServiceClient>();
        _mockProcessorFactory = new Mock<IDataProcessorFactory>();
        _mockProcessor = new Mock<IDataProcessor>();

        _processor = new MachinePredictionProcessor(
            _mockInfluxRepository.Object,
            _mockRegistrationClient.Object,
            _mockProcessorFactory.Object);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenCollectorInfoIsNull()
    {
        var config = CreateValidMachineConfig();
        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync((CollectorInfoDto?)null);

        await _processor.ProcessAsync(config);

        _mockInfluxRepository.Verify(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenNoValidSensorsFound()
    {
        var config = CreateValidMachineConfig();
        var collectorInfo = CreateCollectorInfoWithSensors(new[] { "different_sensor" });

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);

        await _processor.ProcessAsync(config);

        _mockInfluxRepository.Verify(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReturnEarly_WhenNoMeasurementsFound()
    {
        var config = CreateValidMachineConfig();
        var collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync([]);

        await _processor.ProcessAsync(config);

        _mockInfluxRepository.Verify(x => x.WriteMeasurementAsync(It.IsAny<string>(), It.IsAny<IEnumerable<IMeasurementData>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_ShouldCompleteSuccessfully_WhenAllConditionsAreMet()
    {
        var config = CreateValidMachineConfig();
        var collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        var measurements = CreateTestMeasurements();
        var processedData = new List<IMeasurementData> { new MeasurementData<float>(DateTime.Now, "Prediction", 0.85f) };

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockProcessor.Setup(x => x.ProcessAsync(It.IsAny<IEnumerable<IMeasurementData>>()))
            .ReturnsAsync(processedData);
        _mockProcessorFactory.Setup(x => x.CreateProcessors(It.IsAny<IEnumerable<ProcessorDescription>>()))
            .Returns(new List<IDataProcessor> { _mockProcessor.Object });

        await _processor.ProcessAsync(config);

        _mockInfluxRepository.Verify(x => x.WriteMeasurementAsync(config.MachineName, processedData), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldReinitializeInfluxConnection_WhenEndpointChanges()
    {
        var config = CreateValidMachineConfig();
        var collectorInfo1 = CreateCollectorInfoWithSensors(config.InputSensors, "endpoint1");
        var collectorInfo2 = CreateCollectorInfoWithSensors(config.InputSensors, "endpoint2");
        var measurements = CreateTestMeasurements();
        var processedData = new List<IMeasurementData> { new MeasurementData<float>(DateTime.Now, "Prediction", 0.85f) };

        _mockRegistrationClient.SetupSequence(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo1)
            .ReturnsAsync(collectorInfo2);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockProcessor.Setup(x => x.ProcessAsync(It.IsAny<IEnumerable<IMeasurementData>>()))
            .ReturnsAsync(processedData);
        _mockProcessorFactory.Setup(x => x.CreateProcessors(It.IsAny<IEnumerable<ProcessorDescription>>()))
            .Returns(new List<IDataProcessor> { _mockProcessor.Object });

        await _processor.ProcessAsync(config); // First call with endpoint1
        await _processor.ProcessAsync(config); // Second call with endpoint2

        _mockInfluxRepository.Verify(x => x.Initialize(
            collectorInfo1.AssignedInfluxEndpoint.Endpoint,
            collectorInfo1.AssignedInfluxEndpoint.Token), Times.Once);
        _mockInfluxRepository.Verify(x => x.Initialize(
            collectorInfo2.AssignedInfluxEndpoint.Endpoint,
            collectorInfo2.AssignedInfluxEndpoint.Token), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldNotReinitializeInfluxConnection_WhenEndpointIsSame()
    {
        var config = CreateValidMachineConfig();
        var collectorInfo = CreateCollectorInfoWithSensors(config.InputSensors);
        var measurements = CreateTestMeasurements();
        var processedData = new List<IMeasurementData> { new MeasurementData<float>(DateTime.Now, "Prediction", 0.85f) };

        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ReturnsAsync(collectorInfo);
        _mockInfluxRepository.Setup(x => x.QueryMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<SensorInfoDto>>()))
            .ReturnsAsync(measurements);
        _mockProcessor.Setup(x => x.ProcessAsync(It.IsAny<IEnumerable<IMeasurementData>>()))
            .ReturnsAsync(processedData);
        _mockProcessorFactory.Setup(x => x.CreateProcessors(It.IsAny<IEnumerable<ProcessorDescription>>()))
            .Returns(new List<IDataProcessor> { _mockProcessor.Object });

        await _processor.ProcessAsync(config);
        await _processor.ProcessAsync(config);

        _mockInfluxRepository.Verify(x => x.Initialize(
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_ShouldThrowException_WhenRegistrationClientThrows()
    {
        var config = CreateValidMachineConfig();
        _mockRegistrationClient.Setup(x => x.GetCollectorInfoAsync(config.MachineName))
            .ThrowsAsync(new InvalidOperationException("Registration service error"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _processor.ProcessAsync(config));
    }

    private static MachinePredictionConfig CreateValidMachineConfig() => new()
    {
        MachineName = "test_machine",
        InputSensors = ["sensor1", "sensor2"],
        WindowSizeSeconds = 300,
        CycleIntervalSeconds = 60,
        Enabled = true,
        ProcessingPipeline = new List<ProcessorDescription> { new() }
    };

    private static CollectorInfoDto CreateCollectorInfoWithSensors(IEnumerable<string> sensorNames, string endpoint = "http://localhost:8086") => new(
            "test_device",
            "test_location",
            "http://localhost:5000/health",
            new InfluxEndpoint("TestEndpoint", endpoint, "test_token"),
            sensorNames.Select(name => new SensorInfoDto(name, "Type", "Unit", [], SensorDataType.Float)).ToList(),
            []);

    private static List<IMeasurementData> CreateTestMeasurements()
        =>
        [
            new MeasurementData<float>(DateTime.UtcNow, "sensor1", 10.5f),
            new MeasurementData<float>(DateTime.UtcNow, "sensor2", 20.3f)
        ];
}
