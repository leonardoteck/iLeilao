
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Lote
    {
        public int Id { get; set; }
        public virtual IEnumerable<Produto> Produtos { get; set; }
        public decimal ValorMinimo { get; set; }
        public string VendedorId { get; set; }
        [Column(TypeName = "varchar")]
        public string Imagem { get; set; }
        public virtual Usuario Vendedor { get; set; }
    }
}
