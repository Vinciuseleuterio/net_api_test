using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NSubstitute;

namespace Tests;

public class UserServiceTest
{
    // Relembrando que, classes de teste não tem um DUI (Dependecy Injection Interface)
    private readonly IUserRepository _userRepositoryMock;
    private readonly IValidator<CreateUserDto> _createValidatorMock;
    private readonly IValidator<EditUserDto> _editValidatorMock;
    private readonly User.UserBuilder _userBuilder;
    private readonly UserService _sut; // System Under Tests

    public UserServiceTest()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _createValidatorMock = Substitute.For<IValidator<CreateUserDto>>();
        _editValidatorMock = Substitute.For<IValidator<EditUserDto>>();
        _userBuilder = new User.UserBuilder(); 

        // Inicializamos "UserService" passando para o seu construtor os mocks e builder
        _sut = new UserService(
            _userRepositoryMock,
            _createValidatorMock,
            _editValidatorMock,
            _userBuilder);
    }

    // Testes para CreateUser 
    [Fact]
    public async Task CreateUser_WithValidData_ShouldReturnCreatedUser()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "Jhon Doe", Email = "jhondoe@example", AboutMe = "Dev" };

        var validationResult = new ValidationResult(); // Resultado válido
        //var builtUser = _userBuilder.SetName(userDto.Name).SetEmail(userDto.Email).SetAboutMe(userDto.AboutMe).Build();
        var createdUser = _userBuilder
        .SetName(userDto.Name)
        .SetEmail(userDto.Email)
        .SetAboutMe(userDto.AboutMe)
        .Build();

        _createValidatorMock.Validate(userDto).Returns(validationResult);

        _userRepositoryMock.CreateUser(Arg.Is<User>(u => u.Name == "Jhon Doe" && u.Email == "jhondoe@example" && u.AboutMe == "Dev")).Returns(createdUser);
        // Args.Any<User>

        // Act
        var result = await _sut.CreateUser(userDto);

        // Assert
        result.Should().BeEquivalentTo(createdUser);
        await _userRepositoryMock.Received(1).CreateUser(Arg.Any<User>());
    }


    [Fact]
    public async Task CreateUser_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "", Email = "jhondoe@example", AboutMe = "Dev" };
        var validationFailures = new List<ValidationFailure> { new(userDto.Name, "Nome é obrigatório.") };
        var validationResult = new ValidationResult(validationFailures);

        _createValidatorMock.Validate(userDto).Returns(validationResult);

        // Act 
        await Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateUser(userDto));

        // Assert
        await _userRepositoryMock.DidNotReceive().CreateUser(Arg.Is<User>(u => u.Name == "" && u.Email == "jhondoe@example" && u.AboutMe == "Dev"));
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ShouldReturnUser()
    {
            // Arrange
        var userId = 1L;
        var expectedUser = _userBuilder.SetName("Jhon Doe").SetEmail("jhondoe@example.com").SetAboutMe("Dev").Build();
        _userRepositoryMock.GetUserById(userId).Returns(expectedUser);

        // Act
        var result = await _sut.GetUserById(userId);

        // Assert
        result.Should().Be(expectedUser);
        await _userRepositoryMock.Received(1).GetUserById(userId);
    }

    [Fact]
    public async Task UpdateUser_WithValidData_ShouldReturnUpdatedUser()
    {
        // Arrange -> Nesse bloco, iremos definir todos os retornos necessários visando que o teste seja concluído com sucesso
        var userDto = new EditUserDto { Name = "Jhon Smith", AboutMe = "Senior Dev" };
        var userId = 1L;
        var validationResult = new ValidationResult();
        var expectedResult  = _userBuilder
            .SetName(userDto.Name)
            .SetAboutMe(userDto.AboutMe)
            .Build();
        
        _editValidatorMock.Validate(userDto).Returns(validationResult);
        _userRepositoryMock.ExistingUser(userId).Returns(expectedResult );

        // Aqui, definimos que, ao chamar "UpdateUser" passando um usuário com as informações contidas no Dto, o seu retorno deve ser o expectedResult  
        _userRepositoryMock.UpdateUser(Arg.Is<User>(u => u.Name == userDto.Name && u.AboutMe == userDto.AboutMe), userId).Returns(expectedResult);

        // Act 
        var result = await _sut.UpdateUser(userDto, userId);

        // Assert
        result.Should().Be(expectedResult);
        await _userRepositoryMock.Received(1).ExistingUser(userId);
        await _userRepositoryMock.Received(1).UpdateUser(Arg.Is<User>(u => u.Name == userDto.Name && u.AboutMe == userDto.AboutMe), userId);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var userDto = new EditUserDto { Name = "", AboutMe = "" };
        var userId = 1L;
        var validationFailures = new List<ValidationFailure> { new("Name", "Nome é obrigatório.") };
        var validationResult = new ValidationResult(validationFailures);

        _editValidatorMock.Validate(userDto).Returns(validationResult);

        // Act
        Func<Task> act = async () => await _sut.UpdateUser(userDto, userId);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        await _userRepositoryMock.DidNotReceive().ExistingUser(Arg.Any<long>());
        await _userRepositoryMock.DidNotReceive().UpdateUser(Arg.Any<User>(), Arg.Any<long>());
    }


    [Fact]
    public async Task DeleteUserAsync_WithValidId_ShouldCallRepositoryDelete()
    {
        // Arrange
        var userId = 1L;
        var existingUser = _userBuilder.SetName("jhondoe").Build();
        _userRepositoryMock.ExistingUser(userId).Returns(existingUser);

        // Act
        await _sut.DeleteUserAsync(userId);

        // Assert
        await _userRepositoryMock.Received(1).ExistingUser(userId);
        await _userRepositoryMock.Received(1).DeleteUserAsync(existingUser);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteUserAsync_WithInvalidId_ShouldThrowArgumentException(long invalidUserId)
    {
        // Arrange
        // Nenhuma configuração necessária

        // Act
        Func<Task> act = async () => await _sut.DeleteUserAsync(invalidUserId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("userId");

        await _userRepositoryMock.DidNotReceive().ExistingUser(Arg.Any<long>());
        await _userRepositoryMock.DidNotReceive().DeleteUserAsync(Arg.Any<User>());
    }
}
