using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Models;

namespace RecrutamentoJusto.API.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação.
    /// Responsável por gerenciar a conexão com o SQL Server e mapear as entidades (Models) para tabelas.
    /// Utiliza o Entity Framework Core para realizar operações de CRUD.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração do DbContext.
        /// As opções incluem a string de conexão e o provider do banco (SQL Server).
        /// </summary>
        /// <param name="options">Opções de configuração do Entity Framework Core.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // === DBSETS - Representam as tabelas no banco de dados ===

        /// <summary>
        /// Tabela de Empresas cadastradas na plataforma.
        /// </summary>
        public DbSet<Empresa> Empresas { get; set; }

        /// <summary>
        /// Tabela de Usuários do sistema (RH, gestores, admins).
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Tabela de Vagas publicadas pelas empresas.
        /// </summary>
        public DbSet<Vaga> Vagas { get; set; }

        /// <summary>
        /// Tabela de Candidatos cadastrados na plataforma.
        /// </summary>
        public DbSet<Candidato> Candidatos { get; set; }

        /// <summary>
        /// Tabela de Inscrições (relacionamento entre Candidatos e Vagas).
        /// Tabela central que controla o processo seletivo e anonimização.
        /// </summary>
        public DbSet<Inscricao> Inscricoes { get; set; }

        /// <summary>
        /// Tabela de Testes/Avaliações técnicas das vagas.
        /// </summary>
        public DbSet<Teste> Testes { get; set; }

        /// <summary>
        /// Tabela de Questões dos Testes.
        /// </summary>
        public DbSet<Questao> Questoes { get; set; }

        /// <summary>
        /// Tabela de Respostas dadas pelos candidatos nos testes.
        /// </summary>
        public DbSet<RespostaTeste> RespostasTeste { get; set; }


        /// <summary>
        /// Método chamado durante a criação do modelo de dados.
        /// Utilizado para configurar relacionamentos, restrições, índices e regras de negócio
        /// </summary>
        /// <param name="modelBuilder">Construtor do modelo de dados</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === CONFIGURAÇÕES DE RELACIONAMENTOS ===

            // Empresa -> Vagas (1:N)
            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Vagas)
                .WithOne(v => v.Empresa)
                .HasForeignKey(v => v.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict); // Não permite deletar empresa se tiver vagas.

            // Empresa -> Usuarios (1:N)
            modelBuilder.Entity<Empresa>()
                .HasMany(e => e.Usuarios)
                .WithOne(u => u.Empresa)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vaga -> Inscricoes (1:N)
            modelBuilder.Entity<Vaga>()
                .HasMany(v => v.Inscricoes)
                .WithOne(i => i.Vaga)
                .HasForeignKey(i => i.VagaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vaga -> Testes (1:N)
            modelBuilder.Entity<Vaga>()
                .HasMany(v => v.Testes)
                .WithOne(t => t.Vaga)
                .HasForeignKey(t => t.VagaId)
                .OnDelete(DeleteBehavior.Cascade); // Ao deletar vaga, deleta os testes 

            // Candidato -> Inscricoes (1:N)
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Inscricoes)
                .WithOne(i => i.Candidato)
                .HasForeignKey(i => i.CandidatoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Inscricao -> RespostasTestes (1:N)
            modelBuilder.Entity<Inscricao>()
                .HasMany(i => i.Respostas)
                .WithOne(r => r.Inscricao)
                .HasForeignKey(r => r.InscricaoId)
                .OnDelete(DeleteBehavior.Cascade); // Ao deletar inscrição, deleta as respostas

            // Teste -> Questoes (1:N)
            modelBuilder.Entity<Teste>()
                .HasMany(t => t.Questoes)
                .WithOne(q => q.Teste)
                .HasForeignKey(q => q.TesteId)
                .OnDelete(DeleteBehavior.Cascade); // Ao deletar teste, deleta as questões

            // Teste -> RespostasTestes (1:N)
            modelBuilder.Entity<Teste>()
                .HasMany(t => t.Respostas)
                .WithOne(r => r.Teste)
                .HasForeignKey(r => r.TesteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Questao -> RespostasTestes (1:N)
            modelBuilder.Entity<Questao>()
                .HasMany(q => q.Respostas)
                .WithOne(r => r.Questao)
                .HasForeignKey(r => r.QuestaoId)
                .OnDelete(DeleteBehavior.Restrict);


            // === CONFIGURAÇÕES DE PROPRIEDADES ===

            // Configurar precisão decimal para campos de dinheiro e pontuação
            modelBuilder.Entity<Vaga>()
                .Property(v => v.Salario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Inscricao>()
                .Property(i => i.PontuacaoTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Teste>()
                .Property(t => t.PesoNota)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Questao>()
                .Property(q => q.PontosPorQuestao)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RespostaTeste>()
                .Property(r => r.PontosObtidos)
                .HasPrecision(18, 2);


            // === ÍNDICES ÚNICOS ===

            // CNPJ da empresa deve ser único
            modelBuilder.Entity<Empresa>()
                .HasIndex(e => e.CNPJ)
                .IsUnique();

            // Email do usuário deve ser único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // CPF do candidato deve ser único
            modelBuilder.Entity<Candidato>()
                .HasIndex(c => c.CPF)
                .IsUnique();
        }

    }
}
