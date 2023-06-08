namespace apiOperadores.Models
{
    public class AppOpAztecaAcceso
    {
        public int? Id { get; set; }
        public int? FkPerfilId { get; set; }
        public int? FkModuloProcesoId { get; set; }
        public bool? Tipo { get; set; }
        public bool? PerfilStatus { get; set; }
        public bool? ProcesoStatus { get; set; }
    }
}
