<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_ficha_maquina.aspx.cs" Inherits="config_ficha_maquina" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Ficha da Máquina">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Ficha da Máquina</title>
    <!-- Favicon -->
    <link href="../general/assets/img/brand/favicon.png" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="../general/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="../general/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="../general/assets/css/argon.css?v=1.0.0" rel="stylesheet">
    <link href="../vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
    <link type="text/css" href="../alertify/css/alertify.min.css" rel="stylesheet" />
    <link type="text/css" href="../alertify/css/themes/default.min.css" rel="stylesheet" />

    <style>
        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95, #004D95 100%) !important;
        }


        #divLoading {
            border: solid 3px gray;
            z-index: 999999999999999999999999;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
            background-color: white;
            height: 350px;
            width: 61%;
        }

        #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 2; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }


        .col-xl-8 {
            max-width: 99%;
            flex: 0 0 99%;
        }

        .pointer {
            cursor: pointer;
        }

        .autocomplete-items {
            position: absolute;
            border: 1px solid #d4d4d4;
            border-bottom: none;
            border-top: none;
            z-index: 99;
            /*position the autocomplete items to be the same width as the container:*/
            top: 100%;
            left: 0;
            right: 0;
        }

            .autocomplete-items div {
                padding: 10px;
                cursor: pointer;
                background-color: #fff;
                border-bottom: 1px solid #d4d4d4;
            }

                .autocomplete-items div:hover {
                    /*when hovering an item:*/
                    background-color: #e9e9e9;
                }

        .autocomplete-active {
            /*when navigating through the items using the arrow keys:*/
            background-color: DodgerBlue !important;
            color: #ffffff;
        }
    </style>
</head>

<body>
    <span style="display: none;" id="autocompletelist" runat="server"></span>
    <!-- Main content -->
    <div class="main-content">

        <div id="overlay"></div>
        <div id="divLoading" class="variaveis">
            <table style="width: 100%; height: 100%; text-align: center; vertical-align: middle">
                <tr>
                    <td style="vertical-align: bottom">
                        <img src="../general/assets/img/theme/preloader.gif" />
                    </td>
                </tr>
                <tr>
                    <td style="font-size: 17px; vertical-align: top; font-weight: bold"><span id="spanLoading">A reiniciar serviço, por favor aguarde...</span></td>
                </tr>
            </table>
        </div>

        <!-- Top navbar -->
        <nav class="navbar navbar-top navbar-expand-md navbar-dark" id="navbar-main">
            <div class="container-fluid">
                <!-- Brand -->
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block" href="../index.html">Parametrização</a>


            </div>
        </nav>
        <!-- Header -->
        <div class="header pb-8 pt-5 pt-lg-8 d-flex align-items-center" style="min-height: 200px; background-size: cover; background-position: center top;" id="divInfo">
            <!-- Mask -->
            <span class="mask bg-gradient-default opacity-8"></span>
            <!-- Header container -->
            <div class="container-fluid d-flex align-items-center">
                <div class="row">
                    <div class="col-lg-12 col-md-10">
                        <h1 class="display-2 text-white">Máquina Cashdro</h1>
                        <p class="text-white mt-0 mb-5">Crie/edite máquinas Cashdro</p>
                        <a href="#!" class="btn btn-info" onclick="saveData();">Guardar alterações</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Page content -->
        <div class="container-fluid mt--7">
            <div class="row">

                <div class="col-xl-8 order-xl-1">
                    <div class="card bg-secondary shadow">
                        <div class="card-header bg-white border-0">
                            <div class="row align-items-center">
                                <table style="width: 100%; margin-left: 15px">
                                    <tr>
                                        <td style="width: 75%">
                                            <h3 class="mb-0">Dados da Máquina</h3>
                                        </td>
                                        <td style="width: 25%; text-align: right">
                                            <a class="btn btn-sm btn-primary" onclick="obterDados();" style="color: #FFFFFF;">
                                                <span id="spanBtnAlteraStatus">Obter Dados</span>
                                            </a>
                                            <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px; cursor: pointer; margin-left: 10px;' alt='Back' title='Back' onclick='retroceder();'/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="card-body" id="divGrelha">
                            <form>
                                <h6 class="heading-small text-muted mb-4 pointer" onclick="showHideCashdro();">Cashdro</h6>

                                <div class="row" id="cashdroSerialRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Serial Number Cashdro</label>
                                            <input type="text" id="txtSerialNumber" class="form-control form-control-alternative" placeholder="Serial Cashdro">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroFirmwareRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Versão de Firmware</label>
                                            <input type="text" id="txtFirmwareVersion" class="form-control form-control-alternative" placeholder="Versão de Firmware da Máquina">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroModelRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Modelo</label>
                                            <input type="text" id="txtModel" class="form-control form-control-alternative" placeholder="Modelo da Máquina">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroCustomerRow">
                                    <div class="col-md-12">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Cliente</label>
                                            <%--<input type="text" id="txtCustomer" class="form-control form-control-alternative" placeholder="Cliente">--%>
                                            <form runat="server">
                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control form-control-alternative" />
                                            </form>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroDiasIntervaloManutencao">
                                    <div class="col-md-12">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Dias de Intervalo entre Manutenções programadas</label>
                                            <input type="number" id="txtDiasIntervaloManutencao" class="form-control form-control-alternative" placeholder="Nº de Dias de Intervalo entre Manutenções Programadas">
                                        </div>
                                    </div>
                                </div>

                                
                                <div class="row" id="cashdroHoraEmail">
                                    <div class="col-md-1">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Hora de envio de email</label>
                                            <input type="time" id="txtHoraEmail" class="form-control form-control-alternative">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroObservationsRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Localização</label>
                                            <textarea id="txtLocalizacao" rows="3" style="resize: none" class="form-control form-control-alternative" placeholder="Observações diversas"></textarea>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroIPRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">CashDro IP</label>
                                            <input type="text" id="txtIP" class="form-control form-control-alternative" placeholder="URL CashDro">
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="cashdroUserRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Username</label>
                                            <input type="text" id="txtUserName" class="form-control form-control-alternative" placeholder="Username de entrada">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroPassRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Password</label>
                                            <input type="password" id="txtPassword" class="form-control form-control-alternative" placeholder="Password de entrada">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroUserSupportRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Username de Suporte</label>
                                            <input type="text" id="txtUserNameSupport" class="form-control form-control-alternative" placeholder="Username de Suporte">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="cashdroPassSupportRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Password de Suporte</label>
                                            <input type="password" id="txtPasswordSupport" class="form-control form-control-alternative" placeholder="Password de Suporte">
                                        </div>
                                    </div>
                                </div>

                                <h6 class="heading-small text-muted mb-4 pointer" onclick="showHideCoinValidator();">Validador de Moedas</h6>

                                <div class="row" id="coinValidatorNameRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Nome Validador de Moedas</label>
                                            <input type="text" id="txtNameCoinValidator" class="form-control form-control-alternative" placeholder="Nome do Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinValidatorSerialRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Serial Validador de Moedas</label>
                                            <input type="text" id="txtSerialCoinValidator" class="form-control form-control-alternative" placeholder="Serial Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinValidatorSoftwareRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Versão de Software Validador de Moedas</label>
                                            <input type="text" id="txtSoftwareVersionCoinValidator" class="form-control form-control-alternative" placeholder="Versão de Software do Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinValidatorPropertiesRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Propriedades Validador de Moedas</label>
                                            <input type="text" id="txtPropertiesCoinValidator" class="form-control form-control-alternative" placeholder="Propriedades do Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinValidatorInstallDateRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Data de Instalação Validador de Moedas</label>
                                            <input type="text" id="txtInstallDateCoinValidator" class="form-control form-control-alternative" placeholder="Data de Instalação do Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinValidatorPortRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Porta Validador de Moedas</label>
                                            <input type="text" id="txtPortNameCoinValidator" class="form-control form-control-alternative" placeholder="Porta do Validador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <h6 class="heading-small text-muted mb-4 pointer" onclick="showHideCoinDispenser();">Dispensador de Moedas</h6>

                                <div class="row" id="coinDispenserNameRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Nome Dispensador de Moedas</label>
                                            <input type="text" id="txtNameCoinDispenser" class="form-control form-control-alternative" placeholder="Nome do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserSerialRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Serial Validador de Moedas</label>
                                            <input type="text" id="txtSerialCoinDispenser" class="form-control form-control-alternative" placeholder="Serial Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserFirmwareRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Versão de Firmware Dispensador de Moedas</label>
                                            <input type="text" id="txtFirmwareCoinDispenser" class="form-control form-control-alternative" placeholder="Versão de Firmware do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserDatasetRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Versão do Dataset Dispensador de Moedas</label>
                                            <input type="text" id="txtDatasetCoinDispenser" class="form-control form-control-alternative" placeholder="Versão do Dataset do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserPropertiesRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Propriedades Dispensador de Moedas</label>
                                            <input type="text" id="txtPropertiesCoinDispenser" class="form-control form-control-alternative" placeholder="Propriedades do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserInstallDateRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Data de Instalação Validador de Moedas</label>
                                            <input type="text" id="txtInstallDateCoinDispenser" class="form-control form-control-alternative" placeholder="Data de Instalação do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="coinDispenserPortRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Porta Validador de Moedas</label>
                                            <input type="text" id="txtPortNameCoinDispenser" class="form-control form-control-alternative" placeholder="Porta do Dispensador de Moedas">
                                        </div>
                                    </div>
                                </div>

                                <h6 class="heading-small text-muted mb-4 pointer" onclick="showHideBillDispenser();">Dispensador de Notas</h6>

                                <div class="row" id="billDispenserNameRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Nome Dispensador de Notas</label>
                                            <input type="text" id="txtNameBillDispenser" class="form-control form-control-alternative" placeholder="Nome do Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="billDispenserSerialRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Serial Dispensador de Notas</label>
                                            <input type="text" id="txtSerialBillDispenser" class="form-control form-control-alternative" placeholder="Serial Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="billDispenserFirmwareRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Versão de Firmware Dispensador de Notas</label>
                                            <input type="text" id="txtFirmwareBillDispenser" class="form-control form-control-alternative" placeholder="Versão de Firmware do Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="billDispenserPropertiesRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Propriedades Dispensador de Notas</label>
                                            <input type="text" id="txtPropertiesBillDispenser" class="form-control form-control-alternative" placeholder="Propriedades do Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="billDispenserInstallDateRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Data de Instalação Validador de Notas</label>
                                            <input type="text" id="txtInstallDateBillDispenser" class="form-control form-control-alternative" placeholder="Data de Instalação do Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="billDispenserPortRow">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Porta Validador de Notas</label>
                                            <input type="text" id="txtPortNameBillDispenser" class="form-control form-control-alternative" placeholder="Porta do Dispensador de Notas">
                                        </div>
                                    </div>
                                </div>

                                <hr class="my-4" />
                                <!-- Description -->
                                <h6 class="heading-small text-muted mb-4">Propriedades</h6>

                                <div class="row" style="padding-left: 0px" id="contratoRow">
                                    <div class="col-md-6" id="temContratoRow" style="margin: auto;">
                                        <div class="custom-control custom-control-alternative custom-checkbox form-group" id="temContratoSubRow">
                                            <input class="custom-control-input" id="chkTemContrato" type="checkbox">
                                            <label class="custom-control-label" for="chkTemContrato">
                                                <span class="text-muted">Tem Contrato</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="contratoParceiroRow">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Contrato Parceiro</label>
                                            <input type="text" id="txtContratoParceiro" class="form-control form-control-alternative" placeholder="Contrato Parceiro">
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="padding-left: 0px; margin-top: 10px;">
                                    <div class="col-md-6">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkAtivo" type="checkbox">
                                            <label class="custom-control-label" for="chkAtivo">
                                                <span class="text-muted">Máquina ativa</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkMostraLigDireta" type="checkbox">
                                            <label class="custom-control-label" for="chkMostraLigDireta">
                                                <span class="text-muted">Mostrar Ligação Direta à Máquina</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <footer class="footer">
                <div class="row align-items-center justify-content-xl-between">
                    <div class="col-xl-6">
                        <div class="copyright text-center text-xl-left text-muted">
                            <%--&copy; 2019, Plataforma desenvolvida por <a href="http://www.mbsolutions.pt" class="font-weight-bold ml-1" target="_blank">MBSolutions</a>--%>
                        </div>
                    </div>
                </div>
            </footer>

            <div id="hiddenVals" class="variaveis">
                <input id="txtAux" runat="server" type="text" class="variaveis" />
            </div>
        </div>
    </div>

    <!-- Argon Scripts -->
    <!-- Core -->
    <script src="../general/assets/vendor/jquery/dist/jquery.min.js"></script>
    <script src="../general/assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Argon JS -->
    <script src="../general/assets/js/argon.js?v=1.0.0"></script>
    <script src="../vendors/sweetalert2/sweetalert2.min.js"></script>
    <script src="../alertify/alertify.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>
    

    <script>
        $(document).ready(function () {
            var id = localStorage.loga;

            var texto = $('#autocompletelist').text();
            var dados = texto.split('<#SEP#>');
            autocomplete(document.getElementById("txtCustomer"), dados);

            getData();
            fadeInOutDivs(id);
            setAltura();
            defineTablesMaxHeight();
        });

        $(window).resize(function () {
            setAltura();
            defineTablesMaxHeight();
        });

        function setAltura() {
            $("#fraContent").height($(window).height());
        }

        function defineTablesMaxHeight() {
            var windowHeight = $(window).height();
            var divInfoHeight = $('#divInfo').height();
            var navbarHeight = $('#navbar-main').height();
            var maxHeight = windowHeight - divInfoHeight - navbarHeight - 200;

            $('#divGrelha').css({ "maxHeight": maxHeight + "px" });
        }

        function getData() {
            var id = $('#txtAux').val();
            if (id != null && id != 'null') {
                $.ajax({
                    type: "POST",
                    url: "config_ficha_maquina.aspx/getData",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');


                        // Prepara o retorno dos dados
                        var serialnumber = dados[0];
                        var localizacao = dados[1];
                        var url = dados[2];
                        var username = dados[3];
                        var password = dados[4];
                        var s_ativo = dados[5];
                        var serialnumber_pelicano = dados[6];
                        var serialnumber_hooper = dados[7];
                        var serialnumber_cashmodule = dados[8];
                        var firmware_version = dados[9];
                        var password_support = dados[10];
                        var user_support = dados[11];
                        var model = dados[12];
                        var nameCoinValidator = dados[13];
                        var softwareVersionCoinValidator = dados[14];
                        var propertiesCoinValidator = dados[15];
                        var installDateCoinValidator = dados[16];
                        var portNameCoinValidator = dados[17];
                        var nameCoinDispenser = dados[18];
                        var firmwareCoinDispenser = dados[19];
                        var datasetCoinDispenser = dados[20];
                        var propertiesCoinDispenser = dados[21];
                        var installDateCoinDispenser = dados[22];
                        var portNameCoinDispenser = dados[23];
                        var nameBillDispenser = dados[24];
                        var firmwareBillDispenser = dados[25];
                        var propertiesBillDispenser = dados[26];
                        var installDateBillDispenser = dados[27];
                        var portNameBillDispenser = dados[28];
                        var customer = dados[29];
                        var dias_intervalo_manutencao = dados[30];
                        var horaemail = dados[31];
                        var mostraLigDireta = dados[32];
                        var temcontrato = dados[33];
                        var contratoparceiro = dados[34];

                        $('#txtSerialNumber').val(serialnumber);
                        $('#txtLocalizacao').val(localizacao);
                        $('#txtIP').val(url);
                        $('#txtUserName').val(username);
                        $('#txtPassword').val(password);
                        $('#txtSerialCoinValidator').val(serialnumber_pelicano);
                        $('#txtSerialCoinDispenser').val(serialnumber_hooper);
                        $('#txtSerialBillDispenser').val(serialnumber_cashmodule);
                        $('#txtFirmwareVersion').val(firmware_version);
                        $('#txtPasswordSupport').val(password_support);
                        $('#txtUserNameSupport').val(user_support);
                        $('#txtModel').val(model);
                        $('#txtDiasIntervaloManutencao').val(dias_intervalo_manutencao);

                        $('#txtNameCoinValidator').val(nameCoinValidator);
                        $('#txtSoftwareVersionCoinValidator').val(softwareVersionCoinValidator);
                        $('#txtPropertiesCoinValidator').val(propertiesCoinValidator);
                        $('#txtInstallDateCoinValidator').val(installDateCoinValidator);
                        $('#txtPortNameCoinValidator').val(portNameCoinValidator);

                        $('#txtNameCoinDispenser').val(nameCoinDispenser);
                        $('#txtFirmwareCoinDispenser').val(firmwareCoinDispenser);
                        $('#txtDatasetCoinDispenser').val(datasetCoinDispenser);
                        $('#txtPropertiesCoinDispenser').val(propertiesCoinDispenser);
                        $('#txtInstallDateCoinDispenser').val(installDateCoinDispenser);
                        $('#txtPortNameCoinDispenser').val(portNameCoinDispenser);

                        $('#txtNameBillDispenser').val(nameBillDispenser);
                        $('#txtFirmwareBillDispenser').val(firmwareBillDispenser);
                        $('#txtPropertiesBillDispenser').val(propertiesBillDispenser);
                        $('#txtInstallDateBillDispenser').val(installDateBillDispenser);
                        $('#txtPortNameBillDispenser').val(portNameBillDispenser);
                        $('#txtHoraEmail').val(horaemail);
                        $('#txtContratoParceiro').val(contratoparceiro);

                        if (s_ativo == "false")
                            $('#chkAtivo').attr('checked', false);
                        else
                            $('#chkAtivo').attr('checked', true);

                        if (mostraLigDireta == "false")
                            $('#chkMostraLigDireta').attr('checked', false);
                        else
                            $('#chkMostraLigDireta').attr('checked', true);

                        if (temcontrato == "false")
                            $('#chkTemContrato').attr('checked', false);
                        else
                            $('#chkTemContrato').attr('checked', true);

                        getCustomersList();
                        $('#txtCustomer').val(customer);
                    }
                });
            }
            else {
                $('#chkAtivo').attr('checked', true);
                $('#chkMostraLigDireta').attr('checked', true);
                getCustomersList();
            }
        }

        function fadeInOutDivs(id) {
            $.ajax({
                type: "POST",
                url: "config_ficha_maquina.aspx/getUserType",
                data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var mostra = res.d;

                    switch (mostra) {
                        case '1':
                            $('#cashdroDiasIntervaloManutencao').fadeIn();
                            $('#contratoRow').fadeOut();
                            break;
                        case '-1':
                            $('#cashdroDiasIntervaloManutencao').fadeIn();
                            $('#contratoRow').fadeIn();
                            break;
                        case '0':
                            $('#cashdroDiasIntervaloManutencao').fadeOut();
                            $('#contratoRow').fadeOut();
                            break;
                        default:
                            $('#cashdroDiasIntervaloManutencao').fadeOut();
                            $('#contratoRow').fadeOut();
                            break;
                    }
                }
            });
        }

        function getCustomersList() {
            $.ajax({
                type: "POST",
                url: "config_ficha_maquina.aspx/getCustomerList",
                //data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');

                    autocomplete(document.getElementById("txtCustomer"), dados);
                }
            });
        }

        function saveData() {
            var id = $('#txtAux').val();

            var serial = $('#txtSerialNumber').val();
            var localizacao = $('#txtLocalizacao').val();
            var ip = $('#txtIP').val();
            var user = $('#txtUserName').val();
            var password = $('#txtPassword').val();
            var password_support = $('#txtPasswordSupport').val();
            var user_support = $('#txtUserNameSupport').val();
            var firmware = $('#txtFirmwareVersion').val();
            var model = $('#txtModel').val();
            var customer = $('#txtCustomer').val();
            var diasIntervaloManutencao = $('#txtDiasIntervaloManutencao').val();

            var serialPelicano = $('#txtSerialCoinValidator').val();
            var namePelicano = $('#txtNameCoinValidator').val();
            var softwareVersionPelicano = $('#txtSoftwareVersionCoinValidator').val();
            var propertiesPelicano = $('#txtPropertiesCoinValidator').val();
            var installDatePelicano = $('#txtInstallDateCoinValidator').val();
            var portNamePelicano = $('#txtPortNameCoinValidator').val();

            var serialHooper = $('#txtSerialCoinDispenser').val();
            var nameHooper = $('#txtNameCoinDispenser').val();
            var firmwareHooper = $('#txtFirmwareCoinDispenser').val();
            var datasetHooper = $('#txtDatasetCoinDispenser').val();
            var propertiesHooper = $('#txtPropertiesCoinDispenser').val();
            var installDateHooper = $('#txtInstallDateCoinDispenser').val();
            var portNameHooper = $('#txtPortNameCoinDispenser').val();

            var serialCashModule = $('#txtSerialBillDispenser').val();
            var nameCashModule = $('#txtNameBillDispenser').val();
            var firmwareCashModule = $('#txtFirmwareBillDispenser').val();
            var propertiesCashModule = $('#txtPropertiesBillDispenser').val();
            var installDateCashModule = $('#txtInstallDateBillDispenser').val();
            var portNameCashModule = $('#txtPortNameBillDispenser').val();
            var horaEmail = $('#txtHoraEmail').val();
            var contratoParceiro = $('#txtContratoParceiro').val();

            var ativo = $('#chkAtivo').is(":checked") ? 1 : 0;
            var ligDireta = $('#chkMostraLigDireta').is(":checked") ? 1 : 0;
            var temcontrato = $('#chkTemContrato').is(":checked") ? 1 : 0;

            if (diasIntervaloManutencao == '' || diasIntervaloManutencao == null || diasIntervaloManutencao == undefined) {
                diasIntervaloManutencao = '30';
            }

            if (serial == '' || serial == null || serial == undefined) {
                sweetAlertWarning('Serial', 'Por favor indique o serial da máquina');
                return;
            }
            else if (localizacao == '' || localizacao == null || localizacao == undefined) {
                sweetAlertWarning('Localização', 'Por favor indique a localização da máquina');
                return;
            }
            else if (ip == '' || ip == null || ip == undefined) {
                sweetAlertWarning('IP', 'Por favor indique o IP da máquina');
                return;
            }
            else if (user == '' || user == null || user == undefined) {
                sweetAlertWarning('User', 'Por favor indique o User da máquina');
                return;
            }
            else if (password == '' || password == null || password == undefined) {
                sweetAlertWarning('Password', 'Por favor indique a Password da máquina');
                return;
            }
            else if (password_support == '' || password_support == null || password_support == undefined) {
                sweetAlertWarning('Password Suporte', 'Por favor indique a Password Suporte da máquina');
                return;
            }
            else if (user_support == '' || user_support == null || user_support == undefined) {
                sweetAlertWarning('User Suporte', 'Por favor indique o User Suporte da máquina');
                return;
            }

            $.ajax({
                type: "POST",
                url: "config_ficha_maquina.aspx/saveData",
                data: '{"id":"' + id + '","serial":"' + serial + '","localizacao":"' + localizacao + '","ip":"' + ip + '","user":"' + user + '","password":"' + password + '","ativo":"' + ativo
                    + '","serialPelicano":"' + serialPelicano + '","serialHooper":"' + serialHooper + '","serialCashModule":"' + serialCashModule + '","firmwareVersion":"' + firmware
                    + '","password_support":"' + password_support + '","user_support":"' + user_support + '","model":"' + model + '","namePelicano":"' + namePelicano
                    + '","softwareVersionPelicano":"' + softwareVersionPelicano + '","propertiesPelicano":"' + propertiesPelicano + '","installDatePelicano":"' + installDatePelicano
                    + '","portNamePelicano":"' + portNamePelicano + '","nameHooper":"' + nameHooper + '","firmwareHooper":"' + firmwareHooper + '","datasetHooper":"' + datasetHooper
                    + '","propertiesHooper":"' + propertiesHooper + '","installDateHooper":"' + installDateHooper + '","portNameHooper":"' + portNameHooper + '","nameCashModule":"' + nameCashModule
                    + '","firmwareCashModule":"' + firmwareCashModule + '","propertiesCashModule":"' + propertiesCashModule + '","installDateCashModule":"' + installDateCashModule
                    + '","portNameCashModule":"' + portNameCashModule + '","customer":"' + customer + '","dias_intervalo_manutencao":"' + diasIntervaloManutencao + '","horaEmail":"' + horaEmail
                    + '","mostraligdireta":"' + ligDireta + '","temcontrato":"' + temcontrato + '","contratoParceiro":"' + contratoParceiro + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    loadUrl("config_lista_maquinas.aspx");
                }
            });
        }


        function loadUrl(url) {
            window.location = url;
        }

        function retroceder() {
            alertify.confirm("Tem a certeza que pretende sair?!<br />Todas os dados serão perdidos.",
                function () {
                    loadUrl("config_lista_maquinas.aspx");
                },
                function () {

                }).set('labels', { ok: 'Sim', cancel: 'Não' }).set('title', 'Máquina Cashdro');
        }

        function obterDados() {
            var ip = $('#txtIP').val();
            var pass = $('#txtPassword').val();
            var passSupport = $('#txtPasswordSupport').val();
            var userSupport = $('#txtUserNameSupport').val();

            if (ip == null || ip == '') {
                sweetAlertError("Dados da Máquina", "O URL da máquina tem de estar preenchido para conseguir pedir os dados referentes à mesma!");
                return;
            }

            if (pass == null || pass == '') {
                sweetAlertError("Dados da Máquina", "A password de acesso à máquina tem de estar preenchida para conseguir pedir os dados referentes à mesma!");
                return;
            }

            if (passSupport == null || passSupport == '') {
                sweetAlertError("Dados da Máquina", "A password de suporte à máquina tem de estar preenchida para conseguir pedir os dados referentes à mesma!");
                return;
            }

            if (userSupport == null || userSupport == '') {
                sweetAlertError("Dados da Máquina", "O username de suporte à máquina tem de estar preenchido para conseguir pedir os dados referentes à mesma!");
                return;
            }

            loadingOn('A obter informação. Aguarde por favor...');

            // Obtemos os dados para saber se as maquinas estao ligadas ou desligadas
            $.ajax({
                type: "POST",
                url: "config_ficha_maquina.aspx/getMachineData",
                data: '{"url":"' + ip + '","pass":"' + pass + '","passSupport":"' + passSupport + '","userSupport":"' + userSupport + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var ret = res.d.split('<#SEP#>');

                    var propertiesPelicano = ret[0];
                    var installDatePelicano = ret[1];
                    var portNamePelicano = ret[2];
                    var namePelicano = ret[3];
                    var softwareVersionPelicano = ret[4];
                    var serialPelicano = ret[5];
                    var propertiesHooper = ret[6];
                    var installDateHooper = ret[7];
                    var portNameHooper = ret[8];
                    var nameHooper = ret[9];
                    var firmwareHooper = ret[10];
                    var serialHooper = ret[11];
                    var datasetVersionHooper = ret[12];
                    var propertiesCashModule = ret[13];
                    var installDateCashModule = ret[14];
                    var portNameCashModule = ret[15];
                    var nameCashModule = ret[16];
                    var firmwareCashModule = ret[17];
                    var serialCashModule = ret[18];
                    var serial = ret[19];
                    var firmwareVersion = ret[20];
                    var model = ret[21];

                    $('#txtSerialNumber').val(serial);
                    $('#txtFirmwareVersion').val(firmwareVersion);
                    $('#txtModel').val(model);
                    $('#txtNameCoinValidator').val(namePelicano);
                    $('#txtSerialCoinValidator').val(serialPelicano);
                    $('#txtSoftwareVersionCoinValidator').val(softwareVersionPelicano);
                    $('#txtPropertiesCoinValidator').val(propertiesPelicano);
                    $('#txtInstallDateCoinValidator').val(installDatePelicano);
                    $('#txtPortNameCoinValidator').val(portNamePelicano);
                    $('#txtNameCoinDispenser').val(nameHooper);
                    $('#txtSerialCoinDispenser').val(serialHooper);
                    $('#txtFirmwareCoinDispenser').val(firmwareHooper);
                    $('#txtDatasetCoinDispenser').val(datasetVersionHooper);
                    $('#txtPropertiesCoinDispenser').val(propertiesHooper);
                    $('#txtInstallDateCoinDispenser').val(installDateHooper);
                    $('#txtPortNameCoinDispenser').val(portNameHooper);
                    $('#txtNameBillDispenser').val(nameCashModule);
                    $('#txtSerialBillDispenser').val(serialCashModule);
                    $('#txtFirmwareBillDispenser').val(firmwareCashModule);
                    $('#txtPropertiesBillDispenser').val(propertiesCashModule);
                    $('#txtInstallDateBillDispenser').val(installDateCashModule);
                    $('#txtPortNameBillDispenser').val(portNameCashModule);

                    loadingOff();
                }
            });
        }

        function loadingOn(msg) {
            overlayOn();
            $('#spanLoading').html(msg);
            $('#divLoading').show();
        }

        function loadingOff() {
            overlayOff();
            $('#divLoading').hide();
        }

        function overlayOn() {
            overlayOff();
            document.getElementById("overlay").style.display = "block";
        }

        function overlayOff() {
            document.getElementById("overlay").style.display = "none";
        }

        function sweetAlertBasic(msg) {
            swal(msg);
        }

        function sweetAlertError(subject, msg) {
            swal(
                subject,
                msg,
                'error'
            )
        }

        function sweetAlertInfo(subject, msg) {
            swal(
                subject,
                msg,
                'info'
            )
        }

        function sweetAlertWarning(subject, msg) {
            swal(
                subject,
                msg,
                'warning'
            )
        }

        function sweetAlertSuccess(subject, msg) {
            swal(
                subject,
                msg,
                'success'
            )
        }

        function sweetAlertQuestion(subject, msg) {
            swal(
                subject,
                msg,
                'question'
            )
        }

        function showHideCashdro() {
            if ($('#cashdroSerialRow').is(":visible")) {
                $('#cashdroSerialRow').fadeOut();
                $('#cashdroFirmwareRow').fadeOut();
                $('#cashdroModelRow').fadeOut();
                $('#cashdroObservationsRow').fadeOut();
                $('#cashdroIPRow').fadeOut();
                $('#cashdroUserRow').fadeOut();
                $('#cashdroPassRow').fadeOut();
                $('#cashdroUserSupportRow').fadeOut();
                $('#cashdroPassSupportRow').fadeOut();
            }
            else {
                $('#cashdroSerialRow').fadeIn();
                $('#cashdroFirmwareRow').fadeIn();
                $('#cashdroModelRow').fadeIn();
                $('#cashdroObservationsRow').fadeIn();
                $('#cashdroIPRow').fadeIn();
                $('#cashdroUserRow').fadeIn();
                $('#cashdroPassRow').fadeIn();
                $('#cashdroUserSupportRow').fadeIn();
                $('#cashdroPassSupportRow').fadeIn();
            }
        }

        function showHideCoinValidator() {
            if ($('#coinValidatorNameRow').is(":visible")) {
                $('#coinValidatorNameRow').fadeOut();
                $('#coinValidatorSerialRow').fadeOut();
                $('#coinValidatorSoftwareRow').fadeOut();
                $('#coinValidatorPropertiesRow').fadeOut();
                $('#coinValidatorInstallDateRow').fadeOut();
                $('#coinValidatorPortRow').fadeOut();
            }
            else {
                $('#coinValidatorNameRow').fadeIn();
                $('#coinValidatorSerialRow').fadeIn();
                $('#coinValidatorSoftwareRow').fadeIn();
                $('#coinValidatorPropertiesRow').fadeIn();
                $('#coinValidatorInstallDateRow').fadeIn();
                $('#coinValidatorPortRow').fadeIn();
            }
        }

        function showHideCoinDispenser() {
            if ($('#coinDispenserNameRow').is(":visible")) {
                $('#coinDispenserNameRow').fadeOut();
                $('#coinDispenserSerialRow').fadeOut();
                $('#coinDispenserFirmwareRow').fadeOut();
                $('#coinDispenserDatasetRow').fadeOut();
                $('#coinDispenserPropertiesRow').fadeOut();
                $('#coinDispenserInstallDateRow').fadeOut();
                $('#coinDispenserPortRow').fadeOut();
            }
            else {
                $('#coinDispenserNameRow').fadeIn();
                $('#coinDispenserSerialRow').fadeIn();
                $('#coinDispenserFirmwareRow').fadeIn();
                $('#coinDispenserDatasetRow').fadeIn();
                $('#coinDispenserPropertiesRow').fadeIn();
                $('#coinDispenserInstallDateRow').fadeIn();
                $('#coinDispenserPortRow').fadeIn();
            }
        }

        function showHideBillDispenser() {
            if ($('#billDispenserNameRow').is(":visible")) {
                $('#billDispenserNameRow').fadeOut();
                $('#billDispenserSerialRow').fadeOut();
                $('#billDispenserFirmwareRow').fadeOut();
                $('#billDispenserPropertiesRow').fadeOut();
                $('#billDispenserInstallDateRow').fadeOut();
                $('#billDispenserPortRow').fadeOut();
            }
            else {
                $('#billDispenserNameRow').fadeIn();
                $('#billDispenserSerialRow').fadeIn();
                $('#billDispenserFirmwareRow').fadeIn();
                $('#billDispenserPropertiesRow').fadeIn();
                $('#billDispenserInstallDateRow').fadeIn();
                $('#billDispenserPortRow').fadeIn();
            }
        }

        function autocomplete(inp, arr) {
            /*the autocomplete function takes two arguments,
            the text field element and an array of possible autocompleted values:*/
            var currentFocus;
            /*execute a function when someone writes in the text field:*/
            inp.addEventListener("input", function (e) {
                var a, b, i, val = this.value;
                /*close any already open lists of autocompleted values*/
                closeAllLists();
                if (!val) { return false; }
                currentFocus = -1;
                /*create a DIV element that will contain the items (values):*/
                a = document.createElement("DIV");
                a.setAttribute("id", this.id + "autocomplete-list");
                a.setAttribute("class", "autocomplete-items");
                /*append the DIV element as a child of the autocomplete container:*/
                this.parentNode.appendChild(a);
                /*for each item in the array...*/
                for (i = 0; i < arr.length; i++) {
                    /*check if the item starts with the same letters as the text field value:*/
                    if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                        /*create a DIV element for each matching element:*/
                        b = document.createElement("DIV");
                        /*make the matching letters bold:*/
                        b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                        b.innerHTML += arr[i].substr(val.length);
                        /*insert a input field that will hold the current array item's value:*/
                        b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                        /*execute a function when someone clicks on the item value (DIV element):*/
                        b.addEventListener("click", function (e) {
                            /*insert the value for the autocomplete text field:*/
                            inp.value = this.getElementsByTagName("input")[0].value;
                            /*close the list of autocompleted values,
                            (or any other open lists of autocompleted values:*/
                            closeAllLists();
                        });
                        a.appendChild(b);
                    }
                }
            });
            /*execute a function presses a key on the keyboard:*/
            inp.addEventListener("keydown", function (e) {
                var x = document.getElementById(this.id + "autocomplete-list");
                if (x) x = x.getElementsByTagName("div");
                if (e.keyCode == 40) {
                    /*If the arrow DOWN key is pressed,
                    increase the currentFocus variable:*/
                    currentFocus++;
                    /*and and make the current item more visible:*/
                    addActive(x);
                } else if (e.keyCode == 38) { //up
                    /*If the arrow UP key is pressed,
                    decrease the currentFocus variable:*/
                    currentFocus--;
                    /*and and make the current item more visible:*/
                    addActive(x);
                } else if (e.keyCode == 13) {
                    /*If the ENTER key is pressed, prevent the form from being submitted,*/
                    e.preventDefault();
                    if (currentFocus > -1) {
                        /*and simulate a click on the "active" item:*/
                        if (x) x[currentFocus].click();
                    }
                }
            });
            function addActive(x) {
                /*a function to classify an item as "active":*/
                if (!x) return false;
                /*start by removing the "active" class on all items:*/
                removeActive(x);
                if (currentFocus >= x.length) currentFocus = 0;
                if (currentFocus < 0) currentFocus = (x.length - 1);
                /*add class "autocomplete-active":*/
                x[currentFocus].classList.add("autocomplete-active");
            }
            function removeActive(x) {
                /*a function to remove the "active" class from all autocomplete items:*/
                for (var i = 0; i < x.length; i++) {
                    x[i].classList.remove("autocomplete-active");
                }
            }
            function closeAllLists(elmnt) {
                /*close all autocomplete lists in the document,
                except the one passed as an argument:*/
                var x = document.getElementsByClassName("autocomplete-items");
                for (var i = 0; i < x.length; i++) {
                    if (elmnt != x[i] && elmnt != inp) {
                        x[i].parentNode.removeChild(x[i]);
                    }
                }
            }
            /*execute a function when someone clicks in the document:*/
            document.addEventListener("click", function (e) {
                closeAllLists(e.target);
            });
        }
    </script>
</body>

</html>
