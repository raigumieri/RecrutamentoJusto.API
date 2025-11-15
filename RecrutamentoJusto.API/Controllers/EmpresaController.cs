using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Empresa;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Empresas.
    /// Gerencia o cadastro, consulta, atualização e exclusão de empresas na plataforma.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class EmpresaController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public EmpresaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== GET: api/v1/Empresa =====
        /// <summary>
        /// Retorna todas as empresas cadastradas na plataforma.
        /// </summary>
        /// <returns>Lista de empresas.</returns>
        /// <response code="200">Retorna a lista de empresas com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<EmpresaResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<EmpresaResponseDto>>> GetEmpresas()
        {
            // Busca todas as empresas ativas no banco de dados
            var empresas = await _context.Empresas
                .Where(e => e.Ativo)
                .Select(e => new EmpresaResponseDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    CNPJ = e.CNPJ,
                    Email = e.Email,
                    Telefone = e.Telefone,
                    Endereco = e.Endereco,
                    DataCadastro = e.DataCadastro,
                    Ativo = e.Ativo
                })
                .ToListAsync();

            return Ok(empresas);
        }

        // ===== GET: api/v1/Empresa/5 =====
        /// <summary>
        /// Retorna uma empresa específica pelo ID.
        /// </summary>
        /// <param name="id">ID da empresa.</param>
        /// <returns>Dados da empresa.</returns>
        /// <response code="200">Retorna a empresa encontrada.</response>
        /// <response code="404">Empresa não encontrada.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<EmpresaResponseDto>> GetEmpresa(int id)
        {
            // Busca a empresa pelo ID
            var empresa = await _context.Empresas
                .Where(e => e.Id == id && e.Ativo)
                .Select(e => new EmpresaResponseDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    CNPJ = e.CNPJ,
                    Email = e.Email,
                    Telefone = e.Telefone,
                    Endereco = e.Endereco,
                    DataCadastro = e.DataCadastro,
                    Ativo = e.Ativo
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (empresa == null)
            {
                return NotFound(new { mensagem = "Empresa não encontrada." });
            }

            return Ok(empresa);
        }

        // ===== POST: api/v1/Empresa =====
        /// <summary>
        /// Cadastra uma nova empresa na plataforma.
        /// </summary>
        /// <param name="empresaDto">Dados da empresa a ser cadastrada.</param>
        /// <returns>Empresa criada.</returns>
        /// <response code="201">Empresa criada com sucesso.</response>
        /// <response code="400">Dados inválidos ou CNPJ já cadastrado.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(EmpresaResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<EmpresaResponseDto>> PostEmpresa(EmpresaCreateDto empresaDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o CNPJ já está cadastrado
            var cnpjExiste = await _context.Empresas
                .AnyAsync(e => e.CNPJ == empresaDto.CNPJ);

            if (cnpjExiste)
            {
                return BadRequest(new { mensagem = "CNPJ já cadastrado no sistema." });
            }

            // Converte o DTO para a entidade Model
            var empresa = new Empresa
            {
                Nome = empresaDto.Nome,
                CNPJ = empresaDto.CNPJ,
                Email = empresaDto.Email,
                Telefone = empresaDto.Telefone,
                Endereco = empresaDto.Endereco,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            // Adiciona a empresa no banco de dados
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var empresaResponse = new EmpresaResponseDto
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                CNPJ = empresa.CNPJ,
                Email = empresa.Email,
                Telefone = empresa.Telefone,
                Endereco = empresa.Endereco,
                DataCadastro = empresa.DataCadastro,
                Ativo = empresa.Ativo
            };

            // Retorna 201 Created com a empresa criada e a URL para acessá-la
            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresaResponse);
        }

        // ===== PUT: api/v1/Empresa/5 =====
        /// <summary>
        /// Atualiza os dados de uma empresa existente.
        /// </summary>
        /// <param name="id">ID da empresa a ser atualizada.</param>
        /// <param name="empresaDto">Novos dados da empresa.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Empresa atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Empresa não encontrada.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutEmpresa(int id, EmpresaUpdateDto empresaDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a empresa existe
            var empresaExistente = await _context.Empresas.FindAsync(id);
            if (empresaExistente == null)
            {
                return NotFound(new { mensagem = "Empresa não encontrada." });
            }

            // Atualiza os dados (preserva a data de cadastro e CNPJ originais)
            empresaExistente.Nome = empresaDto.Nome;
            empresaExistente.Email = empresaDto.Email;
            empresaExistente.Telefone = empresaDto.Telefone;
            empresaExistente.Endereco = empresaDto.Endereco;
            empresaExistente.Ativo = empresaDto.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Tratamento de erro de concorrência
                return StatusCode(500, new { mensagem = "Erro ao atualizar empresa. Tente novamente." });
            }

            // Retorna 204 No Content (sucesso sem corpo de resposta)
            return NoContent();
        }

        // ===== DELETE: api/v1/Empresa/5 =====
        /// <summary>
        /// Desativa uma empresa (exclusão lógica).
        /// Não remove do banco, apenas marca como inativa.
        /// </summary>
        /// <param name="id">ID da empresa a ser desativada.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Empresa desativada com sucesso.</response>
        /// <response code="404">Empresa não encontrada.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteEmpresa(int id)
        {
            // Busca a empresa
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound(new { mensagem = "Empresa não encontrada." });
            }

            // Exclusão lógica: apenas marca como inativa
            empresa.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
