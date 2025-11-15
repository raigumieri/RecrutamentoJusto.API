using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Questao;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Questões.
    /// Gerencia questões de múltipla escolha dos testes técnicos.
    /// A resposta correta não deve ser exposta ao candidato durante o teste.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class QuestaoController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public QuestaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== GET: api/v1/Questao =====
        /// <summary>
        /// Retorna todas as questões cadastradas no sistema.
        /// Expõe a resposta correta. Usar apenas para RH/Admin.
        /// </summary>
        /// <returns>Lista de questões.</returns>
        /// <response code="200">Retorna a lista de questões com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<QuestaoResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<QuestaoResponseDto>>> GetQuestoes()
        {
            // Busca todas as questões
            var questoes = await _context.Questoes
                .Include(q => q.Teste)
                .Select(q => new QuestaoResponseDto
                {
                    Id = q.Id,
                    TesteId = q.TesteId,
                    TituloTeste = q.Teste != null ? q.Teste.Titulo : null,
                    Enunciado = q.Enunciado,
                    OpcaoA = q.OpcaoA,
                    OpcaoB = q.OpcaoB,
                    OpcaoC = q.OpcaoC,
                    OpcaoD = q.OpcaoD,
                    RespostaCorreta = q.RespostaCorreta,
                    PontosPorQuestao = q.PontosPorQuestao,
                    Ordem = q.Ordem
                })
                .ToListAsync();

            return Ok(questoes);
        }


        // === GET: api/v1/Questao/5 ===

        /// <summary>
        /// Retorna uma questão específica pelo ID.
        /// Expõe a resposta correta. Usar apenas para RH/Admin.
        /// </summary>
        /// <param name="id">ID da questão.</param>
        /// <returns>Dados da questão.</returns>
        /// <response code="200">Retorna a questão encontrada.</response>
        /// <response code="404">Questão não encontrada.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(QuestaoResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<QuestaoResponseDto>> GetQuestao(int id)
        {
            // Busca a questão pelo ID
            var questao = await _context.Questoes
                .Include(q => q.Teste)
                .Where(q => q.Id == id)
                .Select(q => new QuestaoResponseDto
                {
                    Id = q.Id,
                    TesteId = q.TesteId,
                    TituloTeste = q.Teste != null ? q.Teste.Titulo : null,
                    Enunciado = q.Enunciado,
                    OpcaoA = q.OpcaoA,
                    OpcaoB = q.OpcaoB,
                    OpcaoC = q.OpcaoC,
                    OpcaoD = q.OpcaoD,
                    RespostaCorreta = q.RespostaCorreta,
                    PontosPorQuestao = q.PontosPorQuestao,
                    Ordem = q.Ordem
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (questao == null)
            {
                return NotFound(new { mensagem = "Questão não encontrada." });
            }

            return Ok(questao);
        }


        // === GET: api/v1/Questao/Teste/5 ===

        /// <summary>
        /// Retorna todas as questões de um teste específico.
        /// Expõe a resposta correta. Usar apenas para RH/Admin.
        /// </summary>
        /// <param name="testeId">ID do teste.</param>
        /// <returns>Lista de questões do teste.</returns>
        /// <response code="200">Retorna a lista de questões.</response>
        /// <response code="404">Teste não encontrado.</response>
        [Mvc.HttpGet("Teste/{testeId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<QuestaoResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<QuestaoResponseDto>>> GetQuestoesPorTeste(int testeId)
        {
            // Verifica se o teste existe
            var testeExiste = await _context.Testes.AnyAsync(t => t.Id == testeId);
            if (!testeExiste)
            {
                return NotFound(new { mensagem = "Teste não encontrado." });
            }

            // Busca todas as questões do teste, ordenadas
            var questoes = await _context.Questoes
                .Include(q => q.Teste)
                .Where(q => q.TesteId == testeId)
                .OrderBy(q => q.Ordem)
                .Select(q => new QuestaoResponseDto
                {
                    Id = q.Id,
                    TesteId = q.TesteId,
                    TituloTeste = q.Teste != null ? q.Teste.Titulo : null,
                    Enunciado = q.Enunciado,
                    OpcaoA = q.OpcaoA,
                    OpcaoB = q.OpcaoB,
                    OpcaoC = q.OpcaoC,
                    OpcaoD = q.OpcaoD,
                    RespostaCorreta = q.RespostaCorreta,
                    PontosPorQuestao = q.PontosPorQuestao,
                    Ordem = q.Ordem
                })
                .ToListAsync();

            return Ok(questoes);
        }


        // === POST: api/v1/Questao ===

        /// <summary>
        /// Cadastra uma nova questão no sistema.
        /// </summary>
        /// <param name="questaoDto">Dados da questão a ser cadastrada.</param>
        /// <returns>Questão criada.</returns>
        /// <response code="201">Questão criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou teste não encontrado.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(QuestaoResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<QuestaoResponseDto>> PostQuestao(QuestaoCreateDto questaoDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o teste existe
            var teste = await _context.Testes.FindAsync(questaoDto.TesteId);
            if (teste == null || !teste.Ativo)
            {
                return BadRequest(new { mensagem = "Teste não encontrado ou inativo." });
            }

            // Converte o DTO para a entidade Model
            var questao = new Questao
            {
                TesteId = questaoDto.TesteId,
                Enunciado = questaoDto.Enunciado,
                OpcaoA = questaoDto.OpcaoA,
                OpcaoB = questaoDto.OpcaoB,
                OpcaoC = questaoDto.OpcaoC,
                OpcaoD = questaoDto.OpcaoD,
                RespostaCorreta = questaoDto.RespostaCorreta.ToUpper(), // Garante que seja maiúscula
                PontosPorQuestao = questaoDto.PontosPorQuestao,
                Ordem = questaoDto.Ordem
            };

            // Adiciona a questão no banco de dados
            _context.Questoes.Add(questao);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var questaoResponse = new QuestaoResponseDto
            {
                Id = questao.Id,
                TesteId = questao.TesteId,
                TituloTeste = teste.Titulo,
                Enunciado = questao.Enunciado,
                OpcaoA = questao.OpcaoA,
                OpcaoB = questao.OpcaoB,
                OpcaoC = questao.OpcaoC,
                OpcaoD = questao.OpcaoD,
                RespostaCorreta = questao.RespostaCorreta,
                PontosPorQuestao = questao.PontosPorQuestao,
                Ordem = questao.Ordem
            };

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetQuestao), new { id = questao.Id }, questaoResponse);
        }


        // === PUT: api/v1/Questao/5 ===

        /// <summary>
        /// Atualiza os dados de uma questão existente.
        /// </summary>
        /// <param name="id">ID da questão a ser atualizada.</param>
        /// <param name="questaoDto">Novos dados da questão.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Questão atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Questão não encontrada.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutQuestao(int id, QuestaoUpdateDto questaoDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a questão existe
            var questaoExistente = await _context.Questoes.FindAsync(id);
            if (questaoExistente == null)
            {
                return NotFound(new { mensagem = "Questão não encontrada." });
            }

            // Atualiza os dados (preserva teste)
            questaoExistente.Enunciado = questaoDto.Enunciado;
            questaoExistente.OpcaoA = questaoDto.OpcaoA;
            questaoExistente.OpcaoB = questaoDto.OpcaoB;
            questaoExistente.OpcaoC = questaoDto.OpcaoC;
            questaoExistente.OpcaoD = questaoDto.OpcaoD;
            questaoExistente.RespostaCorreta = questaoDto.RespostaCorreta.ToUpper();
            questaoExistente.PontosPorQuestao = questaoDto.PontosPorQuestao;
            questaoExistente.Ordem = questaoDto.Ordem;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar questão. Tente novamente." });
            }

            return NoContent();
        }


        // === DELETE: api/v1/Questao/5 ===

        /// <summary>
        /// Remove uma questão do sistema (exclusão física).
        /// Remove permanentemente do banco de dados.
        /// </summary>
        /// <param name="id">ID da questão a ser removida.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Questão removida com sucesso.</response>
        /// <response code="404">Questão não encontrada.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteQuestao(int id)
        {
            // Busca a questão
            var questao = await _context.Questoes.FindAsync(id);
            if (questao == null)
            {
                return NotFound(new { mensagem = "Questão não encontrada." });
            }

            // Remove permanentemente
            _context.Questoes.Remove(questao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
