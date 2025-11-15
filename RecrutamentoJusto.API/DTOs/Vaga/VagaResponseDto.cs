namespace RecrutamentoJusto.API.DTOs.Vaga
{
    /// <summary>
    /// DTO para resposta com dados de uma vaga.
    /// </summary>
    public class VagaResponseDto
    {
        /// <summary>
        /// Identificador único da vaga.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da empresa que publicou a vaga.
        /// </summary>
        public int EmpresaId { get; set; }

        /// <summary>
        /// Nome da empresa (info adicional).
        /// </summary>
        public string? NomeEmpresa { get; set; }

        /// <summary>
        /// Título ou cargo da vaga.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Requisitos.
        /// </summary>
        public string Requisitos { get; set; } = string.Empty;

        /// <summary>
        /// Benefícios oferecidos.
        /// </summary>
        public string? Beneficios { get; set; }

        /// <summary>
        /// Salário ou faixa salarial.
        /// </summary>
        public decimal? Salario { get; set; }

        /// <summary>
        /// Localização da vaga.
        /// </summary>
        public string Localizacao { get; set; } = string.Empty;

        /// <summary>
        /// Modalidade de trabalho.
        /// </summary>
        public string Modalidade { get; set; } = string.Empty;

        /// <summary>
        /// Data de abertura da vaga.
        /// </summary>
        public DateTime DataAbertura { get; set; }

        /// <summary>
        /// Data limite para inscrições.
        /// </summary>
        public DateTime? DataFechamento { get; set; }

        /// <summary>
        /// Status da vaga.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a vaga está ativa.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Número de inscrições recebidas (informação adicional).
        /// </summary>
        public int TotalInscricoes { get; set; }
    }
}
