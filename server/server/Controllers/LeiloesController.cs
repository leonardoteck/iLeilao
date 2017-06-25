using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Leiloes")]
    //[Authorize]
    public class LeiloesController : Controller
    {
        private readonly LeilaoContext context;

        public LeiloesController(LeilaoContext ctx)
        {
            context = ctx;
        }

        //Retorna todos os leilões em andamento
        // GET: api/Leiloes
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var leiloes = context.Leilao
                    .Include(l => l.MaiorLance)
                    .Include(l => l.Lote)
                    .Where(l => l.Status.Equals(StatusLeilao.EmAndamento));

                var leilaoLista = EncurtarLeilao(leiloes);

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna todos os leilões em andamento, 
        //em ordem decrescente de popularidade
        // GET: api/Leiloes/populares
        [HttpGet("populares")]
        public IActionResult GetPopulares()
        {
            try
            {
                var leiloes = context.Leilao
                    .Include(l => l.MaiorLance)
                    .Include(l => l.Lote)
                    .Include(l => l.Lances)
                    .Where(l => l.Status.Equals(StatusLeilao.EmAndamento))
                    .OrderByDescending(l => l.Lances.Count());

                var leilaoLista = EncurtarLeilao(leiloes);

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna todos os leilões criados pelo usuário
        // GET: api/Leiloes/usuario/usuarioId
        [HttpGet("usuario/{usuarioId}")]
        public IActionResult GetUsuario([FromRoute] string usuarioId)
        {
            try
            {
                var leiloes = context.Leilao
                    .Include(l => l.MaiorLance)
                    .Include(l => l.Lote)
                    .Where(l => l.UsuarioId.Equals(usuarioId));

                var leilaoLista = EncurtarLeilao(leiloes);

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna todos os leilões em andamento que o usuário está participando
        // GET: api/Leiloes/usuario/participando/usuarioId
        [HttpGet("usuario/participando/{usuarioId}")]
        public async Task<IActionResult> GetParticipando([FromRoute] string usuarioId)
        {
            try
            {
                //NÃO FUNCIONA ESSA MERDA***********************************************************
                var leiloes = await context.Lance
                    .Select(l => l.Leilao)
                    .Distinct()
                    .Where(l => l.UsuarioId.Equals(usuarioId))
                    .ToListAsync();


                foreach (var leilao in leiloes)
                {
                    leilao.Lote = await context.Lote.FirstOrDefaultAsync(l => l.Id.Equals(leilao.LoteId));
                    leilao.MaiorLance = await context.Lance.AsNoTracking().FirstOrDefaultAsync(l => l.Id.Equals(leilao.MaiorLanceId));
                }


                var leilaoLista = EncurtarLeilao(leiloes.AsQueryable<Leilao>());

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna todos os leilões encerrados
        // GET: api/Leiloes/encerrados
        [HttpGet("encerrados")]
        public IActionResult GetEncerrados()
        {
            try
            {
                var leiloes = context.Leilao
                    .Include(l => l.Lote)
                    .Include(l => l.MaiorLance)
                    .Where(l => l.Status.Equals(StatusLeilao.Finalizado));

                var leilaoLista = EncurtarLeilao(leiloes);

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna todos os leilões aguardando finalizacao
        // GET: api/Leiloes/finalizacao
        [HttpGet("finalizacao")]
        public IActionResult GetFinalizacao()
        {
            try
            {
                var leiloes = context.Leilao
                    .Include(l => l.Lote)
                    .Include(l => l.MaiorLance)
                    .Where(l => l.Status.Equals(StatusLeilao.AguardandoFinalizacao));

                var leilaoLista = EncurtarLeilao(leiloes);

                return Ok(leilaoLista);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Retorna um leilão específico
        // GET: api/Leiloes/5
        [HttpGet("{leilaoId}")]
        public async Task<IActionResult> Get([FromRoute] int leilaoId)
        {
            try
            {
                var leilao = await context.Leilao
                    .Include(l => l.MaiorLance)
                        .ThenInclude(m => m.Usuario)
                    .Include(l => l.Lote)
                        .ThenInclude(l => l.Produtos)
                    .Include(l => l.Lances)
                        .ThenInclude(l => l.Usuario)
                    .FirstOrDefaultAsync(l => l.Id.Equals(leilaoId));

                if (leilao != null)
                {
                    return Ok(leilao);
                }
                else
                {
                    ModelState.AddModelError("Leilão", "Leilão não encontrado.");
                    return NotFound(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        // PUT: api/Leiloes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] Leilao novoLeilao)
        {
            try
            {
                var leilao = await context.Leilao.AsNoTracking().FirstOrDefaultAsync(s => s.Id.Equals(novoLeilao.Id));

                if (leilao != null)
                {
                    leilao = novoLeilao;

                    context.Leilao.Update(leilao);
                    await context.SaveChangesAsync();

                    return Ok(leilao);
                }
                else
                {
                    ModelState.AddModelError("Leilão", "Leilão não encontrado.");
                    return NotFound(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //Não é um endpoint pois os leilões são criados automaticamente
        //após a aprovação da solicitação
        public async Task<Leilao> Create(Solicitacao solicitacao)
        {
            Leilao leilao = new Leilao
            {
                Nome = solicitacao.Nome,
                DataInicio = DateTime.Now,
                DataFinal = DateTime.Now.AddDays(solicitacao.DiasDuracao),
                TempoLimiteLance = solicitacao.TempoLimiteLance,
                LoteId = solicitacao.LoteId,
                UsuarioId = solicitacao.UsuarioId,
                IncrementoMinimo = solicitacao.IncrementoMinimo,
                Status = StatusLeilao.EmAndamento
            };

            context.Leilao.Add(leilao);
            await context.SaveChangesAsync();

            return leilao;
        }

        // DELETE: api/Leiloes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var leilao = await context.Leilao
                    .Include(s => s.Lote)
                    .FirstOrDefaultAsync(l => l.Id.Equals(id));

                if (leilao != null)
                {
                    //Nem Lote nem Produto dependem de Leilao, então se deletar o leilão, 
                    //produtos e lote se mantém
                    //mas ao apagar o lote (que é relação tanto de leilão, quanto de produto, 
                    //o leilão e os produtos tbm são apagados
                    context.Lote.Remove(leilao.Lote);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("Leilão", "Leilão não encontrado.");
                    return NotFound(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("encerrar/{id}")]
        public async Task<IActionResult> Encerrar([FromRoute] int id, [FromBody] string senha)
        {
            try
            {
                var leilao = await context.Leilao.FirstOrDefaultAsync(l => l.Id.Equals(id));

                if (leilao != null)
                {
                    leilao.Status = StatusLeilao.Finalizado;

                    context.Leilao.Update(leilao);
                    await context.SaveChangesAsync();

                    return Ok(leilao);
                }
                else
                {
                    ModelState.AddModelError("Leilão", "Leilão não encontrado.");
                    return NotFound(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("lance/{id}")]
        public async Task<IActionResult> AddLance([FromRoute] int id, [FromBody] Lance lance)
        {
            try
            {
                var leilao = await context.Leilao
                    .Include(l => l.MaiorLance)
                    .FirstOrDefaultAsync(l => l.Id.Equals(id));

                var usuario = await context.Usuario
                    .FirstOrDefaultAsync(u => u.Id.Equals(lance.UsuarioId));

                if ((leilao != null) && (usuario != null))
                {
                    lance.Data = DateTime.Now;
                    context.Lance.Add(lance);

                    if ((leilao.MaiorLance == null) || (leilao.MaiorLance.Valor < lance.Valor))
                    {
                        leilao.MaiorLance = lance;
                        context.Leilao.Update(leilao);
                    }

                    await context.SaveChangesAsync();
                    return Ok(leilao);
                }
                else
                {
                    ModelState.AddModelError("Leilão", "Leilão não encontrado.");
                    return NotFound(ModelState.Values.SelectMany(e => e.Errors));
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        public IList<LeilaoLista> EncurtarLeilao(IQueryable<Leilao> leiloes)
        {
            IList<LeilaoLista> leilaoLista = new List<LeilaoLista>();

            foreach (var auxLeilao in leiloes)
            {
                LeilaoLista aux = new LeilaoLista
                {
                    Id = auxLeilao.Id,
                    Titulo = auxLeilao.Nome,
                    Imagem = auxLeilao.Lote.Imagem,
                    ValorMinimo = auxLeilao.Lote.ValorMinimo,
                    MaiorLance = auxLeilao.MaiorLance
                };

                leilaoLista.Add(aux);
            }

            return leilaoLista;
        }
    }
}