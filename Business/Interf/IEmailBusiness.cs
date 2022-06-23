namespace CMS.Business;
public interface IEmailBusiness
{
    public void sendEMail(string toEmail, string subject, string content);
}