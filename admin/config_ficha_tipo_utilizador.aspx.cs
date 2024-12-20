using System;
using System.Web.Services;
using System.Data;


public partial class config_ficha_tipo_utilizador : System.Web.UI.Page
{
    string id = "null";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            id = Request.QueryString["id"];
        }
        catch (Exception)
        {
        }

        txtAux.Value = id;
    }



    [WebMethod]
    public static string saveData(string id, string nome, string notas, string administrador, string permissao_dashboard, string permissao_maquinas, string permissao_logs,
        string permissao_parametrizacao, string permissao_intervencoes,string parceiro)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "1", retMessage = "Dados guardados com sucesso.";

        sql = string.Format(@"  DECLARE @id int={0}
                                DECLARE @nome varchar(200)='{1}'
                                DECLARE @notas varchar(500)='{2}'
                                DECLARE @administrador bit={3}
                                DECLARE @permissao_dashboard bit={4}
                                DECLARE @permissao_maquinas bit={5}
                                DECLARE @permissao_logs bit={6}
                                DECLARE @permissao_parametrizacao bit={7}
                                DECLARE @permissao_intervencoes bit={8}
                                DECLARE @parceiro bit={9}
                                DECLARE @ret int
                                DECLARE @retMsg varchar(255)


                                EXECUTE cashdro_parametrizacao_tipos_utilizador_novoedita @id,@nome,@notas,@administrador,@permissao_dashboard,@permissao_maquinas,@permissao_logs,
                                    @permissao_parametrizacao,@permissao_intervencoes,@parceiro,@ret OUTPUT,@retMsg OUTPUT
                                SELECT @ret ret, @retMsg retMsg ",
                                id,
                                nome,
                                notas,
                                administrador,
                                permissao_dashboard,
                                permissao_maquinas,
                                permissao_logs,
                                permissao_parametrizacao,
                                permissao_intervencoes,
                                parceiro);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            ret = oDs.Tables[0].Rows[0]["ret"].ToString().Trim();
            retMessage = oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim();
        }

        return ret + '@' + retMessage;
    }


    [WebMethod]
    public static string getData(string id)
    {
        string sql = "", ret = "", nome = "", notas = "", s_admin = "false", s_perm_dashboard = "false", s_perm_maquinas = "false", s_perm_logs = "false", s_perm_param = "false", s_perm_interv = "false", s_parceiro = "false";

        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  DECLARE @id int = {0}
                                SELECT
	                                id,
	                                nome,
	                                notas,
	                                administrador,
		                            permissao_dashboard,
		                            permissao_maquinas,
		                            permissao_logs,
		                            permissao_parametrizacao,
		                            permissao_intervencoes,
                                    parceiro
                                FROM CASHDRO_REPORT_TIPO_UTILIZADORES(@id) ", id);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            nome = oDs.Tables[0].Rows[0]["nome"].ToString().Trim();
            notas = oDs.Tables[0].Rows[0]["notas"].ToString().Trim();
            s_admin = Convert.ToBoolean(oDs.Tables[0].Rows[0]["administrador"]) ? "true" : "false";
            s_perm_dashboard = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_dashboard"]) ? "true" : "false";
            s_perm_maquinas = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_maquinas"]) ? "true" : "false";
            s_perm_logs = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_logs"]) ? "true" : "false";
            s_perm_param = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_parametrizacao"]) ? "true" : "false";
            s_perm_interv = Convert.ToBoolean(oDs.Tables[0].Rows[0]["permissao_intervencoes"]) ? "true" : "false";
            s_parceiro = Convert.ToBoolean(oDs.Tables[0].Rows[0]["parceiro"]) ? "true" : "false";
        }

        // Prepara o retorno dos dados
        ret = nome + "<#SEP#>" +
              notas + "<#SEP#>" +
              s_admin + "<#SEP#>" +
              s_perm_dashboard + "<#SEP#>" +
              s_perm_maquinas + "<#SEP#>" +
              s_perm_logs + "<#SEP#>" +
              s_perm_param + "<#SEP#>" +
              s_perm_interv + "<#SEP#>" +
              s_parceiro;

        return ret;
    }
}