using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Produtos")]
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly LeilaoContext _context;

        public ProdutosController(LeilaoContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public IActionResult GetAll(string loteID)
        {
            var produtos = _context.Produto.Where(l => l.Id.Equals(loteID));

            if(produtos.Any())
            {
                return Ok(produtos);
            }
            else
            {
                ModelState.AddModelError("Lote", "Produtos em lote não encontrados.");
                return NotFound(ModelState.Values.SelectMany(p => p.Errors));
            }
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var produto = await _context.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));

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
            var loteExists = await _context.Lote.AnyAsync(l => l.Id.Equals(produto.LoteId));

            if(loteExists)
            {
                // Lote encontrado, cria produto
                var novoProduto = new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Quantidade = produto.Quantidade,
                    LoteId = produto.LoteId
                };
                await _context.Produto.AddAsync(novoProduto);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Create", novoProduto);
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
            var produto = await _context.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(novoProduto != null)
            {
                // Produto encontrado no banco de dados, proceder com alterações
                produto.Nome = novoProduto.Nome;
                produto.Descricao = novoProduto.Descricao;
                produto.Quantidade = novoProduto.Quantidade;
                produto.LoteId = novoProduto.LoteId;

                // Salva alterações no banco
                _context.Produto.Update(produto);
                await _context.SaveChangesAsync();
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
            var produto = await _context.Produto.FirstOrDefaultAsync(p => p.Id.Equals(id));

            if(produto != null)
            {
                // Produto encontrado, proceder com exclusão
                _context.Produto.Remove(produto);
                await _context.SaveChangesAsync();
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
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}