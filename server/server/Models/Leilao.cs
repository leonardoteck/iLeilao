using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace server.Models
{
    public enum StatusLeilao
    {
        EmAndamento,
        AguardandoFinalizacao,
        Finalizado
    }

    public class Leilao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public TimeSpan TempoLimiteLance { get; set; }
        public int LoteId { get; set; }
        public int? MaiorLanceId { get; set; }
        public string UsuarioId { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public StatusLeilao Status { get; set; }
        public Lance MaiorLance { get; set; }
        public IEnumerable<Lance> Lances { get; set; }
        public Lote Lote { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
    }
}
