using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class config_lista_utilizadores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }



    [WebMethod]
    public static string getDDlTiposUser()
    {
        string sql = "", ret = "<select id='ddlTipoUser' style='width:300px' onchange='getData();' class='myselect'>";
        string id = "", nome = "";

        DataSqlServer oDB = new DataSqlServer();


        sql = " SELECT CASHDRO_TIPO_UTILIZADORESID id, nome FROM CASHDRO_TIPO_UTILIZADORES ORDER BY nome ";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();

                ret += " <option value='" + id + "'>" + nome + "</option>";
            }
        }

        ret += " </select> ";

        return ret;
    }


    [WebMethod]
    public static string getGrelha(string pesquisa, string order, string filtro)
    {
        string sql = "", html = "";
        string id = "", tipo = "", nome = "", email = "", password = "", telemovel = "", corAtivo = "", corSuspenso = "";
        bool ativo = false, suspenso = false;

        DataSqlServer oDB = new DataSqlServer();

        if (filtro.Trim() == "") filtro = "1=1";
        else filtro = "id_tipo_utilizador = " + filtro;

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col' class='pointer' onclick='ordenaNome();'>Nome</th>
			            <th scope='col' class='pointer' onclick='ordenaEmail();'>Email</th>
			            <th scope='col' class='pointer' onclick='ordenaTelemovel();'>Telemovel</th>
			            <th scope='col' class='pointer' onclick='ordenaPerfil();' style='text-align: left;'>Tipo</th>
                        <th scope='col' class='pointer' onclick='ordenaAtivo();' style='text-align: left;'>Ativo</th>
                        <th scope='col' class='pointer' onclick='ordenaSuspenso();' style='text-align: left;'>Suspenso</th>
			            <th scope='col'></th>
		              </tr>
		            </thead> <tbody>";


        sql = @"declare @id int;
                DECLARE @ativo bit;
                DECLARE @user varchar(150);
                DECLARE @pass varchar(60);
                SELECT
	                id,
	                tipo,
                    [admin],
	                nome,
	                email,
	                password,
	                telemovel,
	                ativo,
                    suspenso
                FROM CASHDRO_REPORT_UTILIZADORES(@id,@user,@pass,@ativo)
                WHERE " + filtro + @" 
                AND (nome like '%" + pesquisa + @"%' or email like '%" + pesquisa + @"%' )" + order;

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                tipo = oDs.Tables[0].Rows[i]["tipo"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();
                email = oDs.Tables[0].Rows[i]["email"].ToString().Trim();
                telemovel = oDs.Tables[0].Rows[i]["telemovel"].ToString().Trim();
                ativo = Convert.ToBoolean(oDs.Tables[0].Rows[i]["ativo"]);
                suspenso = Convert.ToBoolean(oDs.Tables[0].Rows[i]["suspenso"]);

                if (ativo) corAtivo = "#2DCE89";
                else corAtivo = "#F5365C";

                if (suspenso) corSuspenso = "#F5365C";
                else corSuspenso = "#2DCE89";

                html += @"<tr style='cursor:pointer'>  
		                    <th onclick='edita(" + id + @");'  scope='row'>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span  class='mb-0 text-sm' style='font-weight:normal; cursor:pointer'><b>" + nome + @"</b></span>
			                    </div>
		                      </div>
		                    </th>
		                    <td onclick='edita(" + id + @");' > " + email + @"                    
		                    </td>
                            <td onclick='edita(" + id + @");' > " + telemovel + @"                    
		                    </td>
                            <td onclick='edita(" + id + @");' > " + tipo + @"                    
		                    </td>
		                    <td onclick='edita(" + id + @");'  style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corAtivo + @"!important' ></i>
		                      </span>
		                    </td>
                            <td onclick='edita(" + id + @");'  style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corSuspenso + @"!important' ></i>
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


                               EXEC mbs_parametrizacao_utilizador_apaga @id, @ret OUTPUT, @retMsg OUTPUT
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