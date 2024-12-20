using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public partial class lista_logs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string idMaquina = "";
        int pagina = 1;

        if (Request != null && Request.QueryString["i"] != null)
        {
            try
            {
                idMaquina = Request.QueryString["i"].ToString();
            }
            catch (Exception)
            {
            }
        }

        txtId.Value = idMaquina;
        txtPagina.Value = pagina.ToString();
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
    public static string getGrelha(string pesquisa, string idMaquina, string order)
    {
        string sql = "", html = "";
        string id = "", datahora = "", dispositivo = "", operacao = "", mensagem = "", dataultimaatualizacao = "";
        bool statusLigado = false;
        int tot = 0;

        DataSqlServer oDB = new DataSqlServer();

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col'>Hora</th>
			            <th scope='col'>Dispositivo</th>
			            <th scope='col'>Operação</th>
			            <th scope='col'>Mensagem</th>
                        <th scope='col'>Última atualização</th>
		              </tr>
		            </thead> <tbody>";


        sql = String.Format(@"  declare @id int = {0};
                                SELECT
                                    id,
                                    datahora,
                                    dispositivo,
                                    operacao,
                                    mensagem,
                                    dataultimaatualizacao                        
                                FROM cashdro_report_logs(@id)
                                WHERE (dispositivo like '%{1}%' OR operacao like '%{1}%' OR mensagem like '%{1}%')
                                {2}", idMaquina, pesquisa, order);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            tot = oDs.Tables[0].Rows.Count;

            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                datahora = oDs.Tables[0].Rows[i]["datahora"].ToString().Trim();
                dataultimaatualizacao = oDs.Tables[0].Rows[i]["dataultimaatualizacao"].ToString().Trim();
                dispositivo = oDs.Tables[0].Rows[i]["dispositivo"].ToString().Trim();
                operacao = oDs.Tables[0].Rows[i]["operacao"].ToString().Trim();
                mensagem = oDs.Tables[0].Rows[i]["mensagem"].ToString().Trim();

                html += @"<tr> 
		                    <th scope='row'>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span class='mb-0 text-sm' style='font-weight:normal; cursor:normal'><b>" + datahora + @"</b></span>
			                    </div>
		                      </div>
		                    </th>
		                    <td><span>" + dispositivo + @"</span></td>
                            <td><span>" + operacao + @"</span></td>
                            <td><span>" + mensagem + @"</span></td>
                            <td><span>" + dataultimaatualizacao + @"</span></td>
	                      </tr> ";
            }
        }
        else
        {
            html += "  <tr><td colspan='4'>Não existem LOGs a apresentar para a máquina solicitada.</td></tr> ";
        }

        html += "  </tbody> </table>";


        return tot.ToString() + "<#SEP#>" + html;
    }


    [WebMethod]
    public static string getLogs(string idMaquina, string pagina)
    {
        string sql = "", cashdroid = "1", user = "", password = "", url = "", ret = "", logID = "", dataLog = "", deviceId = "", mensagem = "", operacao = "", sqlCmd = "";
        StringBuilder sb = new StringBuilder("");
        sb.AppendFormat("DELETE FROM CASHDRO_MAQUINASLOG WHERE id_maquina={0}", idMaquina);

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  DECLARE @id_maquina int = {0};
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

            var wsurl = url + "/Cashdro3WS/index.php?name=" + user + "&operation=getLog&page=" + pagina + "&password=" + password;
            string jsonOut = getRetornoURL(wsurl);

            if (jsonOut != "")
            {
                JToken contourManifest = JObject.Parse(jsonOut);
                JToken data = contourManifest.SelectToken("data");

                for (int i = 0; i < data.Count(); i++)
                {
                    JToken tk_logid = data[i].SelectToken("LogId");
                    JToken tk_date = data[i].SelectToken("Date");
                    JToken tk_deviceid = data[i].SelectToken("DeviceId");
                    JToken tk_value = data[i].SelectToken("Value");
                    JToken tk_operationid = data[i].SelectToken("OperationId");


                    logID = tk_logid.ToString();
                    dataLog = tk_date.ToString();
                    deviceId = tk_deviceid.ToString();
                    mensagem = tk_value.ToString().Replace("'", "''");
                    operacao = tk_operationid.ToString();

                    sb.AppendFormat(@" INSERT INTO CASHDRO_MAQUINASLOG (id_maquina,cashdrologid,datahora,id_dispositivo,operacao,mensagem)
                                   SELECT {0},{1},'{2}',{3},'{4}','{5}'", idMaquina, logID, dataLog, deviceId, operacao, mensagem);
                }

                // Insere os dados recebidos na base de dados
                oDB.RunDataCommand(sb.ToString(), "");
            }
            else return "-1";
        }

        return "1";
    }



    [WebMethod]
    public static string getDdlMaquinas(string pid)
    {
        DataSqlServer oDB = new DataSqlServer();
        string sql = "", nome = "", id = "", sel = "";
        string ddl = "<img id='backArrow' src='../general/assets/img/theme/back.png' style='height: 40px; width: 40px; cursor: pointer;' alt='Back' title='Back' onclick='back();'/><select id='ddlMaquinas' style='width:80%' onchange='trocaLogsMaquina();' class='myselect'>";

        sql = String.Format(@"  declare @id int = {0};
                                SELECT
                                    id, 
                                    nome 
                                FROM CASHDRO_REPORT_MAQUINAS_LOGS(@id) ", String.IsNullOrEmpty(pid) ? "null" : pid);

        if(String.IsNullOrEmpty(pid))
        {
            ddl += " <option value=-1>Por favor selecione uma máquina para consultar LOGS</option> ";
        }

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();

                if (id == pid) sel = " selected ";
                else sel = "";

                ddl += " <option " + sel + " value='" + id + "'>" + nome + "</option> ";
            }
        }

        ddl += "</select>";

        return ddl;
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


}