using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.RespostaTeste;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Respostas de Testes.
    /// Gerencia as respostas dos candidatos, valida correção e calcula pontuação automaticamente.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class RespostaTesteController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public RespostaTesteController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/RespostaTeste ===
        /// <summary>
        /// Retorna todas as respostas de testes cadastradas no sistema.
        /// </summary>
        /// <returns>Lista de respostas.</returns>
        /// <response code="200">Retorna a lista de respostas com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<RespostaTesteResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<RespostaTesteResponseDto>>> GetRespostas()
        {
            var respostas = await _context.RespostasTeste
                .Include(r => r.Questao)
                .Select(r => new RespostaTesteResponseDto
                {
                    Id = r.Id,
                    InscricaoId = r.InscricaoId,
                    TesteId = r.TesteId,
                    QuestaoId = r.QuestaoId,
                    EnunciadoQuestao = r.Questao != null ? r.Questao.Enunciado : null,
                    RespostaEscolhida = r.RespostaEscolhida,
                    Correta = r.Correta,
                    PontosObtidos = r.PontosObtidos,
                    DataResposta = r.DataResposta
                })
                .ToListAsync();

            return Ok(respostas);
        }


        // === GET: api/v1/RespostaTeste/5 ===
        /// <summary>
        /// Retorna uma resposta específica pelo ID.
        /// </summary>
        /// <param name="id">ID da resposta.</param>
        /// <returns>Dados da resposta.</returns>
        /// <response code="200">Retorna a resposta encontrada.</response>
        /// <response code="404">Resposta não encontrada.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(RespostaTesteResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<RespostaTesteResponseDto>> GetResposta(int id)
        {
            var resposta = await _context.RespostasTeste
                .Include(r => r.Questao)
                .Where(r => r.Id == id)
                .Select(r => new RespostaTesteResponseDto
                {
                    Id = r.Id,
                    InscricaoId = r.InscricaoId,
                    TesteId = r.TesteId,
                    QuestaoId = r.QuestaoId,
                    EnunciadoQuestao = r.Questao != null ? r.Questao.Enunciado : null,
                    RespostaEscolhida = r.RespostaEscolhida,
                    Correta = r.Correta,
                    PontosObtidos = r.PontosObtidos,
                    DataResposta = r.DataResposta
                })
                .FirstOrDefaultAsync();

            if (resposta == null)
            {
                return NotFound(new { mensagem = "Resposta não encontrada." });
            }

            return Ok(resposta);
        }


        // === GET: api/v1/RespostaTeste/Inscricao/5 ===

        /// <summary>
        /// Retorna todas as respostas de uma inscrição específica.
        /// Útil para ver o desempenho do candidato em todos os testes.
        /// </summary>
        /// <param name="inscricaoId">ID da inscrição.</param>
        /// <returns>Lista de respostas da inscrição.</returns>
        /// <response code="200">Retorna a lista de respostas.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpGet("Inscricao/{inscricaoId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<RespostaTesteResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<RespostaTesteResponseDto>>> GetRespostasPorInscricao(int inscricaoId)
        {
            var inscricaoExiste = await _context.Inscricoes.AnyAsync(i => i.Id == inscricaoId);
            if (!inscricaoExiste)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            var respostas = await _context.RespostasTeste
                .Include(r => r.Questao)
                .Where(r => r.InscricaoId == inscricaoId)
                .Select(r => new RespostaTesteResponseDto
                {
                    Id = r.Id,
                    InscricaoId = r.InscricaoId,
                    TesteId = r.TesteId,
                    QuestaoId = r.QuestaoId,
                    EnunciadoQuestao = r.Questao != null ? r.Questao.Enunciado : null,
                    RespostaEscolhida = r.RespostaEscolhida,
                    Correta = r.Correta,
                    PontosObtidos = r.PontosObtidos,
                    DataResposta = r.DataResposta
                })
                .OrderBy(r => r.DataResposta)
                .ToListAsync();

            return Ok(respostas);
        }


        // === GET: api/v1/RespostaTeste/Teste/5/Inscricao/3 ===

        /// <summary>
        /// Retorna as respostas de um candidato (inscrição) em um teste específico.
        /// </summary>
        /// <param name="testeId">ID do teste.</param>
        /// <param name="inscricaoId">ID da inscrição.</param>
        /// <returns>Lista de respostas.</returns>
        /// <response code="200">Retorna a lista de respostas.</response>
        [Mvc.HttpGet("Teste/{testeId}/Inscricao/{inscricaoId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<RespostaTesteResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<RespostaTesteResponseDto>>> GetRespostasPorTesteEInscricao(int testeId, int inscricaoId)
        {
            var respostas = await _context.RespostasTeste
                .Include(r => r.Questao)
                .Where(r => r.TesteId == testeId && r.InscricaoId == inscricaoId)
                .Select(r => new RespostaTesteResponseDto
                {
                    Id = r.Id,
                    InscricaoId = r.InscricaoId,
                    TesteId = r.TesteId,
                    QuestaoId = r.QuestaoId,
                    EnunciadoQuestao = r.Questao != null ? r.Questao.Enunciado : null,
                    RespostaEscolhida = r.RespostaEscolhida,
                    Correta = r.Correta,
                    PontosObtidos = r.PontosObtidos,
                    DataResposta = r.DataResposta
                })
                .OrderBy(r => r.QuestaoId)
                .ToListAsync();

            return Ok(respostas);
        }


        // === POST: api/v1/RespostaTeste ===

        /// <summary>
        /// Registra a resposta de um candidato a uma questão.
        /// O sistema automaticamente:
        /// 1. Valida se a resposta está correta
        /// 2. Calcula os pontos obtidos
        /// 3. Atualiza a pontuação total da inscrição
        /// </summary>
        /// <param name="respostaDto">Dados da resposta.</param>
        /// <returns>Resposta criada.</returns>
        /// <response code="201">Resposta registrada com sucesso.</response>
        /// <response code="400">Dados inválidos, questão já respondida ou entidades não encontradas.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(RespostaTesteResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<RespostaTesteResponseDto>> PostResposta(RespostaTesteCreateDto respostaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a inscrição existe
            var inscricao = await _context.Inscricoes.FindAsync(respostaDto.InscricaoId);
            if (inscricao == null)
            {
                return BadRequest(new { mensagem = "Inscrição não encontrada." });
            }

            // Verifica se o teste existe
            var teste = await _context.Testes.FindAsync(respostaDto.TesteId);
            if (teste == null || !teste.Ativo)
            {
                return BadRequest(new { mensagem = "Teste não encontrado ou inativo." });
            }

            // Verifica se a questão existe e pertence ao teste
            var questao = await _context.Questoes.FindAsync(respostaDto.QuestaoId);
            if (questao == null || questao.TesteId != respostaDto.TesteId)
            {
                return BadRequest(new { mensagem = "Questão não encontrada ou não pertence a este teste." });
            }

            // Verifica se a questão já foi respondida
            var jaRespondida = await _context.RespostasTeste
                .AnyAsync(r => r.InscricaoId == respostaDto.InscricaoId && r.QuestaoId == respostaDto.QuestaoId);

            if (jaRespondida)
            {
                return BadRequest(new { mensagem = "Esta questão já foi respondida nesta inscrição." });
            }


            // === VALIDAÇÃO E CÁLCULO AUTOMÁTICO ===
            // Verifica se a resposta está correta
            var respostaEscolhidaMaiuscula = respostaDto.RespostaEscolhida.ToUpper();
            var estaCorreta = respostaEscolhidaMaiuscula == questao.RespostaCorreta.ToUpper();

            // Calcula pontos: se correta, recebe pontos da questão; senão, zero
            var pontosObtidos = estaCorreta ? questao.PontosPorQuestao : 0;

            // Cria a resposta
            var resposta = new RespostaTeste
            {
                InscricaoId = respostaDto.InscricaoId,
                TesteId = respostaDto.TesteId,
                QuestaoId = respostaDto.QuestaoId,
                RespostaEscolhida = respostaEscolhidaMaiuscula,
                Correta = estaCorreta,
                PontosObtidos = pontosObtidos,
                DataResposta = DateTime.Now
            };

            _context.RespostasTeste.Add(resposta);


            // === ATUALIZA PONTUAÇÃO TOTAL DA INSCRIÇÃO ===
            // Soma todos os pontos obtidos pelo candidato nesta inscrição
            var pontuacaoTotal = await _context.RespostasTeste
                .Where(r => r.InscricaoId == respostaDto.InscricaoId)
                .SumAsync(r => r.PontosObtidos);

            pontuacaoTotal += pontosObtidos; // Adiciona a pontuação desta resposta

            inscricao.PontuacaoTotal = pontuacaoTotal;
            inscricao.Status = "EmAvaliacao"; // Atualiza status

            await _context.SaveChangesAsync();

            // Retorna a resposta
            var respostaResponse = new RespostaTesteResponseDto
            {
                Id = resposta.Id,
                InscricaoId = resposta.InscricaoId,
                TesteId = resposta.TesteId,
                QuestaoId = resposta.QuestaoId,
                EnunciadoQuestao = questao.Enunciado,
                RespostaEscolhida = resposta.RespostaEscolhida,
                Correta = resposta.Correta,
                PontosObtidos = resposta.PontosObtidos,
                DataResposta = resposta.DataResposta
            };

            return CreatedAtAction(nameof(GetResposta), new { id = resposta.Id }, respostaResponse);
        }


        // === DELETE: api/v1/RespostaTeste/5 ===

        /// <summary>
        /// Remove uma resposta (exclusão física).
        /// ATENÇÃO: Ao remover, a pontuação total da inscrição será recalculada.
        /// </summary>
        /// <param name="id">ID da resposta.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Resposta removida com sucesso.</response>
        /// <response code="404">Resposta não encontrada.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteResposta(int id)
        {
            var resposta = await _context.RespostasTeste.FindAsync(id);
            if (resposta == null)
            {
                return NotFound(new { mensagem = "Resposta não encontrada." });
            }

            var inscricaoId = resposta.InscricaoId;

            // Remove a resposta
            _context.RespostasTeste.Remove(resposta);
            await _context.SaveChangesAsync();

            // Recalcula a pontuação total da inscrição
            var inscricao = await _context.Inscricoes.FindAsync(inscricaoId);
            if (inscricao != null)
            {
                var pontuacaoTotal = await _context.RespostasTeste
                    .Where(r => r.InscricaoId == inscricaoId)
                    .SumAsync(r => r.PontosObtidos);

                inscricao.PontuacaoTotal = pontuacaoTotal;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
