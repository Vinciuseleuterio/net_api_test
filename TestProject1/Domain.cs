using Microsoft.EntityFrameworkCore;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Data;
using NotesApp.Infrastructure.Repositories;
using NSubstitute;

namespace Tests
{
    public class Domain
    {
        private readonly IUserRepository _repository;
        private readonly User.UserBuilder _userBuilder;
        private readonly ApplicationContext _context;


        public Domain()
        {

            var options = new DbContextOptions<ApplicationContext>();
            // Mocka o DbSet<User>
            _context = Substitute.For<ApplicationContext>(options);

            // Mocka o contexto e configura para retornar o DbSet

            // Usa a implementação real do repository
            _repository = new UserRepository(_context);

            _userBuilder = new User.UserBuilder();
        }

        [Fact]
        public async Task CreateUser_WithValidRequest_ShouldCallRepository()
        {
            // Arrange
            var user = _userBuilder
                .SetName("Vinícius Eleutério")
                .SetEmail("teste@teste.com")
                .SetAboutMe("Meu nome é Vinícius!")
                .Build();

            // Act
            await _repository.CreateUser(user);

            // Assert
            //_context.Received(1).Add(Arg.Is<User>(u =>
            //    u.Name == user.Name && u.Email == user.Email));

            await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
