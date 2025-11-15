namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa a inscrição de um candidato em uma vaga.
    /// Esta é a entidade CENTRAL da plataforma de recrutamento justo.
    /// Armazena o currículo anonimizado e controla a visibilidade da identidade do candidato.
    /// </summary>
    public class Inscricao
    {
        /// <summary>
        /// Identificador único da inscrição (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da vaga na qual o candidato se inscreveu (chave estrangeira).
        /// </summary>
        public int VagaId { get; set; }

        /// <summary>
        /// Identificador do candidato que realizou a inscrição (chave estrangeira).
        /// </summary>
        public int CandidatoId { get; set; }

        /// <summary>
        /// Data e hora que a inscrição foi realizada.
        /// </summary>
        public DateTime DataInscricao { get; set; } = DateTime.Now;

        /// <summary>
        /// Status atual da inscrição no processo seletivo.
        /// Valores possiveis: 
        /// - "Inscrito": Candidato se inscreveu, aguardando avaliação.
        /// - "Em Avaliação": Candidato está realizando testes.
        /// - "Aprovado Tecnico": Passou na avaliação técnica, identidade pode ser revelada.
        /// - "Reprovado": Não passou na avaliação técnica.
        /// - "Contratado": Foi selecionado pela empresa.
        /// </summary>
        public string Status { get; set; } = "Inscrito";

        /// <summary>
        /// Pontuação total obtida pelo candidato nos testes técnicos.
        /// Calculada automaticamente pela soma das RespostasTeste.
        /// </summary>
        public decimal PontuacaoTotal { get; set; } = 0;

        /// <summary>
        /// Indica se a identidade real do candidato foi revelada ao RH.
        /// False: RH vê apenas dados anonimizados e pontuação.
        /// True: RH pode visualizar nome, idade, gênero e outros dados sensíveis.
        /// Esta flag é ativada quando Status = "Aprovado Tecnico". 
        /// </summary>
        public bool IdentidadeRevelada { get; set; } = false;

        /// <summary>
        /// Versão anonimizada do currículo do candidato.
        /// Remove informações sensíveis como nome, idade, gênero, endereço, foto.
        /// Mantém apenas: experiência profissional, habilidades, escolaridade.
        /// Este é o conteúdo exibido ao RH durante a avaliação técnica.
        /// </summary>
        public string? CurriculoAnonimizado { get; set; }

        /// <summary>
        /// Feedback fornecido pelo RH ou sistema ao candidato (opcional).
        /// Pode conter motivo da reprovação ou recomendações de capacitação.
        /// </summary>
        public string? Feedback { get; set; }


        // === RELACIONAMENTOS ===

        /// <summary>
        /// Vaga à qual esta inscrição pertence.
        /// Relacionamento: Várias inscrições pertencem a uma vaga (N:1).
        /// </summary>
        public Vaga? Vaga { get; set; }

        /// <summary>
        /// Candidato que realizou esta inscrição.
        /// Relacionamento: Várias inscrições pertencem a um candidato (N:1).
        /// </summary>
        public Candidato? Candidato { get; set; }

        /// <summary>
        /// Lista de respostas dadas pelo candidato nos testes desta inscrição.
        /// Relacionamento: Uma inscrição possui várias respostas (1:N).
        /// </summary>
        public ICollection<RespostaTeste> Respostas { get; set; } = new List<RespostaTeste>();

    }
}
