using System.Net;
using System.Net.Mail;
namespace CMS.Business;


public class MyMail
{
    private readonly IConfiguration configuration;

    public MyMail(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void sendEMail(string toEmail, string subject, string content)
    {

        MailAddress to = new MailAddress(toEmail);
        MailAddress from = new MailAddress(configuration["MyMail:account"]);

        MailMessage message = new MailMessage(from, to);
        message.Subject = subject;
        message.Body = content;

        SmtpClient client = new SmtpClient(configuration["MyMail:server"], int.Parse(configuration["MyMail:port"]))
        {
            Credentials = new NetworkCredential(configuration["MyMail:account"], configuration["MyMail:password"]),
            EnableSsl = true
        };
        // code in brackets above needed if authentication required

        try
        {
            client.SendAsync(message, "mymail");
            //client.Send(message);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}