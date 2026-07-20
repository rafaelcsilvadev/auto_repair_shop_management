using Xunit;
using Moq;

public interface IClientRepository
{
    Task<IEnumerable<ClientModel?>> GetAllClientAsync();
    Task<ClientModel?> GetClientByIdAsync(Guid Id);
    Task<ClientModel?> CreateClientAsync();
    Task UpdateClientAsync(Guid Id);
    Task DeleteClientAsync(Guid Id);    
}

public class ClientModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime? Updated_at { get; set; }
    public DateTime? Deleted_at { get; set; }
}

public interface IClientService : IClientRepository;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository clientRepository)
    {
        _repository = clientRepository;
    }

    public Task<IEnumerable<ClientModel?>> GetAllClientAsync()
    {
        return _repository.GetAllClientAsync();
    }

    public Task<ClientModel?> CreateClientAsync()
    {
        return _repository.CreateClientAsync();
    }

    public Task<ClientModel?> GetClientByIdAsync(Guid Id)
    {
        return _repository.GetClientByIdAsync(Id);
    }

    public Task DeleteClientAsync(Guid Id)
    {
        return _repository.DeleteClientAsync(Id);
    }
    public Task UpdateClientAsync(Guid Id)
    {
        return _repository.UpdateClientAsync(Id);
    }


}

public class ClientServiceTest
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly ClientService _service;

    public ClientServiceTest()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _service = new ClientService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ListAllClients()
    {
        //Arrange
        var client = new ClientModel
        {
            Id = new Guid(),
            Name = "Teste",
            Email = "email@email.com",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };

        var clientList = new List<ClientModel> { client };

        _repositoryMock.Setup(r => r.GetAllClientAsync()).ReturnsAsync(clientList);

        //Act
        var result = await _service.GetAllClientAsync();

        //Assert
        Assert.Contains(clientList, r => r.Id == clientList[0].Id);
        Assert.Contains(clientList, r => r.Name == clientList[0].Name);
        Assert.Contains(clientList, r => r.Email == clientList[0].Email);
        Assert.Contains(clientList, r => r.Created_at == clientList[0].Created_at);
        Assert.Contains(clientList, r => r.Updated_at == clientList[0].Updated_at);
        Assert.Contains(clientList, r => r.Deleted_at == clientList[0].Deleted_at);
    }

    [Fact]
    public async Task FindClientById()
    {
        //Arrange
        var client = new ClientModel
        {
            Id = new Guid(),
            Name = "Teste",
            Email = "email@email.com",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.GetClientByIdAsync(client.Id)).ReturnsAsync(client);

        //Act
        var result = await _service.GetClientByIdAsync(client.Id);

        //Assert
        Assert.Equal(client, result);
        Assert.Equal(client.Id, result?.Id);
        Assert.Equal(client.Name, result?.Name);
        Assert.Equal(client.Email, result?.Email);
        Assert.Equal(client.Created_at, result?.Created_at);
        Assert.Equal(client.Updated_at, result?.Updated_at);
        Assert.Equal(client.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task CreateClient()
    {
        //Arrange
        var client = new ClientModel
        {
            Id = new Guid(),
            Name = "Teste",
            Email = "email@email.com",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.CreateClientAsync()).ReturnsAsync(client);

        //Act
        var result = await _service.CreateClientAsync();

        //Assert
        Assert.Equal(client, result);
        Assert.Equal(client.Id, result?.Id);
        Assert.Equal(client.Name, result?.Name);
        Assert.Equal(client.Email, result?.Email);
        Assert.Equal(client.Created_at, result?.Created_at);
        Assert.Equal(client.Updated_at, result?.Updated_at);
        Assert.Equal(client.Deleted_at, result?.Deleted_at);

    }

    [Fact]
    public async Task UpdateClientById()
    {
        //Arrange
        var client = new ClientModel
        {
            Id = new Guid(),
            Name = "Teste",
            Email = "email@email.com",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.UpdateClientAsync(client.Id));

        //Act
        await _service.UpdateClientAsync(client.Id);

    }


    [Fact]
    public async Task DeleteClientById()
    {
        //Arrange
        var client = new ClientModel
        {
            Id = new Guid(),
            Name = "Teste",
            Email = "email@email.com",
            Created_at = new DateTime(),
            Updated_at = new DateTime(),
            Deleted_at = new DateTime(),
        };


        _repositoryMock.Setup(r => r.DeleteClientAsync(client.Id));

        //Act
        await _service.DeleteClientAsync(client.Id);

    }
    

}