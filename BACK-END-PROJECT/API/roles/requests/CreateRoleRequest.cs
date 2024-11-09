using System.ComponentModel.DataAnnotations;

namespace BACK_END_PROJECT.API.roles.requests
{
    public class CreateRoleRequest
    {
        // Validação para o nome da role (apenas letras maiúsculas e números)
        [Required]
        [RegularExpression("^[A-Z][0-9A-Z]*$", ErrorMessage = "O nome deve começar com uma letra maiúscula e pode conter números e letras maiúsculas.")]
        public string Name { get; set; }

        // Validação para a descrição da role (não pode ser em branco)
        [Required]
        public string Description { get; set; }

        // Método que converte para a entidade Role
        public Role ToRole()
        {
            return new Role
            {
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}
