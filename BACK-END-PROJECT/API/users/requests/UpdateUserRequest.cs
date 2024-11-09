using System.ComponentModel.DataAnnotations;

namespace BACK_END_PROJECT.API.users.requests
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(1, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }

        // Construtor opcional para inicializar
        public UpdateUserRequest() { }

        // Construtor com parâmetro
        public UpdateUserRequest(string name)
        {
            Name = name;
        }
    }
}
