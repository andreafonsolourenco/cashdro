using System;
using System.Linq;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

public partial class estadofundo : System.Web.UI.Page
{
    string id_maquina = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            id_maquina = Request.QueryString["id"];

            txtID.Value = id_maquina;

            getJsonFromServer(id_maquina);
        }
        catch (Exception)
        {
        }

        getDadosMaquina();
    }

    private void getDadosMaquina()
    {
        string sql = "", ret = "";
        string idMaq = String.IsNullOrEmpty(id_maquina) ? "null" : id_maquina;

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id int = {0};
                                declare @id_movimento int;
                                declare @id_cliente int;
                                SELECT
	                                id,
	                                serialnumber,
	                                localizacao
                                FROM CASHDRO_REPORT_MAQUINAS(@id)
                                order by serialnumber, localizacao ", idMaq);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                ret = "Estado de Fundo<br />" + oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim() + " [" + oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim() + "]<br />";
            }
        }

        maquina.InnerHtml = ret;
    }


    [WebMethod]
    public static string getGrelha(string id)
    {
        string sql = "", html = "";
        DataSqlServer oDB = new DataSqlServer();

        string moeda = "", nivelfundo = "", totalfundo = "", nivelrecirculador = "", totalrecirculador = "", nivelfalta = "", totalfalta = "", cor = "";

        html += @"  <table class='table align-items-center table-flush'>
		                <thead class='thead-light'>
		                    <tr>
			                    <th scope='col'></th>
			                    <th scope='col' colspan='2'>FUNDO</th>
			                    <th scope='col' colspan='2'>RECIRCULADOR</th>
                                <th scope='col' colspan='2'>EM FALTA</th>
		                      </tr>
                            <tr>
			                    <th scope='col'>Valor</th>
			                    <th scope='col'>Nível</th>
			                    <th scope='col'>Total</th>
			                    <th scope='col'>Nível</th>
                                <th scope='col'>Total</th>
                                <th scope='col'>Nível</th>
                                <th scope='col'>Total</th>
		                    </tr>
		                </thead>
                        <tbody>";


        sql = String.Format(@"  DECLARE @id_maquina int = {0};
                                DECLARE @tipocoin int;

                                SELECT
	                                moeda, nivelfundo, totalfundo, nivelrecirculador, totalrecirculador, nivelfalta, totalfalta
                                FROM CASHDRO_REPORT_ESTADOFUNDO(@id_maquina, @tipocoin)
                                WHERE nivelrecirculador > 0
                                ORDER BY moeda DESC

                                SELECT
	                                sum(totalfundo) as totalfundo,
	                                sum(totalrecirculador) as totalrecirculador,
                                    sum(totalfalta) as totalfalta
                                FROM CASHDRO_REPORT_ESTADOFUNDO(@id_maquina, @tipocoin)
                                WHERE nivelrecirculador > 0", id);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                moeda = oDs.Tables[0].Rows[i]["moeda"].ToString().Trim();
                nivelfundo = oDs.Tables[0].Rows[i]["nivelfundo"].ToString().Trim();
                totalfundo = oDs.Tables[0].Rows[i]["totalfundo"].ToString().Trim().Replace(".", ",");
                nivelrecirculador = oDs.Tables[0].Rows[i]["nivelrecirculador"].ToString().Trim();
                totalrecirculador = oDs.Tables[0].Rows[i]["totalrecirculador"].ToString().Trim().Replace(".", ",");
                nivelfalta = oDs.Tables[0].Rows[i]["nivelfalta"].ToString().Trim();
                totalfalta = oDs.Tables[0].Rows[i]["totalfalta"].ToString().Trim().Replace(".", ",");

                cor = Convert.ToInt32(nivelrecirculador) == 0 ? "style='background-color:#FF0000; color:white'" : "";

                html += @"<tr> 
		                    <td>" + moeda + @" €</td>
                            <td>" + nivelfundo + @"</td>
                            <td>" + totalfundo + @" €</td>
                            <td " + cor + ">" + nivelrecirculador + @"</td>
                            <td>" + totalrecirculador + @" €</td>
                            <td>" + nivelfalta + @"</td>
                            <td>" + totalfalta + @" €</td>
	                      </tr> ";
            }

            for (int i = 0; i < oDs.Tables[1].Rows.Count; i++)
            {
                totalfundo = oDs.Tables[0].Rows[1]["totalfundo"].ToString().Trim().Replace(".", ",");
                totalrecirculador = oDs.Tables[0].Rows[1]["totalrecirculador"].ToString().Trim().Replace(".", ",");
                totalfalta = oDs.Tables[0].Rows[1]["totalfalta"].ToString().Trim().Replace(".", ",");

                html += @"<tr style='font-weight:bold; background-color: #C0C0C0'> 
		                    <td>TOTAL</td>
                            <td></td>
                            <td>" + totalfundo + @" €</td>
                            <td></td>
                            <td>" + totalrecirculador + @" €</td>
                            <td></td>
                            <td>" + totalfalta + @" €</td>
	                      </tr> ";
            }
        }
        else
        {
            html += "  <tr><td colspan='7'>Sem estado de fundo a apresentar.</td></tr> ";
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

            sql = " DELETE FROM CASHDRO_DATA_ESTADOFUNDO WHERE id_maquina=" + id_maquina;
            oDB.RunDataCommand(sql, "");

            var url = urlMaquina + "/Cashdro3WS/index.php?currencyId=EUR&includeImages=0&includeLevels=1&name=" +  user + "&operation=getPiecesCurrency&password=" + pwd;
            string jsonOut = getRetornoURL(url);

            double moeda = 0, totalFundo = 0, totalRecirculador = 0, totalEmFalta = 0, totalCassete=0;
            int nivelfundo = 0, nivelRecirculador = 0, nivelEmFalta = 0, nivelCassete=0;

            string pMoeda = "", pNivelFundo = "", pTotalFundo = "", pNivelRecirculador = "", pTotalRecirculador = "", pNivelFalta = "", pTotalFalta ="";

            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");

            for (int i = 0; i < data.Count(); i++)
            {
                JToken tk_CurrencyId = data[i].SelectToken("CurrencyId");
                JToken tk_Value = data[i].SelectToken("Value");
                JToken tk_Type = data[i].SelectToken("Type");
                JToken tk_Destination = data[i].SelectToken("Destination");
                JToken tk_MinLevel = data[i].SelectToken("MinLevel");
                JToken tk_MaxLevel = data[i].SelectToken("MaxLevel");
                JToken tk_MaxLevelTemp = data[i].SelectToken("MaxLevelTemp");
                JToken tk_DepositLevel = data[i].SelectToken("DepositLevel");
                JToken tk_DepositLevelTemp = data[i].SelectToken("DepositLevelTemp");
                JToken tk_MaxPiecesExchange = data[i].SelectToken("MaxPiecesExchange");
                JToken tk_MaxPiecesChange = data[i].SelectToken("MaxPiecesChange");
                JToken tk_MaxPiecesCancel = data[i].SelectToken("MaxPiecesCancel");
                JToken tk_IsChargeable = data[i].SelectToken("IsChargeable");
                JToken tk_State = data[i].SelectToken("State");
                JToken tk_Image = data[i].SelectToken("Image");
                JToken tk_LevelRecycler = data[i].SelectToken("LevelRecycler");
                JToken tk_LevelCasete = data[i].SelectToken("LevelCasete");

                // Variaveis para cálculos
                moeda = Convert.ToDouble(tk_Value.ToString().Replace("\"", "")) / 100;
                nivelfundo = Convert.ToInt32(tk_DepositLevel.ToString().Replace("\"", ""));
                totalFundo = nivelfundo * moeda;
                nivelRecirculador = Convert.ToInt32(tk_LevelRecycler.ToString().Replace("\"", ""));
                totalRecirculador = nivelRecirculador * moeda;
                nivelEmFalta = nivelfundo - nivelRecirculador;
                if (nivelEmFalta < 0) nivelEmFalta = 0;
                totalEmFalta = nivelEmFalta * moeda;


                nivelCassete = Convert.ToInt32(tk_LevelCasete.ToString().Replace("\"", ""));
                totalCassete = nivelCassete * moeda;


                sql = string.Format(@" INSERT INTO CASHDRO_DATA_ESTADOFUNDO (id_maquina, moeda, nivelfundo, nivelrecirculador,nivelfalta,totalfundo,totalrecirculador,totalfalta,nivelcassete,totalcassete)
                     SELECT {0},{1},{2},{3},{4},{5},{6},{7},{8},{9} ", id_maquina,
                                                                 moeda.ToString().Replace(",", "."),
                                                                 nivelfundo.ToString().Replace(",", "."),
                                                                 nivelRecirculador.ToString().Replace(",", "."),
                                                                 nivelEmFalta.ToString().Replace(",", "."),
                                                                 totalFundo.ToString().Replace(",", "."),
                                                                 totalRecirculador.ToString().Replace(",", "."),
                                                                 totalEmFalta.ToString().Replace(",", "."),
                                                                 nivelCassete.ToString().Replace(",", "."),
                                                                 totalCassete.ToString().Replace(",", ".")
                                                                 );
                oDB.RunDataCommand(sql, "");



                // Marca moeda, ou nota
                sql = string.Format(@" DECLARE @id_maquina INT = {0}
                                       UPDATE CASHDRO_DATA_ESTADOFUNDO SET tipocoin=0 WHERE moeda<5 AND id_maquina=@id_maquina
                                       UPDATE CASHDRO_DATA_ESTADOFUNDO SET tipocoin=1 WHERE moeda>=5 AND id_maquina=@id_maquina ", id_maquina);

                oDB.RunDataCommand(sql, "");

            }

            return "1";
        }
        else return "-1";
    }



}