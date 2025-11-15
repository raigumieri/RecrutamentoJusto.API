namespace RecrutamentoJusto.API.DTOs.Usuario
{
    /// <summary>
    /// DTO para resposta com dados de um usuário.
    /// NÃO expõe a senha por questões de segurança.
    /// </summary>
    public class UsuarioResponseDto
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da empresa a qual o usuário pertence.
        /// </summary>
        public int EmpresaId { get; set; }

        /// <summary>
        /// Nome da empresa (info adicional).
        /// </summary>
        public string? NomeEmpresa { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do usuário.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Tipo/perfil do usuário (RH, Gestor, Admin).
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Data de cadastro do usuário.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo.
        /// </summary>
        public bool Ativo { get; set; }
    }
}
