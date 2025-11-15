using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Candidato
{
    /// <summary>
    /// DTO para criação de um novo candidato.
    /// Coleta todos os dados, incluindo informações sensíveis que serão anonimizadas posteriormente.
    /// </summary>
    public class CandidatoCreateDto
    {
        /// <summary>
        /// Nome completo do candidato.
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do candidato.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato.
        /// </summary>
        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Telefone { get; set; }

        /// <summary>
        /// CPF do candidato (apenas números).
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        public string CPF { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento do candidato.
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Gênero do candidato (opcional).
        /// </summary>
        [StringLength(50, ErrorMessage = "O gênero deve ter no máximo 50 caracteres.")]
        public string? Genero { get; set; }

        /// <summary>
        /// Endereço completo do candidato.
        /// </summary>
        [StringLength(300, ErrorMessage = "O endereço deve ter no máximo 300 caracteres.")]
        public string? Endereco { get; set; }

        /// <summary>
        /// Nível de escolaridade.
        /// </summary>
        [StringLength(100, ErrorMessage = "A escolaridade deve ter no máximo 100 caracteres.")]
        public string? Escolaridade { get; set; }

        /// <summary>
        /// Resumo da experiência profissional.
        /// </summary>
        [StringLength(2000, ErrorMessage = "A experiência deve ter no máximo 2000 caracteres.")]
        public string? Experiencia { get; set; }

        /// <summary>
        /// Lista de habilidades técnicas.
        /// </summary>
        [StringLength(1000, ErrorMessage = "As habilidades devem ter no máximo 1000 caracteres.")]
        public string? Habilidades { get; set; }

        /// <summary>
        /// URL ou caminho do currículo completo.
        /// </summary>
        [StringLength(500, ErrorMessage = "A URL do currículo deve ter no máximo 500 caracteres.")]
        public string? CurriculoUrl { get; set; }

    }
}
