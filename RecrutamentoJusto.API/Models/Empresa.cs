namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa uma empresa cadastrada na plataforma de recrutamento.
    /// Empresas são responsáveis por criar vagas e gerenciar processos seletivos.
    /// </summary>
    public class Empresa
    {
        /// <summary>
        /// Identificador único da empresa (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da empresa.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da empresa (deve ser único no sistema).
        /// </summary>
        public string CNPJ { get; set; } = string.Empty;

        /// <summary>
        /// E-mail corporativo da empresa.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone de contato (opcional).
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Endereço da empresa (opcional).
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Data e hora do cadastro da empresa na plataforma.
        /// Preenchido automaticamente com a data atual.
        /// </summary>
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se a empresa esta ativa no sistema.
        /// True = ativa || False = inativa/bloqueada
        /// </summary>
        public bool Ativo { get; set; } = true;


        // === RELACIONAMENTOS ===

        /// <summary>
        /// Lista de vagas publicadas pela empresa.
        /// Relacionamento: Uma empresa possui varias vagas (1:N).
        /// </summary>
        public ICollection<Vaga> Vagas { get; set; } = new List<Vaga>();

        /// <summary>
        /// Lista de usuários (RH, gestores) vinculados a esta empresa.
        /// Relacionamento: Uma empresa possui vários usuários (1:N).
        /// </summary>
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
