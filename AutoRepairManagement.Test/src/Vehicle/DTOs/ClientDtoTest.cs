using FluentValidation;

public record VehicleDto()
{
   public required string Plate { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
    public required int Kilometers { get; set; }
    public Guid ClientId { get; set; }
};

public class VehicleDtoValidation : AbstractValidator<VehicleDto>
{
    public VehicleDtoValidation()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Plate is required")
            .MaximumLength(7)
            .WithMessage("Plate must be exactly than 7 characters")
            .MinimumLength(7)
            .WithMessage("Plate must be exactly than 7 characters");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .MaximumLength(50)
            .WithMessage("Plate must be less than 50 characters");
        
        RuleFor(x => x.Year)
            .GreaterThan(1886)
            .WithMessage("Year must be greater than 1886")
            .LessThan(DateTime.Now.Year)
            .WithMessage("Year must be less than current year");

        RuleFor(x => x.Kilometers)
            .GreaterThan(0)
            .WithMessage("Kilometers must be greater than 0");
    }
}


public class VehicleDtoTest
{
    private readonly VehicleDtoValidation _validator;

    public VehicleDtoTest()
    {
        _validator = new VehicleDtoValidation();
    }

    [Fact]
    public async Task VehicleMustFailWhenPlateIsEmpty()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10000,
            ClientId = new Guid()
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Plate is required");
    }

    [Fact]
    public async Task VehicleMustFailWhenPlateIsTooLong()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "TesteTeste",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10000,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Plate must be exactly than 7 characters");

    }

    [Fact]
    public async Task VehicleMustFailWhenPlateIsTooShort()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "Teste",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10000,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Plate must be exactly than 7 characters");
    }

    [Fact]
    public async Task VehicleMustFailWhenModelIsEmpty()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "",
            Year = 2020,
            Kilometers = 10000,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Model is required");
    }

    [Fact]
    public async Task VehicleMustFailWhenModelIsTooLong()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "TesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTesteTeste",
            Year = 2020,
            Kilometers = 10000,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Plate must be less than 50 characters");
    }

    [Fact]
    public async Task VehicleMustFailWhenYearIsLessThan1886()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 1885,
            Kilometers = 10,

        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Year must be greater than 1886");
    }

    [Fact]
    public async Task VehicleMustFailWhenYearIsGreaterThanCurrentYear()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 2030,
            Kilometers = 10,

        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Year must be less than current year");
    }

    [Fact]
    public async Task VehicleMustFailWhenKilometersIsLessThan1()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 2020,
            Kilometers = 0,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Kilometers must be greater than 0");
    }

    [Fact]
    public async Task VehicleMustPassWhenAllFieldsAreValid()
    {
        //Arrange
        var dto = new VehicleDto
        {
            Plate = "ABC1234",
            Model = "Teste",
            Year = 2020,
            Kilometers = 10,
        };

        //Act
        var result = _validator.Validate(dto);

        //Assert
        Assert.True(result.IsValid);
    }
}
