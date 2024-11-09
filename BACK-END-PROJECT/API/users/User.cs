using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BACK_END_PROJECT.API.users
{
    [Table("tbUser")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Index(IsUnique = true)] 
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [ManyToMany]
        [JoinTable(
            Name = "UsersRole",
            JoinColumns = new[] { "idUser" },
            InverseJoinColumns = new[] { "idRole" }
        )]
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
