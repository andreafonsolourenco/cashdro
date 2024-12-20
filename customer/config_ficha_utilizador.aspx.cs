using System;
using System.Web.Services;
using System.Data;


public partial class config_ficha_utilizador : System.Web.UI.Page
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
        getTipoUtilizadores();
    }



    [WebMethod]
    public static string saveData(string id, string nome, string tipo, string telemovel, string email, string password, string notas, string ativo, 
        string parceiro, string suspenso, string pin, string nmaquinas, string nif, string morada, string nomeestabelecimento, string pessoaresponsavel, 
        string cashdronome, string mostraligdireta, string nclientesparceiro)
    {
        DataSqlServer oDB = new DataSqlServer();

        string sql = "", ret = "1", retMessage = "Dados guardados com sucesso.";

        if (nmaquinas.Trim() == "") nmaquinas = "null";
        if (nclientesparceiro.Trim() == "" || nclientesparceiro.Trim() == "null" || nclientesparceiro.Trim() == "undefined") nclientesparceiro = "0";

        sql = string.Format(@"  DECLARE @id int={0}
                                DECLARE @nome varchar(255)='{1}'
                                DECLARE @tipo int={2}
                                DECLARE @telemovel varchar(50)='{3}'
                                DECLARE @email varchar(150)='{4}'
                                DECLARE @password varchar(60)='{5}'
                                DECLARE @notas varchar(max)='{6}'
                                DECLARE @ativo bit={7}
                                DECLARE @parceiro varchar(255)={8}
                                DECLARE @id_parceiro int
                                DECLARE @suspenso bit={9}
                                DECLARE @pin varchar(50)={10}
                                DECLARE @nmaq int={11}
                                DECLARE @nif varchar(30)='{12}'
                                DECLARE @morada varchar(255)='{13}'
                                DECLARE @nomeestabelecimento varchar(255)='{14}'
                                DECLARE @pessoaresponsavel varchar(255)='{15}'
                                DECLARE @cashdronome varchar(255)='{16}'
                                DECLARE @mostraligdireta bit={17}
                                DECLARE @nrclientesparceiro int={18}

                                DECLARE @ret int
                                DECLARE @retMsg varchar(255)

                                SELECT @id_parceiro = id from CASHDRO_REPORT_UTILIZADORES(null, null, null, 1) where nome = @parceiro and tipo in ('Parceiro', 'Administrador')

                                EXECUTE mbs_parametrizacao_utilizador_novoedita @id,@nome,@tipo,@telemovel,@email,@password,@notas,@ativo,@id_parceiro,@suspenso,@pin,@nmaq,@nif,@morada,@nomeestabelecimento,@pessoaresponsavel,@cashdronome,@mostraligdireta,@nrclientesparceiro,@ret OUTPUT,@retMsg OUTPUT
                                SELECT @ret ret, @retMsg retMsg ",
                                id,
                                nome,
                                tipo,
                                telemovel,
                                email,
                                password,
                                notas,
                                ativo,
                                parceiro == "" ? "NULL" : "'" + parceiro + "'",
                                suspenso,
                                String.IsNullOrEmpty(pin) ? "''" : String.Format("'{0}'", pin),
                                nmaquinas, 
                                nif,
                                morada,
                                nomeestabelecimento,
                                pessoaresponsavel,
                                cashdronome,
                                mostraligdireta);


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
        string sql = "", ret = "", tipo = "", nome = "", email = "", password = "", notas = "", telemovel = "", id_tipo_utilizador = "", s_ativo = "false", parceiro = "", s_suspenso = "", pin = "", nmaquinas="";
        string nif = "", morada = "", nomeestabelecimento = "", pessoaresponsavel = "", cashdronome = "", s_ligdireta = "", nr_utilizadores_parceiro = "";
        bool ativo = false, suspenso = false, mostra_lig_direta = false;

        DataSqlServer oDB = new DataSqlServer();


        sql = string.Format(@"  DECLARE @id int = {0}
                                DECLARE @ativo bit;
                                DECLARE @user varchar(150);
                                DECLARE @pass varchar(60);
                                SELECT
	                                id,
	                                tipo,
	                                nome,
	                                email,
	                                password,
	                                telemovel,
	                                notas,
	                                ativo,
                                    id_tipo_utilizador,
                                    id_parceiro,
                                    parceiro,
                                    suspenso,
                                    pin,
                                    nmaquinas,
                                    nif,
                                    morada,
                                    nomeestabelecimento,
                                    pessoaresponsavel,
                                    cashdronome,
                                    mostra_ligacao_direta_maquina,
                                    nr_clientes_parceiro
                                FROM cashdro_report_utilizadores(@id,@user,@pass,@ativo) ", id);
        DataSet oDs = oDB.GetDataSet(sql, "").oData;

        if (oDB.validaDataSet(oDs))
        {
            tipo = oDs.Tables[0].Rows[0]["tipo"].ToString().Trim();
            nome = oDs.Tables[0].Rows[0]["nome"].ToString().Trim();
            email = oDs.Tables[0].Rows[0]["email"].ToString().Trim();
            password = oDs.Tables[0].Rows[0]["password"].ToString().Trim();
            telemovel = oDs.Tables[0].Rows[0]["telemovel"].ToString().Trim();
            notas = oDs.Tables[0].Rows[0]["notas"].ToString().Trim();
            ativo = Convert.ToBoolean(oDs.Tables[0].Rows[0]["ativo"]);
            id_tipo_utilizador = oDs.Tables[0].Rows[0]["id_tipo_utilizador"].ToString().Trim();
            parceiro = oDs.Tables[0].Rows[0]["parceiro"].ToString().Trim();
            suspenso = Convert.ToBoolean(oDs.Tables[0].Rows[0]["suspenso"]);
            pin = oDs.Tables[0].Rows[0]["pin"].ToString().Trim();
            nmaquinas = oDs.Tables[0].Rows[0]["nmaquinas"].ToString().Trim();
            nif = oDs.Tables[0].Rows[0]["nif"].ToString().Trim();
            morada = oDs.Tables[0].Rows[0]["morada"].ToString().Trim();
            nomeestabelecimento = oDs.Tables[0].Rows[0]["nomeestabelecimento"].ToString().Trim();
            pessoaresponsavel = oDs.Tables[0].Rows[0]["pessoaresponsavel"].ToString().Trim();
            cashdronome = oDs.Tables[0].Rows[0]["cashdronome"].ToString().Trim();
            nr_utilizadores_parceiro = oDs.Tables[0].Rows[0]["nr_clientes_parceiro"].ToString().Trim();
            mostra_lig_direta = Convert.ToBoolean(oDs.Tables[0].Rows[0]["mostra_ligacao_direta_maquina"]);

            s_ativo = ativo ? "true" : "false";
            s_suspenso = suspenso ? "true" : "false";
            s_ligdireta = mostra_lig_direta ? "true" : "false";
        }

        // Prepara o retorno dos dados
        ret = tipo + "<#SEP#>" +
              nome + "<#SEP#>" +
              email + "<#SEP#>" +
              password + "<#SEP#>" +
              telemovel + "<#SEP#>" +
              notas + "<#SEP#>" +
              s_ativo + "<#SEP#>" +
              id_tipo_utilizador + "<#SEP#>" +
              parceiro + "<#SEP#>" +
              s_suspenso + "<#SEP#>" +
              pin + "<#SEP#>" +
              nmaquinas + "<#SEP#>" +
              nif + "<#SEP#>" +
              morada + "<#SEP#>" +
              nomeestabelecimento + "<#SEP#>" +
              pessoaresponsavel + "<#SEP#>" +
              cashdronome + "<#SEP#>" +
              s_ligdireta + "<#SEP#>" +
              nr_utilizadores_parceiro;

        return ret;
    }

    private void getTipoUtilizadores()
    {
        string sql = "", html = "";
        string id = "", nome = "";

        DataSqlServer oDB = new DataSqlServer();



        html += @" <label class='form-control-label' for='input-username'>Tipo de Utilizador</label>
                   <select id='ddlTipo' onchange='verifyUserType();' disabled class='form-control form-control-alternative'>";


        sql = @"declare @id int;
                SELECT
                    id,
		            nome,
                    administrador
                FROM CASHDRO_REPORT_TIPO_UTILIZADORES(@id)
                order by administrador desc, nome asc ";


        DataSet oDs = oDB.GetDataSet(sql, "").oData;
        if (oDB.validaDataSet(oDs))
        {
            for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
            {
                id = oDs.Tables[0].Rows[i]["id"].ToString().Trim();
                nome = oDs.Tables[0].Rows[i]["nome"].ToString().Trim();


                html += @"<option value='" + id + @"'>" + nome + @"</option>";
            }
        }
        else
        {
            html += @"<option value='0'>Não existem tipos de utilizador a apresentar</option>";
        }


        html += "</select>";


        divTipoUtilizador.InnerHtml = html;
    }

    [WebMethod]
    public static string getPartnerList()
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
                                where tipo in ('Parceiro', 'Administrador')
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

                ret += oDs.Tables[0].Rows[i]["nome"].ToString().Trim();
            }
        }

        return ret;
    }
}