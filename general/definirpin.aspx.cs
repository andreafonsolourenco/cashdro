using System;
using System.Web.Services;
using System.Data;

public partial class definirpin : System.Web.UI.Page
{
    string u = "";
    string p = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request != null && Request.QueryString["u"] != null)
        {
            u = Request.QueryString["u"].ToString();
        }

        if (Request != null && Request.QueryString["p"] != null)
        {
            p = Request.QueryString["p"].ToString();
        }

        username.InnerText = u;
        password.InnerText = p;
    }

    [WebMethod]
    public static string entra(string u, string p, string pin)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "", retMessage = "Dados de autenticação inválidos.";

        sql = string.Format(@"  DECLARE @u varchar(150) = '{0}';
                                DECLARE @p varchar(60) = '{1}';
                                DECLARE @pin varchar(50) = '{2}';
                                DECLARE @ret int;
                                DECLARE @retMsg varchar(max);

                                EXECUTE cashdro_define_pin_and_login @u, @p, @pin, @ret OUTPUT, @retMsg OUTPUT
                                SELECT @ret ret, @retMsg retMsg", u, p, pin);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        ret = oDB.validaDataSet(oDs) ? oDs.Tables[0].Rows[0]["ret"].ToString().Trim() : "-999";
        retMessage = oDB.validaDataSet(oDs) ? oDs.Tables[0].Rows[0]["retMsg"].ToString().Trim() : "Erro ao efetuar login!";

        return ret + "<#SEP#>" + retMessage;
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


    [WebMethod]
    public static string getTipoUser(string i)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "";

        sql = string.Format(@"  DECLARE @id int = {0};
                                DECLARE @u varchar(150);
                                DECLARE @p varchar(60);
                                DECLARE @ativo bit;

                                SELECT tipo FROM CASHDRO_REPORT_UTILIZADORES(@id, @u, @p, @ativo)", i);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        return oDB.validaDataSet(oDs) ? oDs.Tables[0].Rows[0]["tipo"].ToString().Trim() : "";
    }

}