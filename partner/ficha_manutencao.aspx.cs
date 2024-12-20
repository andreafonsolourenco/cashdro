using System;
using System.Web.Services;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;


public partial class ficha_manutencao : System.Web.UI.Page
{
    string id = "";
    string id_maquina = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request != null && Request.QueryString["id_maquina"] != null)
        {
            id_maquina = Request.QueryString["id_maquina"].ToString();
        }

        if (Request != null && Request.QueryString["id"] != null)
        {
            id = Request.QueryString["id"].ToString();
        }

        txtAux.Value = id;
        txtIdMaquina.Value = id_maquina;
        getMaquinas();
    }

    [WebMethod]
    public static string saveData(string id, string id_maquina, string data_hora, string tecnico, string pecas, string notas)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "1", retMessage = "Dados guardados com sucesso.";
        string serial = "", localizacao = "", emailcliente = "", cliente = "", nome = "", estabelecimento = "", pessoaresponsavel = "", cashdronome = "", notasinterv = "";
        string emailfrom = "", emailpwd = "", emailsmtp = "", emailsmtpport = "", emailsalerta = "";
        string emailstosend = "", intro = "", body = "", subject = "";
        Boolean email = false;
        data_hora = data_hora.Replace("T", " ");

        sql = string.Format(@"  DECLARE @id int={0}
                                DECLARE @id_maquina int={1}
                                DECLARE @data_hora datetime='{2}'
                                DECLARE @tecnico varchar(200)='{3}'
                                DECLARE @pecas varchar(max)='{4}'
                                DECLARE @notas varchar(max)='{5}'
                                DECLARE @ret int
                                DECLARE @retMsg varchar(255)
                                DECLARE @emailfrom varchar(150)
                                DECLARE @emailpwd varchar(60)
                                DECLARE @emailsmtp varchar(60)
                                DECLARE @emailsmtpport varchar(60)
                                DECLARE @emailsalerta varchar(max)
                                DECLARE @username varchar(150)
                                DECLARE @password varchar(60)
                                DECLARE @ativo bit = 1
                                DECLARE @id_user int

                                select @emailfrom = email, @emailpwd = email_password, @emailsmtp = email_smtp, @emailsmtpport = email_smtpport, @emailsalerta = emails_alerta from CASHDRO_REPORT_CONFIGS();

                                EXECUTE cashdro_manutencao_novoedita @id,@id_maquina, @data_hora, @tecnico, @pecas, @notas,@ret OUTPUT,@retMsg OUTPUT

                                select 
                                    maq.serialnumber, 
                                    maq.localizacao, 
                                    cliente, 
                                    ut.nome, 
                                    ut.email, 
                                    ut.nomeestabelecimento,
                                    ut.pessoaresponsavel, 
                                    ut.cashdronome, 
                                    manut.notas_intervencao, 
                                    @ret as ret, 
                                    @retMsg as retMsg,
                                    @emailfrom as emailfrom, 
                                    @emailpwd as emailpwd, 
                                    @emailsmtp as emailsmtp, 
                                    @emailsmtpport as emailsmtpport, 
                                    @emailsalerta as emailsalerta
                                from cashdro_report_maquinas(@id_maquina) maq
                                inner join CASHDRO_REPORT_UTILIZADORES(@id_user, @username, @password, @ativo) ut on ut.id = maq.id_cliente
                                inner join CASHDRO_REPORT_MANUTENCOES(@ret, @id_maquina) manut on manut.id_maquina = maq.id",
                                String.IsNullOrEmpty(id) ? "null" : id,
                                id_maquina,
                                data_hora,
                                tecnico,
                                pecas,
                                notas);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim();
            serial = oDs.Tables[0].Rows[0]["serialnumber"].ToString().Trim();
            localizacao = oDs.Tables[0].Rows[0]["localizacao"].ToString().Trim();
            cliente = oDs.Tables[0].Rows[0]["cliente"].ToString().Trim();
            nome = oDs.Tables[0].Rows[0]["nome"].ToString().Trim();
            estabelecimento = oDs.Tables[0].Rows[0]["nomeestabelecimento"].ToString().Trim();
            pessoaresponsavel = oDs.Tables[0].Rows[0]["pessoaresponsavel"].ToString().Trim();
            cashdronome = oDs.Tables[0].Rows[0]["cashdronome"].ToString().Trim();
            notasinterv = oDs.Tables[0].Rows[0]["notas_intervencao"].ToString().Trim();
            emailcliente = oDs.Tables[0].Rows[0]["email"].ToString().Trim();
            emailfrom = oDs.Tables[0].Rows[0]["emailfrom"].ToString().Trim();
            emailpwd = oDs.Tables[0].Rows[0]["emailpwd"].ToString().Trim();
            emailsmtp = oDs.Tables[0].Rows[0]["emailsmtp"].ToString().Trim();
            emailsmtpport = oDs.Tables[0].Rows[0]["emailsmtpport"].ToString().Trim();
            emailsalerta = oDs.Tables[0].Rows[0]["emailsalerta"].ToString().Trim();

            if (!String.IsNullOrEmpty(emailcliente))
            {
                emailstosend += emailcliente;
            }

            if (!String.IsNullOrEmpty(emailsalerta))
            {
                if (!String.IsNullOrEmpty(emailstosend))
                {
                    emailstosend += ";";
                }

                emailstosend += emailsalerta;
            }

            intro = "MANUTENÇÃO - " + cliente + " - " + localizacao + "(" + serial + ")";
            body = "Serve o presente e-mail para informar V.Exª que foi efectuada uma manutenção na sua máquina com os seguintes dados:<br /><br />";
            body += "Nome: " + cliente + "<br />";
            body += "S/N: " + serial + "<br />";
            body += "Nome Estabelecimento: " + estabelecimento + "<br />";
            body += "Pessoa Responsável: " + pessoaresponsavel + "<br /><br />";
            body += "composta pelo seguinte procedimento:<br />";
            body += notasinterv;
            subject = intro;

            email = sendEmailFromTemplate(emailfrom, emailpwd, emailsmtp, emailsmtpport, emailstosend, intro, body, subject);
        }

        return ret + '@' + retMessage;
    }


    [WebMethod]
    public static string getData(string id)
    {
        string sql = "", ret = "", id_maquina = "", data_hora = "", tecnico = "", pecas = "", notas = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  declare @id_intervencao int = {0};
                                declare @id_maquina int;
                                SELECT
                                    id_maquina,
		                            CONVERT(VARCHAR(16), data_hora_intervencao, 126) as data_hora_intervencao,
		                            tecnico_intervencao,
                                    pecas_intervencao,
                                    notas_intervencao
                                FROM CASHDRO_REPORT_MANUTENCOES(@id_intervencao, @id_maquina) ", id);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            id_maquina = oDs.Tables[0].Rows[0]["id_maquina"].ToString().Trim();
            data_hora = oDs.Tables[0].Rows[0]["data_hora_intervencao"].ToString().Trim();
            tecnico = oDs.Tables[0].Rows[0]["tecnico_intervencao"].ToString().Trim();
            pecas = oDs.Tables[0].Rows[0]["pecas_intervencao"].ToString().Trim();
            notas = oDs.Tables[0].Rows[0]["notas_intervencao"].ToString().Trim();
        }

        // Prepara o retorno dos dados
        ret = id_maquina + "<#SEP#>" +
              data_hora + "<#SEP#>" +
              tecnico + "<#SEP#>" +
              pecas + "<#SEP#>" +
              notas;

        return ret;
    }

    private void getMaquinas()
    {
        string sql = "", html = "";
        string id = "", serial = "", localizacao = "";

        DataSqlServer oDB = new DataSqlServer();

        html += @" <label class='form-control-label' for='input-username'>Máquina</label>
                   <select id='ddlMaquinas' class='form-control form-control-alternative'>";


        sql = String.Format(@"declare @id int = {0};
                SELECT
                    id,
		            serialnumber,
                    localizacao
                FROM CASHDRO_REPORT_MAQUINAS(@id)
                order by serialnumber, localizacao ", (String.IsNullOrEmpty(id_maquina) || !String.IsNullOrEmpty(id)) ? "null" : id_maquina);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                serial = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();


                html += @"<option value='" + id + @"'>" + serial + @" - " + localizacao + @"</option>";
            }
        }
        else
        {
            html += @"<option value='0'>Não existem máquinas a apresentar</option>";
        }


        html += "</select>";


        divMaquinas.InnerHtml = html;
    }

    [WebMethod]
    public static Boolean sendEmailFromTemplate(string from, string pwd, string smtp, string smtpport, string emails, string intro, string body, string subject)
    {
        int timeout = 50000;
        string sql = "";
        string[] emailVector;

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
            return false;
        }

        return true;
    }
}