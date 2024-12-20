using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class config_lista_maquinas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string getGrelha(string pesquisa, string order)
    {
        string sql = "", html = "";
        string id = "", referencia = "", nome = "", notas = "", corAtivo = "";
        bool ativo = false;

        DataSqlServer oDB = new DataSqlServer();



        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col' class='pointer' onclick='ordenaSerial();'>Serial Number</th>
			            <th scope='col' class='pointer' onclick='ordenaLocalizacao();'>Localização</th>
			            <th scope='col' class='pointer' onclick='ordenaNotas();'>Notas</th>
			            <th scope='col' class='pointer' onclick='ordenaAtiva();' style='text-align: left;'>Ativa</th>
			            <th scope='col'></th>
		              </tr>
		            </thead> <tbody>";


        sql = @"DECLARE @id int; 
                SELECT
	                id,
	                serialnumber,
	                localizacao,
	                notas,
	                ativo
                FROM CASHDRO_REPORT_MAQUINAS(@id)
                WHERE serialnumber like '%" + pesquisa + @"%' or localizacao like '%" + pesquisa + @"%' or notas like '%" + pesquisa + @"%'" + order;



        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                referencia = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                notas = oDs.Tables[0].Rows[i]["notas"].ToString().Trim();
                ativo = Convert.ToBoolean(oDs.Tables[0].Rows[i]["ativo"]);

                if (ativo) corAtivo = "#2DCE89";
                else corAtivo = "#F5365C";


                html += @"<tr style='cursor:pointer'>  
		                    <th onclick='edita(" + id + @");'  scope='row'>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span  class='mb-0 text-sm' style='font-weight:normal; cursor:pointer'><b>" + referencia + @"</b></span>
			                    </div>
		                      </div>
		                    </th>
		                    <td onclick='edita(" + id + @");' > " + nome + @"                    
		                    </td>
                            <td onclick='edita(" + id + @");' > " + notas + @"                    
		                    </td>
		                    <td onclick='edita(" + id + @");'  style='text-align: left;'>
		                      <span class='badge badge-dot mr-4'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + corAtivo + @"!important' ></i>
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

                               EXEC mbs_parametrizacao_maquinacashdro_apaga @id, @ret OUTPUT, @retMsg OUTPUT
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