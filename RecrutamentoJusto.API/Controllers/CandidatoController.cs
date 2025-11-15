using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Candidato;

namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Candidatos.
    /// Gerencia o cadastro, consulta, atualização e exclusão de candidatos.
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class CandidatoController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public CandidatoController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/Candidato ===

        /// <summary>
        /// Retorna todos os candidatos cadastrados no sistema.
        /// Este endpoint expõe informações sensíveis.
        /// Em produção, deve ser protegido com autenticação e autorização adequadas.
        /// </summary>
        /// <returns>Lista de candidatos.</returns>
        /// <response code="200">Retorna a lista de candidatos com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<CandidatoResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<CandidatoResponseDto>>> GetCandidatos()
        {
            // Busca todos os candidatos ativos
            var candidatos = await _context.Candidatos
                .Include(c => c.Inscricoes)
                .Where(c => c.Ativo)
                .Select(c => new CandidatoResponseDto
                {
                    Id = c.Id,
                    NomeCompleto = c.NomeCompleto,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    CPF = c.CPF,
                    DataNascimento = c.DataNascimento,
                    Genero = c.Genero,
                    Endereco = c.Endereco,
                    Escolaridade = c.Escolaridade,
                    Experiencia = c.Experiencia,
                    Habilidades = c.Habilidades,
                    CurriculoUrl = c.CurriculoUrl,
                    DataCadastro = c.DataCadastro,
                    Ativo = c.Ativo,
                    TotalInscricoes = c.Inscricoes.Count
                })
                .ToListAsync();

            return Ok(candidatos);
        }


        // === GET: api/v1/Candidato/5 ===

        /// <summary>
        /// Retorna um candidato específico pelo ID.
        /// Expõe informações sensíveis do candidato.
        /// </summary>
        /// <param name="id">ID do candidato.</param>
        /// <returns>Dados do candidato.</returns>
        /// <response code="200">Retorna o candidato encontrado.</response>
        /// <response code="404">Candidato não encontrado.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(CandidatoResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<CandidatoResponseDto>> GetCandidato(int id)
        {
            // Busca o candidato pelo ID
            var candidato = await _context.Candidatos
                .Include(c => c.Inscricoes)
                .Where(c => c.Id == id && c.Ativo)
                .Select(c => new CandidatoResponseDto
                {
                    Id = c.Id,
                    NomeCompleto = c.NomeCompleto,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    CPF = c.CPF,
                    DataNascimento = c.DataNascimento,
                    Genero = c.Genero,
                    Endereco = c.Endereco,
                    Escolaridade = c.Escolaridade,
                    Experiencia = c.Experiencia,
                    Habilidades = c.Habilidades,
                    CurriculoUrl = c.CurriculoUrl,
                    DataCadastro = c.DataCadastro,
                    Ativo = c.Ativo,
                    TotalInscricoes = c.Inscricoes.Count
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (candidato == null)
            {
                return NotFound(new { mensagem = "Candidato não encontrado." });
            }

            return Ok(candidato);
        }


        // === POST: api/v1/Candidato ===

        /// <summary>
        /// Cadastra um novo candidato no sistema.
        /// Coleta todos os dados, incluindo informações sensíveis que serão anonimizadas
        /// durante o processo de inscrição em vagas.
        /// </summary>
        /// <param name="candidatoDto">Dados do candidato a ser cadastrado.</param>
        /// <returns>Candidato criado.</returns>
        /// <response code="201">Candidato criado com sucesso.</response>
        /// <response code="400">Dados inválidos ou CPF/e-mail já cadastrado.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(CandidatoResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<CandidatoResponseDto>> PostCandidato(CandidatoCreateDto candidatoDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o CPF já está cadastrado
            var cpfExiste = await _context.Candidatos
                .AnyAsync(c => c.CPF == candidatoDto.CPF);

            if (cpfExiste)
            {
                return BadRequest(new { mensagem = "CPF já cadastrado no sistema." });
            }

            // Verifica se o e-mail já está cadastrado
            var emailExiste = await _context.Candidatos
                .AnyAsync(c => c.Email == candidatoDto.Email);

            if (emailExiste)
            {
                return BadRequest(new { mensagem = "E-mail já cadastrado no sistema." });
            }

            // Converte o DTO para a entidade Model
            var candidato = new Candidato
            {
                NomeCompleto = candidatoDto.NomeCompleto,
                Email = candidatoDto.Email,
                Telefone = candidatoDto.Telefone,
                CPF = candidatoDto.CPF,
                DataNascimento = candidatoDto.DataNascimento,
                Genero = candidatoDto.Genero,
                Endereco = candidatoDto.Endereco,
                Escolaridade = candidatoDto.Escolaridade,
                Experiencia = candidatoDto.Experiencia,
                Habilidades = candidatoDto.Habilidades,
                CurriculoUrl = candidatoDto.CurriculoUrl,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            // Adiciona o candidato no banco de dados
            _context.Candidatos.Add(candidato);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var candidatoResponse = new CandidatoResponseDto
            {
                Id = candidato.Id,
                NomeCompleto = candidato.NomeCompleto,
                Email = candidato.Email,
                Telefone = candidato.Telefone,
                CPF = candidato.CPF,
                DataNascimento = candidato.DataNascimento,
                Genero = candidato.Genero,
                Endereco = candidato.Endereco,
                Escolaridade = candidato.Escolaridade,
                Experiencia = candidato.Experiencia,
                Habilidades = candidato.Habilidades,
                CurriculoUrl = candidato.CurriculoUrl,
                DataCadastro = candidato.DataCadastro,
                Ativo = candidato.Ativo,
                TotalInscricoes = 0
            };

            // Retorna 201 Created
            return CreatedAtAction(nameof(GetCandidato), new { id = candidato.Id }, candidatoResponse);
        }


        // === PUT: api/v1/Candidato/5 ===

        /// <summary>
        /// Atualiza os dados de um candidato existente.
        /// Não permite alterar CPF (apenas dados cadastrais).
        /// </summary>
        /// <param name="id">ID do candidato a ser atualizado.</param>
        /// <param name="candidatoDto">Novos dados do candidato.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Candidato atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos ou e-mail já cadastrado.</response>
        /// <response code="404">Candidato não encontrado.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutCandidato(int id, CandidatoUpdateDto candidatoDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o candidato existe
            var candidatoExistente = await _context.Candidatos.FindAsync(id);
            if (candidatoExistente == null)
            {
                return NotFound(new { mensagem = "Candidato não encontrado." });
            }

            // Verifica se o novo e-mail já está sendo usado por outro candidato
            var emailEmUso = await _context.Candidatos
                .AnyAsync(c => c.Email == candidatoDto.Email && c.Id != id);

            if (emailEmUso)
            {
                return BadRequest(new { mensagem = "E-mail já cadastrado por outro candidato." });
            }

            // Atualiza os dados (preserva CPF e data de cadastro)
            candidatoExistente.NomeCompleto = candidatoDto.NomeCompleto;
            candidatoExistente.Email = candidatoDto.Email;
            candidatoExistente.Telefone = candidatoDto.Telefone;
            candidatoExistente.DataNascimento = candidatoDto.DataNascimento;
            candidatoExistente.Genero = candidatoDto.Genero;
            candidatoExistente.Endereco = candidatoDto.Endereco;
            candidatoExistente.Escolaridade = candidatoDto.Escolaridade;
            candidatoExistente.Experiencia = candidatoDto.Experiencia;
            candidatoExistente.Habilidades = candidatoDto.Habilidades;
            candidatoExistente.CurriculoUrl = candidatoDto.CurriculoUrl;
            candidatoExistente.Ativo = candidatoDto.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar candidato. Tente novamente." });
            }

            return NoContent();
        }


        // === DELETE: api/v1/Candidato/5 ===

        /// <summary>
        /// Desativa um candidato (exclusão lógica).
        /// Não remove do banco, apenas marca como inativo.
        /// </summary>
        /// <param name="id">ID do candidato a ser desativado.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Candidato desativado com sucesso.</response>
        /// <response code="404">Candidato não encontrado.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteCandidato(int id)
        {
            // Busca o candidato
            var candidato = await _context.Candidatos.FindAsync(id);
            if (candidato == null)
            {
                return NotFound(new { mensagem = "Candidato não encontrado." });
            }

            // Exclusão lógica: apenas marca como inativo
            candidato.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
