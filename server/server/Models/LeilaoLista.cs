namespace server.Models
{
    public class LeilaoLista
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Imagem { get; set; }
        public decimal ValorMinimo { get; set; }
        public Lance MaiorLance { get; set; }
    }
}
