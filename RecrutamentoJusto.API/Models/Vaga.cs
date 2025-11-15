using static System.Net.Mime.MediaTypeNames;

namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa uma vaga de emprego publicada por uma empresa.
    /// Contem todas as informacoes sobre a oportunidade de trabalho.
    /// </summary>
    public class Vaga
    {
        /// <summary>
        /// Identificador único da vaga (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da empresa que publicou a vaga (chave estrangeira).
        /// </summary>
        public int EmpresaId { get; set; }

        /// <summary>
        /// Titulo ou cargo da vaga (ex: "Desenvolvedor Full Stack").
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descricao detalhada das responsabilidades e atribuicoes da vaga.
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Requisitos obrigatórios e desejáveis para a vaga.
        /// </summary>
        public string Requisitos { get; set; } = string.Empty;

        /// <summary>
        /// Benefícios oferecidos pela empresa (ex: vale-refeição, plano de saúde).
        /// </summary>
        public string? Beneficios { get; set; }

        /// <summary>
        /// Salário ou faixa salarial oferecida (opcional).
        /// </summary>
        public decimal? Salario { get; set; }

        /// <summary>
        /// Localização da vaga (cidade/estado ou "Remoto").
        /// </summary>
        public string Localizacao { get; set; } = string.Empty;

        /// <summary>
        /// Modalidade de trabalho.
        /// Valores possíveis: "Presencial", "Remoto", "Híbrido".
        /// </summary>
        public string Modalidade { get; set; } = "Presencial";

        /// <summary>
        /// Data de abertura/publicação da vaga.
        /// </summary>
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        /// <summary>
        /// Data limite para inscrições na vaga (opcional).
        /// </summary>
        public DateTime? DataFechamento { get; set; }

        /// <summary>
        /// Status atual da vaga.
        /// Valores possíveis: "Aberta", "Fechada", "Pausada".
        /// </summary>
        public string Status { get; set; } = "Aberta";

        /// <summary>
        /// Indica se a vaga está ativa no sistema.
        /// </summary>
        public bool Ativo { get; set; } = true;


        // === RELACIONAMENTOS ===

        /// <summary>
        /// Empresa que publicou esta vaga.
        /// Relacionamento: Várias vagas pertencem a uma empresa (N:1).
        /// </summary>
        public Empresa? Empresa { get; set; }

        /// <summary>
        /// Lista de inscrições recebidas para esta vaga.
        /// Relacionamento: Uma vaga possui várias inscrições (1:N).
        /// </summary>
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();

        /// <summary>
        /// Lista de testes/avaliações técnicas associadas a esta vaga.
        /// Relacionamento: Uma vaga possui vários testes (1:N).
        /// </summary>
        public ICollection<Teste> Testes { get; set; } = new List<Teste>();
    }
}
