namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa uma questão de múltipla escolha dentro de um teste.
    /// Cada questão possui 4 alternativas (A, B, C, D) e uma resposta correta.
    /// </summary>
    public class Questao
    {
        /// <summary>
        /// Identificador único da questão (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do teste ao qual esta questão pertence (chave estrangeira).
        /// </summary>
        public int TesteId { get; set; }

        /// <summary>
        /// Enunciado da questão (pergunta a ser respondida pelo candidato).
        /// </summary>
        public string Enunciado { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa A.
        /// </summary>
        public string OpcaoA { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa B.
        /// </summary>
        public string OpcaoB { get; set; } = string.Empty;

        /// <summary>
        /// Texto da alternativa C.
        /// </summary>
        public string OpcaoC { get; set; } = string.Empty;

        /// <summary>
        /// Texto para alternativa D.
        /// </summary>
        public string OpcaoD { get; set; } = string.Empty;

        /// <summary>
        /// Letra da resposta correta (A, B, C ou D).
        /// Utilizada para correção automática das respostas dos candidatos.
        /// Esta informação NÃO deve ser exposta ao candidato via API.
        /// </summary>
        public string RespostaCorreta { get; set; } = string.Empty;

        /// <summary>
        /// Pontuação que o candidato ganha ao acertar esta questão.
        /// Permite criar questões com pesos diferentes dentro do mesmo teste
        /// </summary>
        public decimal PontosPorQuestao { get; set; } = 10.0m;

        /// <summary>
        /// Ordem de exibição da questão dentro do teste.
        /// Permite organizar as questões em uma sequência lógica.
        /// </summary>
        public int Ordem { get; set; } = 1;


        // === RELACIONAMENTOS ===

        /// <summary>
        /// Teste ao qual esta questão pertence.
        /// Relacionamento: Várias questões pertencem a um teste (N:1).
        /// </summary>
        public Teste? Teste { get; set; }

        /// <summary>
        /// Lista de respostas dadas por candidatos a esta questão específica.
        /// Relacionamento: Uma questão possui várias respostas (1:N).
        /// </summary>
        public ICollection<RespostaTeste> Respostas { get; set; } = new List<RespostaTeste>();
    }
}
