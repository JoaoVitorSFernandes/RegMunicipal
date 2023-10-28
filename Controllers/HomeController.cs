using DesafioBalta.Data;
using DesafioBalta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioBalta.Controllers
{
    [ApiController]
    [Route("home/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] DataContext context)
            => Ok(context.IBGEs.AsNoTracking().ToList());


        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] IBGE ibge,
            [FromServices] DataContext context)
        {
            if (ibge == null)
                return BadRequest(new { message = "Object posted is null." });

            context.IBGEs.Add(ibge);
            context.SaveChanges();

            return Created($"{ibge.Id}", ibge);
        }


        [HttpPut]
        public async Task<IActionResult> PutAsync(
            [FromBody] IBGE model,
            [FromServices] DataContext context)
        {
            var localidade = context
                                .IBGEs
                                .AsNoTracking()
                                .FirstOrDefault(x => x.Id == model.Id);

            if (localidade == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco" });

            localidade.City = model.City;
            localidade.State = model.State;

            context.IBGEs.Update(localidade);
            context.SaveChanges();

            return Ok(localidade);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string id,
            [FromServices] DataContext context)
        {
            var localidade = context
                                .IBGEs
                                .AsNoTracking()
                                .FirstOrDefault(x => x.Id == id);

            if (localidade == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco"});

            context.IBGEs.Remove(localidade);
            context.SaveChanges();

            return Ok(new { message = "Registro deletado com sucesso" });
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] string id,
            [FromServices] DataContext context)
        {
            var local = context
                        .IBGEs
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Id == id);

            if (local == null)
                return BadRequest(new { message = "Get Não foram encontradas informações em nosso banco" });

            return Ok(local);
        }


        [HttpGet]
        [Route("city/{city}")]
        public async Task<ActionResult> GetByCityAsync(
            [FromRoute] string city,
            [FromServices] DataContext context)
        {
            var locais = context
                            .IBGEs
                            .AsNoTracking()
                            .Where(x => x.City == city)
                            .ToList();

            if (locais == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco" });

            return Ok(locais);
        }


        [HttpGet]
        [Route("state/{state}")]
        public async Task<ActionResult> GetByStateAsycn(
            [FromRoute] string state,
            [FromServices] DataContext context)
        {
            var locais = context
                            .IBGEs
                            .AsNoTracking()
                            .Where(x => x.State == state)
                            .ToList();

            if (locais == null)
                return BadRequest(new { message = "Não foram encontradas informações em nosso banco" });

            return Ok(locais);
        }
    }
}
