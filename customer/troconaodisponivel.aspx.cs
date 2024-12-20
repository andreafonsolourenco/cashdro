using System;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

public partial class troconaodisponivel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            string id_maquina = Request.QueryString["id"];

            txtID.Value = id_maquina;

            getJsonFromServer(id_maquina);
        }
        catch (Exception)
        {
        }        
    }

     
    [WebMethod]
    public static string getGrelha(string id)
    {
        string sql = "", html = "";
        DataSqlServer oDB = new DataSqlServer();

        string operacao = "", data = "", hora = "", utilizador = "", tipodeoperacao = "", montante = "";         

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col'>Operação</th>
			            <th scope='col'>Data</th>
			            <th scope='col'>Hora</th>
			            <th scope='col'>Utilizador</th>
                        <th scope='col'>Tipo de operação</th>
                        <th scope='col'>Montante</th>
		              </tr>
		            </thead> <tbody>";


        sql = @" DECLARE @id INT = " + id + @"
                 SELECT 
	                operationid operacao, 
	                date data, 
	                hour hora,
	                utilizador utilizador, 
	                o.nome tipodeoperacao, 
	                amount montante		
                FROM CASHDRO_DATA_TROCONAODISPONIVEL t
                INNER JOIN CASHDRO_OPERACOESADECORRER o ON o.codigo = t.type
                WHERE id_maquina=@id
                order by CASHDRO_DATA_TROCONAODISPONIVELID DESC  ";


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                operacao = oDs.Tables[0].Rows[i]["operacao"].ToString().Trim();
                data = oDs.Tables[0].Rows[i]["data"].ToString().Trim();
                hora = oDs.Tables[0].Rows[i]["hora"].ToString().Trim();
                utilizador = oDs.Tables[0].Rows[i]["utilizador"].ToString().Trim();
                tipodeoperacao = oDs.Tables[0].Rows[i]["tipodeoperacao"].ToString().Trim();
                montante = oDs.Tables[0].Rows[i]["montante"].ToString().Trim();

                html += @"<tr> 
		                    <td>" + operacao + @"</td>
                            <td>" + data + @"</td>
                            <td>" + hora + @"</td>
                            <td> " + utilizador + @"</td>
                            <td>" + tipodeoperacao + @"</td>
                            <td>" + montante.Replace(".", ",") + @"</td>
	                      </tr> ";
            }
        }


        html += "  </tbody> </table>";


        return html;
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
    public static string formataMoeda(string str)
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
    public static string getJsonFromServer(string id_maquina)
    {
        DataSqlServer oDB = new DataSqlServer();
        string sql = string.Format(@" DECLARE @id INT = {0}
                                      SELECT url, username, password
                                      FROM CASHDRO_MAQUINAS mq
                                      WHERE mq.cashdro_maquinasid = @id", id_maquina);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            string urlMaquina = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            string user = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            string pwd = oDs.Tables[0].Rows[0]["password"].ToString().Trim();

            sql = " DELETE FROM CASHDRO_DATA_TROCONAODISPONIVEL WHERE id_maquina=" + id_maquina;
            oDB.RunDataCommand(sql, "");

            var url = urlMaquina + "/Cashdro3WS/index.php?name=" +  user + "&operation=getChangeNotAvailable&password=" + pwd;
            string jsonOut = getRetornoURL(url);

            //FOrmata o JSON para se conseguir interpretar
            jsonOut = jsonOut.Replace(@"\","");
            jsonOut = jsonOut.Replace(@"""[", @"[");
            jsonOut = jsonOut.Replace(@"}]""}", @"}]}");



            string OperationId = "", Amount = "", Type = "", User = "", Date = "", Hour="";


            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");

            for (int i = 0; i < data.Count(); i++)
            {
                JToken tk_OperationId = data[i].SelectToken("OperationId");
                JToken tk_Amount = data[i].SelectToken("Amount");
                JToken tk_Type = data[i].SelectToken("Type");
                JToken tk_User = data[i].SelectToken("User");
                JToken tk_Date = data[i].SelectToken("Date");
                JToken tk_Hour = data[i].SelectToken("Hour");


                // Variaveis para cálculos
                OperationId = tk_OperationId.ToString();
                Amount = tk_Amount.ToString();
                Type = tk_Type.ToString();
                User = tk_User.ToString();
                Date = tk_Date.ToString();
                Hour = tk_Hour.ToString();

                sql = string.Format(@" INSERT INTO CASHDRO_DATA_TROCONAODISPONIVEL (id_maquina, operationid, amount, type,utilizador, date, hour)
                     SELECT {0},{1},{2},{3},'{4}','{5}','{6}' ", id_maquina,
                                                           OperationId.ToString().Replace(",", "."),
                                                           (Convert.ToDecimal(Amount)/100).ToString().Replace(",", "."),
                                                           Type.ToString().Replace(",", "."),
                                                           User.ToString().Replace(",", "."),
                                                           Date.ToString().Replace(",", "."),
                                                           Hour.ToString().Replace(",", ".")
                                                        );
                oDB.RunDataCommand(sql, "");



                //// Marca moeda, ou nota
                //sql = string.Format(@" DECLARE @id_maquina INT = {0}
                //                       UPDATE CASHDRO_DATA_ESTADOFUNDO SET tipocoin=0 WHERE moeda<5 AND id_maquina=@id_maquina
                //                       UPDATE CASHDRO_DATA_ESTADOFUNDO SET tipocoin=1 WHERE moeda>=5 AND id_maquina=@id_maquina ", id_maquina);

                //oDB.RunDataCommand(sql, "");

            }

            return "1";
        }
        else return "-1";
    }



}