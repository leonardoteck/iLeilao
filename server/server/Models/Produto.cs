
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        [Column(TypeName = "varchar")]
        public string Imagem { get; set; }
        public int LoteId { get; set; }
        [JsonIgnore]
        public virtual Lote Lote { get; set; }
    }
}
