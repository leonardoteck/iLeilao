using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Produces("application/json")]
    [Route("api/Cronometro")]
    public class CronometroController : Controller
    {
        private readonly LeilaoContext context;

        public CronometroController(LeilaoContext ctx)
        {
            context = ctx;
        }

        [HttpPost]
        public async Task VerificarTempo()
        {
            try
            {
                while (true)
                {
                    var leiloes = context.Leilao
                        .Include(l => l.Lances);

                    foreach (var leilao in leiloes)
                    {
                        //Compara a data final do leilão com a atual
                        int resultFinal = DateTime.Compare(leilao.DataFinal, DateTime.Now);

                        if (leilao.Lances != null)
                        {
                            //Pega a data do ultimo lance do leilão, soma com o
                            //tempo limite e compara com a atual
                            Lance ultimoLance = leilao.Lances.LastOrDefault();
                            DateTime limiteLance = ultimoLance.Data;
                            limiteLance.Add(leilao.TempoLimiteLance);
                            int resultLance = DateTime.Compare(limiteLance, DateTime.Now);

                            if (resultFinal < 0 || resultLance < 0)
                            {
                                //Se a hora final do leilão ou o tempo limite pro lance
                                //é maior que a atual,
                                //põe o leilão em status de aguardar finalização
                                leilao.Status = StatusLeilao.AguardandoFinalizacao;
                                context.Leilao.Update(leilao);
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                    TimeSpan minuto = new TimeSpan(0, 1, 0);
                    await Task.Delay(minuto);
                }
            }catch
            {
                throw;
            }
        }
    }
}
