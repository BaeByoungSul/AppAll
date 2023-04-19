namespace Models;

public class Hosting
{
    public string Addr { get; set; } = string.Empty;
    public string PortHttp { get; set; }= string.Empty; 
    public string PortHttps { get; set; } = string.Empty;
    public string SslPath { get; set; } = string.Empty;
    public string SslPassword { get; set; } = string.Empty; 
}
