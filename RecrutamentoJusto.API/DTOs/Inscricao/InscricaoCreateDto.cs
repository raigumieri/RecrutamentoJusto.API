using System.ComponentModel.DataAnnotations;

namespace RecrutamentoJusto.API.DTOs.Inscricao
{
    /// <summary>
    /// DTO para criação de uma nova inscrição.
    /// Ao criar a inscrição, o sistema automaticamente gera o currículo anonimizado.
    /// </summary>
    public class InscricaoCreateDto
    {
        /// <summary>
        /// ID da vaga na qual o candidato deseja se inscrever.
        /// </summary>
        [Required(ErrorMessage = "O ID da vaga é obrigatório.")]
        public int VagaId { get; set; }

        /// <summary>
        /// ID do candidato que está se inscrevendo.
        /// </summary>
        [Required(ErrorMessage = "O ID do candidato é obrigatório.")]
        public int CandidatoId { get; set; }
    }
}
