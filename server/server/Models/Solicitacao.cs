using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "interval")]
        public TimeSpan TempoLimiteLance { get; set; }
        public int LoteId { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string UsuarioId { get; set; }
        public Lote Lote { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
