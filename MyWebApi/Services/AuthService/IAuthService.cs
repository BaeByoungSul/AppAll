using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Models.Auth;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace Services.AuthService;
public interface IAuthService
{
    void RegisterUser(UserRegister user, AuthHash userhash);
    UserInfo? Register(UserRegister user);
    bool UpdateUser_VerifiedDate(string email);
    void UpdateUser_ForgotPassword(string email, string token);
    void UpdateUser_ResetPassword(string email, string password);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    //UserInfo? Login(string username, string password);
    List<UserInfo> GetAllUser();
    UserInfo? GetUserByEmail(string username);

    UserInfo? GetUserByRefreshToken(string refreshToken);
    void UpdateRefreshToken(UserInfo user);
    
    DataSet MyGetDs(SqlCommand cmd);
    public void MyExecuteNonQuery(List<SqlCommand> cmds);
}

