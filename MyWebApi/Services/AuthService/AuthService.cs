using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Services.EmailService;
using Models.Auth;
using Models.Database;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Transactions;
using MyWebApi.Models.Auth;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Services.AuthService;
public class AuthService : IAuthService
{
    private readonly ConnectDBInfo? _connInfo;

    private readonly string _apiDbName = "ApiDbServe";

    public AuthService(
        List<ConnectDBInfo> dBInfos
        )
    {
        //Console.WriteLine( connectDBInfo);
        _connInfo = dBInfos.Find(x => x.ConnectionName == _apiDbName);
        if (_connInfo == null)
            throw new Exception("There's no Connection Info.");
        
    }
    public UserInfo? Register(UserRegister model)
    {
        try
        {
            if (GetUserByEmail(model.Email) != null)
            {
                throw new Exception("User email '" + model.Email + "' is already taken");
                //return BadRequest("User already exists.");
            }

            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var verificationToken = CreateRandomToken();


            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Ins";
            cmd.Parameters.AddWithValue("@EmailAddress ", model.Email);
            cmd.Parameters.AddWithValue("@PasswordHash ", passwordHash);
            cmd.Parameters.AddWithValue("@PasswordSalt ", passwordSalt);
            cmd.Parameters.AddWithValue("@VerificationToken ", verificationToken);
            cmd.Parameters.AddWithValue("@FirstName  ", model.FirstName);
            cmd.Parameters.AddWithValue("@LastName  ", model.LastName);
            cmd.Parameters.AddWithValue("@DisplayName  ", model.DisplayName);

            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);

            return GetUserByEmail(model.Email);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<UserInfo> GetAllUser()
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Sel_All";

            DataSet ds = MyGetDs(cmd);

            List<UserInfo> userList = new List<UserInfo>();
            userList = (from DataRow dr in ds.Tables[0].Rows
                        select new UserInfo()
                        {
                            //StudentId = Convert.ToInt32(dr["StudentId"]),
                            Email = dr["EmailAddress"].ToString() ?? string.Empty,
                            PasswordHash = (byte[])dr["PasswordHash"],
                            PasswordSalt = (byte[])dr["PasswordSalt"],
                            VerificationToken = dr["VerificationToken"].ToString(),
                            VerifiedDate = (dr["VerifiedDate"] == DBNull.Value) ? null : (DateTime)dr["VerifiedDate"],
                            FirstName = dr["FirstName"].ToString() ?? string.Empty,
                            LastName = dr["LastName"].ToString() ?? string.Empty,
                            DisplayName = dr["DisplayName"].ToString() ?? string.Empty,
                            MainRoles = dr["MainRoles"].ToString() ?? string.Empty,
                            Roles = dr["Roles"].ToString() ?? string.Empty,
                            UserHome = dr["UserHome"].ToString() ?? string.Empty,
                            RefreshToken = dr["RefreshToken"].ToString(),
                            RefreshTokenExpiryTime = (dr["RefreshTokenExpiryTime"] == DBNull.Value) ? null : (DateTime)dr["RefreshTokenExpiryTime"],
                            PasswordResetToken = dr["PasswordResetToken"].ToString(),
                            ResetTokenExpires = (dr["ResetTokenExpires"] == DBNull.Value) ? null : (DateTime)dr["ResetTokenExpires"],

                        }).ToList();
            return userList;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public UserInfo? GetUserByEmail(string email)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Sel";
            cmd.Parameters.AddWithValue("@EmailAddress ", email);

            DataSet ds = MyGetDs(cmd);
            if (ds.Tables.Count <= 0) return null;
            if (ds.Tables[0].Rows.Count <= 0) return null;

            //DataTable dt = ds.Tables[0];
            DataRow dr = ds.Tables[0].Rows[0];
            UserInfo user = new UserInfo()
            {
                Email = dr["EmailAddress"].ToString() ?? string.Empty,
                PasswordHash = (byte[])dr["PasswordHash"],
                PasswordSalt = (byte[])dr["PasswordSalt"],
                VerificationToken = dr["VerificationToken"].ToString(),
                VerifiedDate = (dr["VerifiedDate"] == DBNull.Value) ? null : (DateTime)dr["VerifiedDate"],
                FirstName = dr["FirstName"].ToString() ?? string.Empty,
                LastName = dr["LastName"].ToString() ?? string.Empty,
                DisplayName = dr["DisplayName"].ToString() ?? string.Empty,
                MainRoles = dr["MainRoles"].ToString() ?? string.Empty,
                Roles = dr["Roles"].ToString() ?? string.Empty,
                UserHome = dr["UserHome"].ToString() ?? string.Empty,
                RefreshToken = dr["RefreshToken"].ToString(),
                RefreshTokenExpiryTime = (dr["RefreshTokenExpiryTime"] == DBNull.Value) ? null : (DateTime)dr["RefreshTokenExpiryTime"],
                PasswordResetToken = dr["PasswordResetToken"].ToString(),
                ResetTokenExpires = (dr["ResetTokenExpires"] == DBNull.Value) ? null : (DateTime)dr["ResetTokenExpires"],
            };


            return user;
        }
        catch (Exception)
        {
            throw;
        }

      
    }
    public UserInfo? GetUserByRefreshToken(string refreshToken)
    {
        try
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Sel2";
            cmd.Parameters.AddWithValue("@RefreshToken ", refreshToken);

            DataSet ds = MyGetDs(cmd);
            if (ds.Tables.Count <= 0) return null;
            if (ds.Tables[0].Rows.Count <= 0) return null;

            //DataTable dt = ds.Tables[0];
            DataRow dr = ds.Tables[0].Rows[0];
            UserInfo user = new UserInfo()
            {
                Email = dr["EmailAddress"].ToString() ?? string.Empty,
                PasswordHash = (byte[])dr["PasswordHash"],
                PasswordSalt = (byte[])dr["PasswordSalt"],
                VerificationToken = dr["VerificationToken"].ToString(),
                VerifiedDate = (dr["VerifiedDate"] == DBNull.Value) ? null : (DateTime)dr["VerifiedDate"],
                FirstName = dr["FirstName"].ToString() ?? string.Empty,
                LastName = dr["LastName"].ToString() ?? string.Empty,
                DisplayName = dr["DisplayName"].ToString() ?? string.Empty,
                MainRoles = dr["MainRoles"].ToString() ?? string.Empty,
                Roles = dr["Roles"].ToString() ?? string.Empty,
                UserHome = dr["UserHome"].ToString() ?? string.Empty,
                RefreshToken = dr["RefreshToken"].ToString(),
                RefreshTokenExpiryTime = (dr["RefreshTokenExpiryTime"] == DBNull.Value) ? null : (DateTime)dr["RefreshTokenExpiryTime"],
                PasswordResetToken = dr["PasswordResetToken"].ToString(),
                ResetTokenExpires = (dr["ResetTokenExpires"] == DBNull.Value) ? null : (DateTime)dr["ResetTokenExpires"],
            };


            return user;
        }
        catch (Exception)
        {
            throw;
        }


    }


    public void UpdateRefreshToken(UserInfo model)
    {

        try
        {

            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Upd_RefreshToken";
            cmd.Parameters.AddWithValue("@EmailAddress", model.Email);
            cmd.Parameters.AddWithValue("@RefreshToken", model.RefreshToken);
            cmd.Parameters.AddWithValue("@RefreshTokenExpiryTime", model.RefreshTokenExpiryTime);

            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);

           // return GetUserByEmail(model.Email);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void UpdateUser_ForgotPassword(string email, string token)
    {
        try
        {

            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Upd_ForgotPassword";
            cmd.Parameters.AddWithValue("@EmailAddress", email);
            cmd.Parameters.AddWithValue("@PasswordResetToken", token);
            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);

            return;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public void UpdateUser_ResetPassword(string email, string password)
    {
        try
        {

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Upd_ResetPassword";
            cmd.Parameters.AddWithValue("@EmailAddress ", email);
            cmd.Parameters.AddWithValue("@PasswordHash ", passwordHash);
            cmd.Parameters.AddWithValue("@PasswordSalt ", passwordSalt);

            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);

            return ;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool UpdateUser_VerifiedDate(string email)
    {
        try
        {
        

            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Upd_VerifiedDate";
            cmd.Parameters.AddWithValue("@EmailAddress ", email);

            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);

            return true;
        }
        catch (Exception)
        {

            throw;
        }
        
    }

    public UserInfo? Login(string email, string password)
    {
        try
        {

            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Password is incorrect");
            }


            return user;
        }
        catch (Exception)
        {

            throw;
        }
    }


    #region Test ...

    public void RegisterUser(UserRegister model, AuthHash userhash)
    {
        try
        {
            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "WebApi..USP_UserInfo_Ins";
            cmd.Parameters.AddWithValue("@EmailAddress ", model.Email);
            cmd.Parameters.AddWithValue("@PasswordHash ", userhash.PasswordHash);
            cmd.Parameters.AddWithValue("@PasswordSalt ", userhash.PasswordSalt);
            cmd.Parameters.AddWithValue("@VerificationToken ", userhash.VerificationToken);
            cmd.Parameters.AddWithValue("@FirstName  ", model.FirstName);
            cmd.Parameters.AddWithValue("@LastName  ", model.LastName);
            cmd.Parameters.AddWithValue("@DisplayName  ", model.DisplayName);

            cmds.Add(cmd);

            this.MyExecuteNonQuery(cmds);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion Test ...


    /// <summary>
    /// private  function 
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>

    public DataSet MyGetDs(SqlCommand cmd)
    {
        try
        {
            if (_connInfo == null)
                throw new Exception("There's no Connection Info.");

            using (var sqlCon = new SqlConnection(_connInfo.ConnectionString))
            {

                SqlDataAdapter adapter = new SqlDataAdapter();
                cmd.Connection = sqlCon;
                adapter.SelectCommand = cmd;
                sqlCon.Open();
                DataSet ds = new DataSet();
                // Fill 메서드 실행하여 결과 DataSet을 리턴받음
                adapter.Fill(ds);
                return ds;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void MyExecuteNonQuery(List<SqlCommand> cmds)
    {
        try
        {
            if (_connInfo == null)
                throw new Exception("There's no Connection Info.");
            using (SqlConnection conn = new SqlConnection(_connInfo.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                foreach (var cmd in cmds)
                {
                    //SqlCommand cmd = new SqlCommand(commandString, conn, transaction);
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    #region private function 
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA256(passwordSalt))
        {
            var computedHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        //using (var hmac = new HMACSHA512())
        using (var hmac = new HMACSHA256())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }

    #endregion private func


}


