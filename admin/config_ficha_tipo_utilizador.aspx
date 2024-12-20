<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_ficha_tipo_utilizador.aspx.cs" Inherits="config_ficha_tipo_utilizador" %>

<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Ficha do Tipo de Utilizador">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Ficha do Tipo de Utilizador</title>
    <!-- Favicon -->
    <link href="../general/assets/img/brand/favicon.png" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="../general/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="../general/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="../general/assets/css/argon.css?v=1.0.0" rel="stylesheet">
    <link href="../general/assets/css/mbs_div.css" rel="stylesheet" />
    <link type="text/css" href="../vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
    <link type="text/css" href="../alertify/css/alertify.min.css" rel="stylesheet" />
    <link type="text/css" href="../alertify/css/themes/default.min.css" rel="stylesheet" />

    <style>
        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95, #004D95 100%) !important;
        }

        .col-xl-8 {
            max-width: 99%;
            flex: 0 0 99%;
        }
    </style>
</head>

<body>

    <!-- Main content -->
    <div class="main-content">
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
                        <h1 class="display-2 text-white">Tipos de Utilizador</h1>
                        <p class="text-white mt-0 mb-5">Crie/edite os tipos de utilizador</p>
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
                                        <td style="width: 90%">
                                            <h3 class="mb-0">Tipo de Utilizador</h3>
                                        </td>
                                        <td style="width: 10%; text-align: right;">
                                            <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px; cursor: pointer' alt='Back' title='Back' onclick='retroceder();'/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="card-body" id="divGrelha">
                            <form>
                                <h6 class="heading-small text-muted mb-4">Informação do Tipo de Utilizador</h6>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Nome</label>
                                            <input type="text" id="txtNome" class="form-control form-control-alternative" placeholder="Nome">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Observações</label>
                                            <textarea id="txtNotas" rows="3" style="resize: none" class="form-control form-control-alternative" placeholder="Observações diversas"></textarea>
                                        </div>
                                    </div>
                                </div>


                                <hr class="my-4" />
                                <!-- Description -->
                                <h6 class="heading-small text-muted mb-4">Permissões</h6>

                                <div class="row" style="padding-left: 0px">
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkAdmin" type="checkbox">
                                            <label class="custom-control-label" for="chkAdmin">
                                                <span class="text-muted">Administrador</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkPermDashboard" type="checkbox">
                                            <label class="custom-control-label" for="chkPermDashboard">
                                                <span class="text-muted">Acesso ao Dashboard</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkPermMaquinas" type="checkbox">
                                            <label class="custom-control-label" for="chkPermMaquinas">
                                                <span class="text-muted">Acesso às Máquinas</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>


                                <div class="row" style="padding-left: 0px">
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkPermLogs" type="checkbox">
                                            <label class="custom-control-label" for="chkPermLogs">
                                                <span class="text-muted">Acesso aos Logs</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkPermParametrizacao" type="checkbox">
                                            <label class="custom-control-label" for="chkPermParametrizacao">
                                                <span class="text-muted">Acesso à Parametrização</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkPermIntervencoes" type="checkbox">
                                            <label class="custom-control-label" for="chkPermIntervencoes">
                                                <span class="text-muted">Acesso às Intervenções</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-left: 0px">
                                    <div class="col-lg-12">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkParceiro" type="checkbox">
                                            <label class="custom-control-label" for="chkParceiro">
                                                <span class="text-muted">Parceiro</span>
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
            setAltura();
            getData();
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

        function checkaLinha(id) {
            var id = "chk_" + id;
            var ativo = $('#' + id).is(":checked");

            if (ativo)
                $('#' + id).prop('checked', false);
            else
                $('#' + id).prop('checked', true);
        }

        function checkaTodos() {
            var id = "";

            // Primeiro vemos a acao da nossa checkbox mestra
            var ativo = $('#chkTodos').is(":checked");

            var checkedBoxes = document.querySelectorAll('input[name=mycheckboxes]');

            // entao vai ativar as filhas
            if (ativo)
                $('.chkIds').prop('checked', true);
            else
                $('.chkIds').prop('checked', false);
        }

        function showPopup(id) {
            document.getElementById(id).style.display = 'block';
        }

        function hidePopup(id) {
            document.getElementById(id).style.display = 'none';
        }

        function retroceder() {
            alertify.confirm("Tem a certeza que pretende sair?!<br />Todas os dados serão perdidos.",
                function () {
                    loadUrl("config_lista_tipos_utilizador.aspx");
                },
                function () {

                }).set('labels', { ok: 'Sim', cancel: 'Não' }).set('title', 'Tipos de Utilizador');
        }

        function getData() {
            var id = $('#txtAux').val();
            if (id != null && id != 'null') {
                $.ajax({
                    type: "POST",
                    url: "config_ficha_tipo_utilizador.aspx/getData",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');

                        var nome = dados[0];
                        var notas = dados[1];
                        var admin = dados[2];
                        var dashboard = dados[3];
                        var maquinas = dados[4];
                        var logs = dados[5];
                        var params = dados[6];
                        var interv = dados[7];
                        var parceiro = dados[8];

                        $('#txtNome').val(nome);
                        $('#txtNotas').val(notas);

                        if (admin == "false")
                            $('#chkAdmin').attr('checked', false);
                        else
                            $('#chkAdmin').attr('checked', true);

                        if (dashboard == "false")
                            $('#chkPermDashboard').attr('checked', false);
                        else
                            $('#chkPermDashboard').attr('checked', true);

                        if (maquinas == "false")
                            $('#chkPermMaquinas').attr('checked', false);
                        else
                            $('#chkPermMaquinas').attr('checked', true);

                        if (logs == "false")
                            $('#chkPermLogs').attr('checked', false);
                        else
                            $('#chkPermLogs').attr('checked', true);

                        if (params == "false")
                            $('#chkPermParametrizacao').attr('checked', false);
                        else
                            $('#chkPermParametrizacao').attr('checked', true);

                        if (interv == "false")
                            $('#chkPermIntervencoes').attr('checked', false);
                        else
                            $('#chkPermIntervencoes').attr('checked', true);

                        if (parceiro == "false")
                            $('#chkParceiro').attr('checked', false);
                        else
                            $('#chkParceiro').attr('checked', true);
                    }
                });
            }
            else $('#chkAtivo').attr('checked', true);
        }

        function saveData() {
            var id = $('#txtAux').val();
            var nome = $('#txtNome').val();
            var notas = $('#txtNotas').val();

            var administrador = $('#chkAdmin').is(":checked") ? "1" : "0";
            var permissao_dashboard = $('#chkPermDashboard').is(":checked") ? "1" : "0";
            var permissao_maquinas = $('#chkPermMaquinas').is(":checked") ? "1" : "0";
            var permissao_logs = $('#chkPermLogs').is(":checked") ? "1" : "0";
            var permissao_parametrizacao = $('#chkPermParametrizacao').is(":checked") ? "1" : "0";
            var permissao_intervencoes = $('#chkPermIntervencoes').is(":checked") ? "1" : "0";
            var parceiro = $('#chkParceiro').is(":checked") ? "1" : "0";

            if (nome == '' || nome == null || nome == undefined) {
                sweetAlertWarning('Nome', 'Por favor indique o tipo de utilizador');
                return;
            }

            if (nome != '' && nome != null && nome != undefined) {
                $.ajax({
                    type: "POST",
                    url: "config_ficha_tipo_utilizador.aspx/saveData",
                    data: '{"id":"' + id + '","nome":"' + nome + '","notas":"' + notas + '","administrador":"' + administrador + '","permissao_dashboard":"' + permissao_dashboard
                        + '","permissao_maquinas":"' + permissao_maquinas + '","permissao_logs":"' + permissao_logs
                        + '","permissao_parametrizacao":"' + permissao_parametrizacao + '","permissao_intervencoes":"' + permissao_intervencoes + '","parceiro":"' + parceiro + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        if (dados[0] > 0)
                            window.location = "config_lista_tipos_utilizador.aspx";
                        else {
                            sweetAlertWarning('Aviso', dados[1]);
                        }
                    }
                });
            }
        }


        function loadUrl(url) {
            window.location = url;
        }

        function sweetAlertWarning(subject, msg) {
            swal(
                subject,
                msg,
                'warning'
            )
        }
    </script>
</body>

</html>
