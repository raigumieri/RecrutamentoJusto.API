using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Usuario
{
    /// <summary>
    /// DTO para criação de um novo usuário (RH, Gestor, Admin).
    /// </summary>
    public class UsuarioCreateDto
    {
        /// <summary>
        /// ID da empresa à qual o usuário pertence.
        /// </summary>
        [Required(ErrorMessage = "O ID da empresa é obrigatório.")]
        public int EmpresaId { get; set; }

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
        /// Senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Tipo/perfil do usuário.
        /// Valores aceitos: "RH", "Gestor", "Admin".
        /// </summary>
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [RegularExpression("^(RH|Gestor|Admin)$", ErrorMessage = "Tipo inválido. Use: RH, Gestor ou Admin.")]
        public string Tipo { get; set; } = "RH";
    }
}
