using AngaSystem.API.Data;
using AngaSystem.API.Helpers;
using AngaSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AngaSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Login == usuario.Login))
                return BadRequest("Login já existe.");

            if (await _context.Usuarios.AnyAsync(x => x.Email == usuario.Email))
                return BadRequest("Email já cadastrado.");

            usuario.Senha = CryptHelper.Crypt(CryptHelper.CryptProvider.TripleDES, usuario.Senha, "X'!8(-],:=9=$*].~)?5$,^62;`77@~>}$@ *$?%803{9/[|]30[^(99+3!@=@^_");

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Usuario usuario)
        {
            var usuarioBanco = await _context.Usuarios.FindAsync(id);

            if (usuarioBanco == null)
                return BadRequest("Usuario não encontrado.");

            usuarioBanco.Nome = usuario.Nome ?? usuarioBanco.Nome;
            usuarioBanco.Login = usuario.Login ?? usuarioBanco.Login;
            usuarioBanco.Senha = usuario.Senha ?? usuarioBanco.Senha;
            usuarioBanco.Email = usuario.Email ?? usuarioBanco.Email;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return BadRequest("Usuario não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var senhaCriptografada = CryptHelper.Crypt(CryptHelper.CryptProvider.TripleDES, dto.Senha, "X'!8(-],:=9=$*].~)?5$,^62;`77@~>}$@ *$?%803{9/[|]30[^(99+3!@=@^_");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Login == dto.Login && x.Senha == senhaCriptografada);

            if (usuario == null)
                return Unauthorized("Usuário ou senha inválidos");

            // GERAR TOKEN
            var jwt = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwt["Key"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Login),
                new Claim("UserId", usuario.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresInMinutes"])),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // RETORNA TOKEN
            var resposta = new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Id = usuario.Id,
                Nome = usuario.Nome,
                Login = usuario.Login,
                Email = usuario.Email
            };

            return Ok(resposta);
        }
    }
}