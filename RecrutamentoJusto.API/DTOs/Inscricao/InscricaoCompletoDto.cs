namespace RecrutamentoJusto.API.DTOs.Inscricao
{
    /// <summary>
    /// DTO completo de inscrição com TODOS os dados do candidato.
    /// Só deve ser usado quando IdentidadeRevelada = true.
    /// Expõe informações sensíveis do candidato.
    /// </summary>
    public class InscricaoCompletoDto
    {
        /// <summary>
        /// Identificador único da inscrição.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da vaga.
        /// </summary>
        public int VagaId { get; set; }

        /// <summary>
        /// Título da vaga.
        /// </summary>
        public string? TituloVaga { get; set; }

        /// <summary>
        /// ID do candidato.
        /// </summary>
        public int CandidatoId { get; set; }


        // === DADOS COMPLETOS DO CANDIDATO (SENSÍVEIS) ===

        /// <summary>
        /// Nome completo do candidato.
        /// </summary>
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do candidato.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone do candidato.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// CPF do candidato.
        /// </summary>
        public string CPF { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do candidato.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Gênero do candidato.
        /// </summary>
        public string? Genero { get; set; }

        /// <summary>
        /// Endereço do candidato.
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Escolaridade.
        /// </summary>
        public string? Escolaridade { get; set; }

        /// <summary>
        /// Experiência profissional.
        /// </summary>
        public string? Experiencia { get; set; }

        /// <summary>
        /// Habilidades técnicas.
        /// </summary>
        public string? Habilidades { get; set; }


        // === DADOS DA INSCRIÇÃO ===

        /// <summary>
        /// Data da inscrição.
        /// </summary>
        public DateTime DataInscricao { get; set; }

        /// <summary>
        /// Status da inscrição.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação total obtida.
        /// </summary>
        public decimal PontuacaoTotal { get; set; }

        /// <summary>
        /// Indica se a identidade foi revelada.
        /// </summary>
        public bool IdentidadeRevelada { get; set; }

        /// <summary>
        /// Feedback fornecido.
        /// </summary>
        public string? Feedback { get; set; }
    }
}
