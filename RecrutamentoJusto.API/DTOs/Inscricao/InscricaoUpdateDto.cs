using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Inscricao
{
    /// <summary>
    /// DTO para atualização de uma inscrição existente.
    /// Permite alterar status e feedback.
    /// </summary>
    public class InscricaoUpdateDto
    {
        /// <summary>
        /// Status da inscrição.
        /// Valores aceitos: "Inscrito", "EmAvaliaçcao", "AprovadoTecnico", "Reprovado", "Contratado".
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [RegularExpression("^(Inscrito|EmAvaliacao|AprovadoTecnico|Reprovado|Contratado)$",
            ErrorMessage = "Status inválido. Use: Inscrito, EmAvaliacao, AprovadoTecnico, Reporvado, Contratado.")]
        public string Status { get; set; } = "Inscrito";

        /// <summary>
        /// Feedback para o candidato (opcional).
        /// </summary>
        [StringLength(2000, ErrorMessage = "O feedback deve ter no máximo 2000 caracteres.")]
        public string? Feedback { get; set; }
    }
}
