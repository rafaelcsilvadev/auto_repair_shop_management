using FluentValidation;

public record ClientDto()
{
    public required string Name { get; set; }
    public required string Email { get; set; }
};

public class ClientDtoValidation : AbstractValidator<ClientDto>
{
    public ClientDtoValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(10)
            .WithMessage("Name must be less than 10 characters");
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address");

    }
}


public class ClientDtoTest
{
    private readonly ClientDtoValidation _validator;

    public ClientDtoTest()
    {
        _validator = new ClientDtoValidation();
    }


    [Fact]
    public void MustFailWhenNameIsEmpty()
    {
        //Arrange
        var dto = new ClientDto { Email = "email@email.com", Name = "" };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void MustFailWhenNameIsTooLong()
    {
        //Arrange
        var dto = new ClientDto { Email = "email@email.com", Name = "TesteTesteTeste" };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Name must be less than 10 characters");
    }

    [Fact]
    public void MustFailWhenEmailIsEmpty()
    {
        //Arrange
        var dto = new ClientDto { Email = "", Name = "Teste" };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Email is required");
    }

    [Fact]
    public void MustFailWhenEmailIsInvalid()
    {
        //Arrange
        var dto = new ClientDto { Email = "", Name = "Teste" };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Invalid email address");
    }

    [Fact]
    public void MustPassWhenAllFieldsAreValid()
    {
        //Arrange
        var dto = new ClientDto { Email = "email@email.com", Name = "Teste" };

        //Act
        var result = _validator.Validate(dto);

        //Assert  
        Assert.True(result.IsValid);
    }
}