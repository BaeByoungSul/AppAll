using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Data.SqlClient;
using Models.Email;
using Models.Database;

namespace Services.EmailService;

public class EmailService : IEmailService
{

    private readonly ConnectDBInfo? _connInfo;
    private readonly EmailConfig _emailInfo;
    
    public EmailService(
        List<ConnectDBInfo> dBInfos,
        EmailConfig emailInfo)
    {
        _connInfo = dBInfos.Find(x => x.ConnectionName == "KPL_TEST");
        if (_connInfo == null)
            throw new Exception("There's no Connection Info.");
        _emailInfo = emailInfo;

    }

    public void SendDbEmail(EmailDto request)
    {
        try
        {
            // db 연결정보 가져오기
            if (_connInfo == null)
                throw new Exception("There's no Connection Info.");

            using (var sqlCon = new SqlConnection(_connInfo.ConnectionString))
            {
                
                SqlCommand command = new SqlCommand();
                command.Connection = sqlCon;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "msdb..sp_send_dbmail";

                command.Parameters.AddWithValue("@profile_name", "MaintenanceMailProfile");
                command.Parameters.AddWithValue("@recipients", request.To);
                command.Parameters.AddWithValue("@subject", request.Subject);
                command.Parameters.AddWithValue("@body", request.Body);
                //command.Parameters.Add(new SqlParameter("@subject", SqlDbType.NVarChar));
                //command.Parameters.Add(new SqlParameter("@body", SqlDbType.NVarChar));

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        catch (Exception)
        {

            throw;
        }

    }

    public void SendEmail(EmailDto request)
    {
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailInfo.SmtpUsername));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
        
        using var smtp = new SmtpClient();
        //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        //smtp.Authenticate("santino12@ethereal.email", "6pSu6yJAm7uPWNt5YD");

        smtp.Connect(_emailInfo.SmtpServer, _emailInfo.SmtpPort, SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailInfo.SmtpUsername,_emailInfo.SmtpPassword);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
