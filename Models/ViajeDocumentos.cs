namespace apiOperadores.Models
{
    public class ViajeDocumentos
    {
        public int? Id { get; set; }
        public string? DocumentoNombre { get; set; }
        public string? UrlDocumento { get; set; }
        public string? ClienteNombre { get; set; }
        public int? FkEstadoId { get; set; }
        public string? EstadoNombre { get; set; }
        public string? EstadoColor { get; set; }
    }
}
