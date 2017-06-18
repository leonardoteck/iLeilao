
namespace server.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public int LoteId { get; set; }
        public virtual Lote Lote { get; set; }
    }
}
