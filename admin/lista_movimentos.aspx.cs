using System;
using System.Web.Services;
using System.Data;
using System.Net;

public partial class lista_movimentos : System.Web.UI.Page
{
    string idMaquina = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request != null && Request.QueryString["id"] != null)
        {
            idMaquina = Request.QueryString["id"].ToString();
        }

        try
        {
            int x = Int32.Parse(idMaquina);
            txtAux.Value = idMaquina;
        }
        catch (Exception)
        {
            txtAux.Value = "";
        }

        getDadosMaquina();
    }

    private void getDadosMaquina()
    {
        string sql = "", ret = "";
        string idMaq = String.IsNullOrEmpty(idMaquina) ? "null" : idMaquina;

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
                ret = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim() + " [" + oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim() + "]<br />";
                ret += "Movimentos mostrados: <span id='nrMovimentos'>0</span><br />";
            }
        }

        maquina.InnerHtml = ret;
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
    public static string getGrelha(string pesquisa, string order, string idMaq, string idUser, string datainicial, string datafinal)
    {
        string sql = "", html = "";
        string data_mov = "", valor_mov = "", tipo_mov = "", valor_entrada = "", valor_saida = "", valor_transferido = "", vendedor = "", user = "";

        DataSqlServer oDB = new DataSqlServer();

        html += @" <table class='table align-items-center table-flush' id='tableMovimentos'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col' class='pointer' onclick='ordenaOperacao();'>Operação</th>
			            <th scope='col' class='pointer' onclick='ordenaData();'>Data</th>
			            <th scope='col' class='pointer' onclick='ordenaValor();'>€€</th>
                        <th scope='col' class='pointer' onclick='ordenaValorEm();'>€€ em</th>
                        <th scope='col' class='pointer' onclick='ordenaValorFora();'>€€ Fora</th>
                        <th scope='col' class='pointer' onclick='ordenavalorEntr();'>€€ Entrada</th>
                        <th scope='col' class='pointer' onclick='ordenaVendedor();'>Vendedor</th>
                        <th scope='col' class='pointer' onclick='ordenaUser();'>Utilizador</th>
		              </tr>
		            </thead> <tbody>";


        sql = String.Format(@"  declare @id_movimento int;
                                declare @id_maquina int = {0};
                                declare @id_cliente int;
                                declare @di varchar(20) = '{3}';
                                declare @df varchar(20) = '{4}';
                                declare @data_inicial date = cast(@di as date);
                                declare @data_final date = cast(@df as date);

                                SELECT
                                    id_mov,
		                            data_mov,
		                            valor_mov,
		                            tipo_mov,
		                            valor_entrada,
		                            valor_saida,
		                            valor_transferido,
		                            valor_arredondado,
		                            vend_mov,
		                            user_mov,
		                            id_maquina,
		                            serial_maquina,
		                            maquina,
		                            nome_cli,
		                            nome_cli_cashdro
                                FROM CASHDRO_REPORT_MOVIMENTOS_MAQUINA(@id_movimento, @id_maquina, @id_cliente, @data_inicial, @data_final)
                                WHERE (data_mov like {1} OR tipo_mov like {1} OR vend_mov like {1} OR user_mov like {1})
                                {2}", idMaq, String.Format("'%{0}%'", pesquisa), order, datainicial, datafinal);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                data_mov = oDs.Tables[0].Rows[i]["data_mov"].ToString().Trim();
                valor_mov = oDs.Tables[0].Rows[i]["valor_mov"].ToString().Trim();
                tipo_mov = oDs.Tables[0].Rows[i]["tipo_mov"].ToString().Trim(); 
                valor_entrada = oDs.Tables[0].Rows[i]["valor_entrada"].ToString().Trim(); 
                valor_saida = oDs.Tables[0].Rows[i]["valor_saida"].ToString().Trim(); 
                valor_transferido = oDs.Tables[0].Rows[i]["valor_transferido"].ToString().Trim(); 
                vendedor = oDs.Tables[0].Rows[i]["vend_mov"].ToString().Trim(); 
                user = oDs.Tables[0].Rows[i]["user_mov"].ToString().Trim();

                html += @"<tr>  
		                    <td>" + tipo_mov + @"</td>
		                    <td>" + data_mov + @"</td>
                            <td>" + valor_mov + @"</td>
                            <td>" + valor_transferido + @"</td>
                            <td>" + valor_saida + @"</td>
                            <td>" + valor_entrada + @"</td>
                            <td>" + vendedor + @"</td>
                            <td>" + user + @"</td>";

                html += " </tr>";
            }
            html += "<span id='countElements' class='variaveis'>" + oDs.Tables[0].Rows.Count + "</span>";
        }
        else
        {
            html += "<tr><td colspan='4'>Não existem movimentos a apresentar.</td></tr>";
            html += "<span id='countElements' class='variaveis'>0</span>";
        }


        html += "</tbody></table>";


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



}