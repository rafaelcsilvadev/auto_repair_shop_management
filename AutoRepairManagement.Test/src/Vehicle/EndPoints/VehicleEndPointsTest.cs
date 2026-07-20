using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

public static class VehicleEndpoints
{
    public static void MapVehicleEndPoints(this WebApplication app)
    {
        app.MapGet("/vehicle", GetVehicleAsync);
        app.MapGet("/vehicle/{id}", GetVehicleByIdAsync);
        app.MapPost("/vehicle", CreateVehicleAsync);
        app.MapPut("/vehicle/{id}", UpdateVehicleAsync);
        app.MapDelete("/vehicle/{id}", DeleteVehicleAsync);

    }

    public static async Task<IResult> GetVehicleAsync(IVehicleService service)
    {
        try
        {
            var vehicles = await service.GetAllVehicleAsync();

            return Results.Ok(vehicles);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> GetVehicleByIdAsync(
        Guid id,
        IVehicleService service
    )
    {
        try
        {
            var vehicle = await service.GetVehicleByIdAsync(id);

            return Results.Ok(vehicle);

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> CreateVehicleAsync(
        VehicleDto dto,
        IValidator<VehicleDto> validator,
        IVehicleService service
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

            var createdVehicle = await service.CreateVehicleAsync();

            return Results.Created($"/vehicle/{createdVehicle?.Id}", createdVehicle?.Id);


        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> UpdateVehicleAsync(
        Guid id,
        VehicleDto dto,
        IValidator<VehicleDto> validator,
        IVehicleService service
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

            await service.UpdateVehicleAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }

    public static async Task<IResult> DeleteVehicleAsync(
        Guid id,
        IVehicleService service
        )
    {
        try
        {
            await service.DeleteVehicleAsync(id);

            return Results.NoContent();

        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }

    }
}

public class VehicleEndPointTest
{
    private readonly Mock<IVehicleService> _serviceMock;

    public VehicleEndPointTest()
    {
        _serviceMock = new Mock<IVehicleService>();
    }

    [Fact]
    public async Task GetVehicleAsync_MustThereIsTryCatch()
    {
        //Arrange
        var fails = new List<ValidationFailure>
        {
            new("Total", "Name is required"),
        };

        _serviceMock
            .Setup(s => s.GetAllVehicleAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await VehicleEndpoints.GetVehicleAsync(_serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);

    }

    [Fact]
    public async Task GetVehicleByIdAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.GetVehicleByIdAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await VehicleEndpoints.GetVehicleByIdAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task CreateVehicleAsync_MustThereIsTryCatch()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10000,
            ClientId = Guid.NewGuid()
        };
        var validator = new VehicleDtoValidation();

        _serviceMock
            .Setup(s => s.CreateVehicleAsync())
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await VehicleEndpoints.CreateVehicleAsync(dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task UpdateVehicleAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10000,
            ClientId = Guid.NewGuid()
        };
        var validator = new VehicleDtoValidation();

        _serviceMock
            .Setup(s => s.UpdateVehicleAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await VehicleEndpoints.UpdateVehicleAsync(id, dto, validator, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }

    [Fact]
    public async Task DeleteVehicleAsync_MustThereIsTryCatch()
    {
        //Arrange
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.DeleteVehicleAsync(id))
            .ThrowsAsync(new TimeoutException("Timeout"));

        //Act
        var result = await VehicleEndpoints.DeleteVehicleAsync(id, _serviceMock.Object);

        //Assert
        var problem = Assert.IsType<ProblemHttpResult>(result);
        Assert.Equal(500, problem.ProblemDetails.Status);
        Assert.Equal("Timeout", problem.ProblemDetails.Detail);
        Assert.Contains("Timeout", problem.ProblemDetails.Detail);
    }
}
