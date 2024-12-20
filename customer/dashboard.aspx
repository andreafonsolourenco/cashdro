<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="dashboard" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Dashboard">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Dashboard</title>
    <!-- Favicon -->
    <link href="../general/assets/img/brand/favicon.png" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="../general/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="../general/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="../general/assets/css/argon.css?v=1.0.0" rel="stylesheet">

    <style>
        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95, #004D95 100%) !important;
        }

        .variaveis {
            display:none;
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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block">Dashboard</a>
                <!-- Form -->
                <form class="navbar-search navbar-search-dark form-inline mr-3 d-none d-md-flex ml-lg-auto">
                    <div class="form-group mb-0">
                        <div class="input-group input-group-alternative">
                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                            <input class="form-control" placeholder="Pesquisar" type="text">
                        </div>
                    </div>
                </form>
                <!-- User -->
                <ul class="navbar-nav align-items-center d-none d-md-flex pointer">
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

                            <a href="#!" onclick="finishSession();" class="dropdown-item">
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
                    <div class="row text-white text-uppercase" style="margin-bottom: 20px;" id="machinesList"></div>
                    <!-- Card stats -->
                    <div class="row">
                        <div class="col-xl-3 col-lg-6" id="header1">
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
                        <div class="col-xl-3 col-lg-6" id="header2">
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
                        <div class="col-xl-3 col-lg-6" id="header3">
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
                        <div class="col-xl-3 col-lg-6" id="header4">
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
            <div class="row">
                <div class="col-xl-12 mb-5 mb-xl-0">
                    <div class="card bg-gradient-default shadow">
                        <div class="card-header bg-transparent">
                            <div class="row align-items-center">
                                <div class="col">
                                    <h6 class="text-uppercase text-light ls-1 mb-1">Máquinas</h6>
                                    <h2 class="text-white mb-0">Ponto situação das máquinas</h2>
                                </div>
                                <div class="col">
                                    <ul class="nav nav-pills justify-content-end">
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <!-- Chart -->
                            <div class="chart">
                                <!-- Chart wrapper -->
                                <canvas id="dashboard-sales-ano" class="chart-canvas"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-5">
                <div class="col-xl-12 mb-5 mb-xl-0">
                    <div class="card shadow">
                        <div class="card-header border-0">
                            <div class="row align-items-center">
                                <div class="col">
                                    <h3 class="mb-0" style="color: #FB6340">MÁQUINAS COM ERRO</h3>
                                </div>
                            </div>
                        </div>
                        <div id="divGrelhaMaquinasComErro" class="table-responsive">
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-5">
                <div class="col-xl-12 mb-5 mb-xl-0">
                    <div class="card shadow">
                        <div class="card-header border-0">
                            <div class="row align-items-center">
                                <div class="col">
                                    <h3 class="mb-0" style="color: red">MÁQUINAS LIGADAS</h3>
                                </div>
                            </div>
                        </div>
                        <div id="divGrelhaMaquinasLigadas" class="table-responsive">
                            <!-- Projects table -->

                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-5">
                <div class="col-xl-12 mb-5 mb-xl-0">
                    <div class="card shadow">
                        <div class="card-header border-0">
                            <div class="row align-items-center">
                                <div class="col">
                                    <h3 class="mb-0" style="color: red">MÁQUINAS DESLIGADAS</h3>
                                </div>
                            </div>
                        </div>
                        <div id="divGrelhaMaquinasDesligadas" class="table-responsive">
                            <!-- Projects table -->

                        </div>
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <footer class="footer">
                <%-- <div class="row align-items-center justify-content-xl-between">
          <div class="col-xl-6">
            <div class="copyright text-center text-xl-left text-muted">
              &copy; 2019, Plataforma desenvolvida por <a href="http://www.xxxx.pt" class="font-weight-bold ml-1" target="_blank">xxxx</a>
            </div>
          </div>
        </div>--%>
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
    <!-- Optional JS -->
    <script src="../general/assets/vendor/chart.js/dist/Chart.min.js"></script>
    <script src="../general/assets/vendor/chart.js/dist/Chart.extension.js"></script>
    <!-- Argon JS -->
    <script src="../general/assets/js/argon.js?v=1.0.0"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>

    <script>

        $(document).ready(function () {
            loga();
            setAltura();
            getPermissions();
            makeChartVendas();

            getGrelhaMaquinasLigadas();
            getGrelhaMaquinasDesligadas();
            getGrelhaMaquinasComErro();
        });

        $(window).resize(function () {
            setAltura();
        });

        function setAltura() {
            $("#fraContent").height($(window).height());
        }

        function osMeusDados() {
            var id = localStorage.loga;
            window.location = 'config_ficha_utilizador.aspx?id=' + id;
        }

        function loga() {
            var id = localStorage.loga;
            $('#txtAux').val(id);

            if (id == null || id == 'null' || id == undefined || id == '') {
                window.location = "login.aspx";
            }
            // Se temos sessao, temos que ver se a mesma ainda é válida
            else {
                getUserName();
            }
        }

        function getUserName() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "../general/login.aspx/getUname",
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

        function getPermissions() {
            $.ajax({
                type: "POST",
                url: "index.aspx/getPermissoes",
                data: '{"id":"' + $('#txtAux').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');

                    // Prepara o retorno dos dados
                    var administrador = (dados[0] == 'true');
                    var dashboard = (dados[1] == 'true');
                    var maquinas = (dados[2] == 'true');
                    var logs = (dados[3] == 'true');
                    var params = (dados[4] == 'true');
                    var interv = (dados[5] == 'true');

                    getMachinesList();
                    getTotals(true);
                    setCheckboxClickEvent();
                    setInterval(function () {
                        getTotals(false);
                    }, 5000);
                }
            });
        }

        function loadUrl(url) {
            window.location = url;
        }

        function finishSession() {
            window.top.location = "../general/login.aspx";
        }

        function getMachinesList() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getMachines",
                data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d;

                    $('#machinesList').html(dados);
                }
            });
        }

        function setCheckboxClickEvent() {
            for (i = 0; i < parseInt($('#nrMachines').html()); i++) {
                $('#machine' + i.toString()).change(function () {
                    getTotals(true);
                });
            }
        }

        function getTotals(mostraLoading) {
            if (mostraLoading) {
                loadingOn('A calcular valores. Por favor, aguarde...');
            }

            var id = localStorage.loga;

            var selectedMachines = "";
            var flag = false;

            for (i = 0; i < parseInt($('#nrMachines').html()); i++) {
                if ($('#machine' + i.toString()).is(":checked")) {
                    if (!flag) {
                        selectedMachines += $('#machine' + i.toString()).val();
                        flag = true;
                    }
                    else {
                        selectedMachines += "," + $('#machine' + i.toString()).val();
                    }
                }
            }

            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getTotais",
                data: '{"id":"' + id + '","machinesList":"' + selectedMachines + '"}',
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

        function getGrelhaMaquinasLigadas() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getGrelhaMaquinasLigadas",
                data: '{"idCliente":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaMaquinasLigadas').html(res.d);
                }
            });
        }

        function getGrelhaMaquinasDesligadas() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getGrelhaMaquinasDesligadas",
                data: '{"idCliente":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaMaquinasDesligadas').html(res.d);
                }
            });
        }


        function getGrelhaMaquinasComErro() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getGrelhaMaquinasComErro",
                data: '{"idCliente":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaMaquinasComErro').html(res.d);
                }
            });
        }

        function makeChartVendas() {
            var id = localStorage.loga;
            var jan = 0;
            var fev = 0;
            var mar = 0;
            var abr = 0;
            var mai = 0;
            var jun = 0;
            var jul = 0;
            var ago = 0;
            var set = 0;
            var out = 0;
            var nov = 0;
            var dez = 0;

            $.ajax({
                type: "POST",
                url: "dashboard.aspx/getGraficoVendas",
                data: '{"idCliente":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');
                    ligadas = dados[0];
                    desligadas = dados[1];
                    erro = dados[2];

                    var $chart = $('#dashboard-sales-ano');
                    if ($chart.length) {
                        initChartVendas($chart, ligadas, desligadas, erro);
                    }
                }
            });
        }

        function initChartVendas($chart, ligadas, desligadas, erro) {
            var EncomendasClienteChart = new Chart($chart, {
                type: 'pie',
                data: {
                    labels: ["Máquinas ligadas", "Máquinas desligadas", "Máquinas com erro"],
                    datasets: [{
                        backgroundColor: [
                            "#2ecc71",
                            "red",
                            "#e4d96f"
                        ],
                        data: [ligadas, desligadas, erro]
                    }]
                }
            });
        }

        function maquinas(id) {
            loadUrl("lista_maquinas.aspx");
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
