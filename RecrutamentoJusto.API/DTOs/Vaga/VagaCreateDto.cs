using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Vaga
{
    /// <summary>
    /// DTO para criação de uma nova vaga.
    /// </summary>
    public class VagaCreateDto
    {
        /// <summary>
        /// ID da empresa que está publicando a vaga.
        /// </summary>
        [Required(ErrorMessage = "O ID da empresa é obrigatório.")]
        public int EmpresaId { get; set; }

        /// <summary>
        /// Título ou cargo da vaga.
        /// </summary>
        [Required(ErrorMessage = "O título da vaga é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada das responsabilidades.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Requisitos obrigatórios e desejáveis.
        /// </summary>
        [Required(ErrorMessage = "Os requisitos são obrigatórios.")]
        [StringLength(2000, ErrorMessage = "Os requisitos devem ter no máximo 2000 caracteres.")]
        public string Requisitos { get; set; } = string.Empty;

        /// <summary>
        /// Benefícios oferecidos (opcional).
        /// </summary>
        [StringLength(1000, ErrorMessage = "Os benefícios devem ter no máximo 1000 caracteres.")]
        public string? Beneficios { get; set; }

        /// <summary>
        /// Salário ou faixa salarial (opcional).
        /// </summary>
        [Range(0, 999999.99, ErrorMessage = "Salário deve ser maior ou igual a zero.")]
        public decimal? Salario { get; set; }

        /// <summary>
        /// Localização da vaga.
        /// </summary>
        [Required(ErrorMessage = "A localização é obrigatória.")]
        [StringLength(200, ErrorMessage = "A localização deve ter no máximo 200 caracteres.")]
        public string Localizacao { get; set; } = string.Empty;

        /// <summary>
        /// Modalidade de trabalho.
        /// Valores aceitos: "Presencial", "Remoto", "Híbrido".
        /// </summary>
        [Required(ErrorMessage = "A modalidade é obrigatória.")]
        [RegularExpression("^(Presencial|Remoto|Híbrido)$", ErrorMessage = "Modalidade inválida. Use: Presencial, Remoto ou Híbrido.")]
        public string Modalidade { get; set; } = "Presencial";

        /// <summary>
        /// Data limite para inscrições (opcional).
        /// </summary>
        public DateTime? DataFechamento { get; set; }
    }
}
