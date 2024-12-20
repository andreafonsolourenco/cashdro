<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_lista_utilizadores.aspx.cs" Inherits="config_lista_utilizadores" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Lista de Utilizadores">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Lista de Utilizadores</title>
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

    <style type="text/css">
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

        .bg-gradient-primary {
            background: linear-gradient(87deg, #004D95, #004D95 100%) !important;
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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block">Parametrização</a>
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


            </div>
        </nav>


        <!-- Header -->
        <div class="header bg-gradient-primary pb-8 pt-5 pt-md-8" id="divInfo">
            <div class="container-fluid">
                <div class="header-body">
                    <!-- Card stats -->
                </div>
            </div>

            <div class="container-fluid d-flex align-items-center">
                <div class="row">
                    <div class="col-lg-12 col-md-10">
                        <h1 class="display-2 text-white">Utilizadores</h1>
                        <p class="text-white mt-0 mb-5">Parametrize os utilizadores com acesso à loja.</p>
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
                                    <td style="width: 10%">
                                        <h3 class="mb-0">Utilizadores</h3>
                                    </td>

                                    <td style="width: 75%; text-align: left">
                                        <div id="divTipoUser"></div>
                                    </td>

                                    <td style="width: 15%; text-align: right;">
                                        <img src="../general/assets/img/theme/new.png" style="width: 30px; height: 30px" class="pointer" alt="Criar novo utilizador" title="Criar novo utilizador" onclick="novo();" />
                                        <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px; cursor: pointer' alt='Back' title='Back' onclick='retroceder();'/>
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
        var ordNome = 0;
        var ordEmail = 0;
        var ordTelemovel = 0;
        var ordPerfil = 0;
        var ordAtivo = 0;

        $(document).ready(function () {
            setAltura();
            getTipoUsers();
            $("#txtPesquisa").focus();
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

        function retroceder() {
            loadUrl("parametrizacao.aspx");
        }


        function keyPesquisa(e) {
            if (e.keyCode == 13) {
                getData();
            }
        }

        function novo() {
            window.location = "config_ficha_utilizador.aspx?id=null";
        }


        function edita(id) {
            window.location = "config_ficha_utilizador.aspx?id=" + id;
        }

        function apaga(id) {
            swal({
                title: 'Eliminar utilizador',
                text: "O utilizador será eliminado. Confirma?",
                type: 'question',
                showCancelButton: true,
                confirmButtonColor: '#007351',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sim',
                cancelButtonText: 'Não'
            }).then(function () {
                $.ajax({
                    type: "POST",
                    url: "config_lista_utilizadores.aspx/delRow",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        var res = dados[0];
                        var resMsg = dados[1];

                        if (res == -1) {
                            sweetAlertWarning("Eliminar utilizador", resMsg);
                        }
                        else
                            loadUrl("config_lista_utilizadores.aspx");
                    }
                });
            });
        }



        function loadUrl(url) {
            window.location = url;
        }





        function getTipoUsers() {
            $.ajax({
                type: "POST",
                url: "config_lista_utilizadores.aspx/getDDlTiposUser",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divTipoUser').html(res.d);

                    getData();
                }
            });
        }



        // Web services

        function getData() {
            $("#txtPesquisa").focus();
            var pesquisa = $('#txtPesquisa').val();
            var id_tipouser = $("#ddlTipoUser option:selected").val();
            var userid = localStorage.loga;
            var order = "";

            if (ordNome == 0 && ordEmail == 0 && ordTelemovel == 0 && ordPerfil == 0 && ordAtivo == 0) {
                order = ' ORDER BY [admin] desc, nome asc ';
            }
            else {
                order = ' ORDER BY ';

                if (ordNome != 0) {
                    order += ordNome == -1 ? ' nome desc ' : ' nome asc ';
                }
                else if (ordEmail != 0) {
                    order += ordEmail == -1 ? ' email desc ' : ' email asc ';
                }
                else if (ordTelemovel != 0) {
                    order += ordTelemovel == -1 ? ' telemovel desc ' : ' telemovel asc ';
                }
                else if (ordPerfil != 0) {
                    order += ordPerfil == -1 ? ' tipo desc ' : ' tipo asc ';
                }
                else if (ordAtivo != 0) {
                    order += ordAtivo == -1 ? ' ativo desc ' : ' ativo asc ';
                }
            }

            $.ajax({
                type: "POST",
                url: "config_lista_utilizadores.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '","order":"' + order + '","filtro":"' + id_tipouser + '","userid":"' + userid + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaRegistos').html(res.d);
                }
            });
        }

        function ordenaNome() {
            ordEmail = 0;
            ordTelemovel = 0;
            ordPerfil = 0;
            ordAtivo = 0;

            if (ordNome == 0) {
                ordNome = 1;
            }
            else {
                ordNome = ordNome * (-1);
            }

            getData();
        }

        function ordenaEmail() {
            ordNome = 0;
            ordTelemovel = 0;
            ordPerfil = 0;
            ordAtivo = 0;

            if (ordEmail == 0) {
                ordEmail = 1;
            }
            else {
                ordEmail = ordEmail * (-1);
            }

            getData();
        }

        function ordenaTelemovel() {
            ordNome = 0;
            ordEmail = 0;
            ordPerfil = 0;
            ordAtivo = 0;

            if (ordTelemovel == 0) {
                ordTelemovel = 1;
            }
            else {
                ordTelemovel = ordTelemovel * (-1);
            }

            getData();
        }

        function ordenaPerfil() {
            ordNome = 0;
            ordEmail = 0;
            ordTelemovel = 0;
            ordAtivo = 0;

            if (ordPerfil == 0) {
                ordPerfil = 1;
            }
            else {
                ordPerfil = ordPerfil * (-1);
            }

            getData();
        }

        function ordenaAtivo() {
            ordNome = 0;
            ordEmail = 0;
            ordTelemovel = 0;
            ordPerfil = 0;

            if (ordAtivo == 0) {
                ordAtivo = 1;
            }
            else {
                ordAtivo = ordAtivo * (-1);
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
    </script>
</body>

</html>
