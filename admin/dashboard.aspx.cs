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
    public static string getManutencoes(string id)
    {
        string sql = "", html = "";
        string label1 = "", total1 = "", rodape1 = "", label2 = "", total2 = "", rodape2 = "", label3 = "", total3 = "", rodape3 = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = @"    declare @id_maquina int;
                    declare @today date = getdate();

                    select
		                label1,
		                total1,
		                rodape1,
		                label2,
		                total2,
		                rodape2,
		                label3,
		                total3,
		                rodape3
	                from cashdro_dashboard_techelptech (@id_maquina, @today)";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            label1 = oDs.Tables[0].Rows[0]["label1"].ToString().Trim();
            total1 = oDs.Tables[0].Rows[0]["total1"].ToString().Trim();
            rodape1 = oDs.Tables[0].Rows[0]["rodape1"].ToString().Trim();

            label2 = oDs.Tables[0].Rows[0]["label2"].ToString().Trim();
            total2 = oDs.Tables[0].Rows[0]["total2"].ToString().Trim();
            rodape2 = oDs.Tables[0].Rows[0]["rodape2"].ToString().Trim();

            label3 = oDs.Tables[0].Rows[0]["label3"].ToString().Trim();
            total3 = oDs.Tables[0].Rows[0]["total3"].ToString().Trim();
            rodape3 = oDs.Tables[0].Rows[0]["rodape3"].ToString().Trim();
        }


        html = label1 + "@" + total1 + "@" + rodape1 + "@" + label2 + "@" + total2 + "@" + rodape2 + "@" + label3 + "@" + total3 + "@" + rodape3;

        return html;
    }

    [WebMethod]
    public static string getGraficoVendas(string idCliente)
    {
        string sql = "", ret = "", ligadas = "", desligadas = "", erro = "";
        DataSqlServer oDB = new DataSqlServer();


        sql = @"with ligadas as (
	                select
		                count(1) ligadas
	                from cashdro_maquinas 
	                where ativo=1
	                and   status_ligado=1
                )
                , desligadas as (
	                select
		                count(1) desligadas
	                from cashdro_maquinas 
	                where ativo=1
	                and   status_ligado=0
                ),
                erro as (
	                select
		                count(1) erro
	                from cashdro_maquinas 
	                where ativo=1
	                and   status_erro=1
                ) 

                select ligadas, desligadas, erro
                from ligadas
                inner join desligadas on 1=1
                inner join erro on 1=1 ";



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
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "", tem_contrato = "", data_proxima_manutencao = "", tag = "", text_color = "", bg_color = "";
        bool contrato = false;
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col' style='text-align:center;'>Últ. Atual.</th>
                        <th scope='col' style='text-align:center;'>Próx. Manut.</th> 
                        <th scope='col' style='text-align:center;'>Contrato</th> 
		              </tr>
		            </thead> <tbody>";


        sql = @"declare @id_maquina int;
                declare @today date = getdate();
                declare @contaElementos1 int;
                declare @contaElementos2 int;

                select @contaElementos1 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_ligado = 1
                and (data_proxima_manutencao is not null and data_proxima_manutencao <> '') 

                select @contaElementos2 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_ligado = 1
                and (data_proxima_manutencao is null or data_proxima_manutencao = '') 
                
                if(@contaElementos1 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_ligado = 1
                    and (data_proxima_manutencao is not null and data_proxima_manutencao <> '') 
                    order by data_proxima_manutencao desc, serialnumber asc
                end

                if(@contaElementos2 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_ligado = 1
                    and (data_proxima_manutencao is null or data_proxima_manutencao = '') 
                    order by serialnumber asc
                end";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int j = 0; j < oDs.Tables.Count; j++)
            {
                for (int i = 0; i < oDs.Tables[j].Rows.Count; i++)
                {
                    id = oDs.Tables[j].Rows[i]["id"].ToString().Trim();
                    serialnumber = oDs.Tables[j].Rows[i]["serialnumber"].ToString().Trim();
                    localizacao = oDs.Tables[j].Rows[i]["localizacao"].ToString().Trim();
                    ultimacomunicacao = oDs.Tables[j].Rows[i]["ultimacomunicacao"].ToString().Trim();
                    contrato = Convert.ToBoolean(oDs.Tables[j].Rows[i]["tem_contrato"]);
                    data_proxima_manutencao = oDs.Tables[j].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                    tag = oDs.Tables[j].Rows[i]["tag"].ToString().Trim();

                    text_color = (tag == "green" || tag == "red") ? " color: white; " : " ";
                    tem_contrato = contrato ? "green" : "red";
                    bg_color = String.IsNullOrEmpty(data_proxima_manutencao) ? " style='text-align:center; font-weight: bold;'" : " style='text-align:center; font-weight: bold; background-color:" + tag + ";' ";

                    html += @"<tr style='cursor:pointer;' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td style='text-align:center;'>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>
                            <td " + bg_color + @">
		                      <span style='" + text_color + ";'>" + data_proxima_manutencao + @"</span>
		                    </td>
                            <td style='text-align:center;'>
		                      <span class='badge badge-dot mr-12'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + tem_contrato + @"!important' ></i>
		                      </span>
		                    </td>
	                      </tr> ";
                }
            }
        }

        html += "  </tbody> </table>";

        return html;
    }

    [WebMethod]
    public static string getGrelhaMaquinasDesligadas(string idCliente)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "", tem_contrato = "", data_proxima_manutencao = "", tag = "", text_color = "", bg_color = "";
        bool contrato = false;
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col' style='text-align:center;'>Últ. Atual.</th>
                        <th scope='col' style='text-align:center;'>Próx. Manut.</th> 
                        <th scope='col' style='text-align:center;'>Contrato</th> 
		              </tr>
		            </thead> <tbody>";

        sql = @"declare @id_maquina int;
                declare @today date = getdate();
                declare @contaElementos1 int;
                declare @contaElementos2 int;

                select @contaElementos1 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_ligado = 0
                and (data_proxima_manutencao is not null and data_proxima_manutencao <> '') 

                select @contaElementos2 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_ligado = 0
                and (data_proxima_manutencao is null or data_proxima_manutencao = '') 
                
                if(@contaElementos1 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_ligado = 0
                    and (data_proxima_manutencao is not null and data_proxima_manutencao <> '') 
                    order by data_proxima_manutencao desc, serialnumber asc
                end

                if(@contaElementos2 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_ligado = 0
                    and (data_proxima_manutencao is null or data_proxima_manutencao = '') 
                    order by serialnumber asc
                end";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int j = 0; j < oDs.Tables.Count; j++)
            {
                for (int i = 0; i < oDs.Tables[j].Rows.Count; i++)
                {
                    id = oDs.Tables[j].Rows[i]["id"].ToString().Trim();
                    serialnumber = oDs.Tables[j].Rows[i]["serialnumber"].ToString().Trim();
                    localizacao = oDs.Tables[j].Rows[i]["localizacao"].ToString().Trim();
                    ultimacomunicacao = oDs.Tables[j].Rows[i]["ultimacomunicacao"].ToString().Trim();
                    contrato = Convert.ToBoolean(oDs.Tables[j].Rows[i]["tem_contrato"]);
                    data_proxima_manutencao = oDs.Tables[j].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                    tag = oDs.Tables[j].Rows[i]["tag"].ToString().Trim();

                    text_color = (tag == "green" || tag == "red") ? " color: white; " : " ";
                    tem_contrato = contrato ? "green" : "red";
                    bg_color = String.IsNullOrEmpty(data_proxima_manutencao) ? " style='text-align:center; font-weight: bold;'" : " style='text-align:center; font-weight: bold; background-color:" + tag + ";' ";

                    html += @"<tr style='cursor:pointer;' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td style='text-align:center;'>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>
                            <td " + bg_color + @">
		                      <span style='" + text_color + ";'>" + data_proxima_manutencao + @"</span>
		                    </td>
                            <td style='text-align:center;'>
		                      <span class='badge badge-dot mr-12'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + tem_contrato + @"!important' ></i>
		                      </span>
		                    </td>
	                      </tr> ";
                }
            }
        }


        html += "  </tbody> </table>";


        return html;
    }

    [WebMethod]
    public static string getGrelhaMaquinasComErro(string idCliente)
    {
        string sql = "", html = "";
        string id = "", serialnumber = "", localizacao = "", ultimacomunicacao = "", tem_contrato = "", data_proxima_manutencao = "", tag = "", text_color = "", bg_color = "";
        bool contrato = false;
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Serial</th>                        
                        <th scope='col'>Localização</th>
		                <th scope='col' style='text-align:center;'>Últ. Atual.</th>
                        <th scope='col' style='text-align:center;'>Próx. Manut.</th> 
                        <th scope='col' style='text-align:center;'>Contrato</th> 
		              </tr>
		            </thead> <tbody>";

        sql = @"declare @id_maquina int;
                declare @today date = getdate();
                declare @contaElementos1 int;
                declare @contaElementos2 int;

                select @contaElementos1 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_erro = 1
                and (data_proxima_manutencao is not null and data_proxima_manutencao <> '')

                select @contaElementos2 = count(id)
                from cashdro_report_maquinas(@id_maquina)
                where status_erro = 1
                and (data_proxima_manutencao is null or data_proxima_manutencao = '')
                
                if(@contaElementos1 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_erro = 1
                    and (data_proxima_manutencao is not null and data_proxima_manutencao <> '') 
                    order by data_proxima_manutencao desc, serialnumber asc
                end

                if(@contaElementos2 > 0)
                begin
                    select
                        id,
                        serialnumber,
                        localizacao,
                        case
                            when @today = cast(ultimacomunicacao as date) then convert(varchar, ultimacomunicacao, 108)
                            else concat(convert(varchar, ultimacomunicacao, 103), ' ', convert(varchar, ultimacomunicacao, 108))
                        end as ultimacomunicacao,
                        isnull(convert(varchar, cast(data_proxima_manutencao as date), 105), '') as data_proxima_manutencao,
                        tem_contrato,
                        CASE
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 5 THEN 'red'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) <= 15
			                     and datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 6 THEN 'yellow'
		                    WHEN datediff(dd, cast(getdate() as date), cast(data_proxima_manutencao as date)) >= 20 or data_proxima_manutencao is null THEN 'green'
		                    else 'black'
	                    END AS 'tag'
                    from cashdro_report_maquinas(@id_maquina)
                    where status_erro = 1
                    and (data_proxima_manutencao is null or data_proxima_manutencao = '') 
                    order by serialnumber asc
                end";

        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int j = 0; j < oDs.Tables.Count; j++)
            {
                for (int i = 0; i < oDs.Tables[j].Rows.Count; i++)
                {
                    id = oDs.Tables[j].Rows[i]["id"].ToString().Trim();
                    serialnumber = oDs.Tables[j].Rows[i]["serialnumber"].ToString().Trim();
                    localizacao = oDs.Tables[j].Rows[i]["localizacao"].ToString().Trim();
                    ultimacomunicacao = oDs.Tables[j].Rows[i]["ultimacomunicacao"].ToString().Trim();
                    contrato = Convert.ToBoolean(oDs.Tables[j].Rows[i]["tem_contrato"]);
                    data_proxima_manutencao = oDs.Tables[j].Rows[i]["data_proxima_manutencao"].ToString().Trim();
                    tag = oDs.Tables[j].Rows[i]["tag"].ToString().Trim();

                    text_color = (tag == "green" || tag == "red") ? " color: white; " : " ";
                    tem_contrato = contrato ? "green" : "red";
                    bg_color = String.IsNullOrEmpty(data_proxima_manutencao) ? " style='text-align:center; font-weight: bold;'" : " style='text-align:center; font-weight: bold; background-color:" + tag + ";' ";

                    html += @"<tr style='cursor:pointer;' onclick='maquinas(" + id + @");'> 
		                    <td>" + serialnumber + @"                    
		                    </td>
		                    <td>
		                      <span>" + localizacao + @"</span>
		                    </td>
		                    <td style='text-align:center;'>
		                      <span>" + ultimacomunicacao + @"</span>
		                    </td>
                            <td " + bg_color + @">
		                      <span style='" + text_color + ";'>" + data_proxima_manutencao + @"</span>
		                    </td>
                            <td style='text-align:center;'>
		                      <span class='badge badge-dot mr-12'>
			                    <i class='bg-success' style='height: 20px; width: 20px; background-color:" + tem_contrato + @"!important' ></i>
		                      </span>
		                    </td>
	                      </tr> ";
                }
            }
        }


        html += "  </tbody> </table>";


        return html;
    }
}