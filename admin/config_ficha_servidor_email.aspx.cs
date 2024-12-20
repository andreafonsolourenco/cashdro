using System;
using System.Web.Services;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

public partial class config_ficha_servidor_email : System.Web.UI.Page
{
    string id = "null";

    protected void Page_Load(object sender, EventArgs e)
    {

    }



    [WebMethod]
    public static string saveData(string email, string pass, string smtp, string port, string emailsEnvio)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "1", retMessage = "Dados guardados com sucesso.";
        sql = string.Format(@"  DECLARE @email varchar(150) = '{0}';
	                            DECLARE @email_password varchar(60) = '{1}';
	                            DECLARE @email_smtp varchar(60) = '{2}';
	                            DECLARE @email_smtpport varchar(60) = '{3}';
	                            DECLARE @emails_alerta varchar(max) = '{4}';
                                DECLARE @ret int 
                                DECLARE @retMsg VARCHAR(255) 

                                EXEC cashdro_parametrizacao_servidor_email_novoedita @email, @email_password, @email_smtp, @email_smtpport, @emails_alerta,@ret OUTPUT,@retMsg OUTPUT
                                SELECT @ret ret, @retMsg retMsg ", email, pass, smtp, port, emailsEnvio);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim();
        }

        return ret + '@' + retMessage;
    }


    [WebMethod]
    public static string getData(string id)
    {
        string sql = "", ret = "", email = "", email_password = "", email_smtp = "",
            email_smtpport = "", emails_alerta = "";

        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@"  
                                SELECT
		                            email,
		                            email_password,
		                            email_smtp,
		                            email_smtpport,
		                            emails_alerta
                                FROM CASHDRO_REPORT_CONFIGS()");
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            email = oDs.Tables[0].Rows[0]["email"].ToString().Trim();
            email_password = oDs.Tables[0].Rows[0]["email_password"].ToString().Trim();
            email_smtp = oDs.Tables[0].Rows[0]["email_smtp"].ToString().Trim();
            email_smtpport = oDs.Tables[0].Rows[0]["email_smtpport"].ToString().Trim();
            emails_alerta = oDs.Tables[0].Rows[0]["emails_alerta"].ToString().Trim();
        }

        // Prepara o retorno dos dados
        ret = email + "<#SEP#>" +
              email_password + "<#SEP#>" +
              email_smtp + "<#SEP#>" +
              email_smtpport + "<#SEP#>" +
              emails_alerta;

        return ret;
    }

    [WebMethod]
    public static string sendEmailFromTemplate(string from, string pwd, string smtp, string smtpport, string emails)
    {
        int timeout = 50000;
        string sql = "";
        string[] emailVector;
        string intro = "EMAIL TESTE CONFIGURAÇÃO SERVIDOR<br /><img src=\"http://helptechpt.ddns.net:6969/Img/test.png\" style=\"height:150px; width: auto;\" />";
        string body = "ESTE EMAIL É APENAS UM TESTE À CONFIGURAÇÃO DO SERVIDOR STMP.";
        string subject = "EMAIL TESTE CONFIGURAÇÃO SERVIDOR";

        emailVector = emails.Split(';');
        int i = 0;

        try
        {
            MailMessage mailMessage = new MailMessage();

            string newsletterText = string.Empty;
            newsletterText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template.html"));

            newsletterText = newsletterText.Replace("[EMAIL_INTRO]", intro);
            newsletterText = newsletterText.Replace("[EMAIL_TEXTBODY]", body);

            mailMessage.From = new MailAddress(from, "HelpTech");

            foreach (var word in emailVector)
            {
                if (i == 0)
                {
                    mailMessage.To.Add(word);
                }
                else
                {
                    mailMessage.Bcc.Add(word);
                }

                i++;
            }

            mailMessage.Subject = subject;
            mailMessage.Body = newsletterText;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient(smtp);
            NetworkCredential mailAuthentication = new NetworkCredential(from, pwd);

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = mailAuthentication;
            smtpClient.Timeout = timeout;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }
        catch (Exception ex)
        {
          return "ERRO: " + ex.ToString();
        }

        return "Email Teste enviado com sucesso!";
    }

}