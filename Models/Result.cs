namespace apiOperadores.Models
{
    public class Result
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public string? code { get; set; }
        public object? data { get; set; }
        public int? total { get; set; }
    }
}
