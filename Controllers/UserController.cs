using DesafioBalta.Data;
using DesafioBalta.Models;
using DesafioBalta.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioBalta.Controllers
{
    [ApiController]
    [Route("user/")]
    public class UserController : ControllerBase
    {
        private TokenServices _tokenService = new TokenServices();

        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> GetAsync(
            [FromServices] DataContext context)
            => Ok(context.Users.AsNoTracking().ToList());

        [HttpPost]
        [Route("SingUp")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Email == model.Email
                                    || x.ProfileName == model.ProfileName
                                    && x.Password == model.Password);

            if (user == null)
                return BadRequest(new { message = "Usuario ou senha invalidos" });

            var token = _tokenService.GenerateToken(user);

            user.Password = "";

            return Ok(new
            {
                user = user,
                token = token
            });
        }

        [HttpPost]
        [Route("SingIn")]
        public async Task<ActionResult<dynamic>> PostAsync(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Email == model.Email
                                    || x.ProfileName == model.ProfileName
                                    && x.Password == model.Password);

            if (user != null)
                return BadRequest(new { message = "Usuario já existe" });

            var token = _tokenService.GenerateToken(model);

            context.Users.Add(model);
            context.SaveChanges();

            model.Password = "";

            return Created($"{user.Id}", new 
                    { user = user, 
                    token = token}
            );
        }

        [HttpPut]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult> UpdateAsync(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Id == model.Id);

            if (user == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco" });

            context.Users.Update(model);
            context.SaveChanges();

            model.Password = "";

            return Ok(model);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager, employee")]
        public async Task<ActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] DataContext context)
        {
            var user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco" });

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok(new { message = "Registro deletado com sucesso" });
        }
    }
}