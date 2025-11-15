using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Usuario
{
    /// <summary>
    /// DTO para atualização de um usuário existente.
    /// </summary>
    public class UsuarioUpdateDto
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Tipo/perfil do usuário.
        /// </summary>
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [RegularExpression("^(RH|Gestor|Admin)$", ErrorMessage = "Tipo inválido. Use: RH, Gestor ou Admin.")]
        public string Tipo { get; set; } = "RH";

        /// <summary>
        /// Status do usuário (ativo ou inativo).
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
