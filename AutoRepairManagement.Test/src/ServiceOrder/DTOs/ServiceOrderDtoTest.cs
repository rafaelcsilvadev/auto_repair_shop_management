using FluentValidation;

public record ServiceOrderDto()
{
    public required string ServiceOrder { get; set; }
    public required Guid ClientId { get; set; }
    public required Guid VehicleId { get; set; }
    public required string Description { get; set; }
    public required decimal Total { get; set; }
    public required DateTime Entrance { get; set; }
    public required DateTime Estimated { get; set; }
    public required string Status { get; set; }
};

public class ServiceOrderDtoValidation : AbstractValidator<ServiceOrderDto>
{
    public ServiceOrderDtoValidation()
    {
        RuleFor(x => x.ServiceOrder)
            .NotEmpty()
            .WithMessage("Service order is required")
            .MaximumLength(50)
            .WithMessage("Service order must be less than 50 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .NotNull()
            .WithMessage("Description is required")
            .MaximumLength(100)
            .WithMessage("Description must be less than 100 characters");

        RuleFor(x => x.Entrance)
            .LessThan(x => x.Estimated)
            .WithMessage("Entrance must be less than Estimated");

        RuleFor(x => x.Estimated)
            .GreaterThan(x => x.Entrance)
            .WithMessage("Estimated must be greater than Entrance");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .MaximumLength(20)
            .WithMessage("Status must be less than 20 characters");           

    }
}


public class ServiceOrderDtoTest
{
    private readonly ServiceOrderDtoValidation _validator;

    public ServiceOrderDtoTest()
    {
        _validator = new ServiceOrderDtoValidation();
    }


    [Fact]
    public async Task ServiceOrderMustFailWhenServiceOrderIsEmpty()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 100,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Teste"
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Service order is required");
    }

    [Fact]
    public async Task ServiceOrderMustFailWhenServiceOrderIsTooLong()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "TesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTeste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 100,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Teste"
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Service order must be less than 50 characters");

    }

    [Fact]
    public async Task ServiceOrderMustFailWhenDescriptionIsEmpty()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "",
            Total = 100,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Teste"

        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Description is required");

    }

    [Fact]
    public async Task ServiceOrderMustFailWhenDescriptionIsTooLong()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "TesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTeste",
            Total = 100,
            Entrance = new DateTime(),
            Estimated = new DateTime(),
            Status = "Teste"

        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Description must be less than 100 characters");

    }

    [Fact]
    public async Task ServiceOrderMustFailWhenEntranceIsGreaterThanEstimated()
    {
        //Arrange
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 100,
            Estimated = today,
            Status = "Teste",
            Entrance = tomorrow,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Entrance must be less than Estimated");

    }

    [Fact]
    public async Task ServiceOrderMustFailWhenEstimatedIsLessThanEntrance()
    {
        //Arrange
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 100,
            Status = "Teste",
            Entrance = tomorrow,
            Estimated = today,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Estimated must be greater than Entrance");
    }

    [Fact]
    public async Task ServiceOrderMustFailWhenStatusIsEmpty()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 100,
            Status = "",
            Entrance = new DateTime(),
            Estimated = new DateTime(),
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Status is required");

    }

    [Fact]
    public async Task ServiceOrderMustFailWhenStatusIsTooLong()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {
            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 1,
            Status = "TesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTeste",
            Entrance = new DateTime(),
            Estimated = new DateTime(),

        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Status must be less than 20 characters");
    }

    [Fact]
    public async Task ServiceOrderMustPassWhenAllFieldsAreValid()
    {
        //Arrange
        var dto = new ServiceOrderDto
        {

            ServiceOrder = "Teste",
            ClientId = new Guid(),
            VehicleId = new Guid(),
            Description = "Teste",
            Total = 1,
            Status = "Teste",
            Entrance = new DateTime(),
            Estimated = DateTime.Today.AddDays(1),
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.True(result.IsValid);
    }
}

    
