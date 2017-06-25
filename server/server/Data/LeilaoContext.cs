using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace server.Data
{
    public class LeilaoContext : IdentityDbContext<Usuario>
    {
        //O DbContext precisa de uma instancia de DbContextOptions para executar
        public LeilaoContext(DbContextOptions<LeilaoContext> options)
            : base(options) { }

        //Lista de entidades para o BD
        public DbSet<Lance> Lance { get; set; }
        public DbSet<Leilao> Leilao { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Solicitacao> Solicitacao { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<LoginModel>();
            builder.Ignore<LeilaoLista>();

            builder.Entity<Lance>()
                 .HasOne(lance => lance.Leilao)
                 .WithMany(leilao => leilao.Lances)
                 .HasForeignKey(lance => lance.LeilaoId);

            base.OnModelCreating(builder);
        }       
    }
}
