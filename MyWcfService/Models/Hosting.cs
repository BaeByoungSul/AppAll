namespace Models;

public class Hosting
{
    public string Addr { get; set; } = string.Empty;
    public string DbHttpPort { get; set; }= string.Empty; 
    public string DbTcpPort { get; set; } = string.Empty;
    public string FileHttpPort { get; set; } = string.Empty;
    public string FileTcpPort { get; set; } = string.Empty;
    public string SslPath { get; set; } = string.Empty;
    public string SslPassword { get; set; } = string.Empty; 
}
