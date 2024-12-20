using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class parametrizacao : System.Web.UI.Page
{
    string id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            id = Request.QueryString["id"];
            txtAux.Value = id;
        }
        catch (Exception)
        {
        }
    }

    [WebMethod]
    public static string getTotais()
    {
        string sql = "", html = "";
        string label1 = "", total1 = "", rodape1 = "", label2 = "", total2 = "", rodape2 = "", label3 = "", total3 = "", rodape3 = "", label4 = "", total4 = "", rodape4 = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = @" select
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
	            from [mbs_lista_totaispagina_parametrizacao] (null) tmp ";

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

            label4 = oDs.Tables[0].Rows[0]["label4"].ToString().Trim();
            total4 = oDs.Tables[0].Rows[0]["total4"].ToString().Trim();
            rodape4 = oDs.Tables[0].Rows[0]["rodape4"].ToString().Trim();
        }


        html = label1 + "@" + total1 + "@" + rodape1 + "@" + label2 + "@" + total2 + "@" + rodape2 + "@" + label3 + "@" + total3 + "@" + rodape3 + "@" + label4 + "@" + total4 + "@" + rodape4;

        return html;
    }

    [WebMethod]
    public static string getGrelhaEncomendas(string idCliente)
    {
        string sql = "", html = "";
        string id = "", data = "", cliente = "", total = "", qtd = "", estado = "", status = "", pago = "";
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Encomenda Nº</th>			            
			            <th scope='col'>Data</th>
			            <th scope='col'>Total</th>
			            <th scope='col'>Nº produtos</th>
                        <th scope='col'>Paga</th>
			            <th scope='col'>Estado</th>
		              </tr>
		            </thead> <tbody>";




        sql = @" SELECT
	                enc.encomendasid id,
	                case when cast(data as date)=cast(getdate() as date) then 
		                case WHEN DATEDIFF(mi,data, getdate())<60 then rtriM(ltrim(DATEDIFF(mi,data, getdate()))) + ' minutos atrás' ELSE rtrim(ltrim(DATEDIFF(hh,data, getdate()))) + ' horas atrás' end
	                else convert(varchar(10), data, 105) end data,
	                nomecurto + ' (' + isnull(str(id_cliente),'') + ')' cliente,
                    case when enc.pagaem is null then '<span style=color:red>Não</span>' else '<span style=color:green>Sim</span>' end as pago,
	                total,
	                nritens qtd,
                    st.designacao estado
                FROM ENCOMENDAS enc
                inner join ENCOMENDAS_STATUS st ON st.ENCOMENDAS_STATUSID = enc.id_status
                WHERE id_cliente=" + idCliente + @"                              
                ORDER BY enc.ENCOMENDASID DESC  ";



        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                data = oDs.Tables[0].Rows[i]["data"].ToString().Trim();
                cliente = oDs.Tables[0].Rows[i]["cliente"].ToString().Trim();
                total = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";
                qtd = oDs.Tables[0].Rows[i]["qtd"].ToString().Trim();
                estado = oDs.Tables[0].Rows[i]["estado"].ToString().Trim();
                pago = oDs.Tables[0].Rows[i]["pago"].ToString().Trim();

                html += @"<tr> 
		                    <th scope='row'>" + id + @"</th>
		                    <td>" + data + @"                    
		                    </td>
		                    <td>
		                      <span>" + total + @"</span>
		                    </td>
		                    <td>
		                      " + qtd + @"
		                    </td>
                            <td>
		                      " + pago + @"
		                    </td>
		                    <td>
		                      " + estado + @"
		                    </td>		                                    
	                      </tr> ";


            }
        }


        html += "  </tbody> </table>";


        return html;
    }

    [WebMethod]
    public static string getGrelhaProdutos(string idCliente)
    {
        string sql = "", html = "";
        string id = "", produto = "", qtd = "", total = "";
        DataSqlServer oDB = new DataSqlServer();


        html += @" <table class='table align-items-center table-flush'>
		        <thead class='thead-light'>
		              <tr>
                        <th scope='col'>Produto</th>			            
			            <th scope='col'>Quantidade</th>
			            <th scope='col'>Total</th>
		              </tr>
		            </thead> <tbody>";




        sql = @" SELECT id, produto, qtd, total FROM (
	                SELECT
		                ln.id_artigo id,
		                ln.nomeartigo + ' (' + rtrim(ltrim(ln.referencia)) + ')' produto,
		                rtrim(ltrim(isnull(sum(cast(ln.qtd as int)),0))) qtd,
		                rtrim(ltrim(isnull(sum(ln.subtotal),0))) total
	                FROM ENCOMENDAS enc
	                INNER JOIN dbo.ENCOMENDAS_ARTIGOS ln on ln.id_encomenda = enc.encomendasid 
	                WHERE id_cliente=" + idCliente + @"                 
	                group by ln.id_artigo, ln.referencia, ln.nomeartigo
                ) tot
                order by tot.total desc   ";



        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                produto = oDs.Tables[0].Rows[i]["produto"].ToString().Trim();
                qtd = oDs.Tables[0].Rows[i]["qtd"].ToString().Trim();
                total = oDs.Tables[0].Rows[i]["total"].ToString().Trim() + " €";

                html += @"<tr> 
		                    <td>" + produto + @"                    
		                    </td>
		                    <td>
		                      " + qtd + @"
		                    </td>
                            <td>
		                      " + total.Replace(".", ",") + @"
		                    </td>
	                      </tr> ";


            }
        }


        html += "  </tbody> </table>";


        return html;
    }
}