using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolidCursosApi.Data;
using SolidCursosApi.Model;

namespace SolidCursosApi.Controllers
{
    [Route("api/[controller]")]
    public class AutenticacaoController : Controller
    {
        private readonly SqlServerContext _context;

        public AutenticacaoController(SqlServerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<dynamic> Authenticate([FromBody]UsuarioAutenticacao model)
        {
            // Recupera o usuário
            var user = _context.Usuarios.SingleOrDefault(o => o.Login == model.Login && o.Senha == model.Senha);

            // Verifica se o usuário existe
            if (user == null || user.Id == 0)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Senha = "";

            // Retorna os dados
            return new
            {
                user,
                token
            };
        }
    }
}