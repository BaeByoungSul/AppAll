namespace Models.Auth;

public class JwtConfig
{

    public string SecurityKey { get; set; } = string.Empty;
    public string Issuer { get; set; }=string.Empty;
    public string Audience { get; set;} = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public int JwtExpiryMinites { get; set; }
    public int RefreshExpiryDays { get; set; }
    
}
//"Jwt": {
//    "SecurityKey": "TXkgbmFtZSBpcyBCeW91bmdTdWwgQmFlISBIb3cgYXJlIHlvdT8gSSBhbSBsZWFybmluZyBhc3AubmV0IGNvcmUgd2ViIGFwaS4=",
//    "Issuer": "JWTAuthenticationServer",
//    "Audience": "JWTServicePostmanClient",
//    "Subject": "JWTServiceAccessToken",
//    "JwtExpiryMinites": 60,
//    "RefreshExpiryDays": 7
//  },