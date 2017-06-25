using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Lance
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int LeilaoId { get; set; }
        [JsonIgnore]
        public Leilao Leilao { get; set; }
    }
}
