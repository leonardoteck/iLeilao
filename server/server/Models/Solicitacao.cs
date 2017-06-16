using System;

namespace server.Models
{
    public enum StatusSolicitacao
    {
        EmAguardo,
        Aceito,
        Negado
    }

    public class Solicitacao
    {
        public int Id { get; set; }
        public TimeSpan TempoLimiteLance;
        public int LoteId { get; set; }
        public DateTime Data { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public StatusSolicitacao Status { get; set; }
        public Lote Lote { get; set; }      
    }
}
