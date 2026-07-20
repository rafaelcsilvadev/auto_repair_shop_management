using Xunit;
using Moq;

public interface IServiceOrderRepository
{
    Task<IEnumerable<ServiceOrderModel?>> GetAllServiceOrderAsync();
    Task<ServiceOrderModel?> GetServiceOrderByIdAsync(Guid Id);
    Task<ServiceOrderModel?> CreateServiceOrderAsync();
    Task UpdateServiceOrderAsync(Guid Id);
    Task DeleteServiceOrderAsync(Guid Id);    
}

public class ServiceOrderModel
{
    public Guid Id { get; set; }
    public required string ServiceOrder { get; set; }
    public Guid ClientId { get; set; }
    public Guid VehicleId { get; set; }
    public required string Description { get; set; }
    public required decimal Total { get; set; }
    public required DateTime Entrance { get; set; }
    public required DateTime Estimated { get; set; }
    public required string Status { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime? Updated_at { get; set; }
    public DateTime? Deleted_at { get; set; }
}

public interface IServiceOrderService : IServiceOrderRepository;

public class ServiceOrderService : IServiceOrderService
{
    private readonly IServiceOrderRepository _repository;

    public ServiceOrderService(IServiceOrderRepository ServiceOrderRepository)
    {
        _repository = ServiceOrderRepository;
    }

    public Task<IEnumerable<ServiceOrderModel?>> GetAllServiceOrderAsync()
    {
        return _repository.GetAllServiceOrderAsync();
    }

    public Task<ServiceOrderModel?> CreateServiceOrderAsync()
    {
        return _repository.CreateServiceOrderAsync();
    }

    public Task<ServiceOrderModel?> GetServiceOrderByIdAsync(Guid Id)
    {
        return _repository.GetServiceOrderByIdAsync(Id);
    }

    public Task DeleteServiceOrderAsync(Guid Id)
    {
        return _repository.DeleteServiceOrderAsync(Id);
    }
    public Task UpdateServiceOrderAsync(Guid Id)
    {
        return _repository.UpdateServiceOrderAsync(Id);
    }


}

public class ServiceOrderServiceTest
{
    private readonly Mock<IServiceOrderRepository> _repositoryMock;
    private readonly ServiceOrderService _service;

    public ServiceOrderServiceTest()
    {
        _repositoryMock = new Mock<IServiceOrderRepository>();
        _service = new ServiceOrderService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ListAllServiceOrders()
    {
        //Arrange
        var ServiceOrder = new ServiceOrderModel
        {
            Id = new Guid(),
            ServiceOrder = "OS-0001",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Test Description",
            Total = 500m,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Open",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };

        var ServiceOrderList = new List<ServiceOrderModel> { ServiceOrder };

        _repositoryMock.Setup(r => r.GetAllServiceOrderAsync()).ReturnsAsync(ServiceOrderList);

        //Act
        var result = await _service.GetAllServiceOrderAsync();

        //Assert
        Assert.Contains(ServiceOrderList, r => r.Id == ServiceOrderList[0].Id);
        Assert.Contains(ServiceOrderList, r => r.ServiceOrder == ServiceOrderList[0].ServiceOrder);
        Assert.Contains(ServiceOrderList, r => r.ClientId == ServiceOrderList[0].ClientId);
        Assert.Contains(ServiceOrderList, r => r.VehicleId == ServiceOrderList[0].VehicleId);
        Assert.Contains(ServiceOrderList, r => r.Description == ServiceOrderList[0].Description);
        Assert.Contains(ServiceOrderList, r => r.Total == ServiceOrderList[0].Total);
        Assert.Contains(ServiceOrderList, r => r.Entrance == ServiceOrderList[0].Entrance);
        Assert.Contains(ServiceOrderList, r => r.Estimated == ServiceOrderList[0].Estimated);
        Assert.Contains(ServiceOrderList, r => r.Status == ServiceOrderList[0].Status);
        Assert.Contains(ServiceOrderList, r => r.Created_at == ServiceOrderList[0].Created_at);
        Assert.Contains(ServiceOrderList, r => r.Updated_at == ServiceOrderList[0].Updated_at);
        Assert.Contains(ServiceOrderList, r => r.Deleted_at == ServiceOrderList[0].Deleted_at);
    }

    [Fact]
    public async Task FindServiceOrderById()
    {
        //Arrange
        var ServiceOrder = new ServiceOrderModel
        {
            Id = new Guid(),
            ServiceOrder = "OS-0001",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Test Description",
            Total = 500m,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Open",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.GetServiceOrderByIdAsync(ServiceOrder.Id)).ReturnsAsync(ServiceOrder);

        //Act
        var result = await _service.GetServiceOrderByIdAsync(ServiceOrder.Id);

        //Assert
        Assert.Equal(ServiceOrder, result);
        Assert.Equal(ServiceOrder.Id, result?.Id);
        Assert.Equal(ServiceOrder.ServiceOrder, result?.ServiceOrder);
        Assert.Equal(ServiceOrder.ClientId, result?.ClientId);
        Assert.Equal(ServiceOrder.VehicleId, result?.VehicleId);
        Assert.Equal(ServiceOrder.Description, result?.Description);
        Assert.Equal(ServiceOrder.Total, result?.Total);
        Assert.Equal(ServiceOrder.Entrance, result?.Entrance);
        Assert.Equal(ServiceOrder.Estimated, result?.Estimated);
        Assert.Equal(ServiceOrder.Status, result?.Status);
        Assert.Equal(ServiceOrder.Created_at, result?.Created_at);
        Assert.Equal(ServiceOrder.Updated_at, result?.Updated_at);
        Assert.Equal(ServiceOrder.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task CreateServiceOrder()
    {
        //Arrange
        var ServiceOrder = new ServiceOrderModel
        {
            Id = new Guid(),
            ServiceOrder = "OS-0001",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Test Description",
            Total = 500m,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Open",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.CreateServiceOrderAsync()).ReturnsAsync(ServiceOrder);

        //Act
        var result = await _service.CreateServiceOrderAsync();

        //Assert
        Assert.Equal(ServiceOrder, result);
        Assert.Equal(ServiceOrder.Id, result?.Id);
        Assert.Equal(ServiceOrder.ServiceOrder, result?.ServiceOrder);
        Assert.Equal(ServiceOrder.ClientId, result?.ClientId);
        Assert.Equal(ServiceOrder.VehicleId, result?.VehicleId);
        Assert.Equal(ServiceOrder.Description, result?.Description);
        Assert.Equal(ServiceOrder.Total, result?.Total);
        Assert.Equal(ServiceOrder.Entrance, result?.Entrance);
        Assert.Equal(ServiceOrder.Estimated, result?.Estimated);
        Assert.Equal(ServiceOrder.Status, result?.Status);
        Assert.Equal(ServiceOrder.Created_at, result?.Created_at);
        Assert.Equal(ServiceOrder.Updated_at, result?.Updated_at);
        Assert.Equal(ServiceOrder.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task UpdateServiceOrderById()
    {
        //Arrange
        var ServiceOrder = new ServiceOrderModel
        {
            Id = new Guid(),
            ServiceOrder = "OS-0001",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Test Description",
            Total = 500m,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Open",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.UpdateServiceOrderAsync(ServiceOrder.Id));

        //Act
        await _service.UpdateServiceOrderAsync(ServiceOrder.Id);

    }


    [Fact]
    public async Task DeleteServiceOrderById()
    {
        //Arrange
        var ServiceOrder = new ServiceOrderModel
        {
            Id = new Guid(),
            ServiceOrder = "OS-0001",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Test Description",
            Total = 500m,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Open",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.DeleteServiceOrderAsync(ServiceOrder.Id));

        //Act
        await _service.DeleteServiceOrderAsync(ServiceOrder.Id);

    }
    

}