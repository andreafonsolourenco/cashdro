<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lista_logs.aspx.cs" Inherits="lista_logs" %>

<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Lista de Logs">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Lista de Logs</title>
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

    <style>
        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95, #004D95 100%) !important;
        }

        .myselect {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid #E5E5E5;
            border-radius: 5px 5px 5px 5px;
            box-shadow: 0 0 10px #E8E8E8 inset;
            height: 40px;
            margin: 0 0 0 25px;
            padding: 10px;
            width: 110px;
            font-size: 14px;
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

        .imgArrow {
            cursor: pointer;
            width: 33px;
            height: 33px;
        }
    </style>
</head>

<body>

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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block">Consulta de Logs</a>
                <!-- Form -->
                <form id="formPesquisa" class="navbar-search navbar-search-dark form-inline mr-3 d-none d-md-flex ml-lg-auto" onsubmit="return false;" onkeypress="keyPesquisa(event);">
                    <div class="form-group mb-0">
                        <div class="input-group input-group-alternative">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                            <input id="txtPesquisa" class="form-control" placeholder="Pesquisar" type="text">
                        </div>
                    </div>
                </form>

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
        <div class="header bg-gradient-primary pb-8 pt-5 pt-md-8" id="divInfo">
            <div class="container-fluid">
                <div class="header-body"></div>
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
                                        <div id="divDdl"></div>
                                    </td>

                                    <td style="text-align: right" onclick="paginaAnterior();">
                                        <img id="imgL" src="../general/assets/img/theme/setae.png" class="imgArrow" />
                                    </td>
                                    <td style="text-align: right" onclick="paginaSeguinte();">
                                        <img id="imgR" src="../general/assets/img/theme/setad.png" class="imgArrow" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="table-responsive" id="divGrelha">
                            <div id="divGrelhaRegistos"></div>
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

            <input id="txtId" type="text" class="variaveis" runat="server">
            <input id="txtPagina" type="text" class="variaveis" runat="server">
        </div>
    </div>

    <!-- Argon Scripts -->
    <!-- Core -->
    <script src="../general/assets/vendor/jquery/dist/jquery.min.js"></script>
    <script src="../general/assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Argon JS -->
    <script src="../general/assets/js/argon.js?v=1.0.0"></script>
    <script src="../vendors/sweetalert2/sweetalert2.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>

    <script>
        var ordHora = 0;
        var ordDispositivo = 0;
        var ordOperacao = 0;
        var ordMensagem = 0;
        var ordUltimaAtualizacao = 0;

        $(document).ready(function () {
            $("#imgL").attr('src', '../general/assets/img/theme/setae_off.png');
            $("#imgL").css('cursor', 'default');

            loga();
            setAltura();

            $("#txtPesquisa").focus();


            var idMaquina = location.search.split('i=')[1];
            if (idMaquina != undefined && idMaquina != null && idMaquina != '') {
                getDdlMaquinas(idMaquina);
                $("#txtId").val(idMaquina);
                getLogs();
            }
            else {
                getDdlMaquinas('');


                $("#imgL").attr('src', '../general/assets/img/theme/setae_off.png');
                $("#imgL").css('cursor', 'default');
                $("#imgR").attr('src', '../general/assets/img/theme/setad_off.png');
                $("#imgR").css('cursor', 'default');

                $('#divGrelhaRegistos').html("<table class='table align-items-center table-flush'><thead class='thead-light'><tr><th scope='col'>Hora</th><th scope='col'>Dispositivo</th><th scope='col'>Operação</th><th scope='col'>Mensagem</th><th scope='col'>Última atualização</th></tr></thead><tbody><tr><td colspan='4'>Por favor selecione uma máquina para consultar LOGS</td></tr></tbody></table>");

                trocaLogsMaquina();
            }

            defineTablesMaxHeight();
        });

        $(window).resize(function () {
            setAltura();
            defineTablesMaxHeight();
        });

        function defineTablesMaxHeight() {
            var windowHeight = $(window).height();
            var divInfoHeight = $('#divInfo').height();
            var navbarHeight = $('#navbar-main').height();
            var maxHeight = windowHeight - divInfoHeight - navbarHeight - 200;

            $('#divGrelha').css({ "maxHeight": maxHeight + "px" });
        }

        function finishSession() {
            window.top.location = "../general/login.aspx";
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
                    url: "lista_logs.aspx/trataExpiracao",
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
                url: "lista_logs.aspx/getUname",
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
                getData("1");
            }
        }

        function back() {
            var url = "menu_consulta.aspx?id=" + $('#txtId').val();
            loadUrl(url);
        }

        function loadUrl(url) {
            window.location = url;
        }

        // Web services

        function getLogs() {
            loadingOn('A obter informação. Aguarde por favor...');
            $("#ddlMaquinas").prop('disabled', 'disabled');

            $('#divGrelhaRegistos').html('<div style="padding-left: 20px;font-size: 15px;height: 700px;">A obter dados. Por favor aguarde...</div>');
            var pagina = $('#txtPagina').val();
            var id = $('#txtId').val();

            $.ajax({
                type: "POST",
                url: "lista_logs.aspx/getLogs",
                data: '{"idMaquina":"' + id + '","pagina":"' + pagina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var ret = res.d;
                    getData(ret);
                }
            });
        }

        function getData(ret) {
            var pesquisa = $('#txtPesquisa').val();
            var id = $('#txtId').val();
            var order = "";
            var pagina = $('#txtPagina').val();

            if (ordHora == 0 && ordDispositivo == 0 && ordOperacao == 0 && ordMensagem == 0 && ordUltimaAtualizacao == 0) {
                order = ' ORDER BY id DESC ';
            }
            else {
                order = ' ORDER BY ';

                if (ordHora != 0) {
                    order += ordHora == -1 ? ' datahora desc ' : ' datahora asc ';
                }
                else if (ordDispositivo != 0) {
                    order += ordDispositivo == -1 ? ' dispositivo desc ' : ' dispositivo asc ';
                }
                else if (ordOperacao != 0) {
                    order += ordOperacao == -1 ? ' operacao desc ' : ' operacao asc ';
                }
                else if (ordMensagem != 0) {
                    order += ordMensagem == -1 ? ' mensagem desc ' : ' mensagem asc ';
                }
                else if (ordUltimaAtualizacao != 0) {
                    order += ordUltimaAtualizacao == -1 ? ' dataultimaatualizacao desc ' : ' dataultimaatualizacao asc ';
                }
            }

            $.ajax({
                type: "POST",
                url: "lista_logs.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '","idMaquina":"' + id + '","order":"' + order + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var data = res.d.split('<#SEP#>');
                    var tot = data[0];
                    var grelha = data[1];

                    $("#ddlMaquinas").removeAttr("disabled");

                    if (tot > 0 && ret == "1") {
                        $("#imgR").attr('src', '../general/assets/img/theme/setad.png');
                        $("#imgR").css('cursor', 'pointer');
                    }
                    else {
                        if (parseInt(pagina) > 1 && parseInt(ret) == 1 && parseInt(tot) == 0) {
                            $("#imgR").attr('src', '../general/assets/img/theme/setad_off.png');
                            $("#imgR").css('cursor', 'default');
                        }
                        else {
                            $("#imgL").attr('src', '../general/assets/img/theme/setae_off.png');
                            $("#imgL").css('cursor', 'default');
                            $("#imgR").attr('src', '../general/assets/img/theme/setad_off.png');
                            $("#imgR").css('cursor', 'default');
                        }
                    }

                    loadingOff();

                    if (ret == "-1" && tot > 0)
                        sweetAlertWarning("Falha a obter LOGS", "Não foi possível obter LOGS da máquina. Serão apresentados os ultimos LOGS anteriormente obtidos.");
                    else if (ret == "-1" && tot == 0)
                        sweetAlertWarning("Falha a obter LOGS", "Não existem LOGs a apresentar para a máquina solicitada.");


                    $('#divGrelhaRegistos').html(grelha);
                }
            });
        }

        function getDdlMaquinas(idMaquina) {
            $.ajax({
                type: "POST",
                url: "lista_logs.aspx/getDdlMaquinas",
                data: '{"pid":"' + idMaquina + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divDdl').html(res.d);
                }
            });
        }

        function trocaLogsMaquina() {
            $("#txtPesquisa").focus();
            var idMaquina = $('#ddlMaquinas').val();
            if (idMaquina != null && idMaquina != undefined && idMaquina != '' && idMaquina != '-1') {
                $('#txtId').val(idMaquina);
                getLogs();
            }
        }


        function paginaSeguinte() {
            var pagina = $('#txtPagina').val();
            var nPagina = parseInt(pagina) + 1;

            if ($("#imgR").attr('src').indexOf('_off') > -1)
                return;

            $('#txtPagina').val(nPagina);

            getLogs();

            $("#imgL").attr('src', '../general/assets/img/theme/setae.png');
            $("#imgL").css('cursor', 'pointer');
        }

        function paginaAnterior() {
            if ($("#imgL").attr('src').indexOf('_off') > -1)
                return;

            var pagina = $('#txtPagina').val();
            var nPagina = parseInt(pagina);

            if (nPagina > 1) {
                nPagina = parseInt(pagina) - 1;
                $('#txtPagina').val(nPagina);

                getLogs();

                if (nPagina <= 1) {
                    $("#imgL").attr('src', '../general/assets/img/theme/setae_off.png');
                    $("#imgL").css('cursor', 'default');
                }
            }
        }

        function ordenaHora() {
            ordDispositivo = 0;
            ordOperacao = 0;
            ordMensagem = 0;
            ordUltimaAtualizacao = 0;

            if (ordHora == 0) {
                ordHora = 1;
            }
            else {
                ordHora = ordHora * (-1);
            }

            getData();
        }

        function ordenaDispositivo() {
            ordHora = 0;
            ordOperacao = 0;
            ordMensagem = 0;
            ordUltimaAtualizacao = 0;

            if (ordDispositivo == 0) {
                ordDispositivo = 1;
            }
            else {
                ordDispositivo = ordDispositivo * (-1);
            }

            getData();
        }

        function ordenaOperacao() {
            ordHora = 0;
            ordDispositivo = 0;
            ordMensagem = 0;
            ordUltimaAtualizacao = 0;

            if (ordOperacao == 0) {
                ordOperacao = 1;
            }
            else {
                ordOperacao = ordOperacao * (-1);
            }

            getData();
        }

        function ordenaMensagem() {
            ordHora = 0;
            ordDispositivo = 0;
            ordOperacao = 0;
            ordUltimaAtualizacao = 0;

            if (ordMensagem == 0) {
                ordMensagem = 1;
            }
            else {
                ordMensagem = ordMensagem * (-1);
            }

            getData();
        }

        function ordenaUltimaAtualizacao() {
            ordHora = 0;
            ordDispositivo = 0;
            ordOperacao = 0;
            ordMensagem = 0;

            if (ordUltimaAtualizacao == 0) {
                ordUltimaAtualizacao = 1;
            }
            else {
                ordUltimaAtualizacao = ordUltimaAtualizacao * (-1);
            }

            getData();
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
    </script>
</body>

</html>
