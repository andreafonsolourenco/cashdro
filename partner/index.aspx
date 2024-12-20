<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Index">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Página Inicial</title>
    <!-- Favicon -->
    <link href="../Img/favicon.ico" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="../general/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="../general/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="../general/assets/css/argon.css?v=1.0.0" rel="stylesheet">

    <style>
        .navbar-vertical.navbar-expand-md .navbar-brand-img {
            max-height: 5.5rem;
        }
    </style>
</head>

<body style="overflow: hidden;">
    <!-- Sidenav -->
    <nav class="navbar navbar-vertical fixed-left navbar-expand-md navbar-light bg-white" id="sidenav-main">
        <div class="container-fluid">
            <!-- Toggler -->
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#sidenav-collapse-main" aria-controls="sidenav-main" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <!-- Brand -->
            <a class="navbar-brand pt-0 pointer" onclick="loadMainPage();">
                <img id="partner_logo" class="navbar-brand-img" onerror="javascript:this.src='../Img/partner/default_logo.png'">
            </a>
            <!-- User -->
            <ul class="nav align-items-center d-md-none">
                <li class="nav-item dropdown variaveis">
                    <a class="nav-link nav-link-icon" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i class="ni ni-bell-55"></i>
                    </a>
                    <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right" aria-labelledby="navbar-default_dropdown_1">
                        <a class="dropdown-item" href="#">Action</a>
                        <a class="dropdown-item" href="#">Another action</a>
                        <div class="dropdown-divider"></div>
                        <a class="dropdown-item" href="#">Something else here</a>
                    </div>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <div class="media align-items-center">
                            <span class="avatar avatar-sm rounded-circle">
                                <img alt="Image placeholder" id="logo" onerror="javascript:this.src='../Img/partner/default_logo.png'">
                            </span>
                        </div>
                    </a>
                    <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                        <div class=" dropdown-header noti-title">
                            <h6 class="text-overflow m-0" id="txtUsername"></h6>
                        </div>

                        <div class="dropdown-divider"></div>
                        <a href="#!" class="dropdown-item">
                            <i class="ni ni-button-power"></i>
                            <span>Terminar Sessão</span>
                        </a>
                    </div>
                </li>
            </ul>
            <!-- Collapse -->
            <div class="collapse navbar-collapse" id="sidenav-collapse-main">
                <!-- Collapse header -->
                <div class="navbar-collapse-header d-md-none">
                    <div class="row">
                        <div class="col-6 collapse-brand pointer">
                            <a onclick="loadMainPage();" class="pointer">
                                <img id="logo2" class="pointer" onerror="javascript:this.src='../Img/partner/default_logo.png'">
                            </a>
                        </div>
                        <div class="col-6 collapse-close">
                            <button type="button" class="navbar-toggler" data-toggle="collapse" data-target="#sidenav-collapse-main" aria-controls="sidenav-main" aria-expanded="false" aria-label="Toggle sidenav">
                                <span></span>
                                <span></span>
                            </button>
                        </div>
                    </div>
                </div>
                <!-- Form -->
                <form class="mt-4 mb-3 d-md-none">
                    <div class="input-group input-group-rounded input-group-merge">
                        <input type="search" class="form-control form-control-rounded form-control-prepended" placeholder="Pesquisar" aria-label="Pesquisar">
                        <div class="input-group-prepend">
                            <div class="input-group-text">
                                <span class="fa fa-search"></span>
                            </div>
                        </div>
                    </div>
                </form>
                <!-- Navigation -->
                <ul class="navbar-nav">
                    <li id="menuDashboard" class="nav-item variaveis pointer" onclick="loadUrl('dashboard.aspx');" data-toggle="collapse" data-target="#sidenav-collapse-main">
                        <a class="nav-link">
                            <i class="ni ni-tv-2 text-primary"></i>Dashboard
                        </a>
                    </li>


                    <li id="menuMaquinas" class="nav-item variaveis pointer" onclick="loadUrl('lista_maquinas.aspx');" data-toggle="collapse" data-target="#sidenav-collapse-main">
                        <a class="nav-link">
                            <i class="ni ni-archive-2 text-yellow"></i>Máquinas
                        </a>
                    </li>

                    <%--<li id="menuLogs" class="nav-item variaveis pointer" onclick="loadUrl('lista_logs.aspx');" data-toggle="collapse" data-target="#sidenav-collapse-main">
                        <a class="nav-link">
                            <i class="ni ni-money-coins text-blue"></i>LOGs
                        </a>
                    </li>

                    <li id="menuIntervencoes" class="nav-item variaveis pointer" onclick="loadUrl('lista_intervencoes.aspx?page=menu');" data-toggle="collapse" data-target="#sidenav-collapse-main">
                        <a class="nav-link">
                            <i class="ni ni-building text-dark"></i>Intervenções
                        </a>
                    </li>--%>

                    <li id="menuParametrizacao" class="nav-item variaveis pointer" onclick="loadUrl('parametrizacao.aspx');" data-toggle="collapse" data-target="#sidenav-collapse-main">
                        <a class="nav-link">
                            <i class="ni ni-ui-04 text-info"></i>Parametrização
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <!-- Main content -->
    <div class="main-content">
        <iframe id="fraContent" style="width: 100%; padding-right: 1px; height: 100%"></iframe>
    </div>

    <div id="hiddenVals" class="variaveis">
        <input id="txtAux" runat="server" type="text" class="variaveis" />
    </div>

    <!-- Argon Scripts -->
    <!-- Optional JS -->
    <script src="../general/assets/vendor/chart.js/dist/Chart.js"></script>
    <script src="../general/assets/vendor/chart.js/dist/Chart.extension.js"></script>
    <!-- Argon JS -->
    <script src="../general/assets/js/argon.js?v=1.0.0"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>
    <!-- Core -->
    <script src="../general/assets/vendor/jquery/dist/jquery.min.js"></script>
    <script src="../general/assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        var administrador;

        $(document).ready(function () {
            setAltura();
            loga();
            getPermissions();
            getUserName();
            getLogo();
        });

        $(window).resize(function () {
            setAltura();
        });

        function finishSession() {
            window.top.location = "../general/login.aspx";
        }

        function loga() {
            var id = localStorage.loga;
            $('#txtAux').val(id);

            if (id == null || id == 'null' || id == undefined || id == '') {
                window.location = "../general/login.aspx";
            }
            // Se temos sessao, temos que ver se a mesma ainda é válida
            else {
                $.ajax({
                    type: "POST",
                    url: "../general/login.aspx/trataExpiracao",
                    data: '{"i":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');
                        var ret = parseInt(dados[0]);
                        var retMsg = dados[1];

                        // OK
                        if (ret == 0) {
                            window.location = "../general/login.aspx";
                        }
                    }
                });
            }
        }

        function setAltura() {
            $("#fraContent").height($(window).height());
        }

        function loadMainPage() {
            loadUrl('dashboard.aspx');
        }

        function loadUrl(url) {
            $('#fraContent').attr('src', url);
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
                    administrador = (dados[0] == 'true');
                    var dashboard = (dados[1] == 'true');
                    var maquinas = (dados[2] == 'true');
                    var logs = (dados[3] == 'true');
                    var params = (dados[4] == 'true');
                    var interv = (dados[5] == 'true');

                    if (administrador) {
                        $('#menuDashboard').removeClass('variaveis');
                        $('#menuMaquinas').removeClass('variaveis');
                        $('#menuLogs').removeClass('variaveis');
                        $('#menuIntervencoes').removeClass('variaveis');
                        $('#menuParametrizacao').removeClass('variaveis');
                    }
                    else {
                        if (dashboard) {
                            $('#menuDashboard').removeClass('variaveis');
                        }

                        if (maquinas) {
                            $('#menuMaquinas').removeClass('variaveis');
                        }

                        if (logs) {
                            $('#menuLogs').removeClass('variaveis');
                        }

                        if (params) {
                            $('#menuParametrizacao').removeClass('variaveis');
                        }

                        if (interv) {
                            $('#menuIntervencoes').removeClass('variaveis');
                        }
                    }

                    loadMainPage();
                }
            });
        }

        function getLogo() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "index.aspx/getLogo",
                data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var logo = res.d;

                    if (!checkImage(logo)) {
                        logo = '../Img/partner/default_logo.png';
                    }

                    $('#partner_logo').attr('src', logo);
                    $('#logo').attr('src', logo);
                    $('#logo2').attr('src', logo);
                }
            });
        }

        function getUserName() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "login.aspx/getUname",
                data: '{"i":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var nome = res.d;
                    $('#txtUsername').html("Olá, " + nome.split(' ')[0] + "!");
                }
            });
        }

        function checkImage(imageSrc) {
            var img = new Image();
            try {
                img.src = imageSrc;
                return true;
            } catch (err) {
                return false;
            }
        }
    </script>
</body>
</html>
