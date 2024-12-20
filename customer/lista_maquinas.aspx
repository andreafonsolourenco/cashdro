<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lista_maquinas.aspx.cs" Inherits="lista_maquinas" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Lista de Máquinas">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Lista de Máquinas</title>
    <!-- Favicon -->
    <link href="../general/assets/img/brand/favicon.png" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="../general/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="../general/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="../general/assets/css/argon.css?v=1.0.0" rel="stylesheet">
    <!-- Alerts -->
    <link type="text/css" href="../vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
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

        ::placeholder { /* Chrome, Firefox, Opera, Safari 10.1+ */
          color: black !important;
          opacity: 1; /* Firefox */
        }

        :-ms-input-placeholder { /* Internet Explorer 10-11 */
          color: black !important;
        }

        ::-ms-input-placeholder { /* Microsoft Edge */
          color: black !important;
        }

        .th_text {
            font-weight: bold !important;
            color: #000000 !important;
        }

        .th_text_centered {
            text-align: center !important;
        }
    </style>
</head>

<body onload="getData();">

    <!-- Main content -->
    <div class="main-content">

        <div id="overlay"></div>
        <div id="divLoading" style="display: none">
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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block">Máquinas</a>
                <!-- Form -->
                

                <!-- User -->
                <ul class="navbar-nav align-items-center d-none d-md-flex">
                    <li class="nav-item dropdown">
                        <a class="nav-link pr-0" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <div class="media align-items-center">

                                <div class="media-body ml-2 d-none d-lg-block">
                                    <span id="spanNomeUser" class="mb-0 text-sm  font-weight-bold"></span>
                                </div>
                            </div>
                        </a>
                        <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                            <div class=" dropdown-header noti-title">
                                <h6 id="spanOla" class="text-overflow m-0"></h6>
                            </div>



                            <div class="dropdown-divider"></div>
                            <a href="#!" class="dropdown-item" onclick="osMeusDados();">
                                <i class="ni ni-circle-08"></i>
                                <span>Os meus dados</span>
                            </a>
                            <a href="#!" class="dropdown-item" onclick="finishSession();">
                                <i class="ni ni-button-power"></i>
                                <span>Terminar sessão</span>
                            </a>
                        </div>
                    </li>
                </ul>



            </div>
        </nav>



        <!-- Header -->
        <div class="header bg-gradient-primary pb-8 pt-5 pt-md-8">
            <div class="container-fluid">
                <div class="header-body">
                    <!-- Card stats -->
                    <div class="row">
                        <div class="col-xl-3 col-lg-6">
                            <div class="card card-stats mb-4 mb-xl-0">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <h5 id="label1" class="card-title text-uppercase text-muted mb-0"></h5>
                                            <span id="total1" class="h2 font-weight-bold mb-0"></span>
                                        </div>
                                        <div class="col-auto">
                                            <div class="icon icon-shape bg-danger text-white rounded-circle shadow">
                                                <i class="fas fa-chart-bar"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <p class="mt-3 mb-0 text-muted text-sm">
                                        <span id="rodape1" class="text-nowrap"></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-lg-6">
                            <div class="card card-stats mb-4 mb-xl-0">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <h5 id="label2" class="card-title text-uppercase text-muted mb-0"></h5>
                                            <span id="total2" class="h2 font-weight-bold mb-0"></span>
                                        </div>
                                        <div class="col-auto">
                                            <div class="icon icon-shape bg-warning text-white rounded-circle shadow">
                                                <i class="fas fa-chart-pie"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <p class="mt-3 mb-0 text-muted text-sm">
                                        <span id="rodape2" class="text-nowrap"></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-lg-6">
                            <div class="card card-stats mb-4 mb-xl-0">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <h5 id="label3" class="card-title text-uppercase text-muted mb-0"></h5>
                                            <span id="total3" class="h2 font-weight-bold mb-0"></span>
                                        </div>
                                        <div class="col-auto">
                                            <div class="icon icon-shape bg-yellow text-white rounded-circle shadow">
                                                <i class="fas fa-users"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <p class="mt-3 mb-0 text-muted text-sm">
                                        <span id="rodape3" class="text-nowrap"></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-lg-6">
                            <div class="card card-stats mb-4 mb-xl-0">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col">
                                            <h5 id="label4" class="card-title text-uppercase text-muted mb-0"></h5>
                                            <span id="total4" class="h2 font-weight-bold mb-0"></span>
                                        </div>
                                        <div class="col-auto">
                                            <div class="icon icon-shape bg-info text-white rounded-circle shadow">
                                                <i class="fas fa-percent"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <p class="mt-3 mb-0 text-muted text-sm">
                                        <span id="rodape4" class="text-nowrap"></span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>




        <!-- Page content -->
        <div class="container-fluid mt--7">
            <!-- Table -->
            <div class="row">
                <div class="col">
                    <div class="card shadow">
                        <div class="card-header border-0">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 90%">
                                        <div style="float:left">
                                            <h3 class="mb-0">Máquinas</h3>
                                        </div>
                                        
                                        <div style="float:left; padding-left:50px">
                                        <a class="btn btn-sm btn-primary" onclick="atualizaDados();" style="color: #FFFFFF">
                                            <span id="spanBtnAlteraStatus">Atualizar dados</span>
                                        </a>
                                        </div>
                                    </td>
                                    <td style="padding-right:25px; padding-left:25px">
                                        <form id="formPesquisa" class="navbar-search navbar-search-dark form-inline mr-3 d-none d-md-flex ml-lg-auto" onsubmit="return false;" onkeypress="keyPesquisa(event);">
                                            <div class="form-group mb-0">
                                                <div class="input-group input-group-alternative">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text"><i class="fas fa-search"></i></span>
                                                    </div>
                                                    <input id="txtPesquisa" class="form-control" placeholder="Pesquisar..." type="text" style="color:black">
                                                </div>
                                            </div>
                                        </form>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="table-responsive">
                            <div id="divGrelhaRegistos" style="min-height:500px"></div>
                        </div>

                    </div>
                </div>
            </div>

            <!-- Footer -->
            <footer class="footer">
                <div class="row align-items-center justify-content-xl-between">
                    <div class="col-xl-6">
                        <div class="copyright text-center text-xl-left text-muted">
                        </div>
                    </div>
                </div>
            </footer>
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
        var ordSerial = 0;
        var ordLocalizacao = 0;
        var ordUltimaAtualizacao = 0;
        var ordLigada = 0;
        var ordErro = 0;

        $(document).ready(function () {
            loga();
            setAltura();
            getTotals();
            setInterval(function () {
                getTotals();
            }, 5000);

            $("#txtPesquisa").focus();
        });

        $(window).resize(function () {
            setAltura();
        });

        function finishSession() {
            window.top.location = "../general/login.aspx";
        }

        function osMeusDados() {
            var id = localStorage.loga;
            window.location = 'config_ficha_utilizador.aspx?id=' + id;
        }

        function loga() {
            var id = localStorage.loga;

            if (id == null || id == 'null' || id == undefined || id == '') {
                window.location = "login.aspx";
            }
            // Se temos sessao, temos que ver se a mesma ainda é válida
            else {
                $.ajax({
                    type: "POST",
                    url: "lista_maquinas.aspx/trataExpiracao",
                    data: '{"i":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');
                        var ret = parseInt(dados[0]);
                        var retMsg = dados[1];

                        getUserName();
                    }
                });
            }
        }

        function getUserName() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getUname",
                data: '{"i":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var nome = res.d;

                    $('#spanNomeUser').html(nome);
                    $('#spanOla').html("Olá, " + nome.split(' ')[0] + "!");
                }
            });
        }

        function setAltura() {
            $("#fraContent").height($(window).height());
        }

        function keyPesquisa(e) {
            if (e.keyCode == 13) {
                getData();
            }
        }

        function loadUrl(url) {
            window.location = url;
        }

        // Web services

        function novo() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/podeCriarMaquinas",
                data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var ret = res.d;

                    if (ret == "OK")
                        window.location = "config_ficha_maquina.aspx?id=null&uid=" + id;
                    else {
                        sweetAlertWarning("Nº máquinas excedido", "Excedeu o número de máquinas permitidas enquanto parceiro. Não é possível criar novas máquinas.");
                    }
                }
            });            
        }

        function getData() {
            loadingOn('A carregar as máquinas. Por favor aguarde...');
            var pesquisa = $('#txtPesquisa').val();
            var order = "";
            var userid = localStorage.loga;

            if (ordSerial == 0 && ordLocalizacao == 0 && ordUltimaAtualizacao == 0 && ordLigada == 0 && ordErro == 0) {
                order = ' ORDER BY status_erro desc, status_ligado desc, serialnumber, localizacao ';
            }
            else {
                order = ' ORDER BY ';

                if (ordSerial != 0) {
                    order += ordSerial == -1 ? ' serialnumber desc ' : ' serialnumber asc ';
                }
                else if (ordLocalizacao != 0) {
                    order += ordLocalizacao == -1 ? ' localizacao desc ' : ' localizacao asc ';
                }
                else if (ordUltimaAtualizacao != 0) {
                    order += ordUltimaAtualizacao == -1 ? ' ultimacomunicacao desc ' : ' ultimacomunicacao asc ';
                }
                else if (ordLigada != 0) {
                    order += ordLigada == -1 ? ' status_ligado desc ' : ' status_ligado asc ';
                }
                else if (ordErro != 0) {
                    order += ordErro == -1 ? ' status_erro desc ' : ' status_erro asc ';
                }
            }

            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '","order":"' + order + '","userid":"' + userid + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaRegistos').html(res.d);
                    getTotals();
                }
            });
        }

        function getTotals() {
            var id_utilizador = localStorage.loga;

            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getTotais",
                data: '{"id_utilizador":"' + id_utilizador + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('@');

                    var label1 = dados[0];
                    var total1 = dados[1];
                    var rodape1 = dados[2];

                    $('#label1').html(label1);
                    $('#total1').html(total1);
                    $('#rodape1').html(rodape1);

                    var label2 = dados[3];
                    var total2 = dados[4];
                    var rodape2 = dados[5];

                    $('#label2').html(label2);
                    $('#total2').html(total2);
                    $('#rodape2').html(rodape2);

                    var label3 = dados[6];
                    var total3 = dados[7];
                    var rodape3 = dados[8];

                    $('#label3').html(label3);
                    $('#total3').html(total3);
                    $('#rodape3').html(rodape3);

                    var label4 = dados[9];
                    var total4 = dados[10];
                    var rodape4 = dados[11];

                    $('#label4').html(label4);
                    $('#total4').html(total4);
                    $('#rodape4').html(rodape4);

                    loadingOff();
                }
            });
        }

        function atualizaDados() {
            loadingOn('A obter informação. Aguarde por favor...');
            var userid = localStorage.loga;

            // Obtemos os dados para saber se as maquinas estao ligadas ou desligadas
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/atualizaDados",
                data: '{"userid":"' + userid + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    loadingOff();
                    getData();
                }
            });
        }

        function ligacaoDiretaMaquina(idMaquina) {
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getDadosMaquina",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');
                    var url = dados[0];
                    var user = dados[1];
                    var pass = dados[2];

                    url += "/Cashdro3Web/index.html#/login";

                    window.open(url, "_blank");
                }
            });
        }

        function getStatusMaquina(idMaquina) {
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getStatusMaquina",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {

                    if (res.d == "ON") sweetAlertSuccess("Máquina Ligada", "A máquina encontra-se ligada e sem erros");
                    else if (res.d == "ERRO") sweetAlertWarning("Erro", "A máquina encontra-se ligada mas apresenta erros");
                    else sweetAlertError("Máquina Desligada", "A máquina encontra-se desligada");
                }
            });
        }


        function reiniciarServico(idMaquina) {
            loadingOn('A reiniciar serviço, por favor aguarde...');

            // Primeiro reinicia o serviço
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/reiniciaServico",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    loadingOff();
                }
            });
        }

        function getLogs(idMaquina) {
            loadingOn('A obter LOGS da máquina. Aguarde por favor...');

            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getStatusMaquina",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {

                    loadingOff();

                    if (res.d == "ON") window.location = "lista_logs.aspx?i=" + idMaquina;
                    else if (res.d == "ERRO") window.location = "lista_logs.aspx?i=" + idMaquina;
                    else sweetAlertError("Máquina Desligada", "A máquina encontra-se desligada. Não é possível obter LOGS");
                }
            });
        }

        function saveLogs(idMaquina) {
            var today = new Date();
            var yesterday = addDays(today, -1);
            var yesterdayDay = yesterday.getUTCDate() < 10 ? ('0' + yesterday.getUTCDate()) : yesterday.getUTCDate();
            var yesterdayMonth = (yesterday.getUTCMonth() + 1) < 10 ? ('0' + (yesterday.getUTCMonth() + 1)) : (yesterday.getUTCMonth() + 1);
            var yesterdayStr = yesterday.getUTCFullYear() + '-' + yesterdayMonth + '-' + yesterdayDay;

            alertify.prompt('Guardar Logs', 'Selecione a data inicial.<br />Serão enviados dois dias de logs a partir da data selecionada', yesterdayStr,
                function (evt, value) {
                    var minimum = new Date(value);
                    var maximum = addDays(minimum, 1);
                    var minDay = minimum.getUTCDate() < 10 ? ('0' + minimum.getUTCDate()) : minimum.getUTCDate();
                    var minMonth = (minimum.getUTCMonth() + 1) < 10 ? ('0' + (minimum.getUTCMonth() + 1)) : (minimum.getUTCMonth() + 1);
                    var maxDay = maximum.getUTCDate() < 10 ? ('0' + maximum.getUTCDate()) : maximum.getUTCDate();
                    var maxMonth = (maximum.getUTCMonth() + 1) < 10 ? ('0' + (maximum.getUTCMonth() + 1)) : (maximum.getUTCMonth() + 1);
                    var minStr = minimum.getUTCFullYear() + '/' + minMonth + '/' + minDay;
                    var maxStr = maximum.getUTCFullYear() + '/' + maxMonth + '/' + maxDay;

                    loadingOn('A obter LOGS da máquina. Aguarde por favor...');

                    $.ajax({
                        type: "POST",
                        url: "lista_maquinas.aspx/getLogsToSave",
                        data: '{"idMaquina":"' + idMaquina + '","minDate":"' + minStr + '","maxDate":"' + maxStr + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (res) {
                            loadingOff();
                            if (res.d != '') {
                                loadUrl(res.d);
                            }
                            else {
                                sweetAlertError("Guardar Logs", "Ocorreu um erro ao obter os logs da máquina para fazer download. Por favor, tente novamente e se o problema persistir contacte o administrador do sistema!");
                            }
                        }
                    });
                },
                function () {

                })
                .set('type', 'date').set('labels', { ok: 'Sim', cancel: 'Não' });
        }

        function operacaoDecorrer(idMaquina) {
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/getStatusMaquina",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d == "ON" || res.d == "ERRO") {
                        // Se estiver ligada obtem a operação em execução 
                        $.ajax({
                            type: "POST",
                            url: "lista_maquinas.aspx/operacaoDecorrer",
                            data: '{"idMaquina":"' + idMaquina + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (res) {
                                var ret = res.d.split('<#SEP#>');
                                var opid = ret[0];
                                var op = ret[1];

                                sweetAlertWarning("Operação em execução", op);
                            }
                        });
                    }
                    else sweetAlertError("Máquina Desligada", "A máquina encontra-se desligada. Não é possível consultar a operação em execução");
                }
            });
        }

        function reiniciarMaquina(idMaquina) {
            loadingOn('A reiniciar máquina, por favor aguarde...');

            // Primeiro reinicia o serviço
            $.ajax({
                type: "POST",
                url: "lista_maquinas.aspx/reiniciaMaquina",
                data: '{"idMaquina":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    getData();
                    loadingOff();

                    var ret = res.d.split('<#SEP#>');
                    var erro = ret[0];
                    var texto = ret[1];

                    if (erro == '-1') {
                        sweetAlertError("Reiniciar Máquina", texto);
                    }
                }
            });
        }

        function openMenu(id) {
            loadUrl("menu_maquina.aspx?id=" + id);
        }

        function ordenaSerial() {
            ordLocalizacao = 0;
            ordUltimaAtualizacao = 0;
            ordLigada = 0;
            ordErro = 0;

            if (ordSerial == 0) {
                ordSerial = 1;
            }
            else {
                ordSerial = ordSerial * (-1);
            }

            getData();
        }

        function ordenaLocalizacao() {
            ordSerial = 0;
            ordUltimaAtualizacao = 0;
            ordLigada = 0;
            ordErro = 0;

            if (ordLocalizacao == 0) {
                ordLocalizacao = 1;
            }
            else {
                ordLocalizacao = ordLocalizacao * (-1);
            }

            getData();
        }

        function ordenaUltimaAtualizacao() {
            ordSerial = 0;
            ordLocalizacao = 0;
            ordLigada = 0;
            ordErro = 0;

            if (ordUltimaAtualizacao == 0) {
                ordUltimaAtualizacao = 1;
            }
            else {
                ordUltimaAtualizacao = ordUltimaAtualizacao * (-1);
            }

            getData();
        }

        function ordenaLigada() {
            ordSerial = 0;
            ordLocalizacao = 0;
            ordUltimaAtualizacao = 0;
            ordErro = 0;

            if (ordLigada == 0) {
                ordLigada = 1;
            }
            else {
                ordLigada = ordLigada * (-1);
            }

            getData();
        }

        function ordenaErro() {
            ordSerial = 0;
            ordLocalizacao = 0;
            ordUltimaAtualizacao = 0;
            ordLigada = 0;

            if (ordErro == 0) {
                ordErro = 1;
            }
            else {
                ordErro = ordErro * (-1);
            }

            getData();
        }



        function edita(id) {
            var uid = localStorage.loga;
            window.location = "config_ficha_maquina.aspx?id=" + id + "&uid=" + uid;
        }

        function apaga(id) {
            swal({
                title: 'Eliminar máquina',
                text: "A máquina será eliminada. Confirma?",
                type: 'question',
                showCancelButton: true,
                confirmButtonColor: '#007351',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sim',
                cancelButtonText: 'Não'
            }).then(function () {
                $.ajax({
                    type: "POST",
                    url: "lista_maquinas.aspx/delRow",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        var res = dados[0];
                        var resMsg = dados[1];

                        if (res == -1) {
                            sweetAlertWarning("Eliminar máquina", resMsg);
                        }
                        else
                            loadUrl("lista_maquinas.aspx");
                    }
                });
            });
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

        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        }

    </script>
</body>

</html>
