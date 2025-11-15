using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Empresa
{
    /// <summary>
    /// DTO para criação de uma nova empresa.
    /// Contém apenas os dados necessários para cadastro, sem IDs ou relacionamentos.
    /// </summary>
    public class EmpresaCreateDto
    {
        /// <summary>
        /// Nome da empresa.
        /// </summary>
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da empresa (apenas numeros).
        /// </summary>
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 números.")]
        public string CNPJ { get; set; } = string.Empty;

        /// <summary>
        /// E-mail da empresa.
        /// </summary>
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato (opcional).
        /// </summary>
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Telefone { get; set; }

        /// <summary>
        /// Endereço completo da empresa (opcional).
        /// </summary>
        [StringLength(300, ErrorMessage = "O endereço deve ter no máximo 300 caracteres.")]
        public string? Endereco { get; set; }
    }
}
