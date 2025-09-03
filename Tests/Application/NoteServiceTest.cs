using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NotesApp.Application.DTOs;
using NotesApp.Application.Services;
using NotesApp.Domain.Interfaces;
using NSubstitute;

namespace repos
{
    public class NoteServiceTest
    {
        private readonly INoteRepository _noteRepositoryMock;
        private readonly IValidator<NoteDto> _noteDtoValidatorMock;
        private readonly Note.NoteBuilder _noteBuilder;
        private readonly NoteService _sut;

        public NoteServiceTest()
        {
            _noteRepositoryMock = Substitute.For<INoteRepository>();
            _noteDtoValidatorMock = Substitute.For<IValidator<NoteDto>>();
            _noteBuilder = new Note.NoteBuilder();

            _sut = new NoteService
                (
                _noteRepositoryMock,
                _noteDtoValidatorMock,
                _noteBuilder
                );
        }

        [Fact]
        public async Task CreatePersonalNote_ValidRequest_ShouldReturnCreatedNote()
        {
            var userId = 1L;
            var noteDto = new NoteDto { Title = "Jhon Doe", Content = "Dev" };

            var createdNote = _noteBuilder
                .SetTitle("Jhon Doe")
                .SetContent("Dev")
                .SetCreatorId(1L)
                .Build();

            var resultValidation = new ValidationResult();
            _noteDtoValidatorMock.Validate(noteDto).Returns(resultValidation);
            _noteRepositoryMock.CreatePersonalNote(default!, 1L).ReturnsForAnyArgs(createdNote);

            //Act
            var call = await _sut.CreatePersonalNote(noteDto, userId);

            call.Should().BeEquivalentTo(createdNote);

            await _noteRepositoryMock
                .Received(1)
                .CreatePersonalNote(Arg.Is<Note>(n => n.Title == "Jhon Doe" && n.Content == "Dev" && n.CreatorId == 1L), 1L);
        }


        [Fact]
        public async Task CreatePersonalNote_WithInvaliData_ShouldntReturnNote()
        {
            var userId = 1L;
            var noteDto = new NoteDto { Title = "", Content = "Dev" };

            var validationFailure = new List<ValidationFailure> { new(noteDto.Title, "Name is Required") };
            var validationResult = new ValidationResult(validationFailure);

            _noteDtoValidatorMock.Validate(noteDto).Returns(validationResult);

            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.CreatePersonalNote(default!, userId));

            await _noteRepositoryMock.DidNotReceive().CreatePersonalNote(default!, 1L);
        }

        [Fact]
        public async Task GetPersonalNote_WithValidId_ReturnsValidNote()
        {
            //Given
            var userId = 1L;
            var groupId = 1L;
            //Act
            var existingNote = _noteBuilder
              .SetTitle("Jhon Doe")
              .SetContent("Dev")
              .SetCreatorId(1L)
              .Build();

            _noteRepositoryMock.GetNoteById(1L, 1L).Returns(existingNote);

            //Act
            var call = await _sut.GetNoteById(userId, groupId);

            //Assert
            call.Should().BeEquivalentTo(existingNote);
            await _noteRepositoryMock.Received(1).GetNoteById(1L, 1L);
        }

        [Fact]
        public async Task GetNoteById_WithInvalidId_ShouldntReturnNote()
        {
            // Arrange
            var userId = 0;
            var noteId = 1L;

            //Act
            var call = Assert.ThrowsAsync<ValidationException>(async () => await _sut.GetNoteById(userId, noteId));

            //Assert
            await _noteRepositoryMock.DidNotReceive().GetNoteById(0, 1L);
        }

        [Fact]
        public async Task UpdateNote_WithValidData_ReturnsCreatedNote()
        {
            //Arrange
            var userId = 1L;
            var noteId = 1L;

            var noteDto = new NoteDto { Title = "Jhon Doe", Content = "Dev" };

            var createdNote = _noteBuilder
                .SetTitle("Jhon Doe")
                .SetContent("Dev")
                .SetCreatorId(1L)
                .Build();


            var validationResult = new ValidationResult();
            _noteDtoValidatorMock.Validate(noteDto).Returns(validationResult);
            _noteRepositoryMock.ExistingNote(noteId).Returns(createdNote);
            _noteRepositoryMock.UpdateNote(default!, userId, noteId).ReturnsForAnyArgs(createdNote);

            //Act
            await _sut.UpdateNote(noteDto, userId, noteId);

            //Assert
            await _noteRepositoryMock.Received(1).ExistingNote(1L);
            await _noteRepositoryMock.Received(1).UpdateNote(Arg.Any<Note>(), userId, noteId);
        }

        [Fact]
        public async Task UpdateNote_WithInvalidData_ShouldntReturnNote()
        {
            var userId = 1L;
            var noteId = 1L;

            var noteDto = new NoteDto { Title = "", Content = "Dev" };

            var validationFailure = new List<ValidationFailure> { new(noteDto.Title, "Title is Required") };
            var validationResult = new ValidationResult(validationFailure);

            _noteDtoValidatorMock.Validate(noteDto).Returns(validationResult);

            await Assert.ThrowsAsync<ValidationException>(async () => await _sut.UpdateNote(noteDto, userId, noteId));

            //Assert
            await _noteRepositoryMock.DidNotReceive().ExistingNote(1L);
            await _noteRepositoryMock.DidNotReceive().UpdateNote(default!, 1L, 1L);
        }

        // Only two more tests to go of the delete secction

        [Fact]
        public async Task DeleteNote_WithValidId_DeletesNote()
        {
            // Arrange
            var userId = 1L;
            var noteId = 1L;

            // Act
            await _sut.DeleteNote(userId, noteId);

            //Assert
            await _noteRepositoryMock.Received(1).DeleteNote(1L, 1L);
        }

        [Fact]
        public async Task DeleteNote_WithIvalidId_DoesntDeleteNote()
        {
            //Arrange
            var userId = 1L;
            var noteId = 0;
            //Act
            await Assert.ThrowsAsync<InvalidCastException>(async () => await _sut.DeleteNote(userId, noteId));
            //Assert
            await _noteRepositoryMock.DidNotReceive().DeleteNote(1L, 0);
        }
    }

}
