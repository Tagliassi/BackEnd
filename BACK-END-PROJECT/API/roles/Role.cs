using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BACK_END_PROJECT.API.roles
{
    // Definindo a classe Role como uma entidade mapeada para a tabela "Roles"
    [Table("Roles")]  // Opcional, mapeia a tabela para o nome "Roles"
    public class Role
    {
        // Definindo a chave primária
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Geração automática do valor da chave
        public long? Id { get; set; }

        // Coluna "name" como única e não nula
        [Required]
        [Column("Name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        // Coluna "description" não pode ser em branco
        [Required]
        [StringLength(500)]  // Limite de tamanho para a descrição
        public string Description { get; set; }
    }
}
