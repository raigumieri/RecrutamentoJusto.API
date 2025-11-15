namespace RecrutamentoJusto.API.DTOs.Inscricao
{
    /// <summary>
    /// DTO para resposta com dados de uma inscrição.
    /// Este DTO mostra apenas dados anonimizados enquanto IdentidadeRevelada = false.
    /// </summary>
    public class InscricaoResponseDto
    {
        /// <summary>
        /// Idetificador único da inscrição.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da vaga.
        /// </summary>
        public int VagaId { get; set; }

        /// <summary>
        /// Título da vaga (info adicional).
        /// </summary>
        public string? TituloVaga { get; set; }

        /// <summary>
        /// ID do candidato.
        /// </summary>
        public int CandidatoId { get; set; }

        /// <summary>
        /// Nome do candidato (apenas se IdentidadeRevelada = true, caso contrário retorna "Candidato Anônimo".
        /// </summary>
        public string? NomeCandidato { get; set; }

        /// <summary>
        /// Data da inscrição.
        /// </summary>
        public DateTime DataInscricao { get; set; }

        /// <summary>
        /// Status da inscrição.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação total obtida nos testes.
        /// </summary>
        public decimal PontuacaoTotal { get; set; }

        /// <summary>
        /// Indica se a identidade foi revelada ao RH.
        /// </summary>
        public bool IdentidadeRevelada { get; set; }

        /// <summary>
        /// Curriculo anonimizado (sem nome, idade, gênero, endereço).
        /// </summary>
        public string? CurriculoAnonimizado { get; set; }

        /// <summary>
        /// Feedback fornecido ao candidato.
        /// </summary>
        public string? Feedback { get; set; }
    }
}
