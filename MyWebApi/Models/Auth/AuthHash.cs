namespace Models.Auth;
public class AuthHash
{
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public string? VerificationToken { get; set; }
}
