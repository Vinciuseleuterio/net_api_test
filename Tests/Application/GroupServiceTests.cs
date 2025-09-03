using Application.Services;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NotesApp.Application.DTOs;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NSubstitute;

namespace NotesApp.Tests.Application
{
    public class GroupServiceTests
    {
        private readonly IGroupRepository _repositoryMock;
        private readonly IValidator<GroupDto> _validatorMock;
        private readonly Group.GroupBuilder _groupBuilder;
        private readonly GroupService _sut;

        public GroupServiceTests()
        {
            _repositoryMock = Substitute.For<IGroupRepository>();
            _validatorMock = Substitute.For<IValidator<GroupDto>>();
            _groupBuilder = new Group.GroupBuilder();

            _sut = new GroupService
                (
                _repositoryMock,
                _validatorMock,
                _groupBuilder
                );
        }

        [Fact]
        public async Task CreateGroup_WithValidData_ShouldReturnCreatedUser()
        {
            // Arrange
            var userId = 1L;
            var groupDto = new GroupDto { Name = "Jhon Doe", Description = "Dev" };
            var validationResult = new ValidationResult();

            var createdGroup = _groupBuilder
                .SetName("Jhon Doe")
                .SetDescription("Dev")
                .SetCreatorId(1L)
                .Build();

            _validatorMock.Validate(groupDto).Returns(validationResult);

            _repositoryMock.CreateGroup(Arg.Is<Group>(g => g.Name == "Jhon Doe" && g.Description == "Dev"), userId).Returns(createdGroup);

            // Act
            var call = await _sut.CreateGroup(groupDto, userId);

            // Assert
            call.Should().BeEquivalentTo(createdGroup);
            await _repositoryMock.Received(1).CreateGroup(Arg.Is<Group>(g => g.Name == "Jhon Doe" && g.Description == "Dev"), userId);
        }

        [Fact]
        public async Task CreateGroup_WithInvalidData_ShoudntReturnCreatedUser()
        {
            // Arrange
            var userId = 1L;
            var groupDto = new GroupDto { Name = "", Description = "Dev" };
            var validationFailure = new List<ValidationFailure> { new(groupDto.Name, "Name is required") };
            var validationResult = new ValidationResult(validationFailure);

            _validatorMock.Validate(groupDto).Returns(validationResult);

            // Act
            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreateGroup(groupDto, userId));

            // Assert
            await _repositoryMock.DidNotReceive().CreateGroup(Arg.Is<Group>(g => g.Name == "" && g.Description == "Dev"), userId);
        }

        [Fact]
        public async Task GetGroupById_WithValidId_ReturnGroup()
        {
            //Arrange
            var userId = 1L;
            var groupId = 1L;

            var existingGroup = _groupBuilder
                .SetName("Jhon Doe")
                .SetDescription("Dev")
                .SetCreatorId(1L)
                .Build();

            _repositoryMock.GetGroupById(1L, 1L).Returns(existingGroup);

            //Act
            var call = await _sut.GetGroupById(userId, groupId);

            //Assert
            call.Should().BeEquivalentTo(existingGroup); // -> Garantindo que o retorno da chamada será o "existingGroup"
            await _repositoryMock.Received(1).GetGroupById(1L, 1L);
        }

        [Fact]
        public async Task GetGroupById_WithInvalidId_ReturnGroup()
        {
            // Arrange
            var userId = 0;
            var groupId = 1L;

            //Act
            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.GetGroupById(userId, groupId));

            //Assert
            await _repositoryMock.DidNotReceive().GetGroupById(0, 1L);
        }

        [Fact]
        public async Task GetGroupsFromUser_WithValidId_ReturnGroups()
        {
            //Arrange
            var userId = 1L;

            var existingGroup = _groupBuilder
              .SetName("Jhon Doe")
              .SetDescription("Dev")
              .SetCreatorId(1L)
              .Build();

            var expectedGroups = new List<Group> { existingGroup };

            _repositoryMock.GetGroupsFromUser(1L).Returns(expectedGroups);

            //Act
            var call = await _sut.GetGroupsFromUser(userId);

            //Assert
            call.Should().BeEquivalentTo(expectedGroups);
            await _repositoryMock.Received(1).GetGroupsFromUser(1L);
        }

        [Fact]
        public void GetGroupsFromUser_WithInvalidId_ShouldntReturnGroups()
        {
            //Arrange
            var userId = 0;

            //Act
            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.GetGroupsFromUser(userId));

            //Assert
            _repositoryMock.DidNotReceive().GetGroupsFromUser(0);
        }

        [Fact]
        public async Task UpdateGroup_WithValidData_ReturnValidGroup()
        {
            var userId = 1L;
            var groupId = 1L;

            // DTO a ser enviado como parâmetro do método
            var groupDto = new GroupDto { Name = "Jhon Doe", Description = "Dev" };

            // Retorno esperado
            var createdGroup = _groupBuilder
                .SetName("Jhon Doe")
                .SetDescription("Dev")
                .SetCreatorId(1L)
                .Build();

            var validationResult = new ValidationResult();

            // Obrigamos o resultado de validação mockado a retornar uma validação de sucesso
            _validatorMock.Validate(groupDto).Returns(validationResult);
            _repositoryMock.ExistingGroup(1L).Returns(createdGroup);
            _repositoryMock.UpdateGroup(Arg.Is<Group>(g => g.Name == "Jhon Doe" && g.Description == "Dev"), 1L, 1L).Returns(createdGroup);

            //Act
            var call = await _sut.UpdateGroup(groupDto, userId, groupId);

            call.Should().BeEquivalentTo(createdGroup);
            await _repositoryMock.Received(1).UpdateGroup(Arg.Is<Group>(g => g.Name == "Jhon Doe" && g.Description == "Dev"), 1L, 1L);

        }

        [Fact]
        public async Task UpdateGroup_WithInvalidData_ThrowsException()
        {
            var userId = 1L;
            var groupId = 1L;

            var groupDto = new GroupDto { Name = "", Description = "Dev" };

            var validationFailure = new List<ValidationFailure>();
            var valitadionResult = new ValidationResult(validationFailure);

            _validatorMock.Validate(groupDto).Returns(valitadionResult);

            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.UpdateGroup(groupDto, userId, groupId));

            await _repositoryMock.DidNotReceive().UpdateGroup(Arg.Is<Group>(g => g.Name == "Jhon Doe" && g.Description == "Dev"), 1L, 1L);
        }


        [Fact]
        public async Task DeleteGroup_WithValidId_ShouldReturnDeletedGroup()
        {
            //Arrange 
            var userId = 1L;
            var groupId = 1L;

            //Act
            await _sut.DeleteGroup(userId, groupId);

            //Assert
            await _repositoryMock.Received(1).DeleteGroup(1L, 1L);
        }

        [Fact]
        public async Task DeleteGroup_WithInvalidId_ShouldntReturnGroup()
        {
            //Arrange 
            var userId = 0L;
            var groupId = 1L;
            //Act
            var act = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeleteGroup(userId, groupId));
            //Assert
            await _repositoryMock.Received(0).DeleteGroup(1L, 1L);
        }
    }
}
