using System;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;

public partial class lista_intervencoes : System.Web.UI.Page
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
    }

    [WebMethod]
    public static string getDadosMaquina(string idUser, string idMaq)
    {
        string sql = "", ret = "";
        idMaq = String.IsNullOrEmpty(idMaq) ? "null" : idMaq;
        string img = @" <td style='width: 10%; text-align: right; cursor: pointer;'>
                            <img src='../general/assets/img/theme/new.png' style='width: 30px; height: 30px' alt='Criar nova intervenção' title='Criar nova intervenção' onclick='novo();'/>
                        </td>";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id int = {0};
                                declare @idUser int = {1};
                                declare @tipoUser varchar(max);

                                select @tipoUser = tipo from cashdro_report_utilizadores(@idUser, null, null, 1)
                                
                                SELECT
	                                id,
	                                serialnumber,
	                                localizacao,
                                    isnull(CONVERT(varchar, data_proxima_manutencao, 103), '') as data_proxima_manutencao,
                                    CASE
						                WHEN CAST(ISNULL(data_proxima_manutencao, '') as date) <= CAST(GETDATE() as date) AND @tipoUser = 'Técnico Parceiro' THEN 1
						                ELSE 0
					                END AS mostra_icon_novo
                                FROM CASHDRO_REPORT_MAQUINAS_TECPARCEIRO(@idUser)
                                where id = @id
                                order by serialnumber, localizacao ", idMaq, idUser);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                string show = oDs.Tables[0].Rows[i]["mostra_icon_novo"].ToString().Trim();
                string localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                string serial = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                string data = oDs.Tables[0].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                string machine = String.Format(@"{0} [{1}]", localizacao, serial,
                    String.IsNullOrEmpty(data) ? String.Format(@"<br />{0}", data) : "<br />Sem data da próxima manutenção agendada");

                ret = String.Format(@"  <table style='width: 100%'>
                                            <tr>
                                                <td style='width:{0}'>
                                                    <h3 class='mb-0'>Intervenções</h3>
                                                </td>
                                                {1}
                                            </tr>
                                            <tr>
                                                <td style='width: 90%'>
                                                    <h4 class='mb-0'>
                                                        {3}
                                                    </h4>
                                                </td>
                                                <td style='width: 10%; text-align: right; cursor: pointer' runat='server'>
                                                    <img src ='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px' alt='Back' title='Back' onclick='back();' />
                                                </td>
                                            </tr>
                                        </table>", show == "1" ? "90%" : "100%",
                                                    show == "1" ? img : "",
                                                    show == "1" ? "colspan='2'" : "",
                                                    machine);
            }
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
    public static string getGrelha(string pesquisa, string order, string idMaq, string idUser)
    {
        string sql = "", html = "", html_ddl = "";
        string id_intervencao = "", serial = "", maquina = "", data_hora_intervencao = "", tecnico_intervencao = "", mostra_opcoes = "";

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
                declare @iduser int = {3};
                declare @tipouser varchar(max);
                
                SELECT TOP 1 @tipouser = tipo FROM CASHDRO_REPORT_UTILIZADORES(@iduser, null, null, 1)

                SELECT
                    id_intervencao,
		            serial,
		            maquina,
		            data_hora_intervencao,
		            tecnico_intervencao,
                    case 
                        when @tipouser = 'Técnico Parceiro' then 1
                        else 0
                    end as mostra_opcoes
                FROM CASHDRO_REPORT_INTERVENCOES(@id_intervencao, @id_maquina)
                WHERE (data_hora_intervencao like {1} OR tecnico_intervencao like {1} OR maquina like {1} OR serial like {1})
                {2}", idMaq, String.Format("'%{0}%'", pesquisa), order, idUser);

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
                mostra_opcoes = oDs.Tables[0].Rows[i]["mostra_opcoes"].ToString().Trim();

                html_ddl = "<td class='text-right'>"
                    + "<div class='dropdown'>"
                    + "<a class='btn btn-sm btn-icon-only text-light' href='#' role='button' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>"
                    + "<i class='fas fa-ellipsis-v'></i>"
                    + "</a>"
                    + "<div class='dropdown-menu dropdown-menu-right dropdown-menu-arrow'>"
                    + "<a class='dropdown-item' href='#' onclick='edita(" + id_intervencao + @");'>Editar</a>"
                    + "<a class='dropdown-item' href='#' onclick='apaga(" + id_intervencao + @");'>Eliminar</a>"
                    + "</div>"
                    + "</div>"
                    + "</td>";

                if (mostra_opcoes == "1")
                {
                    html += @"<tr style='cursor:pointer'>  
		                    <td onclick='edita(" + id_intervencao + @");'>" + data_hora_intervencao + @"</td>
		                    <td onclick='edita(" + id_intervencao + @");'> " + serial + @"</td>
                            <td onclick='edita(" + id_intervencao + @");'> " + maquina + @"</td>
                            <td onclick='edita(" + id_intervencao + @");'> " + tecnico_intervencao + @"</td>" + html_ddl;
                }
                else
                {
                    html += @"<tr style='cursor:pointer'>  
		                    <td>" + data_hora_intervencao + @"</td>
		                    <td>" + serial + @"</td>
                            <td>" + maquina + @"</td>
                            <td>" + tecnico_intervencao + @"</td>";
                }

                html += "</tr>";
            }
        }
        else
        {
            html += "<tr><td colspan='4'>Não existem intervenções a apresentar.</td></tr>";
        }


        html += "</tbody></table>";


        return html;
    }


    [WebMethod]
    public static string delRow(string id)
    {
        string sql = "", ret = "1", retMessage = "Registo eliminado com sucesso.";
        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@" DECLARE @id INT = {0}
                               DECLARE @ret int
                               DECLARE @retMsg VARCHAR(255)


                               EXEC cashdro_intervencao_apaga @id, @ret OUTPUT, @retMsg OUTPUT
                               SELECT @ret ret, @retMsg retMsg ", id);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim();
        }

        return ret + '@' + retMessage;
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