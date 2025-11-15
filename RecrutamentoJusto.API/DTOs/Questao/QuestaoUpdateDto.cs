using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Questao
{
    /// <summary>
    /// DTO para atualização de uma questão existente.
    /// </summary>
    public class QuestaoUpdateDto
    {
        /// <summary>
        /// Enunciado da questão.
        /// </summary>
        [Required(ErrorMessage = "O enunciado é obrigatório.")]
        [StringLength(1000, ErrorMessage = "O enunciado deve ter no máximo 1000 caracteres.")]
        public string Enunciado { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa A.
        /// </summary>
        [Required(ErrorMessage = "A opção A é obrigatória.")]
        [StringLength(500, ErrorMessage = "A opção A deve ter no máximo 500 caracteres.")]
        public string OpcaoA { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa B.
        /// </summary>
        [Required(ErrorMessage = "A opção B é obrigatória.")]
        [StringLength(500, ErrorMessage = "A opção B deve ter no máximo 500 caracteres.")]
        public string OpcaoB { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa C.
        /// </summary>
        [Required(ErrorMessage = "A opção C é obrigatória.")]
        [StringLength(500, ErrorMessage = "A opção C deve ter no máximo 500 caracteres.")]
        public string OpcaoC { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa D.
        /// </summary>
        [Required(ErrorMessage = "A opção D é obrigatória.")]
        [StringLength(500, ErrorMessage = "A opção D deve ter no máximo 500 caracteres.")]
        public string OpcaoD { get; set; } = string.Empty;

        /// <summary>
        /// Letra da resposta correta (A, B, C ou D).
        /// </summary>
        [Required(ErrorMessage = "A resposta correta é obrigatória.")]
        [RegularExpression("^[A-D]$", ErrorMessage = "Resposta correta deve ser A, B, C ou D.")]
        public string RespostaCorreta { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação desta questão.
        /// </summary>
        [Required(ErrorMessage = "Os pontos são obrigatórios.")]
        [Range(1, 100, ErrorMessage = "Os pontos devem estar entre 1 e 100.")]
        public decimal PontosPorQuestao { get; set; } = 10.0m;

        /// <summary>
        /// Ordem de exibição da questão no teste.
        /// </summary>
        [Required(ErrorMessage = "A ordem é obrigatória.")]
        [Range(1, 1000, ErrorMessage = "A ordem deve ser maior que zero.")]
        public int Ordem { get; set; } = 1;
    }
}
