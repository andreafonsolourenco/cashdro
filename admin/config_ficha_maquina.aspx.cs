using System;
using System.Linq;
using System.Web.Services;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;

public partial class config_ficha_maquina : System.Web.UI.Page
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

        getListaClientes();
    }

    [WebMethod]
    public static string getMachineData(string url, string pass, string passSupport, string userSupport)
    {
        string serial = "", firmwareVersion = "", model = "";
        string propertiesPelicano = "", installDatePelicano = "", portNamePelicano = "", namePelicano = "", softwareVersionPelicano = "", serialPelicano = "";
        string propertiesHooper = "", installDateHooper = "", portNameHooper = "", nameHooper = "", firmwareHooper = "", serialHooper = "", datasetVersionHooper = "";
        string propertiesCashModule = "", installDateCashModule = "", portNameCashModule = "", nameCashModule = "", firmwareCashModule = "", serialCashModule = "";

        var wsurl = url + "/Cashdro3WS/index.php?name=" + userSupport  + "&operation=getInfoDevices&password=" + passSupport;
        string jsonOut = getRetornoURL(wsurl);

        if (jsonOut != "")
        {
            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");
            JToken code = contourManifest.SelectToken("code");

            if (code.ToString() == "1")
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    JToken parametros = data[i].SelectToken("Parametros");
                    JToken name = data[i].SelectToken("Name");
                    JToken properties = data[i].SelectToken("Properties");
                    JToken installDate = data[i].SelectToken("InstallDate");
                    JToken portName = data[i].SelectToken("PortName");

                    switch (name.ToString())
                    {
                        case "COIN_VALIDATOR":
                            propertiesPelicano = properties.ToString();
                            installDatePelicano = installDate.ToString();
                            portNamePelicano = portName.ToString();

                            for (int j = 0; j < parametros.Count(); j++)
                            {
                                JToken paramName = parametros[j].SelectToken("Name");
                                JToken value = parametros[j].SelectToken("Value");

                                if (paramName.ToString() == "SERIAL_NUMBER")
                                {
                                    serialPelicano = value.ToString();
                                }

                                if (paramName.ToString() == "NAME")
                                {
                                    namePelicano = value.ToString();
                                }

                                if (paramName.ToString() == "SOFTWARE_VERSION")
                                {
                                    softwareVersionPelicano = value.ToString();
                                }
                            }
                            break;
                        case "COIN_DISPENSER":
                            propertiesHooper = properties.ToString();
                            installDateHooper = installDate.ToString();
                            portNameHooper = portName.ToString();

                            for (int j = 0; j < parametros.Count(); j++)
                            {
                                JToken paramName = parametros[j].SelectToken("Name");
                                JToken value = parametros[j].SelectToken("Value");

                                if (paramName.ToString() == "SERIAL_NUMBER")
                                {
                                    serialHooper = value.ToString();
                                }

                                if (paramName.ToString() == "DATASET_VERSION")
                                {
                                    datasetVersionHooper = value.ToString();
                                }

                                if (paramName.ToString() == "NAME")
                                {
                                    nameHooper = value.ToString();
                                }

                                if (paramName.ToString() == "FIRMWARE")
                                {
                                    firmwareHooper = value.ToString();
                                }
                            }
                            break;
                        case "BILL_DISPENSER":
                            propertiesCashModule = properties.ToString();
                            installDateCashModule = installDate.ToString();
                            portNameCashModule = portName.ToString();

                            for (int j = 0; j < parametros.Count(); j++)
                            {
                                JToken paramName = parametros[j].SelectToken("Name");
                                JToken value = parametros[j].SelectToken("Value");

                                if (paramName.ToString() == "SERIAL_NUMBER")
                                {
                                    serialCashModule = value.ToString();
                                }

                                if (paramName.ToString() == "FIRMWARE")
                                {
                                    firmwareCashModule = value.ToString();
                                }

                                if (paramName.ToString() == "NAME")
                                {
                                    nameCashModule = value.ToString();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        wsurl = url + "/Cashdro3WS/index.php?operation=getCashdroParams";

        jsonOut = getRetornoURL(wsurl);

        if (jsonOut != "")
        {
            JToken contourManifest = JObject.Parse(jsonOut);
            JToken data = contourManifest.SelectToken("data");
            JToken code = contourManifest.SelectToken("code");

            if (code.ToString() == "1")
            {
                JToken serialNumberCashdro = data.SelectToken("numSerieCashdro");
                JToken versionSistema = data.SelectToken("versionSistema");
                JToken cashdroModel = data.SelectToken("cashdroModel");

                serial = serialNumberCashdro.ToString();
                firmwareVersion = versionSistema.ToString();
                model = cashdroModel.ToString();
            }
        }

        return propertiesPelicano + "<#SEP#>" + installDatePelicano + "<#SEP#>" + portNamePelicano + "<#SEP#>" + namePelicano + "<#SEP#>" + softwareVersionPelicano + "<#SEP#>" + serialPelicano + "<#SEP#>" +
            propertiesHooper + "<#SEP#>" + installDateHooper + "<#SEP#>" + portNameHooper + "<#SEP#>" + nameHooper + "<#SEP#>" + firmwareHooper + "<#SEP#>" + serialHooper + "<#SEP#>" + datasetVersionHooper + "<#SEP#>" +
            propertiesCashModule + "<#SEP#>" + installDateCashModule + "<#SEP#>" + portNameCashModule + "<#SEP#>" + nameCashModule + "<#SEP#>" + firmwareCashModule + "<#SEP#>" + serialCashModule + "<#SEP#>" + 
            serial + "<#SEP#>" + firmwareVersion + "<#SEP#>" + model;
    }


    [WebMethod]
    public static string getRetornoURL(string url)
    {
        try
        {
            WebClient client = new WebClient();

            client.Headers.Add("User-Agent: BrowseAndDownload");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string ret = client.DownloadString(url);

            //TRIMA a string        
            ret = ret.Trim();

            return ret;
        }
        catch (Exception)
        {
            return "";
        }
    }

    [WebMethod]
    public static string saveData(string id, string serial, string localizacao, string ip, string user, string password, string ativo, 
        string serialPelicano, string serialHooper, string serialCashModule, string firmwareVersion, string password_support, string user_support,
        string model, string namePelicano, string softwareVersionPelicano, string propertiesPelicano, string installDatePelicano, string portNamePelicano, 
        string nameHooper, string firmwareHooper, string datasetHooper, string propertiesHooper, string installDateHooper, string portNameHooper, 
        string nameCashModule, string firmwareCashModule, string propertiesCashModule, string installDateCashModule, string portNameCashModule, 
        string customer, string dias_intervalo_manutencao, string horaEmail, string mostraligdireta, string temcontrato, string contratoParceiro)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "1", retMessage = "Dados guardados com sucesso.";
        sql = string.Format(@"  DECLARE @id INT={0}
                                DECLARE @serial VARCHAR(100)='{1}'
                                DECLARE @localizacao VARCHAR(max)='{2}'
                                DECLARE @ip VARCHAR(100)='{3}'
                                DECLARE @user varchar(30)='{4}'
                                DECLARE @password varchar(30)='{5}'
                                DECLARE @ativo bit={6}
                                DECLARE @serialPelicano VARCHAR(100)='{7}'
                                DECLARE @serialHooper VARCHAR(100)='{8}'
                                DECLARE @serialCashModule VARCHAR(100)='{9}'
                                DECLARE @firmwareVersion VARCHAR(100)='{10}'
                                DECLARE @passwordSupport VARCHAR(30)='{11}'
                                DECLARE @userSupport VARCHAR(30)='{12}'
                                DECLARE @model VARCHAR(30)='{13}'
                                DECLARE @nameCoinValidator VARCHAR(max)='{14}'
	                            DECLARE @softwareVersionCoinValidator VARCHAR(max)='{15}'
	                            DECLARE @propertiesCoinValidator VARCHAR(max)='{16}'
	                            DECLARE @installDateCoinValidator VARCHAR(max)='{17}'
	                            DECLARE @portNameCoinValidator VARCHAR(max)='{18}'
	                            DECLARE @nameCoinDispenser VARCHAR(max)='{19}'
	                            DECLARE @firmwareCoinDispenser VARCHAR(max)='{20}'
	                            DECLARE @datasetCoinDispenser VARCHAR(max)='{21}'
	                            DECLARE @propertiesCoinDispenser VARCHAR(max)='{22}'
	                            DECLARE @installDateCoinDispenser VARCHAR(max)='{23}'
	                            DECLARE @portNameCoinDispenser VARCHAR(max)='{24}'
	                            DECLARE @nameBillDispenser VARCHAR(max)='{25}'
	                            DECLARE @firmwareBillDispenser VARCHAR(max)='{26}'
	                            DECLARE @propertiesBillDispenser VARCHAR(max)='{27}'
	                            DECLARE @installDateBillDispenser VARCHAR(max)='{28}'
	                            DECLARE @portNameBillDispenser VARCHAR(max)='{29}'
                                DECLARE @cliente VARCHAR(255)={30}
                                DECLARE @dias_intervalo_manutencao INT = {31}
                                DECLARE @id_cliente int
                                DECLARE @horaEmail varchar(5)='{32}'
                                DECLARE @mostraligdireta bit={33}
                                DECLARE @temcontrato bit={34}
	                            DECLARE @contrato_parceiro varchar(max)='{35}'
                                DECLARE @ret int 
                                DECLARE @retMsg VARCHAR(255)

                                SELECT @id_cliente = UTILIZADORESID FROM UTILIZADORES WHERE nome = @cliente;

                                EXEC parametrizacao_maquinacashdro_novoedita @id,@serial,@localizacao,@ip,@user,@password,@ativo,@serialPelicano,@serialHooper,@serialCashModule,@firmwareVersion,
                                    @passwordSupport,@userSupport,@model,@nameCoinValidator,@softwareVersionCoinValidator,@propertiesCoinValidator,@installDateCoinValidator,@portNameCoinValidator,
                                    @nameCoinDispenser,@firmwareCoinDispenser,@datasetCoinDispenser,@propertiesCoinDispenser,@installDateCoinDispenser,@portNameCoinDispenser,
                                    @nameBillDispenser,@firmwareBillDispenser,@propertiesBillDispenser,@installDateBillDispenser,@portNameBillDispenser,@dias_intervalo_manutencao,
                                    @id_cliente, @horaEmail,@mostraligdireta,@temcontrato,@contrato_parceiro,@ret OUTPUT,@retMsg OUTPUT
                                SELECT @ret ret, @retMsg retMsg ", 
                                id, 
                                serial, 
                                localizacao, 
                                ip, 
                                user, 
                                password, 
                                ativo, 
                                serialPelicano, 
                                serialHooper, 
                                serialCashModule, 
                                firmwareVersion, 
                                password_support, 
                                user_support,
                                model,
                                namePelicano, 
                                softwareVersionPelicano, 
                                propertiesPelicano, 
                                installDatePelicano, 
                                portNamePelicano,
                                nameHooper, 
                                firmwareHooper, 
                                datasetHooper, 
                                propertiesHooper, 
                                installDateHooper, 
                                portNameHooper,
                                nameCashModule, 
                                firmwareCashModule, 
                                propertiesCashModule, 
                                installDateCashModule, 
                                portNameCashModule,
                                customer == "" ? "NULL" : "'" + customer + "'",
                                dias_intervalo_manutencao,
                                horaEmail,
                                mostraligdireta,
                                temcontrato,
                                contratoParceiro);

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
        string sql = "", ret = "", serialnumber = "", localizacao = "", url = "", username = "", password = "", password_suporte = "", user_suporte = "",
            serialnumber_pelicano = "", serialnumber_hooper = "", serialnumber_cashmodule = "", firmware_version = "";
        string model = "", nameCoinValidator = "", softwareVersionCoinValidator = "", propertiesCoinValidator = "", installDateCoinValidator = "", portNameCoinValidator = "", 
            nameCoinDispenser = "", firmwareCoinDispenser = "", datasetCoinDispenser = "", propertiesCoinDispenser = "", installDateCoinDispenser = "", portNameCoinDispenser = "", 
            nameBillDispenser = "", firmwareBillDispenser = "", propertiesBillDispenser = "", installDateBillDispenser = "", portNameBillDispenser = "", cliente = "",
            dias_intervalo_manutencao = "", horaenvioemail="", contratoparceiro="";

        bool ativo = false, mostraligdireta = false, temcontrato = false;
        string s_ativo = "false", s_mostraligdireta = "false", s_temcontrato = "false";

        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@"  DECLARE @id int = {0}
                                SELECT
	                                id,
	                                serialnumber,
	                                localizacao, 
                                    url,
	                                username,
	                                password, 
	                                ativo,
                                    serialnumber_pelicano,
                                    serialnumber_hooper,
                                    serialnumber_cashmodule,
                                    firmware_version,
                                    password_suporte,
                                    user_suporte,
                                    model,
		                            nameCoinValidator,
		                            softwareVersionCoinValidator,
		                            propertiesCoinValidator,
		                            installDateCoinValidator,
		                            portNameCoinValidator,
		                            nameCoinDispenser,
		                            firmwareCoinDispenser,
		                            datasetCoinDispenser,
		                            propertiesCoinDispenser,
		                            installDateCoinDispenser,
		                            portNameCoinDispenser,
		                            nameBillDispenser,
		                            firmwareBillDispenser,
		                            propertiesBillDispenser,
		                            installDateBillDispenser,
		                            portNameBillDispenser,
                                    dias_intervalo_manutencao,
                                    id_cliente,
                                    cliente,
                                    horaenvioemail,
                                    mostra_ligacao_direta,
                                    tem_contrato,
                                    contrato_parceiro
                                FROM CASHDRO_REPORT_MAQUINAS(@id)", id);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            serialnumber = oDs.Tables[0].Rows[0]["serialnumber"].ToString().Trim();
            localizacao = oDs.Tables[0].Rows[0]["localizacao"].ToString().Trim();
            url = oDs.Tables[0].Rows[0]["url"].ToString().Trim();
            username = oDs.Tables[0].Rows[0]["username"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();
            ativo = Convert.ToBoolean(oDs.Tables[0].Rows[0]["ativo"]);
            serialnumber_pelicano = oDs.Tables[0].Rows[0]["serialnumber_pelicano"].ToString().Trim();
            serialnumber_hooper = oDs.Tables[0].Rows[0]["serialnumber_hooper"].ToString().Trim();
            serialnumber_cashmodule = oDs.Tables[0].Rows[0]["serialnumber_cashmodule"].ToString().Trim();
            firmware_version = oDs.Tables[0].Rows[0]["firmware_version"].ToString().Trim();
            password_suporte = oDs.Tables[0].Rows[0]["password_suporte"].ToString().Trim();
            user_suporte = oDs.Tables[0].Rows[0]["user_suporte"].ToString().Trim();
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
            cliente = oDs.Tables[0].Rows[0]["cliente"].ToString().Trim();
            dias_intervalo_manutencao = oDs.Tables[0].Rows[0]["dias_intervalo_manutencao"].ToString().Trim();
            horaenvioemail = oDs.Tables[0].Rows[0]["horaenvioemail"].ToString().Trim();
            mostraligdireta = Convert.ToBoolean(oDs.Tables[0].Rows[0]["mostra_ligacao_direta"]);
            temcontrato = Convert.ToBoolean(oDs.Tables[0].Rows[0]["tem_contrato"]);
            contratoparceiro = oDs.Tables[0].Rows[0]["contrato_parceiro"].ToString().Trim();

            s_ativo = ativo ? "true" : "false";
            s_mostraligdireta = mostraligdireta ? "true" : "false";
            s_temcontrato = temcontrato ? "true" : "false";
        }

        // Prepara o retorno dos dados
        ret = serialnumber + "<#SEP#>" +
              localizacao + "<#SEP#>" +
              url + "<#SEP#>" +
              username + "<#SEP#>" +
              password + "<#SEP#>" +
              s_ativo + "<#SEP#>" +
              serialnumber_pelicano + "<#SEP#>" +
              serialnumber_hooper + "<#SEP#>" +
              serialnumber_cashmodule + "<#SEP#>" +
              firmware_version + "<#SEP#>" +
              password_suporte + "<#SEP#>" +
              user_suporte + "<#SEP#>" +
              model + "<#SEP#>" +
              nameCoinValidator + "<#SEP#>" +
              softwareVersionCoinValidator + "<#SEP#>" +
              propertiesCoinValidator + "<#SEP#>" +
              installDateCoinValidator + "<#SEP#>" +
              portNameCoinValidator + "<#SEP#>" +
              nameCoinDispenser + "<#SEP#>" +
              firmwareCoinDispenser + "<#SEP#>" +
              datasetCoinDispenser + "<#SEP#>" +
              propertiesCoinDispenser + "<#SEP#>" +
              installDateCoinDispenser + "<#SEP#>" +
              portNameCoinDispenser + "<#SEP#>" +
              nameBillDispenser + "<#SEP#>" +
              firmwareBillDispenser + "<#SEP#>" +
              propertiesBillDispenser + "<#SEP#>" +
              installDateBillDispenser + "<#SEP#>" +
              portNameBillDispenser + "<#SEP#>" +
              cliente + "<#SEP#>" +
              dias_intervalo_manutencao + "<#SEP#>" +
              horaenvioemail + "<#SEP#>" +
              s_mostraligdireta + "<#SEP#>" +
              s_temcontrato + "<#SEP#>" +
              contratoparceiro;

        return ret;
    }

    [WebMethod]
    public static string getCustomerList()
    {
        string sql = "", ret = "";

        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@"  DECLARE @id int
                                DECLARE @username varchar(150)
                                DECLARE @password varchar(60)
                                DECLARE @ativo bit = 1;
                                SELECT
	                                id,
                                    nome,
                                    tipo
                                FROM CASHDRO_REPORT_UTILIZADORES(@id, @username, @password, @ativo)
                                where tipo in ('Administrador', 'Cliente')
                                order by tipo, nome");
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                if(i>0)
                {
                    ret += "<#SEP#>";
                }

                ret += oDs.Tables[0].Rows[i]["nome"].ToString().Trim();
            }
        }

        return ret;
    }

    [WebMethod]
    public static string getUserType(string id)
    {
        string sql = "";

        DataSqlServer oDB = new DataSqlServer();

        sql = string.Format(@"  DECLARE @id int={0};
                                DECLARE @ativo bit=1;
                                DECLARE @user varchar(150);
                                DECLARE @pass varchar(60);
                                SELECT
	                                tipo
                                FROM CASHDRO_REPORT_UTILIZADORES(@id,@user,@pass,@ativo)", id);

        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs) && oDs.Tables[0].Rows[0]["tipo"].ToString().Trim() == "Cliente")
        {
            return "0";
        }
        if (oDB.validaDataSet(oDs) && oDs.Tables[0].Rows[0]["tipo"].ToString().Trim() == "Administrador")
        {
            return "-1";
        }

        return "1";
    }

    private void getListaClientes()
    {
        string sql = "", ret = "";

        DataSqlServer oDB = new DataSqlServer();
        sql = string.Format(@"  DECLARE @id int
                                DECLARE @username varchar(150)
                                DECLARE @password varchar(60)
                                DECLARE @ativo bit = 1;
                                SELECT
	                                id,
                                    nome,
                                    tipo
                                FROM CASHDRO_REPORT_UTILIZADORES(@id, @username, @password, @ativo)
                                where tipo in ('Cliente')
                                order by tipo, nome");


        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                if (i > 0)
                {
                    ret += "<#SEP#>";
                }

                ret += oDs.Tables[0].Rows[i]["id"].ToString().Trim() + " [" + oDs.Tables[0].Rows[i]["nome"].ToString().Trim() + "]";                
            }
        }

        autocompletelist.InnerText = ret;
    }
}