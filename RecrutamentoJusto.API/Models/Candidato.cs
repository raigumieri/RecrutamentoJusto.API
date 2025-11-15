namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa um candidato cadastrado na plataforma.
    /// Armazena TODOS os dados pessoais e sensíveis do candidato.
    /// Esses dados NÃO são expostos durante o processo de avaliação técnica (anonimização).
    /// </summary>
    public class Candidato
    {
        /// <summary>
        /// Identificador único do candidato (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do candidato.
        /// Informação sensível - não exposta durante avaliação técnica.
        /// </summary>
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do candidato para contato.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato do candidato.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// CPF do candidato.
        /// Informação sensível - não exposta durante avaliação técnica.
        /// </summary>
        public string CPF { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do candidato.
        /// Informação sensível - não exposta para evitar viés de idade.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Gênero do candidato (opcional).
        /// Informação sensível - não exposta para evitar viés de gênero.
        /// </summary>
        public string? Genero { get; set; }

        /// <summary>
        /// Endereço completo do candidato.
        /// Informação sensível - não exposta para evitar viés de localização.
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Nível de escolaridade do candidato (ex: "Ensino Superior Completo").
        /// </summary>
        public string? Escolaridade { get; set; }

        /// <summary>
        /// Resumo da experiência profissional do candidato.
        /// Esta informação PODE ser exposta de forma anonimizada.
        /// </summary>
        public string? Experiencia { get; set; }

        /// <summary>
        /// Lista de habilidades técnicas do candidato.
        /// Esta informação PODE ser exposta de forma anonimizada.
        /// </summary>
        public string? Habilidades { get; set; }

        /// <summary>
        /// URL ou caminho do arquivo do currículo completo do candidato.
        /// </summary>
        public string? CurriculoUrl { get; set; }

        /// <summary>
        /// Data de cadastro do candidato na plataforma.
        /// </summary>
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se o candidato está ativo no sistema.
        /// </summary>
        public bool Ativo { get; set; } = true;

        // ===== RELACIONAMENTOS =====

        /// <summary>
        /// Lista de inscrições realizadas por este candidato em vagas.
        /// Relacionamento: Um candidato possui várias inscrições (1:N).
        /// </summary>
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();

    }
}
