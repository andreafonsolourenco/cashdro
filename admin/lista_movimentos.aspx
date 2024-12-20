﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lista_movimentos.aspx.cs" Inherits="lista_movimentos" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Movimentos">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Movimentos</title>
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

        /*.autocomplete {
  /*the container must be positioned relative:
  position: relative;
  display: inline-block;
}
input {
  border: 1px solid transparent;
  background-color: #f1f1f1;
  padding: 10px;
  font-size: 16px;
}
input[type=text] {
  background-color: #f1f1f1;
  width: 100%;
}
input[type=submit] {
  background-color: DodgerBlue;
  color: #fff;
}*/
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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block">Movimentos</a>
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
                                    <td style="width:100%" colspan="2">
                                        <h3 class='mb-0'>Movimentos Diários</h3>
                                        <h4 class="mb-0" runat="server" id="maquina"></h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%; text-align: left">
                                        <div class="form-group autocomplete" style="float: left;">
                                            <label class="form-control-label" for="input-username">Data Inicial</label>
                                            <input type="date" id="dataInicial" class="form-control form-control-alternative" onchange="getData();">
                                        </div>
                                        <div class="form-group autocomplete" style="float: right;">
                                            <label class="form-control-label" for="input-username">Data Final</label>
                                            <input type="date" id="dataFinal" class="form-control form-control-alternative" onchange="getData();">
                                        </div>
                                    </td>
                                    <td style="width: 50%; text-align: right; cursor: pointer" runat="server">
                                        <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px' alt='Back' title='Back' onclick='back();'/>
                                        <a href="#!" class="btn btn-info" onclick="exportTableToExcel('xlsx');">Exportar Excel</a>
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

            <input id="txtAux" runat="server" type="text" class="variaveis" />
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
    <!-- Export JS -->
    <script src="../general/assets/js/xlsx.full.min.js"></script>

    <script>
        var ordOperacao = 0;
        var ordData = 0;
        var ordValor = 0;
        var ordValorEm = 0;
        var ordValorFora = 0;
        var ordValorEntr = 0;
        var ordVendedor = 0;
        var ordUtilizador = 0;

        $(document).ready(function () {
            loga();
            setAltura();
            getTotals();
            setInterval(function () {
                getTotals();
            }, 5000);

            $("#txtPesquisa").focus();
            setDatePickerValues();
            getData();
            defineTablesMaxHeight();
        });

        $(window).resize(function () {
            setAltura();
            defineTablesMaxHeight();
        });

        function setDatePickerValues() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            today = yyyy + '-' + mm + '-' + dd;

            document.getElementById('dataInicial').valueAsDate = new Date();
            document.getElementById("dataInicial").setAttribute("max", today);
            document.getElementById('dataFinal').valueAsDate = new Date();            
            document.getElementById("dataFinal").setAttribute("max", today);
        }

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
                    url: "lista_movimentos.aspx/trataExpiracao",
                    data: '{"i":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');
                        var ret = parseInt(dados[0]);
                        var retMsg = dados[1];

                        // OK
                        getUserName();
                    }
                });
            }
        }

        function getUserName() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "lista_movimentos.aspx/getUname",
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

        function back() {
            var url = "menu_consulta.aspx?id=" + $('#txtAux').val();
            loadUrl(url);
        }

        function loadUrl(url) {
            window.location = url;
        }

        // Web services

        function getData() {
            loadingOn('A carregar os movimentos.<br />Por favor, aguarde...');
            var pesquisa = $('#txtPesquisa').val();
            var order = "";

            if (ordOperacao == 0 && ordData == 0 && ordValor == 0 && ordValorEm == 0 && ordValorFora == 0 && ordValorEntr == 0 && ordVendedor == 0 && ordUtilizador == 0) {
                order = ' ORDER BY data_mov desc';
            }
            else {
                order = ' ORDER BY ';

                if (ordOperacao != 0) {
                    order += ordOperacao == -1 ? ' tipo_mov desc, data_mov desc ' : ' tipo_mov asc, data_mov desc ';
                }
                else if (ordData != 0) {
                    order += ordData == -1 ? ' data_mov desc ' : ' data_mov desc ';
                }
                else if (ordValor != 0) {
                    order += ordValor == -1 ? ' valor_mov desc, data_mov desc ' : ' valor_mov asc, data_mov desc ';
                }
                else if (ordValorEm != 0) {
                    order += ordValorEm == -1 ? ' valor_transferido desc, data_mov desc ' : ' valor_transferido asc, data_mov desc ';
                }
                else if (ordValorFora != 0) {
                    order += ordValorFora == -1 ? ' valor_saida desc, data_mov desc ' : ' valor_saida asc, data_mov desc ';
                }
                else if (ordValorEntr != 0) {
                    order += ordValorEntr == -1 ? ' valor_entrada desc, data_mov desc ' : ' valor_entrada asc, data_mov desc ';
                }
                else if (ordVendedor != 0) {
                    order += ordVendedor == -1 ? ' vend_mov desc, data_mov desc ' : ' vend_mov asc, data_mov desc ';
                }
                else if (ordUtilizador != 0) {
                    order += ordUtilizador == -1 ? ' user_mov desc, data_mov desc ' : ' user_mov asc, data_mov desc ';
                }
            }

            var di = $('#dataInicial').val();
            var df = $('#dataFinal').val();

            $.ajax({
                type: "POST",
                url: "lista_movimentos.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '","order":"' + order + '","idMaq":"' + $('#txtAux').val() + '","idUser":"' + localStorage.loga + '","datainicial":"' + di + '","datafinal":"' + df + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaRegistos').html(res.d);
                    $('#nrMovimentos').html($('#countElements').html());
                    loadingOff();
                }
            });
        }

        function getTotals() {
            var id_utilizador = localStorage.loga;

            $.ajax({
                type: "POST",
                url: "lista_movimentos.aspx/getTotais",
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
                }
            });
        }

        function ordenaOperacao() {
            ordData = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordOperacao == 0) {
                ordOperacao = 1;
            }
            else {
                ordOperacao = ordOperacao * (-1);
            }

            getData();
        }

        function ordenaData() {
            ordOperacao = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordData == 0) {
                ordData = 1;
            }
            else {
                ordData = ordData * (-1);
            }

            getData();
        }

        function ordenaValor() {
            ordData = 0;
            ordOperacao = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordValor == 0) {
                ordValor = 1;
            }
            else {
                ordValor = ordValor * (-1);
            }

            getData();
        }

        function ordenaValorEm() {
            ordData = 0;
            ordValor = 0;
            ordOperacao = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordValorEm == 0) {
                ordValorEm = 1;
            }
            else {
                ordValorEm = ordValorEm * (-1);
            }

            getData();
        }

        function ordenaValorFora() {
            ordData = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordOperacao = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordValorFora == 0) {
                ordValorFora = 1;
            }
            else {
                ordValorFora = ordValorFora * (-1);
            }

            getData();
        }

        function ordenavalorEntr() {
            ordData = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordOperacao = 0;
            ordVendedor = 0;
            ordUtilizador = 0;

            if (ordValorEntr == 0) {
                ordValorEntr = 1;
            }
            else {
                ordValorEntr = ordValorEntr * (-1);
            }

            getData();
        }

        function ordenaVendedor() {
            ordData = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordOperacao = 0;
            ordUtilizador = 0;

            if (ordVendedor == 0) {
                ordVendedor = 1;
            }
            else {
                ordVendedor = ordVendedor * (-1);
            }

            getData();
        }

        function ordenaUser() {
            ordData = 0;
            ordValor = 0;
            ordValorEm = 0;
            ordValorFora = 0;
            ordValorEntr = 0;
            ordVendedor = 0;
            ordOperacao = 0;

            if (ordUtilizador == 0) {
                ordUtilizador = 1;
            }
            else {
                ordUtilizador = ordUtilizador * (-1);
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
                inp.val()
                closeAllLists(e.target);
            });
        }

        function exportTableToExcel(type, fn, dl) {
            var di = $('#dataInicial').val();
            var df = $('#dataFinal').val();
            var tableName = "TabelaMovimentos_" + di + "_" + df + ".";

            var elt = document.getElementById('tableMovimentos');
            var wb = XLSX.utils.table_to_book(elt, { sheet: di + "_" + df });
            return dl ?
                XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
                XLSX.writeFile(wb, fn || (tableName + (type || 'xlsx')));
        }
    </script>
</body>

</html>
