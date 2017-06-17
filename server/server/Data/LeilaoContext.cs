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

        //O usuário não precisa estar nessa lista, pois ele é declarado
        //na hora que a herança é feita

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<LoginModel>();
            //builder.Ignore<JwtIssuerOptions>();

            builder.Entity<Lance>()
                 .HasOne(lance => lance.Leilao)
                 .WithMany(leilao => leilao.Lances)
                 .HasForeignKey(lance => lance.LeilaoId);

            builder.Entity<Leilao>()
                .HasOne(leilao => leilao.MaiorLance)
                .WithMany()
                .HasForeignKey(leilao => leilao.MaiorLanceId);

            base.OnModelCreating(builder);
        }

        //O usuário não precisa estar nessa lista, pois ele é declarado
        //na hora que a herança é feita

        public DbSet<server.Models.Usuario> Usuario { get; set; }
    }
}
