using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace server.Models
{
    public enum TipoUsuario
    {
        Vendedor,
        Administrador,
        Leiloeiro
    }

    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public TipoUsuario Tipo { get; set; }
    }
}
