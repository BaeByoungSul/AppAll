using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Models.Auth;
using Models.Email;
using Services.AuthService;
using Services.TokenService;
using Services;
using System.Security.Cryptography;
using Services.EmailService;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Models.Auth;

namespace MyWebApi.Controllers;
/// <summary>
/// 2023.10.25 refresh Token은 login할 때만 생성한다. RefreshToken 함주에서는 생성하지 않음
/// 2023.10.25 Login할 때 SetRefreshToken이 어떤 기능을 하는지 몰라서 일단 주석처리
/// </summary>

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    //private readonly AngularConfig _angularconfig;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        IAuthService authService,
        IEmailService emailService,
        JwtTokenService jwtTokenService)
        //AngularConfig angularConfig)
    {
        _authService = authService;
        _emailService = emailService;
        //_angularconfig = angularConfig; 
        
        _jwtTokenService = jwtTokenService;
    }

    
    [HttpPost("Login")]
    public IActionResult Login(UserLogin _userData)
    {
        if (_userData == null ||
            _userData.Email == null ||
            _userData.Password == null)
        {
            return Unauthorized();
        }

        try
        {
            var user = _authService.GetUserByEmail(_userData.Email);
            if (user == null)
            {
                return BadRequest("Unregistered email ");
                //return Unauthorized("Unregistered email");
            }
            if (!_authService.VerifyPasswordHash(_userData.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect ");
            }

            //var user = _authService.Login(_userData.Email, _userData.Password);

            //var token = CreateToken(user);
            var accessToken = _jwtTokenService.CreateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiryTime = refreshToken.Expires;

            _authService.UpdateRefreshToken(user);
            // SetRefreshToken(refreshToken);

            //return Ok(new
            //{
            //    AccessToken = accessToken,
            //    RefreshToken = refreshToken,
            //    UserHome = user.UserHome??string.Empty
            //});

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            //throw;
        }

    }
    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser(UserRegister model)
    {
        try
        {
            var user = _authService.GetUserByEmail(model.Email);
            if (user != null)
            {
                return BadRequest("User email '" + model.Email + "' is already registered");
            }

            var regUser = _authService.Register(model);
            if (regUser == null)
            {
                return BadRequest("User email '" + model.Email + "' register error");
            }

            var param = new Dictionary<string, string?> {
                { "token", regUser.VerificationToken },
                { "email", regUser.Email }
            };

            //var confirmationLink = Url.Action(
            //        nameof(ConfirmEmail)
            //        , "Auth"
            //        , param
            //        , Request.Scheme);
            //var callback = QueryHelpers.AddQueryString("http://localhost:4200/auth/verify-user", param);
            //var callback = QueryHelpers.AddQueryString(_angularconfig.UserVerifyUrl, param);
            var callback = QueryHelpers.AddQueryString(model.VerifyUserUrl, param);

            EmailDto emailContent = new EmailDto();
            emailContent.Subject = "Veryfy User Register Email";
            emailContent.To = regUser.Email;
            //emailContent.Body = confirmationLink ?? string.Empty;
            emailContent.Body = callback ?? string.Empty;
            _emailService.SendDbEmail(emailContent);

            return Ok(new { message = "Registration successful. Before login, please confirm your email." });

        }
        catch (Exception ex)
        {

            return Problem(ex.Message);

        }



        //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth"
        //            , new { token = userHash.VerificationToken, email = request.Email }, Request.Scheme);

        //string bodyString = "Please confirm your account by clicking <a href=\"" +
        //    confirmationLink + "\">here</a>";
        //EmailDto email = new EmailDto();
        //email.Subject = "Confirmation email";
        //email.To = request.Email;
        //email.Body = bodyString;

        //_emailService.SendDbEmail(email);


    }

    [HttpPost("VerifyEmail")]
    public IActionResult VerifyEmail(UserVerify model)
    {
        try
        {
            //string token, string email
            var user = _authService.GetUserByEmail(model.Email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }
            if (user.VerificationToken != model.Token)
            {
                return BadRequest("Invalid token.");
            }
            if (user.VerifiedDate != null)
            {
                return BadRequest("This user has been already verified .");
            }
            _authService.UpdateUser_VerifiedDate(model.Email);

            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);

        }

    }


    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword(string email, string resetPasswordUrl)
    {
        try
        {
            var user = _authService.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest("Invalid user");
            }

            var token = CreateRandomToken();
            _authService.UpdateUser_ForgotPassword(email, token);

            var param = new Dictionary<string, string?> {
                { "token", token }, { "email", email }
            };

            //var callback = QueryHelpers.AddQueryString("http://localhost:4200/auth/reset-password", param);

            //var callback = QueryHelpers.AddQueryString(_angularconfig.ResetPasswordUrl, param);
            var callback = QueryHelpers.AddQueryString(resetPasswordUrl, param);

            EmailDto emailContent = new EmailDto();
            emailContent.Subject = "Reset password email";
            emailContent.To = email;
            emailContent.Body = callback;

            _emailService.SendDbEmail(emailContent);
            return Ok();

        }
        catch (Exception ex)
        {

            return Problem(ex.Message);

        }


    }

    [HttpPost("ResetPassword")]
    public IActionResult ResetPassword(ResetPassword request)
    {
        var user = _authService.GetUserByEmail(request.Email);
        if (user == null)
        {
            return BadRequest("User not found.");
        }
        if (user.PasswordResetToken != request.Token)
        {
            return BadRequest("Token Error.");
        }

        if (user.ResetTokenExpires < DateTime.Now)
        {
            return BadRequest("Invalid Token.");
        }
        _authService.UpdateUser_ResetPassword(request.Email, request.Password);

        return Ok();
    }

    [HttpGet("RefreshToken")]
    public ActionResult RefreshToken(string refreshToken)
    {
        
        if (refreshToken.IsNullOrEmpty())
            return BadRequest("Invalid Refresh Token.");

        //if (jwtsecurity.ValidTo > DateTime.UtcNow)
        //    return BadRequest("Access token has not expired.");

        var user = _authService.GetUserByRefreshToken(refreshToken);
        if (user == null)
            return BadRequest("Invalid User.");

        //var refreshToken = Request.Cookies["refreshToken"];
        if (user.RefreshToken == null)
            return BadRequest("Invalid Refresh Token.");

        //if (!user.RefreshToken.Equals(refrshToken))
        //{
        //    return BadRequest("Invalid Refresh Token.");
        //}

        if (user.RefreshTokenExpiryTime < DateTime.Now)
            return BadRequest("Token expired.");

        var accessToken = _jwtTokenService.CreateToken(user);

        return Ok(new
        {
            AccessToken = accessToken
        });

    }
    [HttpPost("RefreshToken_bak")]
    public ActionResult RefreshToken_bak(Tokens token)
    {
        if (token.AccessToken == "")
            return BadRequest("Invalid Access Token.");

        if (token.RefreshToken == "")
            return BadRequest("Invalid Refresh Token.");

        var jwtsecurity = _jwtTokenService.ValidateJwtToken(token.AccessToken);
        if (jwtsecurity == null)
            return BadRequest("Invalid Access_Token Token.");

        if (jwtsecurity.ValidTo > DateTime.UtcNow)
            return BadRequest("Access token has not expired.");

        var userEmail = jwtsecurity.Claims.First(x => x.Type == "emailaddress").Value;
        if (userEmail == null)
            return BadRequest("Invalid Access Token.");


        var user = _authService.GetUserByEmail(userEmail);
        if (user == null)
            return BadRequest("Invalid User.");

        //var refreshToken = Request.Cookies["refreshToken"];
        if (user.RefreshToken == null)
            return BadRequest("Invalid Refresh Token.");

        if (!user.RefreshToken.Equals(token.RefreshToken))
        {
            return BadRequest("Invalid Refresh Token.");
        }
        else if (user.RefreshTokenExpiryTime < DateTime.Now)
        {
            return BadRequest("Token expired.");
        }

        var accessToken = _jwtTokenService.CreateToken(user);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiryTime = refreshToken.Expires;

        _authService.UpdateRefreshToken(user);



        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });



    }
    /// <summary>
    /// angular client 에서 cookie에 저장이 되지 않음. postman, swagger Ok
    /// </summary>
    /// <param name="newRefreshToken"></param>
    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken",
            newRefreshToken.Token,
            cookieOptions);

        // user.RefreshToken = newRefreshToken.Token;
        // user.TokenCreated = newRefreshToken.Created;
        // user.TokenExpires = newRefreshToken.Expires;
        //var cookie = new CookieOptions
        //{
        //    HttpOnly = true,
        //    Expires = DateTime.UtcNow.AddDays(10),
        //    //SameSite = SameSiteMode.None,
        //    //Secure = true,
        //    //Domain = "https://localhost:4200"
        //};

        //HttpContext.Response.Cookies.Append("rToken", refreshToken.Token, cookie);
        //Response.Headers.Add("Access-Control-Allow-Credentials", "true");

    }
    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    //private string ipAddress()
    //{
    //    // get source ip address for the current request
    //    if (Request.Headers.ContainsKey("X-Forwarded-For"))
    //        return Request.Headers["X-Forwarded-For"];
    //    else
    //        return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    //}

}
