namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa a resposta de um candidato a uma questão específica de um teste.
    /// Registra a alternativa escolhida, se estava correta e os pontos obtidos.
    /// </summary>
    public class RespostaTeste
    {
        /// <summary>
        /// Identificador único da resposta (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da inscrição a qual esta resposta pertence (chave estrangeira).
        /// Vincula a resposta ao candidato e a vaga específica.
        /// </summary>
        public int InscricaoId { get; set; }

        /// <summary>
        /// Identificador do teste respondido (chave estrangeira).
        /// </summary>
        public int TesteId { get; set; }

        /// <summary>
        /// Identificador da questão respondida (chave estrangeira).
        /// </summary>
        public int QuestaoId { get; set; }

        /// <summary>
        /// Alternativa escolhida pelo candidato (A, B, C ou D).
        /// </summary>
        public string RespostaEscolhida { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a resposta escolhida está correta.
        /// Calculado automaticamente comparando RespostaEscolhida com Questao.RespostaCorreta.
        /// </summary>
        public bool Correta { get; set; } = false;

        /// <summary>
        /// Pontuação obtida nesta questão.
        /// Se Correta = True, recebe Questao.PontosPorQuestao.
        /// Se Correta = False, recebe 0.
        /// </summary>
        public decimal PontosObtidos { get; set; } = 0;

        /// <summary>
        /// Data e hora em que a resposta foi registrada.
        /// Útil para controlar o tempo de realização do teste.
        /// </summary>
        public DateTime DataResposta { get; set; } = DateTime.Now;


        // === RELACIONAMENTOS ===

        /// <summary>
        /// Inscrição à qual esta resposta pertence.
        /// Relacionamento: Várias respostas pertencem a uma inscrição (N:1).
        /// </summary>
        public Inscricao? Inscricao { get; set; }

        /// <summary>
        /// Teste ao qual esta resposta pertence.
        /// Relacionamento: Várias respostas pertencem a um teste (N:1).
        /// </summary>
        public Teste? Teste { get; set; }

        /// <summary>
        /// Questão que foi respondida.
        /// Relacionamento: Várias respostas pertencem a uma questão (N:1).
        /// </summary>
        public Questao? Questao { get; set; }

    }
}
