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
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
 
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
