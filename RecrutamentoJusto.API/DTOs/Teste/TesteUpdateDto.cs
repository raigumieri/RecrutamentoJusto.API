using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Teste
{
    /// <summary>
    /// DTO para atualização de um teste existente
    /// </summary>
    public class TesteUpdateDto
    {
        /// <summary>
        /// Titulo do teste.
        /// </summary>
        [Required(ErrorMessage = "O título do teste é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição e instruções do teste (opcional).
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Duração máxima do teste em minutos.
        /// </summary>
        [Required(ErrorMessage = "A duração é obrigatória.")]
        [Range(1, 300, ErrorMessage = "A duração deve ser entre 1 e 300 minutos.")]
        public int DuracaoMinutos { get; set; } = 60;

        /// <summary>
        /// Peso deste teste na pontuação final.
        /// </summary>
        [Required(ErrorMessage = "O peso da nota é obrigatório.")]
        [Range(0.1, 10.0, ErrorMessage = "O peso deve ser entre 0.1 e 10.0.")]
        public decimal PesoNota { get; set; } = 1.0m;

        /// <summary>
        /// Status do teste (ativo ou inativo).
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
