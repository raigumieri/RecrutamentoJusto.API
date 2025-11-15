using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Empresa
{
    /// <summary>
    /// DTO para atualização de uma empresa existente.
    /// Não permite alterar CNPJ (apenas dados cadastrais).
    /// </summary>
    public class EmpresaUpdateDto
    {
        /// <summary>
        /// Nome da empresa.
        /// </summary>
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

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

        /// <summary>
        /// Status da empresa (ativa ou inativa).
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
