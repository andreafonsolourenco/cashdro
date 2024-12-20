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

public partial class menu_operacoes : System.Web.UI.Page
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
    public static string mostraLigDireta(string idUser, string idMaq)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "";

        sql = string.Format(@"  DECLARE @id_user int = {0};
                                DECLARE @id_maquina int = {1};
                                DECLARE @ligdireta_maq bit
                                DECLARE @ligdireta_user bit
                                DECLARE @u varchar(150);
                                DECLARE @p varchar(60);
                                DECLARE @ativo bit = 1;

                                select @ligdireta_maq = mostra_ligacao_direta_maquina 
                                from cashdro_report_utilizadores(@id_user, @u, @p, @ativo)

                                select @ligdireta_user = mostra_ligacao_direta
                                from cashdro_report_maquinas(@id_maquina)

                                select
	                                case
		                                when @ligdireta_maq = 1 AND @ligdireta_user = 1 THEN 1
	                                else 0
	                                END AS mostraligdireta", idUser, idMaq);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["mostraligdireta"].ToString().Trim();
        }

        return ret;
    }

    [WebMethod]
    public static string verificaPinOperacoes(string idUser, string pin)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "", retMsg = "";

        sql = string.Format(@"  DECLARE @id_user int = {0};
                                DECLARE @pin varchar(50) = '{1}';
                                DECLARE @ret int;
                                DECLARE @retmsg varchar(255);

                                EXEC cashdro_verifica_pin_operacoes @id_user, @pin, @ret output, @retmsg output

                                select @ret as ret, @retmsg as retmsg", idUser, pin);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMsg = oDs.Tables[0].Rows[0]["retmsg"].ToString().Trim();
        }

        return ret + "@" + retMsg;
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
	                            from [cashdro_estadomaquinas_totaispagina_tecparceiro] (@id_user) tmp ", id_utilizador);

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
    public static string reiniciaServico(string idMaquina)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", ret = "OK";

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

            var wsurl = url + "/Cashdro3WS/index.php?command=11&commandParams=&name=" + user + "&operation=execSupportCommand&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            // a seguir anda a testar até estar ON e retorna a pagina 
            var on = false;
            do
            {
                wsurl = url + "/Cashdro3WS/index.php?operation=getDiagnosis";
                jsonOut = getRetornoURL(wsurl);


                if (jsonOut.Replace("\"", "'").Contains("WithError\':false"))
                {
                    on = true;
                }

            } while (!on);

            // Esperamos mais 15 segundos
            Thread.Sleep(15000);

        }

        return ret;
    }

    [WebMethod]
    public static string reiniciaMaquina(string idMaquina)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", localizacao = "", serial = "", ret = "", operationId = "-1", operacao = "Ocorreu um erro ao reiniciar a máquina!";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id_maquina int = {0}
                                SELECT
	                                id,
	                                serialnumber,
	                                localizacao, 
		                            [url],
                                    email_alerta_enviado,
                                    [password],
                                    username
                                FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)", idMaquina);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            cashdroid = oDs.Tables[0].Rows[0]["id"].ToString().Trim();
            url = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            user = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();
            localizacao = oDs.Tables[0].Rows[0]["localizacao"].ToString().Trim();
            serial = oDs.Tables[0].Rows[0]["serialnumber"].ToString().Trim();

            var wsurl = url + "/Cashdro3WS/index.php?command=5&commandParams=&name=" + user + "&operation=execSupportCommand&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            if (jsonOut != "")
            {
                JToken contourManifest = JObject.Parse(jsonOut);
                JToken data = contourManifest.SelectToken("data");
                JToken code = contourManifest.SelectToken("code");

                if (data.ToString() == "" && code.ToString() == "1")
                {
                    Boolean onOff = testaMaquinasOnOff(cashdroid, user, password, url);

                    if (onOff)
                    {
                        Boolean erro = testaMaquinasComErro(url, localizacao, serial, cashdroid);
                    }

                    return "0<#SEP#>Máquina reiniciada com sucesso!";
                }
            }
            else
            {
                return "-1<#SEP#>Erro ao reiniciar a máquina: Máquina desligada!";
            }
        }

        return "-1<#SEP#>Ocorreu um erro ao reiniciar a máquina!";
    }

    [WebMethod]
    public static Boolean testaMaquinasOnOff(string id, string user, string pass, string url)
    {
        Boolean ret = true;
        string sql = "";
        url += "/Cashdro3WS/index.php?operation=doTest&name=" + user + "&password=" + pass;

        DataSqlServer oDB = new DataSqlServer();

        var jsonOut = getRetornoURL(url);

        if (jsonOut != "")
        {
            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");
            JToken code = contourManifest.SelectToken("code");
            string codeStr = code.ToString();

            sql = String.Format(@"  declare @ligado bit = {0};
                                    declare @id int = {1};
                                    declare @ret int;

                                    exec cashdro_regista_maquina_ligada_desligada @id, @ligado, @ret output", codeStr == "1" ? "1" : "0", id);

            ret = codeStr == "1" ? true : false;
        }
        else
        {
            sql = String.Format(@"  declare @ligado bit = {0};
                                    declare @id int = {1};
                                    declare @ret int;

                                    exec cashdro_regista_maquina_ligada_desligada @id, @ligado, @ret output", "0", id);

            ret = false;
        }

        oDB.RunDataCommand(sql, "");

        return ret;
    }

    [WebMethod]
    public static Boolean testaMaquinasComErro(string url, string maquina, string serial, string id)
    {
        string sql = "";
        Boolean erro = false, emailSend = false, email_alerta_enviado = false;
        string device = "", error = "";
        string subject = "", body = "", intro = "";
        string ligacaoDireta = "";

        DataSqlServer oDB = new DataSqlServer();

        device = "";
        error = "";
        erro = false;
        emailSend = false;
        subject = "";
        body = "";
        intro = "";
        ligacaoDireta = url + "/Cashdro3Web/index.html#/splash/true";

        url += "/Cashdro3WS/index.php?operation=getDiagnosis";
        var jsonOut = getRetornoURL(url);

        if (jsonOut != "")
        {
            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");

            JToken withError = data.SelectToken("WithError");
            JToken withErrorMRX = data.SelectToken("WithErrorMRX");
            JToken devices = data.SelectToken("Devices");

            erro = (Convert.ToBoolean(withError.ToString()) || (withErrorMRX != null && Convert.ToBoolean(withErrorMRX.ToString())));

            if (erro)
            {
                for (int k = 0; k < devices.Count(); k++)
                {
                    JToken deviceName = devices[k].SelectToken("deviceName");
                    JToken deviceState = devices[k].SelectToken("deviceState");
                    JToken deviceMessagesInfo = devices[k].SelectToken("deviceMessagesInfo");
                    JToken deviceWithError = devices[k].SelectToken("deviceWithError");

                    if (Convert.ToBoolean(deviceWithError.ToString()))
                    {
                        device = device + deviceName.ToString() + " ";

                        for (int a = 0; a < deviceMessagesInfo.Count(); a++)
                        {
                            JToken msg = deviceMessagesInfo[a].SelectToken("msg");
                            error = error + msg.ToString() + " ";
                        }
                    }
                }

                intro = "ERRO MÁQUINA " + maquina + "<br /><img src=\"http://helptechpt.ddns.net:6969/Img/error.png\" style=\"height:150px; width: auto;\" />";
                subject = "Cashdro Support - Máquina " + maquina + " com Erro";
                body = "Informa-se para efeitos de prevenção que existe uma máquina com erro:<br /><br />";
                body += maquina + " [" + serial + "]<br />";
                body += "Módulos a dar erro: " + device + "<br />";
                body += "Erro: " + error + "<br />";
                body += "Acesso à máquina: " + ligacaoDireta;
            }
        }

        if (!erro && email_alerta_enviado)
        {
            intro = "MÁQUINA " + maquina + " OK<br /><img src=\"http://helptechpt.ddns.net:6969/Img/ok.png\" style=\"height:150px; width: auto;\" />";
            subject = "Cashdro Support - Máquina " + maquina + " OK";
            body = "Informa-se que a máquina<br /><br />";
            body += maquina + " [" + serial + "]<br />";
            body += "se encontra novamente em pleno funcionamento.";

            emailSend = sendEmail(subject, body, intro);
            email_alerta_enviado = !emailSend;
        }
        else if (erro && !email_alerta_enviado)
        {
            emailSend = sendEmail(subject, body, intro);
            email_alerta_enviado = emailSend;
        }

        sql = String.Format(@"  DECLARE @id_maquina int = {0}; 
                                DECLARE @status_erro bit = {1};
                                DECLARE @email_alerta_enviado bit = {2};
                                declare @ret int;

                                exec cashdro_regista_maquina_estado_erro @id_maquina, @status_erro, @email_alerta_enviado, @ret output", id, erro ? "1" : "0", email_alerta_enviado ? "1" : "0");

        oDB.RunDataCommand(sql, "");

        return true;
    }

    [WebMethod]
    public static string getDadosMaquina(string idMaquina)
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

            ret = url + "<#SEP#>" + user + "<#SEP#>" + password;
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
}