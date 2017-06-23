using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Solicitacoes")]
    public class SolicitacoesController : Controller
    {
        private readonly LeilaoContext context;

        public SolicitacoesController(LeilaoContext ctx)
        {
            context = ctx;
        }

        // GET: api/Solicitacoes
        [HttpGet]
        public IEnumerable<Solicitacao> GetSolicitacao()
        {
            return context.Solicitacao;
        }

        // GET: api/Solicitacoes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolicitacao([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solicitacao = await context.Solicitacao.SingleOrDefaultAsync(m => m.Id == id);

            if (solicitacao == null)
            {
                return NotFound();
            }

            return Ok(solicitacao);
        }

        // PUT: api/Solicitacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitacao([FromRoute] int id, [FromBody] Solicitacao solicitacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != solicitacao.Id)
            {
                return BadRequest();
            }

            context.Entry(solicitacao).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Solicitacoes
        [HttpPost]
        public async Task<IActionResult> PostSolicitacao([FromBody] Solicitacao solicitacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Solicitacao.Add(solicitacao);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitacao", new { id = solicitacao.Id }, solicitacao);
        }

        // DELETE: api/Solicitacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitacao([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var solicitacao = await context.Solicitacao.SingleOrDefaultAsync(m => m.Id == id);
            if (solicitacao == null)
            {
                return NotFound();
            }

            context.Solicitacao.Remove(solicitacao);
            await context.SaveChangesAsync();

            return Ok(solicitacao);
        }

        private bool SolicitacaoExists(int id)
        {
            return context.Solicitacao.Any(e => e.Id == id);
        }
    }
}