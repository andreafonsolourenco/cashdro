using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.IO;

public partial class config_upload_partner_images : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string getGrelha(string pesquisa)
    {
        string sql = "", html = "";
        string id = "", nome = "";

        DataSqlServer oDB = new DataSqlServer();

        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
			            <th scope='col' class='pointer'>Tipo</th>
		              </tr>
		            </thead> <tbody>";

        sql = @"declare @id_user int
                declare @username varchar(150)
                declare @password varchar(60)
                declare @ativo bit = 1 
                SELECT
	                id,
	                nome
                FROM CASHDRO_REPORT_UTILIZADORES(@id_user,@username,@password,@ativo)
                WHERE (nome like '%" + pesquisa + @"%')
                AND tipo = 'Parceiro'
                ORDER BY nome";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();

                html += @"<tr style='cursor:pointer' onclick='selectRow(" + id + ", " + i.ToString() + ");' id='ln_" + i.ToString() + @"'>  
		                    <th>
		                      <div class='media align-items-center'>                       
			                    <div class='media-body'>
			                      <span class='mb-0 text-sm' style='font-weight:normal; cursor:pointer'><b>" + nome + @"</b></span>
			                    </div>
		                      </div>
		                    </th>          
	                      </tr>";
            }
        }

        html += "  </tbody> </table>";
        html += "<span class='variaveis' id='countElements'>" + oDs.Tables[0].Rows.Count.ToString() + "</span>";
        html += "<span class='variaveis' id='lblidselected'></span>";


        return html;
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (FileUploadControl.HasFile)
        {
            if(photoName.Text.ToString() == "")
            {
                StatusLabel.Text = "Por favor, selecione um Parceiro antes de carregar a foto!";
                return;
            }
            
            try
            {
                string filename = Path.GetFileName(FileUploadControl.FileName);
                string name = photoName.Text.ToString() + ".png";
                FileUploadControl.SaveAs(Server.MapPath("../Img/partner/") + name);

                StatusLabel.Text = "Estado do Carregamento: Fotografia carregada com sucesso!";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Estado do Carregamento: Ocorreu um erro ao carregar a fotografia: " + ex.Message;
            }
        }
        else
        {
            StatusLabel.Text = "Por favor, selecione um Logo e um Parceiro antes de carregar a foto!";
            return;
        }
    }
}