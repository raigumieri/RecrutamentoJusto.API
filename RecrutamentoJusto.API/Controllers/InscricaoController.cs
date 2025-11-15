using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Inscricao;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Inscrições.
    /// Core da plataforma de recrutamento justo.
    /// Gerencia inscrições, anonimização de currículos e revelação de identidade.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class InscricaoController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public InscricaoController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/Inscricao ===

        /// <summary>
        /// Retorna todas as inscrições cadastradas no sistema.
        /// Mostra apenas dados anonimizados para inscrições com IdentidadeRevelada = false.
        /// </summary>
        /// <returns>Lista de inscrições.</returns>
        /// <response code="200">Retorna a lista de inscrições com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<InscricaoResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<InscricaoResponseDto>>> GetInscricoes()
        {
            var inscricoes = await _context.Inscricoes
                .Include(i => i.Vaga)
                .Include(i => i.Candidato)
                .Select(i => new InscricaoResponseDto
                {
                    Id = i.Id,
                    VagaId = i.VagaId,
                    TituloVaga = i.Vaga != null ? i.Vaga.Titulo : null,
                    CandidatoId = i.CandidatoId,
                    // Se identidade revelada: mostra nome; caso contrário: "Candidato Anônimo"
                    NomeCandidato = i.IdentidadeRevelada && i.Candidato != null
                        ? i.Candidato.NomeCompleto
                        : "Candidato Anônimo",
                    DataInscricao = i.DataInscricao,
                    Status = i.Status,
                    PontuacaoTotal = i.PontuacaoTotal,
                    IdentidadeRevelada = i.IdentidadeRevelada,
                    CurriculoAnonimizado = i.CurriculoAnonimizado,
                    Feedback = i.Feedback
                })
                .ToListAsync();

            return Ok(inscricoes);
        }


        // === GET: api/v1/Inscricao/5 ===

        /// <summary>
        /// Retorna uma inscrição específica pelo ID.
        /// Mostra dados anonimizados se IdentidadeRevelada = false.
        /// </summary>
        /// <param name="id">ID da inscrição.</param>
        /// <returns>Dados da inscrição.</returns>
        /// <response code="200">Retorna a inscrição encontrada.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<InscricaoResponseDto>> GetInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .Include(i => i.Vaga)
                .Include(i => i.Candidato)
                .Where(i => i.Id == id)
                .Select(i => new InscricaoResponseDto
                {
                    Id = i.Id,
                    VagaId = i.VagaId,
                    TituloVaga = i.Vaga != null ? i.Vaga.Titulo : null,
                    CandidatoId = i.CandidatoId,
                    NomeCandidato = i.IdentidadeRevelada && i.Candidato != null
                        ? i.Candidato.NomeCompleto
                        : "Candidato Anônimo",
                    DataInscricao = i.DataInscricao,
                    Status = i.Status,
                    PontuacaoTotal = i.PontuacaoTotal,
                    IdentidadeRevelada = i.IdentidadeRevelada,
                    CurriculoAnonimizado = i.CurriculoAnonimizado,
                    Feedback = i.Feedback
                })
                .FirstOrDefaultAsync();

            if (inscricao == null)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            return Ok(inscricao);
        }


        // === GET: api/v1/Inscricao/Completo/5 ===

        /// <summary>
        /// Retorna dados COMPLETOS de uma inscrição, incluindo informações sensíveis do candidato.
        /// Só deve ser acessado quando IdentidadeRevelada = true.
        /// </summary>
        /// <param name="id">ID da inscrição.</param>
        /// <returns>Dados completos da inscrição e candidato.</returns>
        /// <response code="200">Retorna os dados completos.</response>
        /// <response code="403">Identidade ainda não foi revelada.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpGet("Completo/{id}")]
        [Mvc.ProducesResponseType(typeof(InscricaoCompletoDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<InscricaoCompletoDto>> GetInscricaoCompleta(int id)
        {
            var inscricao = await _context.Inscricoes
                .Include(i => i.Vaga)
                .Include(i => i.Candidato)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscricao == null)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            // Verifica se a identidade foi revelada
            if (!inscricao.IdentidadeRevelada)
            {
                return StatusCode(403, new { mensagem = "Identidade ainda não revelada. Aprove tecnicamente primeiro." });
            }

            // Retorna dados completos
            var inscricaoCompleta = new InscricaoCompletoDto
            {
                Id = inscricao.Id,
                VagaId = inscricao.VagaId,
                TituloVaga = inscricao.Vaga?.Titulo,
                CandidatoId = inscricao.CandidatoId,
                NomeCompleto = inscricao.Candidato?.NomeCompleto ?? "",
                Email = inscricao.Candidato?.Email ?? "",
                Telefone = inscricao.Candidato?.Telefone,
                CPF = inscricao.Candidato?.CPF ?? "",
                DataNascimento = inscricao.Candidato?.DataNascimento,
                Genero = inscricao.Candidato?.Genero,
                Endereco = inscricao.Candidato?.Endereco,
                Escolaridade = inscricao.Candidato?.Escolaridade,
                Experiencia = inscricao.Candidato?.Experiencia,
                Habilidades = inscricao.Candidato?.Habilidades,
                DataInscricao = inscricao.DataInscricao,
                Status = inscricao.Status,
                PontuacaoTotal = inscricao.PontuacaoTotal,
                IdentidadeRevelada = inscricao.IdentidadeRevelada,
                Feedback = inscricao.Feedback
            };

            return Ok(inscricaoCompleta);
        }


        // === GET: api/v1/Inscricao/Vaga/5 ===

        /// <summary>
        /// Retorna todas as inscrições de uma vaga específica.
        /// </summary>
        /// <param name="vagaId">ID da vaga.</param>
        /// <returns>Lista de inscrições da vaga.</returns>
        /// <response code="200">Retorna a lista de inscrições.</response>
        /// <response code="404">Vaga não encontrada.</response>
        [Mvc.HttpGet("Vaga/{vagaId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<InscricaoResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<InscricaoResponseDto>>> GetInscricoesPorVaga(int vagaId)
        {
            var vagaExiste = await _context.Vagas.AnyAsync(v => v.Id == vagaId);
            if (!vagaExiste)
            {
                return NotFound(new { mensagem = "Vaga não encontrada." });
            }

            var inscricoes = await _context.Inscricoes
                .Include(i => i.Vaga)
                .Include(i => i.Candidato)
                .Where(i => i.VagaId == vagaId)
                .Select(i => new InscricaoResponseDto
                {
                    Id = i.Id,
                    VagaId = i.VagaId,
                    TituloVaga = i.Vaga != null ? i.Vaga.Titulo : null,
                    CandidatoId = i.CandidatoId,
                    NomeCandidato = i.IdentidadeRevelada && i.Candidato != null
                        ? i.Candidato.NomeCompleto
                        : "Candidato Anônimo",
                    DataInscricao = i.DataInscricao,
                    Status = i.Status,
                    PontuacaoTotal = i.PontuacaoTotal,
                    IdentidadeRevelada = i.IdentidadeRevelada,
                    CurriculoAnonimizado = i.CurriculoAnonimizado,
                    Feedback = i.Feedback
                })
                .OrderByDescending(i => i.PontuacaoTotal) // Ordena por pontuação
                .ToListAsync();

            return Ok(inscricoes);
        }


        // === POST: api/v1/Inscricao ===

        /// <summary>
        /// Cadastra uma nova inscrição.
        /// Automaticamente gera o currículo anonimizado removendo informações sensíveis.
        /// </summary>
        /// <param name="inscricaoDto">Dados da inscrição.</param>
        /// <returns>Inscrição criada.</returns>
        /// <response code="201">Inscrição criada com sucesso.</response>
        /// <response code="400">Dados inválidos, vaga/candidato não encontrado ou já inscrito.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(InscricaoResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<InscricaoResponseDto>> PostInscricao(InscricaoCreateDto inscricaoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a vaga existe e está aberta
            var vaga = await _context.Vagas.FindAsync(inscricaoDto.VagaId);
            if (vaga == null || !vaga.Ativo || vaga.Status != "Aberta")
            {
                return BadRequest(new { mensagem = "Vaga não encontrada, inativa ou fechada." });
            }

            // Verifica se o candidato existe
            var candidato = await _context.Candidatos.FindAsync(inscricaoDto.CandidatoId);
            if (candidato == null || !candidato.Ativo)
            {
                return BadRequest(new { mensagem = "Candidato não encontrado ou inativo." });
            }

            // Verifica se já existe inscrição deste candidato nesta vaga
            var inscricaoExiste = await _context.Inscricoes
                .AnyAsync(i => i.VagaId == inscricaoDto.VagaId && i.CandidatoId == inscricaoDto.CandidatoId);

            if (inscricaoExiste)
            {
                return BadRequest(new { mensagem = "Candidato já inscrito nesta vaga." });
            }


            // === ANONIMIZAÇÃO DO CURRÍCULO ===
            // Remove informações sensíveis: nome, CPF, idade, gênero, endereço
            // Mantém apenas: escolaridade, experiência, habilidades
            var curriculoAnonimizado = $@" 
=== PERFIL PROFISSIONAL ANONIMIZADO ===

ESCOLARIDADE:
{candidato.Escolaridade ?? "Não informado"}

EXPERIÊNCIA PROFISSIONAL:
{candidato.Experiencia ?? "Não informado"}

HABILIDADES TÉCNICAS:
{candidato.Habilidades ?? "Não informado"}

---
Este currículo foi anonimizado para garantir um processo seletivo justo e sem vieses.
A identidade do candidato será revelada apenas após aprovação na etapa técnica.
";

            // Cria a inscrição
            var inscricao = new Inscricao
            {
                VagaId = inscricaoDto.VagaId,
                CandidatoId = inscricaoDto.CandidatoId,
                DataInscricao = DateTime.Now,
                Status = "Inscrito",
                PontuacaoTotal = 0,
                IdentidadeRevelada = false,
                CurriculoAnonimizado = curriculoAnonimizado.Trim()
            };

            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            // Retorna a resposta
            var inscricaoResponse = new InscricaoResponseDto
            {
                Id = inscricao.Id,
                VagaId = inscricao.VagaId,
                TituloVaga = vaga.Titulo,
                CandidatoId = inscricao.CandidatoId,
                NomeCandidato = "Candidato Anônimo",
                DataInscricao = inscricao.DataInscricao,
                Status = inscricao.Status,
                PontuacaoTotal = inscricao.PontuacaoTotal,
                IdentidadeRevelada = inscricao.IdentidadeRevelada,
                CurriculoAnonimizado = inscricao.CurriculoAnonimizado,
                Feedback = inscricao.Feedback
            };

            return CreatedAtAction(nameof(GetInscricao), new { id = inscricao.Id }, inscricaoResponse);
        }


        // === PUT: api/v1/Inscricao/5 ===

        /// <summary>
        /// Atualiza o status e feedback de uma inscrição.
        /// </summary>
        /// <param name="id">ID da inscrição.</param>
        /// <param name="inscricaoDto">Novos dados.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Inscrição atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutInscricao(int id, InscricaoUpdateDto inscricaoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            // Atualiza status e feedback
            inscricao.Status = inscricaoDto.Status;
            inscricao.Feedback = inscricaoDto.Feedback;

            // Se aprovado tecnicamente, revela identidade
            if (inscricaoDto.Status == "AprovadoTecnico")
            {
                inscricao.IdentidadeRevelada = true;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar inscrição." });
            }

            return NoContent();
        }


        // === PUT: api/v1/Inscricao/RevelarIdentidade/5 ===

        /// <summary>
        /// Revela a identidade do candidato (quando aprovado tecnicamente).
        /// </summary>
        /// <param name="id">ID da inscrição.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Identidade revelada com sucesso.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpPut("RevelarIdentidade/{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> RevelarIdentidade(int id)
        {
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            // Revela identidade
            inscricao.IdentidadeRevelada = true;
            inscricao.Status = "AprovadoTecnico";

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // === DELETE: api/v1/Inscricao/5 ===

        /// <summary>
        /// Remove uma inscrição (exclusão física).
        /// </summary>
        /// <param name="id">ID da inscrição.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Inscrição removida com sucesso.</response>
        /// <response code="404">Inscrição não encontrada.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteInscricao(int id)
        {
            var inscricao = await _context.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound(new { mensagem = "Inscrição não encontrada." });
            }

            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
