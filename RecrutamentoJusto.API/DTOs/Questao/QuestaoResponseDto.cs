namespace RecrutamentoJusto.API.DTOs.Questao
{
    /// <summary>
    /// DTO para resposta com dados de uma questão.
    /// A resposta correta não deve ser exposta ao candidato durante o teste.
    /// </summary>
    public class QuestaoResponseDto
    {
        /// <summary>
        /// Identificador único da questão.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do teste ao qual esta questão pertence.
        /// </summary>
        public int TesteId { get; set; }

        /// <summary>
        /// Título do teste (info adicional).
        /// </summary>
        public string? TituloTeste { get; set; }

        /// <summary>
        /// Enunciado da questão.
        /// </summary>
        public string Enunciado { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa A.
        /// </summary>
        public string OpcaoA { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa B.
        /// </summary>
        public string OpcaoB { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa C.
        /// </summary>
        public string OpcaoC { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa D.
        /// </summary>
        public string OpcaoD { get; set; } = string.Empty;

        /// <summary>
        /// Letra da resposta correta (A, B, C ou D).
        /// Não expor ao candidato durante o teste.
        /// </summary>
        public string RespostaCorreta { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação desta questão.
        /// </summary>
        public decimal PontosPorQuestao { get; set; }

        /// <summary>
        /// Ordem de exibição no teste.
        /// </summary>
        public int Ordem { get; set; }
    }
}
