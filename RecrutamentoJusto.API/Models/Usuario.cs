namespace RecrutamentoJusto.API.Models
{
    /// <summary>
    /// Representa um usuário do sistema (RH, gestor ou administrador).
    /// Usuários são vinculados a empresas e gerenciam processos seletivos
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuario (chave primaria).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da empresa a qual o usuario pertence (chave estrangeira).
        /// </summary>
        public int EmpresaId { get; set; }

        /// <summary>
        /// Nome completo do usuario.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do usuario para login e contato.
        /// Deve ser único no sistema.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuario (deve ser criptografada em produção).
        /// </summary>
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Tipo/perfil do usuário no sistema.
        /// Valores possíveis: "RH", "Gestor", "Admin"
        /// </summary>
        public string Tipo { get; set; } = "RH";

        /// <summary>
        /// Data e hora do cadastro do usuário.
        /// </summary>
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        /// <summary>
        /// Indica se o usuário está ativo no sistema.
        /// </summary>
        public bool Ativo { get; set; } = true;

        // === RELACIONAMENTOS ===

        /// <summary>
        /// Empresa a qual este usuário pertence.
        /// Relacionamento: Vários usuários pertencem a uma empresa (N:1).
        /// </summary>
        public Empresa? Empresa { get; set; }
    }
}
