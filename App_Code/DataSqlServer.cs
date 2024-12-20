using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DataSqlServer
/// </summary>
public class DataSqlServer
{
    SqlConnection oConn = null;

    public DataSqlServer()
    {

    }

    public struct getDataResult
    {
        public int Code;
        public string Message;
        public DataSet oData;
    }

    public struct CommandResult
    {
        public string Code;
        public string Message;
    }

    private string getConnectionString()
    {
        string ret = ConfigurationManager.ConnectionStrings["ConnBTT"].ConnectionString.ToString();

        return ret;
    }

    private void openDataBase()
    {
        if (oConn.State == ConnectionState.Closed) oConn.Open();
    }

    private void closeDataBase()
    {
        if (oConn.State == ConnectionState.Open) oConn.Close();
    }

    public bool validaDataSet(DataSet oDs)
    {
        if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            return true;
        else
            return false;
    }

    public getDataResult GetDataSet(string query, string session)
    {
        getDataResult oResult = new getDataResult();
        DataSet oData = new DataSet();


        if (session.Trim() != "")
        {
            query += " UPDATE SESSAO SET ultimaiteracao=getdate() WHERE session='" + session + "'";
        }

        // Verifica se temos objeto conexao à bd, se nao tivermos criamos
        if (oConn == null)
        {
            oConn = new SqlConnection(getConnectionString());
        }

        // Executamos o sql para retornar os dados pretendidos
        SqlCommand cmd = new SqlCommand(query, oConn);

        // Abre a conexao à base de dados, caso esta esteja fechada
        openDataBase();


        // create data adapter
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        // this will query your database and return the result to your datatable
        da.Fill(oData);

        // Fecha a conexao à base de dados, caso esta esteja aberta
        closeDataBase();

        // Destroimos o objet DataAdapter
        da.Dispose();


        // Verificamos a consitencia e preparamos o retorno
        if (validaDataSet(oData))
        {
            oResult.Code = 1;
            oResult.Message = "Foram retornados " + oData.Tables[0].Rows.Count.ToString() + " registos.";
            oResult.oData = oData;
        }

        return oResult;
    }

    public CommandResult RunDataCommand(string query, string session)
    {
        CommandResult oRes = new CommandResult();
        oRes.Code = "";
        oRes.Message = "";


        if (session.Trim() != "")
        {
            query += " UPDATE SESSAO SET ultimaiteracao=getdate() WHERE session='" + session + "'";
        }

        try
        {
            // Verifica se temos objeto conexao à bd, se nao tivermos criamos
            if (oConn == null)
            {
                oConn = new SqlConnection(getConnectionString());
            }

            // Abre a conexao à base de dados, caso esta esteja fechada
            openDataBase();

            SqlCommand cmdExecute = new SqlCommand(query, oConn);
            cmdExecute.ExecuteNonQuery();

            // Fecha a conexao à base de dados, caso esta esteja aberta
            closeDataBase();

            cmdExecute.Dispose();

            oRes.Code = "<OK>";
            oRes.Message = "Comando executado com sucesso.";
        }
        catch (SqlException ex)
        {
            oRes.Code = "<ERROR>";
            oRes.Message = ex.Message.ToString();
        }

        return oRes;
    }
}