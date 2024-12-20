using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;



public partial class dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod]
    public static string getTotais(string id)
    {
        string sql = "", html = "";
        string label1 = "", total1 = "", rodape1 = "", label2 = "", total2 = "", rodape2 = "", label3 = "", total3 = "", rodape3 = "", label4 = "", total4 = "", rodape4 = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = @"    declare @monday date;
                    declare @today date = getdate();
                    declare @sunday date;
                    declare @id_maquina int;

                    exec cashdro_get_weekdays @today, @monday output, @sunday output

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
	                from [cashdro_dashboard_totaispagina_dashboard] (@id_maquina, @monday, @today, @sunday)";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            label1 = oDs.Tables[0].Rows[0]["label1"].ToString().Trim();
            total1 = oDs.Tables[0].Rows[0]["total1"].ToString().Trim() + " €";
            rodape1 = oDs.Tables[0].Rows[0]["rodape1"].ToString().Trim();

            label2 = oDs.Tables[0].Rows[0]["label2"].ToString().Trim();
            total2 = oDs.Tables[0].Rows[0]["total2"].ToString().Trim() + " €";
            rodape2 = oDs.Tables[0].Rows[0]["rodape2"].ToString().Trim();

            label3 = oDs.Tables[0].Rows[0]["label3"].ToString().Trim();
            total3 = oDs.Tables[0].Rows[0]["total3"].ToString().Trim() + " €";
            rodape3 = oDs.Tables[0].Rows[0]["rodape3"].ToString().Trim();

            label4 = oDs.Tables[0].Rows[0]["label4"].ToString().Trim();
            total4 = oDs.Tables[0].Rows[0]["total4"].ToString().Trim() + " €";
            rodape4 = oDs.Tables[0].Rows[0]["rodape4"].ToString().Trim();
        }


        html = label1 + "@" + total1 + "@" + rodape1 + "@" + label2 + "@" + total2 + "@" + rodape2 + "@" + label3 + "@" + total3 + "@" + rodape3 + "@" + label4 + "@" + total4 + "@" + rodape4;

        return html;
    }

    [WebMethod]
    public static string getGraficoVendas(string idCliente)
    {
        string sql = "", ret = "", ligadas = "", desligadas = "", erro = "";
        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@" DECLARE @id INT = {0}
                ;with maqs as (
	                SELECT mq.CASHDRO_MAQUINASID
	                FROM UTILIZADORES ut
	                INNER JOIN CASHDRO_TIPO_UTILIZADORES tp ON tp.CASHDRO_TIPO_UTILIZADORESID = ut.id_tipo_utilizador
	                INNER JOIN CASHDRO_PARCEIRO_CLIENTE pc ON pc.id_parceiro = ut.UTILIZADORESID
	                INNER JOIN CASHDRO_MAQUINAS mq ON mq.id_cliente = pc.ID_CLIENTE
	                WHERE ut.UTILIZADORESID=@id
	                AND   tp.nome='Parceiro'

	                UNION ALL 

	                SELECT mq.CASHDRO_MAQUINASID
	                FROM UTILIZADORES ut
	                INNER JOIN CASHDRO_TIPO_UTILIZADORES tp ON tp.CASHDRO_TIPO_UTILIZADORESID = ut.id_tipo_utilizador
	                INNER JOIN CASHDRO_PARCEIRO_TECNICO pt ON pt.ID_TECNICO = ut.UTILIZADORESID
	                INNER JOIN CASHDRO_PARCEIRO_CLIENTE pc ON pc.id_parceiro = pt.ID_PARCEIRO
	                INNER JOIN CASHDRO_MAQUINAS mq ON mq.id_cliente = pc.ID_CLIENTE
	                WHERE ut.UTILIZADORESID=@id
	                AND   tp.nome='Técnico Parceiro'
                ),
                ligadas as (
	                select
		                count(1) ligadas
	                from maqs 
	                INNER JOIN cashdro_maquinas ON cashdro_maquinas.CASHDRO_MAQUINASID = maqs.CASHDRO_MAQUINASID
	                where ativo=1
	                and   status_ligado=1
                )
                , desligadas as (
	                select
		                count(1) desligadas
	                from maqs 
	                INNER JOIN cashdro_maquinas ON cashdro_maquinas.CASHDRO_MAQUINASID = maqs.CASHDRO_MAQUINASID
	                where ativo=1
	                and   status_ligado=0
                ),
                erro as (
	                select
		                count(1) erro
	                from maqs 
	                INNER JOIN cashdro_maquinas ON cashdro_maquinas.CASHDRO_MAQUINASID = maqs.CASHDRO_MAQUINASID
	                where ativo=1
	                and   status_erro=1
                ) 

                select ligadas, desligadas, erro
                from ligadas
                inner join desligadas on 1=1
                inner join erro on 1=1 ", idCliente);



        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            ligadas = oDs.Tables[0].Rows[0]["ligadas"].ToString().Trim();
            desligadas = oDs.Tables[0].Rows[0]["desligadas"].ToString().Trim();
            erro = oDs.Tables[0].Rows[0]["erro"].ToString().Trim();
        }

        ret = ligadas.Replace(",", ".") + "<#SEP#>" +
                desligadas.Replace(",", ".") + "<#SEP#>" +
                erro.Replace(",", ".") + "<#SEP#>";

        return ret;
    }


    [WebMethod]
    public static string getGrelhaMaquinasLigadas(string idCliente)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "";
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col'>Últ. Atual.</th>                        
		              </tr>
		            </thead> <tbody>";


        sql = string.Format(@"  DECLARE @id INT = {0};
                                declare @today date = getdate();

                                SELECT
                                    id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                FROM CASHDRO_REPORT_MAQUINAS_TECPARCEIRO(@id)
                                WHERE status_ligado = 1
                                ORDER BY serialnumber", idCliente);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                serialnumber = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                ultimacomunicacao = oDs.Tables[0].Rows[i]["ultimacomunicacao"].ToString().Trim();


                html += @"<tr style='cursor:pointer' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>		                   
	                      </tr> ";
            }
        }


        html += "  </tbody> </table>";


        return html;
    }


    [WebMethod]
    public static string getGrelhaMaquinasDesligadas(string idCliente)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "";
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col'>Últ. Atual.</th>                        
		              </tr>
		            </thead> <tbody>";


        sql = string.Format(@"  DECLARE @id INT = {0};
                                declare @today date = getdate();

                                SELECT
                                    id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                FROM CASHDRO_REPORT_MAQUINAS_TECPARCEIRO(@id)
                                WHERE status_ligado = 0
                                ORDER BY serialnumber", idCliente);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                serialnumber = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                ultimacomunicacao = oDs.Tables[0].Rows[i]["ultimacomunicacao"].ToString().Trim();


                html += @"<tr style='cursor:pointer' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>		                   
	                      </tr> ";
            }
        }


        html += "  </tbody> </table>";


        return html;
    }

    [WebMethod]
    public static string getGrelhaMaquinasComErro(string idCliente)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "";
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col'>Últ. Atual.</th>                        
		              </tr>
		            </thead> <tbody>";

        sql = string.Format(@"  DECLARE @id INT = {0};
                                declare @today date = getdate();

                                SELECT
                                    id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                FROM CASHDRO_REPORT_MAQUINAS_TECPARCEIRO(@id)
                                WHERE status_erro = 1
                                ORDER BY serialnumber", idCliente);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                serialnumber = oDs.Tables[0].Rows[i]["serialnumber"].ToString().Trim();
                localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
                ultimacomunicacao = oDs.Tables[0].Rows[i]["ultimacomunicacao"].ToString().Trim();


                html += @"<tr style='cursor:pointer' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>		                   
	                      </tr> ";
            }
        }


        html += "  </tbody> </table>";


        return html;
    }

}