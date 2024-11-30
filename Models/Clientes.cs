using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoihc.Models
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório.")]
        [StringLength(12, ErrorMessage = "O RG deve ter no máximo 12 caracteres.")]
        public string RG { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O estado civil é obrigatório.")]
        [StringLength(20, ErrorMessage = "O estado civil deve ter no máximo 20 caracteres.")]
        public string EstadoCivil { get; set; }

        [StringLength(100, ErrorMessage = "O nome do pai deve ter no máximo 100 caracteres.")]
        public string NomePai { get; set; }

        [StringLength(100, ErrorMessage = "O nome da mãe deve ter no máximo 100 caracteres.")]
        public string NomeMae { get; set; }

        // Relacionamento com Endereco
        [Required]
        public int EnderecoId { get; set; }

        public Endereco Endereco { get; set; }
    }
}
