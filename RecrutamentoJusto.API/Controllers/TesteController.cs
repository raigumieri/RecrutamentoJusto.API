using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Teste;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Testes.
    /// Gerencia avaliações técnicas associadas às vagas.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class TesteController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public TesteController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/Teste ===

        /// <summary>
        /// Retorna todos os testes cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de testes.</returns>
        /// <response code="200">Retorna a lista de testes com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<TesteResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<TesteResponseDto>>> GetTestes()
        {
            // Busca todos os testes ativos
            var testes = await _context.Testes
                .Include(t => t.Vaga)
                .Include(t => t.Questoes)
                .Where(t => t.Ativo)
                .Select(t => new TesteResponseDto
                {
                    Id = t.Id,
                    VagaId = t.VagaId,
                    TituloVaga = t.Vaga != null ? t.Vaga.Titulo : null,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    DuracaoMinutos = t.DuracaoMinutos,
                    PesoNota = t.PesoNota,
                    DataCriacao = t.DataCriacao,
                    Ativo = t.Ativo,
                    TotalQuestoes = t.Questoes.Count
                })
                .ToListAsync();

            return Ok(testes);
        }


        // === GET: api/v1/Teste/5 ===

        /// <summary>
        /// Retorna um teste específico pelo ID.
        /// </summary>
        /// <param name="id">ID do teste.</param>
        /// <returns>Dados do teste.</returns>
        /// <response code="200">Retorna o teste encontrado.</response>
        /// <response code="404">Teste não encontrado.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(TesteResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<TesteResponseDto>> GetTeste(int id)
        {
            // Busca o teste pelo ID
            var teste = await _context.Testes
                .Include(t => t.Vaga)
                .Include(t => t.Questoes)
                .Where(t => t.Id == id && t.Ativo)
                .Select(t => new TesteResponseDto
                {
                    Id = t.Id,
                    VagaId = t.VagaId,
                    TituloVaga = t.Vaga != null ? t.Vaga.Titulo : null,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    DuracaoMinutos = t.DuracaoMinutos,
                    PesoNota = t.PesoNota,
                    DataCriacao = t.DataCriacao,
                    Ativo = t.Ativo,
                    TotalQuestoes = t.Questoes.Count
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (teste == null)
            {
                return NotFound(new { mensagem = "Teste não encontrado." });
            }

            return Ok(teste);
        }


        // === GET: api/v1/Teste/Vaga/5 ===

        /// <summary>
        /// Retorna todos os testes de uma vaga específica.
        /// </summary>
        /// <param name="vagaId">ID da vaga.</param>
        /// <returns>Lista de testes da vaga.</returns>
        /// <response code="200">Retorna a lista de testes.</response>
        /// <response code="404">Vaga não encontrada.</response>
        [Mvc.HttpGet("Vaga/{vagaId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<TesteResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<TesteResponseDto>>> GetTestesPorVaga(int vagaId)
        {
            // Verifica se a vaga existe
            var vagaExiste = await _context.Vagas.AnyAsync(v => v.Id == vagaId);
            if (!vagaExiste)
            {
                return NotFound(new { mensagem = "Vaga não encontrada." });
            }

            // Busca todos os testes da vaga
            var testes = await _context.Testes
                .Include(t => t.Vaga)
                .Include(t => t.Questoes)
                .Where(t => t.VagaId == vagaId && t.Ativo)
                .Select(t => new TesteResponseDto
                {
                    Id = t.Id,
                    VagaId = t.VagaId,
                    TituloVaga = t.Vaga != null ? t.Vaga.Titulo : null,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    DuracaoMinutos = t.DuracaoMinutos,
                    PesoNota = t.PesoNota,
                    DataCriacao = t.DataCriacao,
                    Ativo = t.Ativo,
                    TotalQuestoes = t.Questoes.Count
                })
                .ToListAsync();

            return Ok(testes);
        }


        // === POST: api/v1/Teste ===

        /// <summary>
        /// Cadastra um novo teste no sistema.
        /// </summary>
        /// <param name="testeDto">Dados do teste a ser cadastrado.</param>
        /// <returns>Teste criado.</returns>
        /// <response code="201">Teste criado com sucesso.</response>
        /// <response code="400">Dados inválidos ou vaga não encontrada.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(TesteResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<TesteResponseDto>> PostTeste(TesteCreateDto testeDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a vaga existe
            var vaga = await _context.Vagas.FindAsync(testeDto.VagaId);
            if (vaga == null || !vaga.Ativo)
            {
                return BadRequest(new { mensagem = "Vaga não encontrada ou inativa." });
            }

            // Converte o DTO para a entidade Model
            var teste = new Teste
            {
                VagaId = testeDto.VagaId,
                Titulo = testeDto.Titulo,
                Descricao = testeDto.Descricao,
                DuracaoMinutos = testeDto.DuracaoMinutos,
                PesoNota = testeDto.PesoNota,
                DataCriacao = DateTime.Now,
                Ativo = true
            };

            // Adiciona o teste no banco de dados
            _context.Testes.Add(teste);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var testeResponse = new TesteResponseDto
            {
                Id = teste.Id,
                VagaId = teste.VagaId,
                TituloVaga = vaga.Titulo,
                Titulo = teste.Titulo,
                Descricao = teste.Descricao,
                DuracaoMinutos = teste.DuracaoMinutos,
                PesoNota = teste.PesoNota,
                DataCriacao = teste.DataCriacao,
                Ativo = teste.Ativo,
                TotalQuestoes = 0
            };

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetTeste), new { id = teste.Id }, testeResponse);
        }


        // === PUT: api/v1/Teste/5 ===

        /// <summary>
        /// Atualiza os dados de um teste existente.
        /// </summary>
        /// <param name="id">ID do teste a ser atualizado.</param>
        /// <param name="testeDto">Novos dados do teste.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Teste atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Teste não encontrado.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutTeste(int id, TesteUpdateDto testeDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o teste existe
            var testeExistente = await _context.Testes.FindAsync(id);
            if (testeExistente == null)
            {
                return NotFound(new { mensagem = "Teste não encontrado." });
            }

            // Atualiza os dados (preserva vaga e data de criação)
            testeExistente.Titulo = testeDto.Titulo;
            testeExistente.Descricao = testeDto.Descricao;
            testeExistente.DuracaoMinutos = testeDto.DuracaoMinutos;
            testeExistente.PesoNota = testeDto.PesoNota;
            testeExistente.Ativo = testeDto.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar teste. Tente novamente." });
            }

            return NoContent();
        }


        // === DELETE: api/v1/Teste/5 ===

        /// <summary>
        /// Desativa um teste (exclusão lógica).
        /// Não remove do banco, apenas marca como inativo.
        /// </summary>
        /// <param name="id">ID do teste a ser desativado.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Teste desativado com sucesso.</response>
        /// <response code="404">Teste não encontrado.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteTeste(int id)
        {
            // Busca o teste
            var teste = await _context.Testes.FindAsync(id);
            if (teste == null)
            {
                return NotFound(new { mensagem = "Teste não encontrado." });
            }

            // Exclusão lógica: apenas marca como inativo
            teste.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
