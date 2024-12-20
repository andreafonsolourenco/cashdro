using System;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;

public partial class lista_manutencoes : System.Web.UI.Page
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
        string img = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"declare @id int = {0};
                SELECT
	                id,
	                serialnumber,
	                localizacao,
                    isnull(CONVERT(varchar, data_proxima_manutencao, 103), '') as data_proxima_manutencao
                FROM CASHDRO_REPORT_MAQUINAS(@id)
                order by serialnumber, localizacao ", idMaq);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                ret = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim() + " [" + oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim() + "]";
                if(!String.IsNullOrEmpty(oDs.Tables[0].Rows[i]["data_proxima_manutencao"].ToString().Trim()))
                {
                    ret += "<br />" + oDs.Tables[0].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                }
                else
                {
                    ret += "<br />Sem data da próxima manutenção agendada";
                }
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
    public static string getGrelha(string pesquisa, string order, string idMaq)
    {
        string sql = "", html = "", html_ddl = "";
        string id_intervencao = "", serial = "", maquina = "", data_hora_intervencao = "", tecnico_intervencao = "";

        DataSqlServer oDB = new DataSqlServer();

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col' class='pointer' onclick='ordenaData();'>Data</th>
			            <th scope='col' class='pointer' colspan='2' onclick='ordenaSerialMaquina();'>Máquina</th>
			            <th scope='col' class='pointer' onclick='ordenaTecnico();'>Técnico</th>
                        <th scope='col'></th>
		              </tr>
		            </thead> <tbody>";


        sql = String.Format(@"declare @id_intervencao int;
                declare @id_maquina int = {0};

                SELECT
                    id_intervencao,
		            serial,
		            maquina,
		            data_hora_intervencao,
		            tecnico_intervencao
                FROM CASHDRO_REPORT_MANUTENCOES(@id_intervencao, @id_maquina)
                WHERE (data_hora_intervencao like {1} OR tecnico_intervencao like {1} OR maquina like {1} OR serial like {1})
                {2}", idMaq, String.Format("'%{0}%'", pesquisa), order);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id_intervencao = oDs.Tables[0].Rows[i]["id_intervencao"].ToString().Trim();
                serial = oDs.Tables[0].Rows[i]["serial"].ToString().Trim();
                maquina = oDs.Tables[0].Rows[i]["maquina"].ToString().Trim();
                data_hora_intervencao = oDs.Tables[0].Rows[i]["data_hora_intervencao"].ToString().Trim();
                tecnico_intervencao = oDs.Tables[0].Rows[i]["tecnico_intervencao"].ToString().Trim();

                html += @"<tr style='cursor:pointer'>  
		                    <td>" + data_hora_intervencao + @"</td>
		                    <td>" + serial + @"</td>
                            <td>" + maquina + @"</td>
                            <td>" + tecnico_intervencao + @"</td></tr>";
            }
        }
        else
        {
            html += "<tr><td colspan='4'>Não existem manutenções programadas a apresentar.</td></tr>";
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