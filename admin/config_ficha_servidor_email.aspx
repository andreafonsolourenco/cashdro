<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_ficha_servidor_email.aspx.cs" Inherits="config_ficha_servidor_email" %>

<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Ficha do Servidor Email">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Ficha do Servidor Email</title>
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
                        <img src="assets/img/theme/preloader.gif" />
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
                        <h1 class="display-2 text-white">Serviço de Email</h1>
                        <p class="text-white mt-0 mb-5">Parametrize o serviço de email</p>
                        <div style="width: 50%; float: left;">
                            <a href="#!" class="btn btn-info" onclick="saveData();">Guardar alterações</a>
                        </div>
                        <div style="width: 50%; float: right;">
                            <a href="#!" class="btn btn-info" onclick="sendEmail();">Enviar Email de Teste</a>
                        </div>
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
                                <table style="width: 100%; margin-left: 15px;">
                                    <tr>
                                        <td style="width: 90%">
                                            <h3 class="mb-0">Parametrização do Serviço de Email</h3>
                                        </td>
                                        <td style="width: 10%; text-align: right">
                                            <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px; cursor: pointer' alt='Back' title='Back' onclick='retroceder();'/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="card-body" id="divGrelha">
                            <form>
                                <h6 class="heading-small text-muted mb-4">Informações de Parametrização do Serviço de Email</h6>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Email</label>
                                            <input type="text" id="txtEmail" class="form-control form-control-alternative" placeholder="Email a partir de onde serão enviados os alertos">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Password</label>
                                            <input type="password" id="txtPassword" class="form-control form-control-alternative" placeholder="Password de Login no Email">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Servidor SMTP</label>
                                            <input type="text" id="txtSMTP" class="form-control form-control-alternative" placeholder="Servidor SMTP para envio de emails">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Porta SMTP</label>
                                            <input type="text" id="txtSMTPPort" class="form-control form-control-alternative" placeholder="Porta STMP para envio de emails">
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Emails para envio (separe os emails com ';')</label>
                                            <textarea id="txtEmailsEnvio" rows="3" style="resize: none" class="form-control form-control-alternative" placeholder="Emails para onde serão enviados os alertas"></textarea>
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

        function getData() {
            $.ajax({
                type: "POST",
                url: "config_ficha_servidor_email.aspx/getData",
                data: '{"id":"' + '' + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');

                    // Prepara o retorno dos dados
                    var email = dados[0];
                    var email_password = dados[1];
                    var email_smtp = dados[2];
                    var email_smtpport = dados[3];
                    var emails_alerta = dados[4];

                    $('#txtEmail').val(email);
                    $('#txtPassword').val(email_password);
                    $('#txtSMTP').val(email_smtp);
                    $('#txtSMTPPort').val(email_smtpport);
                    $('#txtEmailsEnvio').val(emails_alerta);
                }
            });
        }

        function saveData() {
            var email = $('#txtEmail').val();
            var pass = $('#txtPassword').val();
            var smtp = $('#txtSMTP').val();
            var port = $('#txtSMTPPort').val();
            var emailsEnvio = $('#txtEmailsEnvio').val();

            $.ajax({
                type: "POST",
                url: "config_ficha_servidor_email.aspx/saveData",
                data: '{"email":"' + email + '","pass":"' + pass + '","smtp":"' + smtp + '","port":"' + port + '","emailsEnvio":"' + emailsEnvio + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    window.location = "parametrizacao.aspx";
                }
            });
        }


        function loadUrl(url) {
            window.location = url;
        }

        function retroceder() {
            loadUrl("parametrizacao.aspx");
        }

        function sendEmail() {
            loadingOn('A enviar email de teste, por favor aguarde...');

            var from = $('#txtEmail').val();
            var pwd = $('#txtPassword').val();
            var smtp = $('#txtSMTP').val();
            var smtpport = $('#txtSMTPPort').val();
            var emails = $('#txtEmailsEnvio').val();

            $.ajax({
                type: "POST",
                url: "config_ficha_servidor_email.aspx/sendEmailFromTemplate",
                data: '{"from":"' + from + '", "pwd":"' + pwd + '", "smtp":"' + smtp + '", "smtpport":"' + smtpport
                    + '", "emails":"' + emails + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d != null) {
                        loadingOff();
                        if (res.d.indexOf('ERRO') !== -1) {
                            sweetAlertError("Email de Teste", res.d);
                        }
                        else {
                            sweetAlertSuccess("Email de Teste", res.d);
                        }
                    }
                }
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

    </script>

</body>

</html>
