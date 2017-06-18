
using System.Collections.Generic;

namespace server.Models
{
    public class Lote
    {
        public int Id { get; set; }
        public virtual IEnumerable<Produto> Produtos { get; set; }
        public decimal ValorMinimo { get; set; }
        public string VendedorId { get; set; }
        public virtual Usuario Vendedor { get; set; }
    }
}
