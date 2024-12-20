using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class config_lista_tipos_utilizador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string getGrelha(string pesquisa, string order)
    {
        string sql = "", html = "";
        string id = "", nome = "", corAdmin = "", corParceiro ="",corDashboard="",corParametrizacao="", corMaquinas="", corIntervencoes="", corLogs="";
        bool administrador = false, parceiro=false, dashboard=false, parametrizacao=false, maquinas=false, intervencoes=false, logs=false;

        DataSqlServer oDB = new DataSqlServer();



        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col' class='pointer' onclick='ordenaTipo();'>Tipo</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Administrador</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Parceiro</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Dashboard</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Parametrização</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Máquinas</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Intervenções</th>
                        <th scope='col' class='pointer' style='text-align: left;' onclick='ordenaAdministrador();'>Logs</th>

                        <th scope='col'></th>
		              </tr>
		            </thead> <tbody>";


        sql = @"declare @id int; 
                SELECT
	                id,
	                nome,
                    administrador,
                    parceiro,
                    permissao_dashboard,
                    permissao_maquinas,
                    permissao_logs,
                    permissao_parametrizacao,
                    permissao_intervencoes
                FROM CASHDRO_REPORT_TIPO_UTILIZADORES(@id)
                WHERE (nome like '%" + pesquisa + @"%' )" + order;

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();
                administrador = Convert.ToBoolean(oDs.Tables[0].Rows[i]["administrador"]);
                parceiro = Convert.ToBoolean(oDs.Tables[0].Rows[i]["parceiro"]);
                dashboard = Convert.ToBoolean(oDs.Tables[0].Rows[i]["permissao_dashboard"]);
                maquinas = Convert.ToBoolean(oDs.Tables[0].Rows[i]["permissao_maquinas"]);
                intervencoes = Convert.ToBoolean(oDs.Tables[0].Rows[i]["permissao_intervencoes"]);
                parametrizacao = Convert.ToBoolean(oDs.Tables[0].Rows[i]["permissao_parametrizacao"]);
                logs = Convert.ToBoolean(oDs.Tables[0].Rows[i]["permissao_logs"]);

                if (administrador) corAdmin = "#2DCE89";
                else corAdmin = "#004D95";

                if (parceiro) corParceiro = "#2DCE89";
                else corParceiro = "#004D95";

                if (dashboard) corDashboard = "#2DCE89";
                else corDashboard = "#004D95";

                if (parametrizacao) corParametrizacao = "#2DCE89";
                else corParametrizacao = "#004D95";

                if (maquinas) corMaquinas = "#2DCE89";
                else corMaquinas = "#004D95";

                if (intervencoes) corIntervencoes = "#2DCE89";
                else corIntervencoes = "#004D95";

                if (logs) corLogs = "#2DCE89";
                else corLogs = "#004D95";


                html += @"<tr style='cursor:pointer'>  
		                    <th onclick='edita(" + id + @");' scope='row'>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span  class='mb-0 text-sm' style='font-weight:normal; cursor:pointer'><b>" + nome + @"</b></span>
			                    </div>
		                      </div>
		                    </th>
		                    <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corAdmin + @"!important' ></i>
		                      </span>
		                    </td>
                            <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corParceiro + @"!important' ></i>
		                      </span>
		                    </td>
                            <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corDashboard + @"!important' ></i>
		                      </span>
		                    </td>
                             <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corParametrizacao + @"!important' ></i>
		                      </span>
		                    </td>
                             <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corMaquinas + @"!important' ></i>
		                      </span>
		                    </td>
                             <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corIntervencoes + @"!important' ></i>
		                      </span>
		                    </td>
                             <td onclick='edita(" + id + @");' style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corLogs + @"!important' ></i>
		                      </span>
		                    </td>

		                    <td class='text-right'>
		                      <div class='dropdown'>
			                    <a class='btn btn-sm btn-icon-only text-light' href='#' role='button' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>
			                      <i class='fas fa-ellipsis-v'></i>
			                    </a>
			                    <div class='dropdown-menu dropdown-menu-right dropdown-menu-arrow'>
			                      <a class='dropdown-item' href='#' onclick='edita(" + id + @");'>Editar</a>
			                      <a class='dropdown-item' href='#' onclick='apaga(" + id + @");'>Eliminar</a>
			                    </div>
		                      </div>
		                    </td>                    
	                      </tr> ";


            }
        }


        html += "  </tbody> </table>";


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


                               EXEC cashdro_parametrizacao_tipos_utilizador_apaga @id, @ret OUTPUT, @retMsg OUTPUT
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