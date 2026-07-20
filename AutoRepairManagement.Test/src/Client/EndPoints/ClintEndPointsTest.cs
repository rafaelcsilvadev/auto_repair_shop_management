using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

public static class ClientEndpoints
{
    public static void MapClientEndPoints(this WebApplication app)
    {
        app.MapGet("/client", GetClientAsync);
        app.MapGet("/client/{id}", GetClientByIdAsync);
        app.MapPost("/client", CreateClientAsync);
        app.MapPut("/client/{id}", UpdateClientAsync);
        app.MapDelete("/client/{id}", DeleteClientAsync);

    }

    public static async Task<IResult> GetClientAsync(IClientService service)
    {
        try
        {
            var clients = await service.GetAllClientAsync();

            return Results.Ok(clients);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> GetClientByIdAsync(
        Guid id,
        IClientService service
    )
    {
        try
        {
            var client = await service.GetClientByIdAsync(id);

            return Results.Ok(client);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> CreateClientAsync(
        ClientDto dto,
        IValidator<ClientDto> validator,
        IClientService service
    )
    {
        try
        {
            var validation = await validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    });

                return Results.BadRequest(errors);
            }

            var createdClient = await service.CreateClientAsync();

            return Results.Created($"/client/{createdClient?.Id}", createdClient?.Id);


        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> UpdateClientAsync(
        Guid id,
        ClientDto dto,
        IValidator<ClientDto> validator,
        IClientService service
        )
    {
        try
        {
            var validation = await validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    });

                return Results.BadRequest(errors);
            }

            await service.UpdateClientAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> DeleteClientAsync(
        Guid id,
        IClientService service
        )
    {
        try
        {
            await service.DeleteClientAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }
}

public class ClientEndPointTest
{
    private readonly Mock<IClientService> _serviceMock;

    public ClientEndPointTest()
    {
        _serviceMock = new Mock<IClientService>();
    }

    [Fact]
    public async Task GetClientAsync_MustThereIsTryCatch()
    {
        //Arrange
        var fails = new List<ValidationFailure>
        {
            new("Total", "Name is required"),
        };

        _serviceMock
            .Setup(s => s.GetAllClientAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ClientEndpoints.GetClientAsync(_serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);

    }

    [Fact]
    public async Task GetClientByIdAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.GetClientByIdAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ClientEndpoints.GetClientByIdAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task CreateClientAsync_MustThereIsTryCatch()
    {
        //Arrange
        var dto = new ClientDto { Name = "Teste", Email = "email@email.com" };
        var validator = new ClientDtoValidation();

        _serviceMock
            .Setup(s => s.CreateClientAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ClientEndpoints.CreateClientAsync(dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task UpdateClientAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();
        var dto = new ClientDto { Name = "Teste", Email = "email@email.com" };
        var validator = new ClientDtoValidation();

        _serviceMock
            .Setup(s => s.UpdateClientAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ClientEndpoints.UpdateClientAsync(id, dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task DeleteClientAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.DeleteClientAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ClientEndpoints.DeleteClientAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }
}