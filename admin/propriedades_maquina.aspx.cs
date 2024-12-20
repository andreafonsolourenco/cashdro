using System;
using System.Web.Services;
using System.Data;
using System.Net;
using System.IO;
using System.Text;

public partial class propriedades_maquina : System.Web.UI.Page
{
    string idMaquina = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request != null && Request.QueryString["id"] != null)
        {
            idMaquina = Request.QueryString["id"].ToString();
        }

        try
        {
            int x = Int32.Parse(idMaquina);
        }
        catch (Exception)
        {
            
        }

        txtAux.Value = idMaquina;

        getMachineName();
        getMachineData();
    }

    private void getMachineName()
    {
        string sql = "", nome = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = String.Format(@"  declare @id int = {0};
                                SELECT
	                                localizacao
                                FROM CASHDRO_REPORT_MAQUINAS(@id)
                                order by serialnumber, localizacao ", idMaquina);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                nome = oDs.Tables[0].Rows[i]["localizacao"].ToString().Trim();
            }
        }

        machineTitle.InnerHtml = nome;
    }

    private void getMachineData()
    {
        string sql = "", serialnumber = "", serialnumber_pelicano = "", serialnumber_hooper = "", serialnumber_cashmodule = "", firmware_version = "";
        string model = "", nameCoinValidator = "", softwareVersionCoinValidator = "", propertiesCoinValidator = "", installDateCoinValidator = "", portNameCoinValidator = "",
            nameCoinDispenser = "", firmwareCoinDispenser = "", datasetCoinDispenser = "", propertiesCoinDispenser = "", installDateCoinDispenser = "", portNameCoinDispenser = "",
            nameBillDispenser = "", firmwareBillDispenser = "", propertiesBillDispenser = "", installDateBillDispenser = "", portNameBillDispenser = "";

        string grelhaCashdro = "", grelhaCoinValidator = "", grelhaCoinDispenser = "", grelhaBillDispenser = "", grelhaExportExcel = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  DECLARE @id int = {0}
                                SELECT
	                                serialnumber,
                                    firmware_version,
                                    model,
                                    serialnumber_pelicano,
                                    nameCoinValidator,
		                            softwareVersionCoinValidator,
		                            propertiesCoinValidator,
		                            installDateCoinValidator,
		                            portNameCoinValidator,
                                    serialnumber_hooper,
                                    nameCoinDispenser,
		                            firmwareCoinDispenser,
		                            datasetCoinDispenser,
		                            propertiesCoinDispenser,
		                            installDateCoinDispenser,
		                            portNameCoinDispenser,
                                    serialnumber_cashmodule,
		                            nameBillDispenser,
		                            firmwareBillDispenser,
		                            propertiesBillDispenser,
		                            installDateBillDispenser,
		                            portNameBillDispenser
                                FROM CASHDRO_REPORT_MAQUINAS(@id)", idMaquina);


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            grelhaExportExcel += "<table class='table align-items-center table-flush'>";

            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                serialnumber = oDs.Tables[0].Rows[0]["serialnumber"].ToString().Trim();
                serialnumber_pelicano = oDs.Tables[0].Rows[0]["serialnumber_pelicano"].ToString().Trim();
                serialnumber_hooper = oDs.Tables[0].Rows[0]["serialnumber_hooper"].ToString().Trim();
                serialnumber_cashmodule = oDs.Tables[0].Rows[0]["serialnumber_cashmodule"].ToString().Trim();
                firmware_version = oDs.Tables[0].Rows[0]["firmware_version"].ToString().Trim();
                model = oDs.Tables[0].Rows[0]["model"].ToString().Trim();
                nameCoinValidator = oDs.Tables[0].Rows[0]["nameCoinValidator"].ToString().Trim();
                softwareVersionCoinValidator = oDs.Tables[0].Rows[0]["softwareVersionCoinValidator"].ToString().Trim();
                propertiesCoinValidator = oDs.Tables[0].Rows[0]["propertiesCoinValidator"].ToString().Trim();
                installDateCoinValidator = oDs.Tables[0].Rows[0]["installDateCoinValidator"].ToString().Trim();
                portNameCoinValidator = oDs.Tables[0].Rows[0]["portNameCoinValidator"].ToString().Trim();
                nameCoinDispenser = oDs.Tables[0].Rows[0]["nameCoinDispenser"].ToString().Trim();
                firmwareCoinDispenser = oDs.Tables[0].Rows[0]["firmwareCoinDispenser"].ToString().Trim();
                datasetCoinDispenser = oDs.Tables[0].Rows[0]["datasetCoinDispenser"].ToString().Trim();
                propertiesCoinDispenser = oDs.Tables[0].Rows[0]["propertiesCoinDispenser"].ToString().Trim();
                installDateCoinDispenser = oDs.Tables[0].Rows[0]["installDateCoinDispenser"].ToString().Trim();
                portNameCoinDispenser = oDs.Tables[0].Rows[0]["portNameCoinDispenser"].ToString().Trim();
                nameBillDispenser = oDs.Tables[0].Rows[0]["nameBillDispenser"].ToString().Trim();
                firmwareBillDispenser = oDs.Tables[0].Rows[0]["firmwareBillDispenser"].ToString().Trim();
                propertiesBillDispenser = oDs.Tables[0].Rows[0]["propertiesBillDispenser"].ToString().Trim();
                installDateBillDispenser = oDs.Tables[0].Rows[0]["installDateBillDispenser"].ToString().Trim();
                portNameBillDispenser = oDs.Tables[0].Rows[0]["portNameBillDispenser"].ToString().Trim();
            }
        }

        grelhaCashdro = String.Format(@" <table class='table align-items-center table-flush'>
		                        <thead class='thead-light'>
		                            <tr>
                                        <th scope='col' colspan='2'>Cashdro</th>
		                            </tr>
		                        </thead>
                                <tbody>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>{0}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>{1}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>MODELO</td>
                                        <td class='text-right'>{2}</td>
                                    </tr>
                                </tbody>
                            </table>", serialnumber, firmware_version, model);

        grelhaExportExcel += @" <tbody>
                                    <tr>
                                        <td scope='col' colspan='2'>Cashdro</td>
		                            </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>" + serialnumber + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>" + firmware_version + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>MODELO</td>
                                        <td class='text-right'>" + model + @"</td>
                                    </tr>
                                    <tr>
                                        <td scope='col' colspan='2'></td>
		                            </tr>";

        grelhaCoinValidator = String.Format(@" <table class='table align-items-center table-flush'>
		                        <thead class='thead-light'>
		                            <tr>
                                        <th scope='col' colspan='2'>Validador de Moedas</th>
		                            </tr>
		                        </thead>
                                <tbody>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>{0}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>{1}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE SOFTWARE</td>
                                        <td class='text-right'>{2}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>{3}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>{4}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>{5}</td>
                                    </tr>
                                </tbody>
                            </table>", nameCoinValidator, serialnumber_pelicano, softwareVersionCoinValidator, propertiesCoinValidator, installDateCoinValidator, portNameCoinValidator);

        grelhaExportExcel += @"
                                    <tr>
                                        <td scope='col' colspan='2'>Validador de Moedas</tD>
		                            </tr>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>" + nameCoinValidator + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>" + serialnumber_pelicano + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE SOFTWARE</td>
                                        <td class='text-right'>" + softwareVersionCoinValidator + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>" + propertiesCoinValidator + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>" + installDateCoinValidator + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>" + portNameCoinValidator + @"</td>
                                    </tr>
                                    <tr>
                                        <td scope='col' colspan='2'></td>
		                            </tr>";

        grelhaCoinDispenser = String.Format(@" <table class='table align-items-center table-flush'>
		                        <thead class='thead-light'>
		                            <tr>
                                        <th scope='col' colspan='2'>Dispensador de Moedas</th>
		                            </tr>
		                        </thead>
                                <tbody>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>{0}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>{1}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>{2}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE DATASET</td>
                                        <td class='text-right'>{3}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>{4}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>{5}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>{6}</td>
                                    </tr>
                                </tbody>
                            </table>", nameCoinDispenser, serialnumber_hooper, firmwareCoinDispenser, datasetCoinDispenser, propertiesCoinDispenser, installDateCoinDispenser, portNameCoinDispenser);

        grelhaExportExcel += @"     <tr>
                                        <td scope='col' colspan='2'>Dispensador de Moedas</td>
		                            </tr>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>" + nameCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>" + serialnumber_hooper + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>" + firmwareCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE DATASET</td>
                                        <td class='text-right'>" + datasetCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>" + propertiesCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>" + installDateCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>" + portNameCoinDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td scope='col' colspan='2'></td>
		                            </tr>";

        grelhaBillDispenser = String.Format(@" <table class='table align-items-center table-flush'>
		                        <thead class='thead-light'>
		                            <tr>
                                        <th scope='col' colspan='2'>Dispensador de Notas</th>
		                            </tr>
		                        </thead>
                                <tbody>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>{0}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>{1}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>{2}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>{3}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>{4}</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>{5}</td>
                                    </tr>
                                </tbody>
                            </table>", nameBillDispenser, serialnumber_cashmodule, firmwareBillDispenser, propertiesBillDispenser, installDateBillDispenser, portNameBillDispenser);

        grelhaExportExcel += @"     <tr>
                                        <td scope='col' colspan='2'>Dispensador de Notas</td>
		                            </tr>
                                    <tr>
                                        <td class='text-left'>NOME</td>
                                        <td class='text-right'>" + nameBillDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>SERIAL</td>
                                        <td class='text-right'>" + serialnumber_cashmodule + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>VERSÃO DE FIRMWARE</td>
                                        <td class='text-right'>" + firmwareBillDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PROPRIEDADES</td>
                                        <td class='text-right'>" + propertiesBillDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>DATA DE INSTALAÇÃO</td>
                                        <td class='text-right'>" + installDateBillDispenser + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='text-left'>PORTA</td>
                                        <td class='text-right'>" + portNameBillDispenser + @"</td>
                                    </tr></tbody></table>";

        divGrelhaCashdro.InnerHtml = grelhaCashdro;
        divGrelhaCoinValidator.InnerHtml = grelhaCoinValidator;
        divGrelhaCoinDispenser.InnerHtml = grelhaCoinDispenser;
        divGrelhaBillDispenser.InnerHtml = grelhaBillDispenser;
        divGrelhaExportExcel.InnerHtml = grelhaExportExcel;
    }

    [WebMethod]
    public static string getTotais(string id_utilizador)
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
	            from [cashdro_estadomaquinas_totaispagina] (null) tmp ";

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
}