namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa um teste ou avaliação técnica associada a uma vaga.
    /// Uma vaga pode ter múltiplos testes (ex: teste de lógica, teste técnico, teste comportamental).
    /// </summary>
    public class Teste
    {
        /// <summary>
        /// Identificador único do teste (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da vaga a qual este teste pertence (chave estrangeira).
        /// </summary>
        public int VagaId { get; set; }

        /// <summary>
        /// Titulo do teste (ex: "Avaliação de Conhecimentos Técnicos em C#).
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição e instruções do teste para o candidato.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Duração máxima do teste em minutos.
        /// Candidato terá este tempo para responder todas as perguntas.
        /// </summary>
        public int DuracaoMinutos { get; set; } = 60;

        /// <summary>
        /// Peso deste teste na pontuação final.
        /// Permite dar mais importância a alguns testes que a outros.
        /// Exemplo: Se um teste tem peso 2.0 e outro peso 1.0, o primerio vale o dobro.
        /// </summary>
        public decimal PesoNota { get; set; } = 1.0m;

        /// <summary>
        /// Data de criação do teste.
        /// </summary>
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se o teste está ativo e disponível para uso.
        /// </summary>
        public bool Ativo { get; set; } = true;

        // === RELACIONAMENTOS ===

        /// <summary>
        /// Vaga à qual este teste pertence.
        /// Relacionamento: Vários testes pertencem a uma vaga (N:1).
        /// </summary>
        public Vaga? Vaga { get; set; }

        /// <summary>
        /// Lista de questões que compõem este teste.
        /// Relacionamento: Um teste possui várias questões (1:N).
        /// </summary>
        public ICollection<Questao> Questoes { get; set; } = new List<Questao>();

        /// <summary>
        /// Lista de respostas dadas por candidatos a este teste.
        /// Relacionamento: Um teste possui várias respostas (1:N).
        /// </summary>
        public ICollection<RespostaTeste> Respostas { get; set; } = new List<RespostaTeste>();





    }
}
