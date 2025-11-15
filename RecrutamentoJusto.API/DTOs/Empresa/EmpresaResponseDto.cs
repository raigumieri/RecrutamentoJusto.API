namespace RecrutamentoJusto.API.DTOs.Empresa
{
    /// <summary>
    /// DTO para resposta com dados de uma empresa.
    /// Retorna informações básicas sem expor relacionamentos complexos.
    /// </summary>
    public class EmpresaResponseDto
    {
        /// <summary>
        /// Identificador único da empresa.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da empresa.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da empresa.
        /// </summary>
        public string CNPJ { get; set; } = string.Empty;

        /// <summary>
        /// E-mail corporativo da empresa.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Telefone da empresa.
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Endereço da empresa.
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Data de cadastro da empresa na plataforma.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Indica se a empresa está ativa.
        /// </summary>
        public bool Ativo { get; set; }
    }
}
