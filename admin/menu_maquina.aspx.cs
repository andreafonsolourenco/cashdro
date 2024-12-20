using System;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net.Mail;

public partial class menu_maquina : System.Web.UI.Page
{
    string id = "null";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            id = Request.QueryString["id"];
        }
        catch (Exception)
        {
        }

        txtAux.Value = id;

        getMachineData();
    }

    private void getMachineData()
    {
        string sql = "", ret = "";

        DataSqlServer oDB = new DataSqlServer();
        sql = String.Format(@"  declare @id int = {0};
                                SELECT
                                    id,
		                            serialnumber,
                                    localizacao,
                                    cliente
                                FROM CASHDRO_REPORT_MAQUINAS(@id)", String.IsNullOrEmpty(id) ? "null" : id);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                ret = String.Format(@"{0}{1} [{2}]", String.IsNullOrEmpty(oDs.Tables[0].Rows[i]["cliente"].ToString().Trim()) ? oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim() + "<br />" : oDs.Tables[0].Rows[i]["cliente"].ToString().Trim() + "<br />",
                    oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim(), oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim());
            }
        }

        machineTitle.InnerHtml = ret;
    }

    private string formataMoeda(string str)
    {
        try
        {
            var dados = str.Split('.');
            var pInteira = dados[0];
            var pDecimal = dados[1];

            if (pDecimal.Length < 2)
                pDecimal += "0";

            str = pInteira.Trim() + "." + pDecimal.Trim() + " €";
        }
        catch (Exception ex)
        {
            str += " €";
        }

        return str;
    }

    [WebMethod]
    public static string trataExpiracao(string i)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "-1", retMessage = "Sessão expirada";

        sql = string.Format(@" DECLARE @u varchar(150);
                               DECLARE @p varchar(60);
                               DECLARE @id int = {0};
                               DECLARE @ativo bit = 1;
                               DECLARE @sessaomax int = (select sessaomaximaminutos from config_geral);
                               SELECT 
	                                CASE WHEN DATEDIFF(mi, ut.lastlogin, getdate()) > @sessaomax then 0 else 1 end ret
                               FROM CASHDRO_REPORT_UTILIZADORES(@id, @u, @p, @ativo) ut ", i);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = "Sessão válida e ativa";
        }
        else
        {
            ret = "0";
            retMessage = "Sessão expirada";
        }

        return ret + "<#SEP#>" + retMessage;
    }

    [WebMethod]
    public static string getUname(string i)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "";

        sql = string.Format(@" DECLARE @u varchar(150);
                               DECLARE @p varchar(60);
                               DECLARE @id int = {0};
                               DECLARE @ativo bit = 1;

                               SELECT TOP 1 nome FROM CASHDRO_REPORT_UTILIZADORES(@id, @u, @p, @ativo) ", i);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["nome"].ToString().Trim();
        }

        return ret;
    }


    [WebMethod]
    public static string getTotais(string id_utilizador)
    {
        string sql = "", html = "";
        string label1 = "", total1 = "", rodape1 = "", label2 = "", total2 = "", rodape2 = "", label3 = "", total3 = "", rodape3 = "", label4 = "", total4 = "", rodape4 = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id_user int = {0};
                                select
		                            label1,
		                            total1,
		                            rodape1,
		                            label2,
		                            total2,
		                            rodape2,
		                            label3,
		                            total3,
		                            rodape3,
		                            label4,
		                            total4,
		                            rodape4
	                            from [cashdro_estadomaquinas_totaispagina_cliente] (@id_user) tmp ", id_utilizador);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            label1 = oDs.Tables[0].Rows[0]["label1"].ToString().Trim();
            total1 = oDs.Tables[0].Rows[0]["total1"].ToString().Trim();
            rodape1 = oDs.Tables[0].Rows[0]["rodape1"].ToString().Trim();

            label2 = oDs.Tables[0].Rows[0]["label2"].ToString().Trim();
            total2 = oDs.Tables[0].Rows[0]["total2"].ToString().Trim();
            rodape2 = oDs.Tables[0].Rows[0]["rodape2"].ToString().Trim();

            label3 = oDs.Tables[0].Rows[0]["label3"].ToString().Trim();
            total3 = oDs.Tables[0].Rows[0]["total3"].ToString().Trim();
            rodape3 = oDs.Tables[0].Rows[0]["rodape3"].ToString().Trim();

            label4 = oDs.Tables[0].Rows[0]["label4"].ToString().Trim();
            total4 = oDs.Tables[0].Rows[0]["total4"].ToString().Trim();
            rodape4 = oDs.Tables[0].Rows[0]["rodape4"].ToString().Trim();
        }


        html = label1 + "@" + total1 + "@" + rodape1 + "@" + label2 + "@" + total2 + "@" + rodape2 + "@" + label3 + "@" + total3 + "@" + rodape3 + "@" + label4 + "@" + total4 + "@" + rodape4;

        return html;
    }

    [WebMethod]
    public static string getStatusMaquina(string idMaquina)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", ret = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id_maquina int = {0}
                                SELECT 
	                                id,
	                                username,
	                                password,
                                    url
                                FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)", idMaquina);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            cashdroid = oDs.Tables[0].Rows[0]["id"].ToString().Trim();
            url = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            user = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();

            var wsurl = url + "/Cashdro3WS/index.php?operation=doTest&name=" + user + "&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            // SE TIVER ERROS
            if (jsonOut.Contains("WithError"))
                ret = "ERRO";
            // ESTA LIGADA
            else if (jsonOut == "{\"code\":1,\"data\":\"\"}")
                ret = "ON";
            // SE ESTIVER DESLIGADA ENTAO REGISTA
            else
                ret = "OFF";

        }

        return ret;
    }

    [WebMethod]
    public static string operacaoDecorrer(string idMaquina)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", ret = "", operationId = "-1", operacao = "Não há operações em execução";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id_maquina int = {0}
                                SELECT 
	                                id,
	                                username,
	                                password,
                                    url
                                FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)", idMaquina);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            cashdroid = oDs.Tables[0].Rows[0]["id"].ToString().Trim();
            url = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            user = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();

            var wsurl = url + "/Cashdro3WS/index.php?operation=askOperationExecuting&name=" + user + "&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            if (jsonOut != "")
            {
                JToken contourManifest = JObject.Parse(jsonOut);
                JToken data = contourManifest.SelectToken("data");

                for (int i = 0; i < data.Count(); i++)
                {
                    JToken tk_operationId = data[i].SelectToken("LogId");
                    operationId = tk_operationId.ToString();
                    if (operationId != "-1")
                    {
                        // Vamos obter o nome da operacao em curso
                        sql = " SELECT nome FROM CASHDRO_OPERACOESADECORRER WHERE codigo='" + operationId.Trim() + "'";
                        oDs = oDB.GetDataSet(sql, "").oData;
                        if (oDB.validaDataSet(oDs))
                        {
                            operacao = oDs.Tables[0].Rows[0]["nome"].ToString().Trim();
                        }
                    }
                }
            }
        }

        return operationId + "<#SEP#>" + operacao;
    }

    [WebMethod]
    public static string getLogsToSave(string idMaquina, string minDate, string maxDate)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", ret = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id_maquina int = {0}
                                SELECT 
	                                id,
	                                username,
	                                password,
                                    url
                                FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)", idMaquina);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            cashdroid = oDs.Tables[0].Rows[0]["id"].ToString().Trim();
            url = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            user = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();

            var wsurl = url + "/Cashdro3WS/index.php?command=19&commandParams={\"StartDate\":\"" + minDate + "\",\"FinishDate\":\"" + maxDate + "\",\"Download\":\"true\"}&name=" + user + "&operation=execSupportCommand&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            if (jsonOut != "")
            {
                JToken contourManifest = JObject.Parse(jsonOut);
                JToken data = contourManifest.SelectToken("data");
                JToken code = contourManifest.SelectToken("code");

                if (code.ToString() == "1")
                {
                    Thread.Sleep(30000);
                    return url + "/emergency/downloadlogs.php";
                }
            }

        }

        return ret;
    }

    [WebMethod]
    public static string getRetornoURL(string url)
    {
        try
        {
            WebClient client = new WebClient();

            client.Headers.Add("User-Agent: BrowseAndDownload");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string ret = client.DownloadString(url);

            //TRIMA a string        
            ret = ret.Trim();

            return ret;
        }
        catch (Exception)
        {
            return "";
        }
    }

    [WebMethod]
    public static Boolean sendEmail(string subject, string body, string intro)
    {
        int timeout = 50000;
        int i = 0;
        string sql = "", sendemail = "", sendcc = "", sendbcc = "", from = "", pwd = "", smtp = "", smtpport = "", emails = "";
        string[] emailVector;

        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  SELECT
                                    email,
                                    email_password,
                                    email_smtp,
                                    email_smtpport,
                                    emails_alerta
                                FROM CASHDRO_REPORT_CONFIGS()");
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            from = oDs.Tables[0].Rows[0]["email"].ToString().Trim();
            pwd = oDs.Tables[0].Rows[0]["email_password"].ToString().Trim();
            smtp = oDs.Tables[0].Rows[0]["email_smtp"].ToString().Trim();
            smtpport = oDs.Tables[0].Rows[0]["email_smtpport"].ToString().Trim();
            emails = oDs.Tables[0].Rows[0]["emails_alerta"].ToString();
            emailVector = emails.Split(';');
            i = 0;

            foreach (var word in emailVector)
            {
                if (i == 0)
                {
                    sendemail = word;
                }
                else
                {
                    if (i == 1)
                    {
                        sendbcc += word;
                    }
                    else
                    {
                        sendbcc += ";" + word;
                    }
                }
                i++;
            }
        }

        try
        {
            MailMessage mailMessage = new MailMessage();

            string newsletterText = string.Empty;
            newsletterText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template.html"));

            newsletterText = newsletterText.Replace("[EMAIL_INTRO]", intro);
            newsletterText = newsletterText.Replace("[EMAIL_TEXTBODY]", body);

            mailMessage.From = new MailAddress(from, "HelpTech Cashdro Support");

            mailMessage.To.Add(sendemail);

            if (sendcc.Trim() != "")
                mailMessage.CC.Add(sendcc);
            if (sendbcc.Trim() != "")
                mailMessage.Bcc.Add(sendbcc);

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

    [WebMethod]
    public static string podeCriarEditar(string id)
    {
        string sql = "", ret = "";
        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  DECLARE @id int = {0}
                                DECLARE @ativo bit = 1;
	                            select tipo 
                                from CASHDRO_REPORT_UTILIZADORES(@id, null, null, 1)", id);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["tipo"].ToString().Trim();
        }

        return ret;
    }

    [WebMethod]
    public static string delRow(string id)
    {
        string sql = "", ret = "1", retMessage = "Registo eliminado com sucesso.";
        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@" DECLARE @id INT = {0}
                               DECLARE @ret int
                               DECLARE @retMsg VARCHAR(255)

                               EXEC mbs_parametrizacao_maquinacashdro_apaga @id, @ret OUTPUT, @retMsg OUTPUT
                               SELECT @ret ret, @retMsg retMsg ", id);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim();
        }

        return ret + '@' + retMessage;
    }
}