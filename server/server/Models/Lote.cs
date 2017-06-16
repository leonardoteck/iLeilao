
using System.Collections.Generic;

namespace server.Models
{
    public class Lote
    {
        public int Id { get; set; }
        public IEnumerable<Produto> Produtos { get; set; }
        public decimal ValorMinimo { get; set; }
        public string VendedorId { get; set; }
        public Usuario Vendedor { get; set; }
    }
}
