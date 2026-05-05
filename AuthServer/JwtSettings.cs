namespace AuthServer;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Iss { get; set; } = string.Empty;
    public string Aud { get; set; } = string.Empty;
}
