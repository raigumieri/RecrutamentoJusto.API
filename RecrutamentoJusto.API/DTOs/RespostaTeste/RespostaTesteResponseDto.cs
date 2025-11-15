namespace RecrutamentoJusto.API.DTOs.RespostaTeste
{
    /// <summary>
    /// DTO para resposta com dados de uma resposta de teste.
    /// Não expõe a resposta correta ao candidato.
    /// </summary>
    public class RespostaTesteResponseDto
    {
        /// <summary>
        /// Identificador único da resposta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da inscrição.
        /// </summary>
        public int InscricaoId { get; set; }

        /// <summary>
        /// ID do teste.
        /// </summary>
        public int TesteId { get; set; }

        /// <summary>
        /// ID da questão.
        /// </summary>
        public int QuestaoId { get; set; }

        /// <summary>
        /// Enunciado da questão (info adicional).
        /// </summary>
        public string? EnunciadoQuestao { get; set; }

        /// <summary>
        /// Alternativa escolhida pelo candidato.
        /// </summary>
        public string RespostaEscolhida { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a resposta está correta.
        /// </summary>
        public bool Correta { get; set; }

        /// <summary>
        /// Pontuação obtida nesta questão.
        /// </summary>
        public decimal PontosObtidos { get; set; }

        /// <summary>
        /// Data e hora em que a resposta foi registrada.
        /// </summary>
        public DateTime DataResposta { get; set; }
    }
}
