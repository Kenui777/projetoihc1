using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoihc.Models
{
    public class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnderecoId { get; set; }

        [Required(ErrorMessage = "O Logradouro é obrigatório.")]
        [StringLength(150, ErrorMessage = "O Logradouro não pode exceder 150 caracteres.")]
        public string Logradouro { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "O Bairro não pode exceder 50 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "A Localidade não pode exceder 50 caracteres.")]
        public string Localidade { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "O Complemento não pode exceder 10 caracteres.")]
        public string Complemento { get; set; } = string.Empty;

        [StringLength(2, ErrorMessage = "O UF deve ter 2 caracteres.")]
        public string UF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O CEP deve ter exatamente 9 caracteres.")]
        public string CEP { get; set; } = string.Empty;

        // Chave estrangeira para Clientes
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Clientes Cliente { get; set; }
    }
}
