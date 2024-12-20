<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ficha_manutencao.aspx.cs" Inherits="ficha_manutencao" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Ficha de Intervenção">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Ficha de Intervenção</title>
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
                <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block" href="../index.html">Manutenção Programada</a>

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
                        <h1 class="display-2 text-white">Manutenções Programadas</h1>
                        <p class="text-white mt-0 mb-5">Crie/edite os dados da manutenção</p>
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
                                            <h3 class="mb-0">Manutenção</h3>
                                        </td>
                                        <td style="width: 10%; text-align: right; cursor: pointer" runat="server">
                                            <img src='../general/assets/img/theme/setae.png' style='width: 30px; height: 30px' alt='Back' title='Back' onclick='back();'/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="card-body" id="divGrelha">
                            <form>
                                <h6 class="heading-small text-muted mb-4">Informação da Manutenção</h6>
                                <div class="row">
                                    <div class="col-md-6" id="divMaquinas" runat="server"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="form-control-label" for="txtDataHora">Data / Hora</label>
                                            <input type="datetime-local" id="txtDataHora" class="form-control form-control-alternative" value="2021-01-01T00:00">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="txtTecnico">Nome</label>
                                            <input type="text" id="txtTecnico" class="form-control form-control-alternative" placeholder="Técnico que efetuou a intervenção">
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="txtPecas">Peças Intervencionadas / Trocadas</label>
                                            <textarea id="txtPecas" rows="3" style="resize: none" class="form-control form-control-alternative" placeholder="Peças Intervencionadas / Trocadas"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="txtNotas">Observações</label>
                                            <textarea id="txtNotas" rows="3" style="resize: none" class="form-control form-control-alternative" placeholder="Descrição da Intervenção"></textarea>
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
                <input id="txtIdMaquina" runat="server" type="text" class="variaveis" />
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
            getData();
            setAltura();
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

        function showPopup(id) {
            document.getElementById(id).style.display = 'block';
        }

        function hidePopup(id) {
            document.getElementById(id).style.display = 'none';
        }

        function back() {
            var idMaquina = $('#txtIdMaquina').val();

            alertify.confirm("Tem a certeza que pretende sair?!<br />Todas os dados serão perdidos.",
                function () {
                    loadUrl("lista_manutencoes.aspx?id=" + idMaquina);
                },
                function () {

                }).set('labels', { ok: 'Sim', cancel: 'Não' }).set('title', 'Manutenções Programadas');
        }

        function getData() {
            var id = $('#txtAux').val().trim();
            if (id != null && id != 'null' && id != '') {
                $.ajax({
                    type: "POST",
                    url: "ficha_manutencao.aspx/getData",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');

                        var id_maquina = dados[0];
                        var data_hora = dados[1];
                        var tecnico = dados[2];
                        var pecas = dados[3];
                        var notas = dados[4];

                        $("#ddlMaquinas option[value=" + id_maquina + "]").attr('selected', 'selected');
                        var now = new Date(data_hora);
                        now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
                        document.getElementById('txtDataHora').value = now.toISOString().slice(0, 16);
                        $('#txtTecnico').val(tecnico);
                        $('#txtPecas').val(pecas);
                        $('#txtNotas').val(notas);
                    }
                });
            }
            else {
                $('#chkAtivo').attr('checked', true);
                getUserName();
                var now = new Date();
                now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
                document.getElementById('txtDataHora').value = now.toISOString().slice(0, 16);
            }
        }

        function saveData() {
            var id = $('#txtAux').val();

            var id_maquina = $('#ddlMaquinas').val();
            var data = $('#txtDataHora').val();
            var tecnico = $('#txtTecnico').val();
            var pecas = $('#txtPecas').val();
            var notas = $('#txtNotas').val();

            if (data == '' || data == null || data == undefined) {
                sweetAlertWarning('Data / Hora', 'Por favor indique a data e hora da intervenção');
                return;
            }
            else if (tecnico == '' || tecnico == null || tecnico == undefined) {
                sweetAlertWarning('Técnico', 'Por favor indique o técnico que efetuou a intervenção');
                return;
            }

            if (data != '' && data != null && data != undefined &&
                tecnico != '' && tecnico != null && tecnico != undefined) {
                $.ajax({
                    type: "POST",
                    url: "ficha_manutencao.aspx/saveData",
                    data: '{"id":"' + id + '","id_maquina":"' + id_maquina + '","data_hora":"' + data + '","tecnico":"' + tecnico + '","pecas":"' + pecas + '","notas":"' + notas + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        var idMaq = $('#txtIdMaquina').val();

                        if (dados[0] > 0) {
                            loadUrl('lista_manutencoes.aspx?id=' + idMaq);
                            return;
                        }
                        else {
                            sweetAlertWarning('Aviso', dados[1]);
                        }
                    }
                });
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
                    $('#txtTecnico').val(nome);

                    $.ajax({
                        type: "POST",
                        url: "../general/login.aspx/getTipoUser",
                        data: '{"i":"' + id + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (res) {
                            var tp = res.d;
                            if (tp == 'Técnico Helptech' || tp == 'Técnico Parceiro')
                                $("#txtTecnico").prop("readonly", true);
                        }
                    });
                }
            });
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
