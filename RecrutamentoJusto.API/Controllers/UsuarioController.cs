using Microsoft.EntityFrameworkCore;
using RecrutamentoJusto.API.Data;
using RecrutamentoJusto.API.Models;
using Asp.Versioning;
using Mvc = Microsoft.AspNetCore.Mvc;
using RecrutamentoJusto.API.DTOs.Usuario;


namespace RecrutamentoJusto.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de CRUD de Usuários.
    /// Gerencia usuários do sistema (RH, Gestores e Administradores).
    /// </summary>
    [Mvc.Route("api/v{version:apiVersion}/[controller]")]
    [Mvc.ApiController]
    [ApiVersion("1.0")]
    public class UsuarioController : Mvc.ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core.</param>
        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }


        // === GET: api/v1/Usuario ===
        /// <summary>
        /// Retorna todos os usuários cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        /// <response code="200">Retorna a lista de usuários com sucesso.</response>
        [Mvc.HttpGet]
        [Mvc.ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
        public async Task<Mvc.ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        {
            // Busca todos os usuários ativos, incluindo o nome da empresa
            var usuarios = await _context.Usuarios
                .Include(u => u.Empresa)
                .Where(u => u.Ativo)
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    EmpresaId = u.EmpresaId,
                    NomeEmpresa = u.Empresa != null ? u.Empresa.Nome : null,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo,
                    DataCadastro = u.DataCadastro,
                    Ativo = u.Ativo
                })
                .ToListAsync();

            return Ok(usuarios);
        }


        // === GET: api/v1/Usuario/5 ===

        /// <summary>
        /// Retorna um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Dados do usuário.</returns>
        /// <response code="200">Retorna o usuário encontrado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Mvc.HttpGet("{id}")]
        [Mvc.ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<UsuarioResponseDto>> GetUsuario(int id)
        {
            // Busca o usuário pelo ID
            var usuario = await _context.Usuarios
                .Include(u => u.Empresa)
                .Where(u => u.Id == id && u.Ativo)
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    EmpresaId = u.EmpresaId,
                    NomeEmpresa = u.Empresa != null ? u.Empresa.Nome : null,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo,
                    DataCadastro = u.DataCadastro,
                    Ativo = u.Ativo
                })
                .FirstOrDefaultAsync();

            // Se não encontrar, retorna 404
            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            return Ok(usuario);
        }


        // === GET: api/v1/Usuario/Empresa/5 ===

        /// <summary>
        /// Retorna todos os usuários de uma empresa específica.
        /// </summary>
        /// <param name="empresaId">ID da empresa.</param>
        /// <returns>Lista de usuários da empresa.</returns>
        /// <response code="200">Retorna a lista de usuários.</response>
        /// <response code="404">Empresa não encontrada.</response>
        [Mvc.HttpGet("Empresa/{empresaId}")]
        [Mvc.ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuariosPorEmpresa(int empresaId)
        {
            // Verifica se a empresa existe
            var empresaExiste = await _context.Empresas.AnyAsync(e => e.Id == empresaId);
            if (!empresaExiste)
            {
                return NotFound(new { mensagem = "Empresa não encontrada." });
            }

            // Busca todos os usuários da empresa
            var usuarios = await _context.Usuarios
                .Include(u => u.Empresa)
                .Where(u => u.EmpresaId == empresaId && u.Ativo)
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    EmpresaId = u.EmpresaId,
                    NomeEmpresa = u.Empresa != null ? u.Empresa.Nome : null,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo,
                    DataCadastro = u.DataCadastro,
                    Ativo = u.Ativo
                })
                .ToListAsync();

            return Ok(usuarios);
        }


        // === POST: api/v1/Usuario ===

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="usuarioDto">Dados do usuário a ser cadastrado.</param>
        /// <returns>Usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Dados inválidos, e-mail já cadastrado ou empresa não encontrada.</response>
        [Mvc.HttpPost]
        [Mvc.ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Mvc.ActionResult<UsuarioResponseDto>> PostUsuario(UsuarioCreateDto usuarioDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a empresa existe
            var empresa = await _context.Empresas.FindAsync(usuarioDto.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest(new { mensagem = "Empresa não encontrada ou inativa." });
            }

            // Verifica se o e-mail já está cadastrado
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == usuarioDto.Email);

            if (emailExiste)
            {
                return BadRequest(new { mensagem = "E-mail já cadastrado no sistema." });
            }

            // Converte o DTO para a entidade Model
            // NOTA: Em produção, a senha deve ser criptografada (ex: usando BCrypt)
            var usuario = new Usuario
            {
                EmpresaId = usuarioDto.EmpresaId,
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Senha = usuarioDto.Senha, // TODO: Criptografar em produção
                Tipo = usuarioDto.Tipo,
                DataCadastro = DateTime.Now,
                Ativo = true
            };

            // Adiciona o usuário no banco de dados
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Converte a entidade para DTO de resposta
            var usuarioResponse = new UsuarioResponseDto
            {
                Id = usuario.Id,
                EmpresaId = usuario.EmpresaId,
                NomeEmpresa = empresa.Nome,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo,
                DataCadastro = usuario.DataCadastro,
                Ativo = usuario.Ativo
            };

            // Retorna 201 Created com o usuário criado
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioResponse);
        }


        // === PUT: api/v1/Usuario/5 ===

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="usuarioDto">Novos dados do usuário.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Usuário atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos ou e-mail já cadastrado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Mvc.HttpPut("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> PutUsuario(int id, UsuarioUpdateDto usuarioDto)
        {
            // Valida se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o usuário existe
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            // Verifica se o novo e-mail já está sendo usado por outro usuário
            var emailEmUso = await _context.Usuarios
                .AnyAsync(u => u.Email == usuarioDto.Email && u.Id != id);

            if (emailEmUso)
            {
                return BadRequest(new { mensagem = "E-mail já cadastrado por outro usuário." });
            }

            // Atualiza os dados (preserva empresa, senha e data de cadastro)
            usuarioExistente.Nome = usuarioDto.Nome;
            usuarioExistente.Email = usuarioDto.Email;
            usuarioExistente.Tipo = usuarioDto.Tipo;
            usuarioExistente.Ativo = usuarioDto.Ativo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar usuário. Tente novamente." });
            }

            return NoContent();
        }


        // === DELETE: api/v1/Usuario/5 ===

        /// <summary>
        /// Desativa um usuário (exclusão lógica).
        /// Não remove do banco, apenas marca como inativo.
        /// </summary>
        /// <param name="id">ID do usuário a ser desativado.</param>
        /// <returns>Sem conteúdo.</returns>
        /// <response code="204">Usuário desativado com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Mvc.HttpDelete("{id}")]
        [Mvc.ProducesResponseType(StatusCodes.Status204NoContent)]
        [Mvc.ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Mvc.IActionResult> DeleteUsuario(int id)
        {
            // Busca o usuário
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuário não encontrado." });
            }

            // Exclusão lógica: apenas marca como inativo
            usuario.Ativo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
