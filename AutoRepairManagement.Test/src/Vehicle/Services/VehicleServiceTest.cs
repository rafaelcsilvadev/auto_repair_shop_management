using Xunit;
using Moq;

public interface IVehicleRepository
{
    Task<IEnumerable<VehicleModel?>> GetAllVehicleAsync();
    Task<VehicleModel?> GetVehicleByIdAsync(Guid Id);
    Task<VehicleModel?> CreateVehicleAsync();
    Task UpdateVehicleAsync(Guid Id);
    Task DeleteVehicleAsync(Guid Id);    
}

public class VehicleModel
{
    public Guid Id { get; set; }
    public required string Plate { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
    public required int Kilometers { get; set; }
    public Guid ClientId { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime? Updated_at { get; set; }
    public DateTime? Deleted_at { get; set; }
}

public interface IVehicleService : IVehicleRepository;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(IVehicleRepository VehicleRepository)
    {
        _repository = VehicleRepository;
    }

    public Task<IEnumerable<VehicleModel?>> GetAllVehicleAsync()
    {
        return _repository.GetAllVehicleAsync();
    }

    public Task<VehicleModel?> CreateVehicleAsync()
    {
        return _repository.CreateVehicleAsync();
    }

    public Task<VehicleModel?> GetVehicleByIdAsync(Guid Id)
    {
        return _repository.GetVehicleByIdAsync(Id);
    }

    public Task DeleteVehicleAsync(Guid Id)
    {
        return _repository.DeleteVehicleAsync(Id);
    }
    public Task UpdateVehicleAsync(Guid Id)
    {
        return _repository.UpdateVehicleAsync(Id);
    }


}

public class VehicleServiceTest
{
    private readonly Mock<IVehicleRepository> _repositoryMock;
    private readonly VehicleService _service;

    public VehicleServiceTest()
    {
        _repositoryMock = new Mock<IVehicleRepository>();
        _service = new VehicleService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ListAllVehicles()
    {
        //Arrange
        var Vehicle = new VehicleModel
        {
            Id = new Guid(),
            Plate = "ABC123",
            Model = "Test Model",
            Year = 2020,
            Kilometers = 50000,
            ClientId = new Guid(),
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };

        var VehicleList = new List<VehicleModel> { Vehicle };

        _repositoryMock.Setup(r => r.GetAllVehicleAsync()).ReturnsAsync(VehicleList);

        //Act
        var result = await _service.GetAllVehicleAsync();

        //Assert
        Assert.Contains(VehicleList, r => r.Id == VehicleList[0].Id);
        Assert.Contains(VehicleList, r => r.Plate == VehicleList[0].Plate);
        Assert.Contains(VehicleList, r => r.Model == VehicleList[0].Model);
        Assert.Contains(VehicleList, r => r.Year == VehicleList[0].Year);
        Assert.Contains(VehicleList, r => r.Kilometers == VehicleList[0].Kilometers);
        Assert.Contains(VehicleList, r => r.ClientId == VehicleList[0].ClientId);
        Assert.Contains(VehicleList, r => r.Created_at == VehicleList[0].Created_at);
        Assert.Contains(VehicleList, r => r.Updated_at == VehicleList[0].Updated_at);
        Assert.Contains(VehicleList, r => r.Deleted_at == VehicleList[0].Deleted_at);
    }

    [Fact]
    public async Task FindVehicleById()
    {
        //Arrange
        var Vehicle = new VehicleModel
        {
            Id = new Guid(),
            Plate = "ABC123",
            Model = "Test Model",
            Year = 2020,
            Kilometers = 50000,
            ClientId = new Guid(),
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.GetVehicleByIdAsync(Vehicle.Id)).ReturnsAsync(Vehicle);

        //Act
        var result = await _service.GetVehicleByIdAsync(Vehicle.Id);

        //Assert
        Assert.Equal(Vehicle, result);
        Assert.Equal(Vehicle.Id, result?.Id);
        Assert.Equal(Vehicle.Plate, result?.Plate);
        Assert.Equal(Vehicle.Model, result?.Model);
        Assert.Equal(Vehicle.Year, result?.Year);
        Assert.Equal(Vehicle.Kilometers, result?.Kilometers);
        Assert.Equal(Vehicle.ClientId, result?.ClientId);
        Assert.Equal(Vehicle.Created_at, result?.Created_at);
        Assert.Equal(Vehicle.Updated_at, result?.Updated_at);
        Assert.Equal(Vehicle.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task CreateVehicle()
    {
        //Arrange
        var Vehicle = new VehicleModel
        {
            Id = new Guid(),
            Plate = "ABC123",
            Model = "Test Model",
            Year = 2020,
            Kilometers = 50000,
            ClientId = new Guid(),
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.CreateVehicleAsync()).ReturnsAsync(Vehicle);

        //Act
        var result = await _service.CreateVehicleAsync();

        //Assert
        Assert.Equal(Vehicle, result);
        Assert.Equal(Vehicle.Id, result?.Id);
        Assert.Equal(Vehicle.Plate, result?.Plate);
        Assert.Equal(Vehicle.Model, result?.Model);
        Assert.Equal(Vehicle.Year, result?.Year);
        Assert.Equal(Vehicle.Kilometers, result?.Kilometers);
        Assert.Equal(Vehicle.ClientId, result?.ClientId);
        Assert.Equal(Vehicle.Created_at, result?.Created_at);
        Assert.Equal(Vehicle.Updated_at, result?.Updated_at);
        Assert.Equal(Vehicle.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task UpdateVehicleById()
    {
        //Arrange
        var Vehicle = new VehicleModel
        {
            Id = new Guid(),
            Plate = "ABC123",
            Model = "Test Model",
            Year = 2020,
            Kilometers = 50000,
            ClientId = new Guid(),      
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.UpdateVehicleAsync(Vehicle.Id));

        //Act
        await _service.UpdateVehicleAsync(Vehicle.Id);

    }


    [Fact]
    public async Task DeleteVehicleById()
    {
        //Arrange
        var Vehicle = new VehicleModel
        {
            Id = new Guid(),
            Plate = "ABC123",
            Model = "Test Model",
            Year = 2020,
            Kilometers = 50000,
            ClientId = new Guid(),
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.DeleteVehicleAsync(Vehicle.Id));

        //Act
        await _service.DeleteVehicleAsync(Vehicle.Id);

    }
    

}