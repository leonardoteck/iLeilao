using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Produtos")]
    //[Authorize]
    public class ProdutosController : Controller
    {
        private readonly LeilaoContext ctx;

        public ProdutosController(LeilaoContext context)
        {
            ctx = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public IEnumerable GetAll()
        {
            return ctx.Produto.ToList();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            // ERRO: Não retorna resposta com eager loading. Sem eager loading,
            // retorna corretamente

            var produto = await ctx.Produto
                .Include(p => p.Lote)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(produto != null)
            {
                return Ok(produto);
            }
            else
            {
                ModelState.AddModelError("Produto", "Produto não encontrado no sistema.");
                return NotFound(ModelState.Values.SelectMany(p => p.Errors));
            }
        }

        // POST: api/Produtos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Produto produto)
        {
            var loteExists = await ctx.Lote.AnyAsync(l => l.Id.Equals(produto.LoteId));

            if(loteExists)
            {
                // Lote encontrado, cria produto
                ctx.Produto.Add(produto);
                await ctx.SaveChangesAsync();
                return CreatedAtAction("Create", produto);
            }
            else
            {
                // Lote não encontrado, retorna erro
                ModelState.AddModelError("Lote", "Lote não encontrado no sistema.");
                return NotFound(ModelState.Values.SelectMany(e => e.Errors));
            }
        }

        // PUT: api/Produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] Produto novoProduto)
        {
            var produto = await ctx.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(novoProduto != null)
            {
                // Produto encontrado no banco de dados, proceder com alterações
                produto.Nome = novoProduto.Nome;
                produto.Descricao = novoProduto.Descricao;
                produto.Quantidade = novoProduto.Quantidade;
                produto.LoteId = novoProduto.LoteId;

                // Salva alterações no banco
                ctx.Produto.Update(produto);
                await ctx.SaveChangesAsync();
                return Ok(produto);
            }
            else
            {
                // Produto não encontrado, retornar erro
                ModelState.AddModelError("Produto", "Produto não encontrado no sistema.");
                return NotFound(ModelState.Values.SelectMany(e => e.Errors));
            }
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var produto = await ctx.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(produto != null)
            {
                // Produto encontrado, proceder com exclusão
                ctx.Produto.Remove(produto);
                await ctx.SaveChangesAsync();
                return Ok();
            }
            else
            {
                // Produto não encontrado, retornar erro
                ModelState.AddModelError("Produto", "Produto não encontrado no sistema.");
                return NotFound(ModelState.Values.SelectMany(e => e.Errors));
            }
        }

        // Utilitario
        private bool Exists(int id)
        {
            return ctx.Produto.Any(e => e.Id == id);
        }
    }
}