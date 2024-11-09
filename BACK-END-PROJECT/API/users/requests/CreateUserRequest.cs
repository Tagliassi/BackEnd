using System.ComponentModel.DataAnnotations;

namespace BACK_END_PROJECT.API.users.requests
{
    public class CreateUserRequest
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }

        // Método para converter para o objeto User
        public User ToUser()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
        }
    }
}
