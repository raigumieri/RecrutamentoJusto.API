namespace RecrutamentoJusto.API.DTOs.Candidato
{
    /// <summary>
    /// DTO para resposta com dados completos de um candidato.
    /// Este DTO expõe informações sensíveis e só deve ser usado:
    /// - Pelo próprio candidato visualizando seu perfil
    /// - Por RH após a identidade ter sido revelada (IdentidadeRevelada = true)
    /// </summary>
    public class CandidatoResponseDto
    {
        /// <summary>
        /// Identificador único do candidato.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do candidato.
        /// Info sensível - não expor durante avaliação técnica.
        /// </summary>
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do candidato.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// CPF do candidato.
        /// Info sensível - não expor durante avaliação técnica.
        /// </summary>
        public string CPF { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do candidato.
        /// Info sensível - não expor durante avaliação técnica.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Gênero do candidato.
        /// Info sensível - não expor durante avaliação técnica.
        /// </summary>
        public string? Genero { get; set; }

        /// <summary>
        /// Endereço completo do candidato.
        /// Info sensível - não expor durante avaliação técnica.
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Nível de escolaridade.
        /// </summary>
        public string? Escolaridade { get; set; }

        /// <summary>
        /// Resumo da experiência profissional.
        /// </summary>
        public string? Experiencia { get; set; }

        /// <summary>
        /// Lista de habilidades técnicas.
        /// </summary>
        public string? Habilidades { get; set; }

        /// <summary>
        /// URL do currículo completo.
        /// </summary>
        public string? CurriculoUrl { get; set; }

        /// <summary>
        /// Data de cadastro na plataforma.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Indica se o candidato está ativo.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Número de inscrições realizadas (informação adicional).
        /// </summary>
        public int TotalInscricoes { get; set; }
    }
}
