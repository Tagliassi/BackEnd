using BACK_END_PROJECT.API.users;
using BACK_END_PROJECT.API;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BACK_END_PROJECT.TESTS
{
    public class UserTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public UserTests()
        {
            // Configura o banco de dados em memória para os testes
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void CreateUser_ShouldAddUserToDatabase()
        {
            // Arrange: Configuração inicial
            using (var context = new ApplicationDbContext(_options))
            {
                var user = new User { Name = "Test User", Email = "test@example.com", Password = "securepassword" };

                // Act: Ação que queremos testar
                context.Users.Add(user);
                context.SaveChanges();

                // Assert: Verificações sobre o resultado
                var savedUser = context.Users.FirstOrDefault(u => u.Email == "test@example.com");
                Assert.NotNull(savedUser);
                Assert.Equal("Test User", savedUser.Name);
            }
        }

        [Fact]
        public void CreateUser_WithoutEmail_ShouldFailValidation()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var user = new User { Name = "Test User", Password = "securepassword" }; 

                Assert.Throws<DbUpdateException>(() =>
                {
                
                    context.Users.Add(user);
                    context.SaveChanges();
                });
            }
        }

        [Fact]
        public void DeleteUser_ShouldRemoveUserFromDatabase()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var user = new User { Name = "User to Delete", Email = "delete@example.com", Password = "password" };
                context.Users.Add(user);
                context.SaveChanges();

                context.Users.Remove(user);
                context.SaveChanges();

                var deletedUser = context.Users.FirstOrDefault(u => u.Email == "delete@example.com");
                Assert.Null(deletedUser);
            }
        }
    }
}
