using DataAggregator.Collector.Shared.Abstraction;
using DataAggregator.Collector.Shared.Abstraction.Configuration;
using DataAggregator.Collector.Shared.DataStorage;
using DataAggregator.Collector.Shared.LocalStorage;
using DataAggregator.Collector.Shared.Models;
using DataAggregator.Collector.Shared.Registration;
using Moq;

namespace DataAggregator.Collector.Tests;

public class CollectorServiceTests
{
    private readonly Mock<IDataSourceConnector> _dataSourceConnectorMock = new();
    private readonly Mock<IDataRepository> _dataRepositoryMock = new();
    private readonly Mock<CollectorEndpointManager> _initializationServiceMock;
    private readonly Mock<DataBufferService> _dataBufferServiceMock = new();
    private readonly CollectorConfiguration _configuration = new() { DeviceName = "TestDevice" };

    public CollectorServiceTests()
    {
        // CollectorEndpointManager n'est pas une interface, on mocke via MockBehavior.Loose
        _initializationServiceMock = new Mock<CollectorEndpointManager>(MockBehavior.Loose, null!, _configuration);
    }

    private CollectorService CreateService()
        => new(
            _dataSourceConnectorMock.Object,
            _dataRepositoryMock.Object,
            _dataBufferServiceMock.Object,
            _configuration);

    [Fact]
    public async Task StartAsync_ShouldInitializeAndStart_WhenNotRunning()
    {
        // Arrange
        _dataRepositoryMock.Setup(x => x.InitializeAsync()).Returns(Task.CompletedTask);
        _dataSourceConnectorMock.Setup(x => x.ConnectAsync()).Returns(Task.CompletedTask);
        _dataBufferServiceMock.Setup(x => x.GetBufferSize()).Returns(10);
        _dataBufferServiceMock.Setup(x => x.GetAndClearBuffer()).Returns(new List<IMeasurementData>());
        CollectorService service = CreateService();

        // Act
        await service.StartAsync();

        // Assert
        _dataRepositoryMock.Verify(x => x.InitializeAsync(), Times.Once);
        _dataSourceConnectorMock.Verify(x => x.ConnectAsync(), Times.Once);
        _dataBufferServiceMock.Verify(x => x.GetAndClearBuffer(), Times.Once);
    }

    [Fact]
    public async Task StartAsync_ShouldNotStart_WhenAlreadyRunning()
    {
        // Arrange
        _dataRepositoryMock.Setup(x => x.InitializeAsync()).Returns(Task.CompletedTask);
        _dataSourceConnectorMock.Setup(x => x.ConnectAsync()).Returns(Task.CompletedTask);
        _dataBufferServiceMock.Setup(x => x.GetAndClearBuffer()).Returns(new List<IMeasurementData>());
        CollectorService service = CreateService();
        await service.StartAsync();

        // Act
        await service.StartAsync();

        // Assert
        _dataRepositoryMock.Verify(x => x.InitializeAsync(), Times.Once);
        _dataSourceConnectorMock.Verify(x => x.ConnectAsync(), Times.Once);
    }

    [Fact]
    public async Task StopAsync_ShouldDisconnectAndProcessBuffer_WhenRunning()
    {
        // Arrange
        _dataSourceConnectorMock.Setup(x => x.DisconnectAsync()).Returns(Task.CompletedTask);
        _dataBufferServiceMock.Setup(x => x.GetAndClearBuffer()).Returns(new List<IMeasurementData>());
        _dataBufferServiceMock.Setup(x => x.GetBufferSize()).Returns(10);
        CollectorService service = CreateService();
        // Simule l'état démarré
        typeof(CollectorService).GetField("_isRunning", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(service, true);
        typeof(CollectorService).GetField("_cancellationTokenSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(service, new CancellationTokenSource());

        // Act
        await service.StopAsync();

        // Assert
        _dataSourceConnectorMock.Verify(x => x.DisconnectAsync(), Times.Once);
        _dataBufferServiceMock.Verify(x => x.GetAndClearBuffer(), Times.Once);
    }

    [Fact]
    public async Task StopAsync_ShouldNotDisconnect_WhenNotRunning()
    {
        // Arrange
        CollectorService service = CreateService();

        // Act
        await service.StopAsync();

        // Assert
        _dataSourceConnectorMock.Verify(x => x.DisconnectAsync(), Times.Never);
        _dataBufferServiceMock.Verify(x => x.GetAndClearBuffer(), Times.Never);
    }
}
