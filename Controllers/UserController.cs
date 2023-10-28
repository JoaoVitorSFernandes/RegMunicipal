using DesafioBalta.Data;
using DesafioBalta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioBalta.Controllers
{
    [ApiController]
    [Route("user/")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("{emailuser}&{password}")]
        public async Task<ActionResult> GetAsync(
            [FromRoute] string emailuser,
            [FromRoute] string password,
            [FromServices] DataContext context)
        {
            var user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Email == emailuser && x.Password == password);

            if (user == null)
                user = context
                        .Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.ProfileName == emailuser && x.Password == password);

            if(user == null)
                return BadRequest(new {message = "Email ou Senha Invalidados"});

            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult> PostAsync(
            [FromBody] User user,
            [FromServices] DataContext context)
        {
            context.Users.Add(user);
            context.SaveChanges();

            return Created($"{user.Id}", user);
        }

        [HttpPut]
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