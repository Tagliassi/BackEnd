using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BACK_END_PROJECT.API.roles;

namespace BACK_END_PROJECT.API.users
{
    [Table("tbUser")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")] // Defina explicitamente o tipo do campo para MySQL
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Column(TypeName = "VARCHAR(450)")] // Definindo um tamanho específico para o email
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "TEXT")] // Para o campo de senha, podemos usar TEXT, já que pode ser longo
        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
