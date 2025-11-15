using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.RespostaTeste
{
    /// <summary>
    /// DTO para registrar a resposta de um candidato a uma questão.
    /// O sistema automaticamente valida se está correta e calcula os pontos.
    /// </summary>
    public class RespostaTesteCreateDto
    {
        /// <summary>
        /// ID da inscrição (vincula candidato + vaga).
        /// </summary>
        [Required(ErrorMessage = "O ID da inscrição é obrigatório.")]
        public int InscricaoId { get; set; }

        /// <summary>
        /// ID do teste sendo respondido.
        /// </summary>
        [Required(ErrorMessage = "O ID do teste é obrigatório.")]
        public int TesteId { get; set; }

        /// <summary>
        /// ID da questão sendo respondida.
        /// </summary>
        [Required(ErrorMessage = "O ID da questão é obrigatório.")]
        public int QuestaoId { get; set; }

        /// <summary>
        /// Alternativa escolhida pelo candidato (A, B, C ou D).
        /// </summary>
        [Required(ErrorMessage = "A resposta escolhida é obrigatória.")]
        [RegularExpression("^[A-D]$", ErrorMessage = "A resposta deve ser: A, B, C ou D.")]
        public string RespostaEscolhida { get; set; } = string.Empty;
    }
}
