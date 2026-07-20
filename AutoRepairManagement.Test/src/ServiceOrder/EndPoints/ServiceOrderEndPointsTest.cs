using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

public static class ServiceOrderEndpoints
{
    public static void MapServiceOrderEndPoints(this WebApplication app)
    {
        app.MapGet("/service-order", GetServiceOrderAsync);
        app.MapGet("/service-order/{id}", GetServiceOrderByIdAsync);
        app.MapPost("/service-order", CreateServiceOrderAsync);
        app.MapPut("/service-order/{id}", UpdateServiceOrderAsync);
        app.MapDelete("/service-order/{id}", DeleteServiceOrderAsync);

    }

    public static async Task<IResult> GetServiceOrderAsync(IServiceOrderService service)
    {
        try
        {
            var serviceOrders = await service.GetAllServiceOrderAsync();

            return Results.Ok(serviceOrders);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> GetServiceOrderByIdAsync(
        Guid id,
        IServiceOrderService service
    )
    {
        try
        {
            var serviceOrder = await service.GetServiceOrderByIdAsync(id);

            return Results.Ok(serviceOrder);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> CreateServiceOrderAsync(
        ServiceOrderDto dto,
        IValidator<ServiceOrderDto> validator,
        IServiceOrderService service
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

            var createdServiceOrder = await service.CreateServiceOrderAsync();

            return Results.Created($"/service-order/{createdServiceOrder?.Id}", createdServiceOrder?.Id);


        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> UpdateServiceOrderAsync(
        Guid id,
        ServiceOrderDto dto,
        IValidator<ServiceOrderDto> validator,
        IServiceOrderService service
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

            await service.UpdateServiceOrderAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> DeleteServiceOrderAsync(
        Guid id,
        IServiceOrderService service
        )
    {
        try
        {
            await service.DeleteServiceOrderAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }
}

public class ServiceOrderEndPointTest
{
    private readonly Mock<IServiceOrderService> _serviceMock;

    public ServiceOrderEndPointTest()
    {
        _serviceMock = new Mock<IServiceOrderService>();
    }

    [Fact]
    public async Task GetServiceOrderAsync_MustThereIsTryCatch()
    {
        //Arrange
        var fails = new List<ValidationFailure>
        {
            new("Total", "Name is required"),
        };

        _serviceMock
            .Setup(s => s.GetAllServiceOrderAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ServiceOrderEndpoints.GetServiceOrderAsync(_serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);

    }

    [Fact]
    public async Task GetServiceOrderByIdAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.GetServiceOrderByIdAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ServiceOrderEndpoints.GetServiceOrderByIdAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task CreateServiceOrderAsync_MustThereIsTryCatch()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "OS-0001",
            ClientId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            Description = "Teste",
            Total = 100,
            Entrance = DateTime.Today,
            Estimated = DateTime.Today.AddDays(1),
            Status = "Open"
        };
        var validator = new ServiceOrderDtoValidation();

        _serviceMock
            .Setup(s => s.CreateServiceOrderAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ServiceOrderEndpoints.CreateServiceOrderAsync(dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task UpdateServiceOrderAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "OS-0001",
            ClientId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            Description = "Teste",
            Total = 100,
            Entrance = DateTime.Today,
            Estimated = DateTime.Today.AddDays(1),
            Status = "Open"
        };
        var validator = new ServiceOrderDtoValidation();

        _serviceMock
            .Setup(s => s.UpdateServiceOrderAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ServiceOrderEndpoints.UpdateServiceOrderAsync(id, dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task DeleteServiceOrderAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.DeleteServiceOrderAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await ServiceOrderEndpoints.DeleteServiceOrderAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }
}
