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
    public static string getMachines(string id)
    {
        string sql = "", html = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id int = {0};
                                declare @ativo bit = 1;

                                select maq.id, maq.localizacao
                                from cashdro_report_maquinas(null) maq
                                inner join cashdro_report_utilizadores(@id, null, null, @ativo) ut on  ut.id = maq.id_cliente
                                order by localizacao", id);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            html = "<div class='col-xl-12 col-lg-12 col-md-12 col-sm-12'><fieldset><legend>Máquinas</legend>";

            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                string id_maquina = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                string localizacao = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();

                html += @"  <div class='form-check form-check-inline col-xl-4 col-lg-4 col-md-6 col-sm-6' style='margin-right: 0 !important;'>
                                <input class='form-check-input' type='checkbox' name='machine' value='" + id_maquina + "' id='machine" + i.ToString() + "' checked>" +
                                "<label class='form-check-label' for='machine" + i.ToString() + "'>" + localizacao + "</label>" + 
                            "</div>";
            }

            html += "</fieldset></div><span class='variaveis' id='nrMachines'>" + oDs.Tables[0].Rows.Count + "</span>";
        }
        else
        {
            html += "</fieldset></div><span class='variaveis' id='nrMachines'>0</span>";
        }

        return html;
    }

    [WebMethod]
    public static string getTotais(string id, string machinesList)
    {
        string sql = "", html = "";
        string label1 = "HOJE", total1 = "", rodape1 = "Transações de Hoje", label2 = "SEMANA", total2 = "", rodape2 = "Transações na Semana Atual";
        string label3 = "MÊS", total3 = "", rodape3 = "Transações no Mês atual", label4 = "ANO", total4 = "", rodape4 = "Transações no Ano Atual";

        DataSqlServer oDB = new DataSqlServer();

        if(String.IsNullOrEmpty(machinesList))
        {
            total1 = "0,00 €";
            total2 = "0,00 €";
            total3 = "0,00 €";
            total4 = "0,00 €";

            html = label1 + "@" + total1 + "@" + rodape1 + "@" + label2 + "@" + total2 + "@" + rodape2 + "@" + label3 + "@" + total3 + "@" + rodape3 + "@" + label4 + "@" + total4 + "@" + rodape4;

            return html;
        }

        sql = String.Format(@"  declare @monday date;
                                declare @today date = getdate();
                                declare @sunday date;
                                declare @id_maquina int;
                                declare @id int = {0};

                                exec cashdro_get_weekdays @today, @monday output, @sunday output

                                select
		                            isnull(sum(total), 0.00) as total,
                                    tag
	                            from [cashdro_dashboard_totaispagina_dashboard_cliente] (@id_maquina, @monday, @today, @sunday, @id)
                                {1}
                                group by tag", id, String.IsNullOrEmpty(machinesList) ? "" : "where id in (" + machinesList + ")");

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                switch(oDs.Tables[0].Rows[i]["tag"].ToString().Trim())
                {
                    case "dia":
                        total1 = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";
                        break;
                    case "semana":
                        total2 = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";
                        break;
                    case "mes":
                        total3 = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";
                        break;
                    case "ano":
                        total4 = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";
                        break;
                    default:
                        break;
                }
            }

            if(String.IsNullOrEmpty(total1))
            {
                total1 = "0,00 €";
            }

            if (String.IsNullOrEmpty(total2))
            {
                total2 = "0,00 €";
            }

            if (String.IsNullOrEmpty(total3))
            {
                total3 = "0,00 €";
            }

            if (String.IsNullOrEmpty(total4))
            {
                total4 = "0,00 €";
            }
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
                    select CASHDRO_MAQUINASid from CASHDRO_MAQUINAS where id_cliente=@id
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


        sql = string.Format(@"  DECLARE @id_cliente INT = {0}
                                declare @id_maquina int;
                                declare @today date = getdate();
                                
                                select
	                                id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                from cashdro_report_maquinas_clientes(@id_maquina, @id_cliente)
                                where status_ligado = 1
                                order by serialnumber", idCliente);

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

        sql = string.Format(@"  DECLARE @id_cliente INT = {0}
                                declare @id_maquina int;
                                declare @today date = getdate();
                                
                                select
	                                id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                from cashdro_report_maquinas_clientes(@id_maquina, @id_cliente)
                                where status_ligado = 0
                                order by serialnumber", idCliente);

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

        sql = string.Format(@"  DECLARE @id_cliente INT = {0}
                                declare @id_maquina int;
                                declare @today date = getdate();
                                
                                select
	                                id,
	                                serialnumber,
	                                localizacao,
	                                case
                                        when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                                        else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                                    end as ultimacomunicacao
                                from cashdro_report_maquinas_clientes(@id_maquina, @id_cliente)
                                where status_erro = 1
                                order by serialnumber", idCliente);

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