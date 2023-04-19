using Microsoft.IdentityModel.Tokens;
using Models.Auth;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.TokenService;

/// <summary>
/// 토큰에는 최소한의 정보만 넣는게 보안에 좋다고 함.
/// </summary>
public class JwtTokenService
{
    private const int ExpirationMinutes = 30;
    private readonly JwtConfig _jwtconfig;

    public JwtTokenService(JwtConfig jwtConfig1)
    {
        _jwtconfig = jwtConfig1;
    }
    public string CreateToken( UserInfo user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtconfig.SecurityKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = CreateClaims(user);
        var token = new JwtSecurityToken(
            issuer: _jwtconfig.Issuer,
            audience: _jwtconfig.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtconfig.JwtExpiryMinites),
            claims: claims,
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(_jwtconfig.RefreshExpiryDays),
            Created = DateTime.Now
        };

        return refreshToken;
    }
   
    public JwtSecurityToken ValidateJwtToken(string tokenString)
    {
        try
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters validation = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtconfig.Issuer,
                ValidAudience = _jwtconfig.Audience,
                //LifetimeValidator = CustomLifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtconfig.SecurityKey))
            };
            SecurityToken token;
            ClaimsPrincipal principal = handler.ValidateToken(tokenString, validation, out token);
            return (JwtSecurityToken)token;
        }
        catch (Exception)
        {
            throw;
        }
       
    }
    private List<Claim> CreateClaims(UserInfo user)
    {
        try
        {
            var userClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, _jwtconfig.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                //new Claim(ClaimTypes.Name, user.Email),
                new Claim("name", user.Email),
                new Claim("emailaddress", user.Email)
            };
            string[] userMainRoles = user.MainRoles?.Split(",") ?? new[] { string.Empty };
            foreach (var item in userMainRoles)
            {
                userClaims.Add(new Claim("mainRoles", item.Trim()));
            }

            string[] userRoles = user.Roles?.Split(",") ?? new[] { string.Empty };
            foreach (var item in userRoles)
            {
                //userClaims.Add(new Claim(ClaimTypes.Role, item.Trim()));
                userClaims.Add(new Claim("role", item.Trim()));
            }

            return userClaims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
