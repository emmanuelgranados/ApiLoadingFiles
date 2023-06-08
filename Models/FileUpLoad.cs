namespace apiOperadores.Models
{
    public class FileUpLoad
    {
        public IFormFile? File { get; set; }
        public string? NumeroViaje { get; set; }
        public string? Operador { get; set; }
        public int? Kilometraje { get; set; }
        public string? Observaciones { get; set; }
        public int? DocumentoId { get; set; }

    }
}
