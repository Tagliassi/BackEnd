using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BACK_END_PROJECT.API.users;

namespace BACK_END_PROJECT.API.roles
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? Id { get; set; }

        [Required]
        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        // Relação muitos-para-muitos com User
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
