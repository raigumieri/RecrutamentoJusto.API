namespace RecrutamentoJusto.API.DTOs.Teste
{
    /// <summary>
    /// DTO para resposta com dados de um teste.
    /// </summary>
    public class TesteResponseDto
    {
        /// <summary>
        /// Identificador único do teste.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da vaga a qual este teste pertence.
        /// </summary>
        public int VagaId { get; set; }

        /// <summary>
        /// Título da vaga (info adicional).
        /// </summary>
        public string? TituloVaga { get; set; }

        /// <summary>
        /// Título do teste.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição e instruções.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Duração máxima em minutos.
        /// </summary>
        public int DuracaoMinutos { get; set; }

        /// <summary>
        /// Peso na pontuação final.
        /// </summary>
        public decimal PesoNota { get; set; }

        /// <summary>
        /// Data de criação do teste.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Status do teste.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Número de questões neste teste (info adicional).
        /// </summary>
        public int TotalQuestoes { get; set; }
    }
}
