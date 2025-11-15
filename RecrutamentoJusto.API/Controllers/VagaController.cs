using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.DTOs.Vaga;
using RecrutamentoJusto.API.Models;
using Mvc = Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Vagas.
    /// Gerencia publicação, consulta, atualização e fechamento de vagas.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class VagaController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public VagaController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/Vaga ===

        /// <summary>
        /// Retorna todas as vagas cadastradas no sistema.
        /// </summary>
        /// <returns>Lista de vagas.</returns>
        /// <response code="200">Retorna a lista de vagas com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<VagaResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<VagaResponseDto>>> GetVagas()
        {
            // Busca todas as vagas ativas, incluindo informações da empresa
            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .Include(v => v.Inscricoes)
                .Where(v => v.Ativo)
                .Select(v => new VagaResponseDto
                {
                    Id = v.Id,
                    EmpresaId = v.EmpresaId,
                    NomeEmpresa = v.Empresa != null ? v.Empresa.Nome : null,
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Beneficios = v.Beneficios,
                    Salario = v.Salario,
                    Localizacao = v.Localizacao,
                    Modalidade = v.Modalidade,
                    DataAbertura = v.DataAbertura,
                    DataFechamento = v.DataFechamento,
                    Status = v.Status,
                    Ativo = v.Ativo,
                    TotalInscricoes = v.Inscricoes.Count
                })
                .ToListAsync();

            return Ok(vagas);
        }


        // === GET: api/v1/Vaga/5 ===

        /// <summary>
        /// Retorna uma vaga específica pelo ID.
        /// </summary>
        /// <param name="id">ID da vaga.</param>
        /// <returns>Dados da vaga.</returns>
        /// <response code="200">Retorna a vaga encontrada.</response>
        /// <response code="404">Vaga não encontrada.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(VagaResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<VagaResponseDto>> GetVaga(int id)
        {
            // Busca a vaga pelo ID
            var vaga = await _context.Vagas
                .Include(v => v.Empresa)
                .Include(v => v.Inscricoes)
                .Where(v => v.Id == id && v.Ativo)
                .Select(v => new VagaResponseDto
                {
                    Id = v.Id,
                    EmpresaId = v.EmpresaId,
                    NomeEmpresa = v.Empresa != null ? v.Empresa.Nome : null,
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Beneficios = v.Beneficios,
                    Salario = v.Salario,
                    Localizacao = v.Localizacao,
                    Modalidade = v.Modalidade,
                    DataAbertura = v.DataAbertura,
                    DataFechamento = v.DataFechamento,
                    Status = v.Status,
                    Ativo = v.Ativo,
                    TotalInscricoes = v.Inscricoes.Count
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (vaga == null)
            {
                return NotFound(new { mensagem = "Vaga não encontrada." });
            }

            return Ok(vaga);
        }


        // === GET: api/v1/Vaga/Empresa/5 ===

        /// <summary>
        /// Retorna todas as vagas de uma empresa específica.
        /// </summary>
        /// <param name="empresaId">ID da empresa.</param>
        /// <returns>Lista de vagas da empresa.</returns>
        /// <response code="200">Retorna a lista de vagas.</response>
        /// <response code="404">Empresa não encontrada.</response>
        [Mvc.HttpGet("Empresa/{empresaId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<VagaResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<VagaResponseDto>>> GetVagasPorEmpresa(int empresaId)
        {
            // Verifica se a empresa existe
            var empresaExiste = await _context.Empresas.AnyAsync(e => e.Id == empresaId);
            if (!empresaExiste)
            {
                return NotFound(new { mensagem = "Empresa não encontrada." });
            }

            // Busca todas as vagas da empresa
            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .Include(v => v.Inscricoes)
                .Where(v => v.EmpresaId == empresaId && v.Ativo)
                .Select(v => new VagaResponseDto
                {
                    Id = v.Id,
                    EmpresaId = v.EmpresaId,
                    NomeEmpresa = v.Empresa != null ? v.Empresa.Nome : null,
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Beneficios = v.Beneficios,
                    Salario = v.Salario,
                    Localizacao = v.Localizacao,
                    Modalidade = v.Modalidade,
                    DataAbertura = v.DataAbertura,
                    DataFechamento = v.DataFechamento,
                    Status = v.Status,
                    Ativo = v.Ativo,
                    TotalInscricoes = v.Inscricoes.Count
                })
                .ToListAsync();

            return Ok(vagas);
        }


        // === GET: api/v1/Vaga/Abertas ===

        /// <summary>
        /// Retorna apenas as vagas com status "Aberta".
        /// Útil para candidatos visualizarem oportunidades disponíveis.
        /// </summary>
        /// <returns>Lista de vagas abertas.</returns>
        /// <response code="200">Retorna a lista de vagas abertas.</response>
        [Mvc.HttpGet("Abertas")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<VagaResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<VagaResponseDto>>> GetVagasAbertas()
        {
            // Busca apenas vagas com status "Aberta"
            var vagas = await _context.Vagas
                .Include(v => v.Empresa)
                .Include(v => v.Inscricoes)
                .Where(v => v.Ativo && v.Status == "Aberta")
                .Select(v => new VagaResponseDto
                {
                    Id = v.Id,
                    EmpresaId = v.EmpresaId,
                    NomeEmpresa = v.Empresa != null ? v.Empresa.Nome : null,
                    Titulo = v.Titulo,
                    Descricao = v.Descricao,
                    Requisitos = v.Requisitos,
                    Beneficios = v.Beneficios,
                    Salario = v.Salario,
                    Localizacao = v.Localizacao,
                    Modalidade = v.Modalidade,
                    DataAbertura = v.DataAbertura,
                    DataFechamento = v.DataFechamento,
                    Status = v.Status,
                    Ativo = v.Ativo,
                    TotalInscricoes = v.Inscricoes.Count
                })
                .ToListAsync();

            return Ok(vagas);
        }


        // === POST: api/v1/Vaga ===

        /// <summary>
        /// Cadastra uma nova vaga no sistema.
        /// </summary>
        /// <param name="vagaDto">Dados da vaga a ser cadastrada.</param>
        /// <returns>Vaga criada.</returns>
        /// <response code="201">Vaga criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou empresa não encontrada.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(VagaResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<VagaResponseDto>> PostVaga(VagaCreateDto vagaDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a empresa existe
            var empresa = await _context.Empresas.FindAsync(vagaDto.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest(new { mensagem = "Empresa não encontrada ou inativa." });
            }

            // Converte o DTO para a entidade Model
            var vaga = new Vaga
            {
                EmpresaId = vagaDto.EmpresaId,
                Titulo = vagaDto.Titulo,
                Descricao = vagaDto.Descricao,
                Requisitos = vagaDto.Requisitos,
                Beneficios = vagaDto.Beneficios,
                Salario = vagaDto.Salario,
                Localizacao = vagaDto.Localizacao,
                Modalidade = vagaDto.Modalidade,
                DataAbertura = DateTime.Now,
                DataFechamento = vagaDto.DataFechamento,
                Status = "Aberta",
                Ativo = true
            };

            // Adiciona a vaga no banco de dados
            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var vagaResponse = new VagaResponseDto
            {
                Id = vaga.Id,
                EmpresaId = vaga.EmpresaId,
                NomeEmpresa = empresa.Nome,
                Titulo = vaga.Titulo,
                Descricao = vaga.Descricao,
                Requisitos = vaga.Requisitos,
                Beneficios = vaga.Beneficios,
                Salario = vaga.Salario,
                Localizacao = vaga.Localizacao,
                Modalidade = vaga.Modalidade,
                DataAbertura = vaga.DataAbertura,
                DataFechamento = vaga.DataFechamento,
                Status = vaga.Status,
                Ativo = vaga.Ativo,
                TotalInscricoes = 0
            };

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetVaga), new { id = vaga.Id }, vagaResponse);
        }


        // === PUT: api/v1/Vaga/5 ===

        /// <summary>
        /// Atualiza os dados de uma vaga existente.
        /// </summary>
        /// <param name="id">ID da vaga a ser atualizada.</param>
        /// <param name="vagaDto">Novos dados da vaga.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Vaga atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Vaga não encontrada.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutVaga(int id, VagaUpdateDto vagaDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a vaga existe
            var vagaExistente = await _context.Vagas.FindAsync(id);
            if (vagaExistente == null)
            {
                return NotFound(new { mensagem = "Vaga não encontrada." });
            }

            // Atualiza os dados (preserva empresa e data de abertura)
            vagaExistente.Titulo = vagaDto.Titulo;
            vagaExistente.Descricao = vagaDto.Descricao;
            vagaExistente.Requisitos = vagaDto.Requisitos;
            vagaExistente.Beneficios = vagaDto.Beneficios;
            vagaExistente.Salario = vagaDto.Salario;
            vagaExistente.Localizacao = vagaDto.Localizacao;
            vagaExistente.Modalidade = vagaDto.Modalidade;
            vagaExistente.DataFechamento = vagaDto.DataFechamento;
            vagaExistente.Status = vagaDto.Status;
            vagaExistente.Ativo = vagaDto.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar vaga. Tente novamente." });
            }

            return NoContent();
        }


        // === DELETE: api/v1/Vaga/5 ===

        /// <summary>
        /// Desativa uma vaga (exclusão lógica).
        /// Não remove do banco, apenas marca como inativa.
        /// </summary>
        /// <param name="id">ID da vaga a ser desativada.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Vaga desativada com sucesso.</response>
        /// <response code="404">Vaga não encontrada.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteVaga(int id)
        {
            // Busca a vaga
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
            {
                return NotFound(new { mensagem = "Vaga não encontrada." });
            }

            // Exclusão lógica: apenas marca como inativa
            vaga.Ativo = false;
            vaga.Status = "Fechada";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
