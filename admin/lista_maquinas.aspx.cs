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

public partial class lista_maquinas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

        sql = @" select
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
	            from [cashdro_estadomaquinas_totaispagina] (null) tmp ";

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
    public static string getGrelha(string pesquisa, string order)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "", corLigado = "", corErro = "", data_proxima_manutencao = "", tag = "", text_color = "", bg_color = "";
        bool statusLigado = false;
        bool status_erro = false;

        DataSqlServer oDB = new DataSqlServer();

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col' class='pointer th_text' onclick='ordenaSerial();'>Serial</th>
			            <th scope='col' class='pointer th_text' onclick='ordenaLocalizacao();'>Localização</th>
			            <th scope='col' class='pointer th_text th_text_centered' onclick='ordenaUltimaAtualizacao();'>Últ. Atual.</th>
                        <th scope='col' class='pointer th_text th_text_centered' onclick='ordenaProxManut();'>Próx. Manut.</th>
			            <th scope='col' class='pointer th_text th_text_centered' onclick='ordenaLigada();'>ON</th>
                        <th scope='col' class='pointer th_text th_text_centered' onclick='ordenaErro();'>Erro</th>
		              </tr>
		            </thead> <tbody>";

        sql = String.Format(@"  declare @id_maquina int;
                                declare @today date = getdate();
                                declare @contaElementos1 int;
                                declare @contaElementos2 int;
                                declare @prox_manut bit = {2};
                                declare @table table (id INT, serialnumber varchar(100), localizacao varchar(max), ultimacomunicacao datetime, status_ligado bit, status_erro bit, prox_manutencao datetime, tag varchar(max))

                                if(@prox_manut = 1)
                                begin
                                    select @contaElementos1 = count(id)
                                    from cashdro_report_maquinas(@id_maquina)
                                    where ativo = 1 and data_proxima_manutencao is not null and data_proxima_manutencao <> ''
                                    and (serialnumber like {0} or localizacao like {0})

                                    select @contaElementos2 = count(id)
                                    from cashdro_report_maquinas(@id_maquina)
                                    where ativo = 1 and (data_proxima_manutencao = '' or data_proxima_manutencao is null)
                                    and (serialnumber like {0} or localizacao like {0})

                                    if(@contaElementos1 > 0)
                                    begin
	                                    insert into @table(id, serialnumber, localizacao, ultimacomunicacao, status_ligado, status_erro, prox_manutencao, tag)
                                        SELECT 
                                            id,
                                            serialnumber,
                                            localizacao,
                                            ultimacomunicacao, 
                                            status_ligado,
                                            status_erro,
		                                    data_proxima_manutencao,
                                            CASE
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
                                                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
                                                else 'black'
                                            END AS 'tag'
                                        FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)
                                        where ativo = 1 and data_proxima_manutencao is not null and data_proxima_manutencao <> ''
                                        and (serialnumber like {0} or localizacao like {0})
                                        {1}
                                    end

                                    if(@contaElementos2 > 0)
                                    begin
	                                    insert into @table(id, serialnumber, localizacao, ultimacomunicacao, status_ligado, status_erro, prox_manutencao, tag)
                                        SELECT 
                                            id,
                                            serialnumber,
                                            localizacao,
                                            ultimacomunicacao, 
                                            status_ligado,
                                            status_erro,
		                                    data_proxima_manutencao,
                                            CASE
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
                                                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
                                                WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
                                                else 'black'
                                            END AS 'tag'
                                        FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)
                                        where ativo = 1 and (data_proxima_manutencao = '' or data_proxima_manutencao is null)
                                        and (serialnumber like {0} or localizacao like {0})
                                        {1}
                                    end

                                    select
	                                    id,
                                        serialnumber,
                                        localizacao,
                                        case
                                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                        end as ultimacomunicacao, 
                                        status_ligado,
                                        status_erro,
	                                    prox_manutencao,
	                                    isnull(convert(varchar, cast(prox_manutencao as date), 103), '') as data_proxima_manutencao,
                                        tag
                                    from @table
                                    where prox_manutencao is not null and prox_manutencao <> ''
                                    {3}

                                    select
	                                    id,
                                        serialnumber,
                                        localizacao,
                                        case
                                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                        end as ultimacomunicacao, 
                                        status_ligado,
                                        status_erro,
	                                    prox_manutencao,
	                                    isnull(convert(varchar, cast(prox_manutencao as date), 103), '') as data_proxima_manutencao,
                                        tag
                                    from @table
                                    where (prox_manutencao is null or prox_manutencao = '')
                                    {3}
                                end
                                else
                                begin
                                    insert into @table(id, serialnumber, localizacao, ultimacomunicacao, status_ligado, status_erro, prox_manutencao, tag)
                                    SELECT 
	                                    id,
	                                    serialnumber,
	                                    localizacao,
	                                    ultimacomunicacao, 
	                                    status_ligado,
                                        status_erro,
                                        data_proxima_manutencao,
                                        CASE
					                        WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
					                        WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
						                         and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
					                        WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
					                        else 'black'
				                        END AS 'tag'
                                    FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)
                                    where ativo = 1
                                    and (serialnumber like {0} or localizacao like {0})
                                    {1}

                                    select
	                                    id,
                                        serialnumber,
                                        localizacao,
                                        case
                                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                        end as ultimacomunicacao, 
                                        status_ligado,
                                        status_erro,
	                                    prox_manutencao,
	                                    isnull(convert(varchar, cast(prox_manutencao as date), 103), '') as data_proxima_manutencao,
                                        tag
                                    from @table
                                    where 1 = 1
                                    {3}
                                end", 
                                    String.Format("'%{0}%'", pesquisa), 
                                    order, 
                                    order.Contains("data_proxima_manutencao") ? "1" : "0", 
                                    order.Contains("data_proxima_manutencao") 
                                        ? (order.Contains("asc") ? " order by prox_manutencao asc " : " order by prox_manutencao desc ") 
                                        : String.Format(@" and (serialnumber like {0} or localizacao like {0}) {1} ", String.Format("'%{0}%'", pesquisa), order));

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int j = 0; j < oDs.Tables.Count; j++)
            {
                for (int i = 0; i < oDs.Tables[j].Rows.Count; i++)
                {
                    id = oDs.Tables[j].Rows[i]["id"].ToString().Trim();
                    serialnumber = oDs.Tables[j].Rows[i]["serialnumber"].ToString().Trim();
                    localizacao = oDs.Tables[j].Rows[i]["localizacao"].ToString().Trim();
                    ultimacomunicacao = oDs.Tables[j].Rows[i]["ultimacomunicacao"].ToString().Trim();
                    statusLigado = Convert.ToBoolean(oDs.Tables[j].Rows[i]["status_ligado"]);
                    status_erro = Convert.ToBoolean(oDs.Tables[j].Rows[i]["status_erro"]);
                    data_proxima_manutencao = oDs.Tables[j].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                    tag = oDs.Tables[j].Rows[i]["tag"].ToString().Trim();

                    text_color = (tag == "green" || tag == "red") ? " color: white; " : " ";
                    corLigado = statusLigado ? "#2DCE89" : "red";
                    corErro = status_erro ? "#e4d96f" : "#2DCE89";
                    bg_color = String.IsNullOrEmpty(data_proxima_manutencao) ? " style='text-align:center; font-weight: bold;'" : " style='text-align:center; font-weight: bold; background-color:" + tag + ";' ";

                    html += @"<tr style='cursor:pointer' onclick='openMenu(" + id + @");'> 
		                    <th scope='row'>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span class='mb-0 text-sm' style='font-weight:normal; cursor:normal'><b>" + serialnumber + @"</b></span>
			                    </div>
		                      </div>
		                    </th>
		                    <td>" + localizacao + @"
		                    </td>
		                    <td style='text-align: center;'>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>
		                    <td " + bg_color + @">
		                      <span style='" + text_color + ";'>" + data_proxima_manutencao + @"</span>
		                    </td>
		                    <td style='text-align: center;'>
		                      <span class='badge badge-dot mr-12'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corLigado + @"!important' ></i>
		                      </span>
		                    </td>
                            <td style='text-align: center;'>
		                      <span class='badge badge-dot mr-12'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corErro + @"!important' ></i>
		                      </span>
		                    </td>                    
	                      </tr> ";
                }
            }
        }
        else
        {
            html += "  <tr><td colspan='7'>Não existem máquinas a apresentar.</td></tr> ";
        }


        html += "  </tbody> </table>";


        return html;
    }

    [WebMethod]
    public static string atualizaDados()
    {
        DataSqlServer oSql = new DataSqlServer();
        DataSqlServer oSqlUpdate = new DataSqlServer();
        var sql = "";
        var sqlUpdate = "";
        string id = "", user = "", password = "", url = "", serial = "", maquina = "";
        Boolean email_alerta_enviado = false;

        sql = @"    declare @id_maquina int;
                                    declare @ativo bit = 1;
                                    SELECT
	                                    id,
	                                    serialnumber,
	                                    localizacao, 
		                                [url],
                                        email_alerta_enviado,
                                        [password],
                                        username
                                    FROM CASHDRO_REPORT_MAQUINAS(@id_maquina)
                                    WHERE ATIVO = @ativo";

        DataSet oDs = oSql.GetDataSet(sql, "").oData;

        // Vai obter os dados das máquinas
        if (oSql.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                user = oDs.Tables[0].Rows[i]["username"].ToString().Trim();
                password = oDs.Tables[0].Rows[i]["password"].ToString().Trim();
                url = oDs.Tables[0].Rows[i]["url"].ToString().Trim();
                serial = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                maquina = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                email_alerta_enviado = Convert.ToBoolean(oDs.Tables[0].Rows[i]["email_alerta_enviado"]);

                // As que estao ligadas ou desligadas
                Boolean retOnOff = testaMaquinasOnOff(id, user, password, url);

                if (retOnOff)
                {
                    // As que estao com erro
                    Boolean retErro = testaMaquinasComErro(url, maquina, serial, id);
                }
            }

            return "1";
        }

        return "0";
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