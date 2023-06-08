namespace apiOperadores.Models
{
    public class AppOpAztecaViajes
    {
        public int? Id { get; set; }
        public string? NumeroViaje { get; set; }
        public int? NumeroLiquidacion { get; set; }
        public string? RutaNombre { get; set; }
        public string? ClienteRemitente { get; set; }
        public string? ClienteDestinario { get; set; }
        public decimal? PesoToneladas { get; set; }
        public string? TipoArmado { get; set; }
        public string? ClaveUnidad { get; set; }
        public string? ClaveRemolque1 { get; set; }
        public string? ClaveDolly { get; set; }
        public string? ClaveRemolque2 { get; set; }
        public decimal? KilometrajeInicial { get; set; }
        public decimal? KilometrajeFinal { get; set; }
        public decimal? KilometrosRutaManual { get; set; }
        public decimal? KilometrosRecorridos { get; set; }

        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public int? FkEstadoDocumentosId { get; set; }
        public string? EstadoDocumento { get; set; }
        public string? EstadoDocumentoColor { get; set; }
        public int? TotalDocumentos { get; set; }
        public int? TotalDocumentosAprobados { get; set; }
        public int? FkEstadoViajeId { get; set; }
        public string? EstadoViaje { get; set; }
    }
}
