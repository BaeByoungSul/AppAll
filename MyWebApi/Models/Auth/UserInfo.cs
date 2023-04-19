using System.ComponentModel.DataAnnotations;
namespace Models.Auth;

public class UserInfo
{
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    
    public string? MainRoles { get; set; }
    public string? Roles { get; set; }
    public string? UserHome { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}

