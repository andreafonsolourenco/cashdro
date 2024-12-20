<%@ Page Language="C#" AutoEventWireup="true" CodeFile="definirpin.aspx.cs" Inherits="definirpin" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Login">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - PIN</title>
    <!-- Favicon -->
    <link href="../Img/favicon.ico" rel="icon" type="image/png">
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="assets/css/argon.css?v=1.0.1" rel="stylesheet">
    <link href="../vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />

    <style>
        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95 0, white 100%) !important;
        }

        .fill-default {
            fill: #004D95;
        }
    </style>

</head>

<body class="bg-default" style="background-color: #004D95!important">
    <div class="main-content">
        <!-- Header -->
        <div class="header bg-gradient-primary py-7 py-lg-8">
            <div class="container">
                <div class="header-body text-center mb-7">
                    <div class="row justify-content-center">
                        <div class="col-lg-5 col-md-6">
                            <h1 class="text-white">Bem-vindo!</h1>
                            <p class="text-lead text-white">Por favor, defina o seu PIN de operações!</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="separator separator-bottom separator-skew zindex-100">
                <svg x="0" y="0" viewBox="0 0 2560 100" preserveAspectRatio="none" version="1.1" xmlns="http://www.w3.org/2000/svg">
                    <polygon class="fill-default" points="2560 0 2560 100 0 100"></polygon>
                </svg>
            </div>
        </div>
        <!-- Page content -->
        <div class="container mt--8 pb-5">
            <div class="row justify-content-center">
                <div class="col-lg-5 col-md-7">
                    <div class="card bg-secondary shadow border-0">

                        <div class="card-body px-lg-5 py-lg-5">
                            <div class="text-center text-muted mb-4">
                                <small>PIN de Operações</small>
                            </div>
                            <form role="form">
                                <div class="form-group mb-3">
                                    <div class="input-group input-group-alternative">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="ni ni-key-25"></i></span>
                                        </div>
                                        <input id="txtPin" class="form-control" placeholder="PIN" type="text">
                                    </div>
                                </div>
                                <div class="text-center">
                                    <button type="button" class="btn btn-primary my-4" onclick="entra();">Guardar</button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <span class="variaveis" id="username" runat="server"></span>
    <span class="variaveis" id="password" runat="server"></span>

    <!-- Argon Scripts -->
    <!-- Core -->
    <script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Argon JS -->
    <script src="assets/js/argon.js?v=1.0.0"></script>
    <script src="../vendors/sweetalert2/sweetalert2.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            localStorage.setItem("loga", null);
            $('#txtPin').focus();
        });

        $(document).keypress(function (e) {
            if (e.which == 13) {
                if ($("#txtPin").is(":focus")) {
                    entra();
                }
            }
        });

        function entra() {
            var u = $('#username').text();
            var p = $('#password').text();
            var pin = $('#txtPin').val();

            if (pin == '' || pin == null || pin == undefined) {
                sweetAlertInfo("PIN de Operações", "Por favor introduza um PIN válido!");
                return;
            }

            if (u != '' && u != null && u != undefined && p != '' && p != null && p != undefined && pin != '' && pin != null && pin != undefined) {
                $.ajax({
                    type: "POST",
                    url: "definirpin.aspx/entra",
                    data: '{"u":"' + u + '","p":"' + p + '","pin":"' + pin + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');
                        var ret = parseInt(dados[0]);
                        var retMsg = dados[1];

                        if (ret > 0) {
                            localStorage.setItem("loga", ret);

                            window.location = retMsg;
                        }
                        else {
                            sweetAlertInfo("Login", retMsg);
                        }
                    }
                });
            }
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
    </script>
</body>

</html>
