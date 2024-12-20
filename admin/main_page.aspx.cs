using System;
using System.Web.Services;
using System.Data;



public partial class main_page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string getPermissoes(string id)
    {
        string sql = "", ret = "", admin = "", dashboard = "", maquinas = "", logs = "", parametrizacao = "",
            intervencoes = "";

        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@"  DECLARE @id int = {0}
                                DECLARE @ativo bit = 1;
                                DECLARE @user varchar(150);
                                DECLARE @pass varchar(60);
                                SELECT
	                                [admin],
		                            permissao_dashboard,
		                            permissao_maquinas,
		                            permissao_logs,
		                            permissao_parametrizacao,
		                            permissao_intervencoes
                                FROM CASHDRO_REPORT_UTILIZADORES(@id, @user, @pass, @ativo)", id);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            admin = Convert.ToBoolean(oDs.Tables[0].Rows[0]["admin"]) ? "true" : "false";
            dashboard = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_dashboard"]) ? "true" : "false";
            maquinas = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_maquinas"]) ? "true" : "false";
            logs = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_logs"]) ? "true" : "false";
            parametrizacao = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_parametrizacao"]) ? "true" : "false";
            intervencoes = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_intervencoes"]) ? "true" : "false";
        }

        // Prepara o retorno dos dados
        ret = admin + "<#SEP#>" +
              dashboard + "<#SEP#>" +
              maquinas + "<#SEP#>" +
              logs + "<#SEP#>" +
              parametrizacao + "<#SEP#>" +
              intervencoes;

        return ret;
    }
}