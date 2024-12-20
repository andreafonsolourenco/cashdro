﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_lista_tipos_utilizador.aspx.cs" Inherits="config_lista_tipos_utilizador" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Lista de Tipos de Utilizador">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Lista de Tipos de Utilizador</title>
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
    </style>
</head>

<body onload="getData();">

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
                        <h1 class="display-2 text-white">Tipos de Utilizador</h1>
                        <p class="text-white mt-0 mb-5">Parametrize os tipos de utilizador e as respetivas permissões de acesso ao software.</p>
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
                                    <td style="width: 85%">
                                        <h3 class="mb-0">Tipos de Utilizador</h3>
                                    </td>
                                    <td style="width: 15%; text-align: right;">
                                        <img src="../general/assets/img/theme/new.png" style="width: 30px; height: 30px" class="pointer" alt="Criar novo Tipo de Utilizador" title="Criar novo Tipo de Utilizador" onclick="novo();" />
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
        var ordTipo = 0;
        var ordAdmin = 0;

        $(document).ready(function () {
            setAltura();

            $("#txtPesquisa").focus();
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

        function setAltura() {
            $("#fraContent").height($(window).height());
        }

        function keyPesquisa(e) {
            if (e.keyCode == 13) {
                getData();
            }
        }

        function novo() {
            window.location = "config_ficha_tipo_utilizador.aspx?id=null";
        }

        function edita(id) {
            window.location = "config_ficha_tipo_utilizador.aspx?id=" + id;
        }

        function retroceder() {
            loadUrl("parametrizacao.aspx");
        }

        function apaga(id) {
            swal({
                title: 'Eliminar Tipo de Utilizador',
                text: "O tipo de utilizador será eliminado. Confirma?",
                type: 'question',
                showCancelButton: true,
                confirmButtonColor: '#007351',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sim',
                cancelButtonText: 'Não'
            }).then(function () {
                $.ajax({
                    type: "POST",
                    url: "config_lista_tipos_utilizador.aspx/delRow",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        var res = dados[0];
                        var resMsg = dados[1];

                        if (res == -1) {
                            sweetAlertWarning("Eliminar Tipo de Utilizador", resMsg);
                        }
                        else
                            loadUrl("config_lista_tipos_utilizador.aspx");
                    }
                });
            });
        }



        function loadUrl(url) {
            window.location = url;
        }


        // Web services

        function getData() {
            var pesquisa = $('#txtPesquisa').val();
            var order = "";

            if (ordTipo == 0 && ordAdmin == 0) {
                order = ' ORDER BY administrador desc, nome asc ';
            }
            else {
                order = ' ORDER BY ';

                if (ordTipo != 0) {
                    order += ordTipo == -1 ? ' nome desc ' : ' nome asc ';
                }
                else if (ordAdmin != 0) {
                    order += ordAdmin == -1 ? ' administrador desc ' : ' administrador asc ';
                }
            }

            $.ajax({
                type: "POST",
                url: "config_lista_tipos_utilizador.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '","order":"' + order + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaRegistos').html(res.d);
                }
            });
        }

        function ordenaTipo() {
            ordAdmin = 0;

            if (ordTipo == 0) {
                ordTipo = 1;
            }
            else {
                ordTipo = ordTipo * (-1);
            }

            getData();
        }

        function ordenaAdministrador() {
            ordTipo = 0;

            if (ordAdmin == 0) {
                ordAdmin = 1;
            }
            else {
                ordAdmin = ordAdmin * (-1);
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
