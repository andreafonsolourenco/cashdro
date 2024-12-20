<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_upload_partner_images.aspx.cs" Inherits="config_upload_partner_images" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Lista de Tipos de Utilizador">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Upload Imagens dos Parceiros</title>
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

        .tbodyTrSelected:hover {
            background-color: gray;
        }

        .tbodyTrSelected {
            background-color: #11cdef;
        }

        .back_button {
            width: 30px; 
            height: 30px;
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
                        <h1 class="display-2 text-white">Logos dos Parceiros</h1>
                        <p class="text-white mt-0 mb-5">Parametrize os logotipos a serem usados no software por cada Parceiro e respetivos Clientes.</p>
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
                                    <td style="width: 50%">
                                        <h3 class="mb-0">Logos dos Parceiros</h3><br />
                                        <img src='../general/assets/img/theme/setae.png' class="back_button pointer" alt='Back' title='Back' onclick='retroceder();'/>
                                    </td>
                                    <td style="width: 50%; text-align: right;">
                                        <form id="formUploadPhoto" runat="server">
                                            <asp:TextBox ID="photoName" runat="server" CssClass="form-control variaveis" ClientIDMode="Static" />
                                            <asp:FileUpload id="FileUploadControl" runat="server" CssClass="btn btn-info"/>
                                            <asp:Button runat="server" id="UploadButton" text="Carregar Foto" onclick="UploadButton_Click" CssClass="btn btn-info"/>
                                            <br />
                                            <asp:Label runat="server" id="StatusLabel" text="Estado do Carregamento: " />
                                        </form>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="card shadow" id="divGrelha">
                            <div class="table-responsive">
                                <div id="divGrelhaRegistos"></div>
                            </div>
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

        function loadUrl(url) {
            window.location = url;
        }

        function retroceder() {
            loadUrl("parametrizacao.aspx");
        }


        // Web services

        function getData() {
            var pesquisa = $('#txtPesquisa').val();

            $.ajax({
                type: "POST",
                url: "config_upload_partner_images.aspx/getGrelha",
                data: '{"pesquisa":"' + pesquisa + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('#divGrelhaRegistos').html(res.d);
                }
            });
        }

        function selectRow(id, x) {
            var i = 0;

            for (i = 0; i < parseInt($('#countElements').html()); i++) {
                $('#ln_' + i.toString()).removeClass('tbodyTrSelected');
            }

            if ($('#lblidselected').html() == '') {
                $('#ln_' + x.toString()).addClass('tbodyTrSelected');
                $('#lblidselected').html(id);
                $('#photoName').val(id);

                //if ($.UrlExists('img/socios/' + nr_socio + '.jpg'))
                //    $('#photoDiv').html('<img src="img/socios/' + nr_socio + '.jpg" style="height: 100%; width: auto" />');
                //else {
                //    //$('#img_socio').height($('#divDados').height());
                //    $('#photoDiv').html('');
                //    $('#photoDiv').height(150);
                //}
            }
            else {
                if ($('#lblidselected').html() == id) {
                    $('#lblidselected').html('');
                    $('#photoName').val('');
                    //$('#photoDiv').html('');
                    //$('#photoDiv').height(150);
                }
                else {
                    $('#ln_' + x.toString()).addClass('tbodyTrSelected');
                    $('#lblidselected').html(id);
                    $('#photoName').val(id);

                    //if ($.UrlExists('img/socios/' + nr_socio + '.jpg'))
                    //    $('#photoDiv').html('<img src="img/socios/' + nr_socio + '.jpg" style="height: 100%; width: auto" />');
                    //else {
                    //    //$('#img_socio').height($('#divDados').height());
                    //    $('#photoDiv').html('');
                    //    $('#photoDiv').height(150);
                    //}
                }
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
