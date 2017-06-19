using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Lotes")]
    [Authorize]
    public class LotesController : Controller
    {
        private readonly LeilaoContext _context;

        public LotesController(LeilaoContext context)
        {
            _context = context;
        }

        // GET: api/Lotes
        [HttpGet]
        public IActionResult GetAll()
        {
            return BadRequest();
        }

        // GET: api/Lotes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            return BadRequest();
        }

        // POST: api/Lotes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lote lote)
        {
            return BadRequest();
        }

        // PUT: api/Lotes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] Lote lote)
        {
            return BadRequest();
        }

        // DELETE: api/Lotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return BadRequest();
        }

        // Utilitario
        private bool Exists(int id)
        {
            return _context.Lote.Any(e => e.Id == id);
        }
    }
}