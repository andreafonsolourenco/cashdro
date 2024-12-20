<%@ Page Language="C#" AutoEventWireup="true" CodeFile="config_ficha_utilizador.aspx.cs" Inherits="config_ficha_utilizador" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="CASHDRO Software - Ficha do Utilizador">
    <meta name="author" content="André Lourenço | Márcio Borges">
    <title>CASHDRO - Ficha do Utilizador</title>
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
    <link href="../vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
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
                        <h1 class="display-2 text-white">Utilizador</h1>
                        <p class="text-white mt-0 mb-5">Crie/edite os dados de utilizador</p>
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
                                            <h3 class="mb-0">Utilizador</h3>
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
                                <h6 class="heading-small text-muted mb-4">Informação do utilização</h6>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Nome</label>
                                            <input type="text" id="txtNome" class="form-control form-control-alternative" placeholder="Nome">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6" id="divTipoUtilizador" runat="server"></div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Contacto telefónico</label>
                                            <input type="text" id="txtTelemovel" class="form-control form-control-alternative" placeholder="Contacto telefónico">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Email</label>
                                            <input type="text" id="txtEmail" class="form-control form-control-alternative" placeholder="Email">
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="form-control-label" for="input-username">Password</label>
                                            <input type="password" id="txtPassword" class="form-control form-control-alternative" placeholder="Password">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">PIN de Operações</label>
                                            <input type="password" id="txtPin" class="form-control form-control-alternative" placeholder="Pin">
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="rowPartner">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Parceiro</label>
                                            <input type="text" id="txtPartner" class="form-control form-control-alternative" placeholder="Parceiro">
                                        </div>
                                    </div>
                                </div>

                                <div class="row" id="rowNMaq">
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Nº de Máquinas</label>
                                            <input type="text" id="txtNMaquinas" class="form-control form-control-alternative" placeholder="Nº de máquinas">
                                        </div>
                                    </div>
                                    <div class="col-md-6 variaveis" id="divNrClientesParceiro">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Nº de Clientes</label>
                                            <input type="text" id="txtNClientesParceiro" class="form-control form-control-alternative" placeholder="Nº de Clientes">
                                        </div>
                                    </div>
                                </div>



                                <div class="row" id="opcoesCashDrow1">
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Nome do estabelecimento</label>
                                            <input type="text" id="txtNomeEstab" class="form-control form-control-alternative" placeholder="Nome do estabelecimento">
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Pessoa responsável</label>
                                            <input type="text" id="txtPessoaContacto" class="form-control form-control-alternative" placeholder="Pessoa responsável">
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="opcoesCashDrow2">
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">NIF</label>
                                            <input type="text" id="txtNIF" class="form-control form-control-alternative" placeholder="NIF">
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Morada (Local)</label>
                                            <input type="text" id="txtMorada" class="form-control form-control-alternative" placeholder="Morada (Local)">
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="opcoesCashDrow3">
                                    <div class="col-md-6">
                                        <div class="form-group autocomplete">
                                            <label class="form-control-label" for="input-username">Cashdro (Nome)</label>
                                            <input type="text" id="txtCashDroNome" class="form-control form-control-alternative" placeholder="Cashdro (Nome)">
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
                                <h6 class="heading-small text-muted mb-4">Estado</h6>

                                <div class="row" style="padding-left: 0px">
                                    <div class="col-md-6">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkAtivo" type="checkbox">
                                            <label class="custom-control-label" for="chkAtivo">
                                                <span class="text-muted">Ativo</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkSuspenso" type="checkbox">
                                            <label class="custom-control-label" for="chkSuspenso">
                                                <span class="text-muted">Suspenso</span>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="chkMostraLigDireta" type="checkbox">
                                            <label class="custom-control-label" for="chkMostraLigDireta">
                                                <span class="text-muted">Mostrar Ligação Direta à Máquina</span>
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

        function getUserType() {
            var id = localStorage.loga;
            $.ajax({
                type: "POST",
                url: "../general/login.aspx/getTipoUser",
                data: '{"i":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var tipo = res.d;
                    var typeDDL = $('#ddlTipo option:selected').text();

                    if (tipo == 'Administrador' && typeDDL == 'Parceiro') {
                        $('#divNrClientesParceiro').fadeIn();
                    }
                    else {
                        $('#divNrClientesParceiro').fadeOut();
                    }
                }
            });
        }

        function getData() {
            var id = $('#txtAux').val();
            if (id != null && id != 'null') {
                $.ajax({
                    type: "POST",
                    url: "config_ficha_utilizador.aspx/getData",
                    data: '{"id":"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('<#SEP#>');

                        var tipo = dados[0];
                        var nome = dados[1];
                        var email = dados[2];
                        var password = dados[3];
                        var telemovel = dados[4];
                        var notas = dados[5];
                        var ativo = dados[6];
                        var id_tipo = dados[7];
                        var parceiro = dados[8];
                        var suspenso = dados[9];
                        var pin = dados[10];
                        var nmaquinas = dados[11];

                        var nif = dados[12];
                        var morada = dados[13];
                        var nomeestabelecimento = dados[14];
                        var pessoaresponsavel = dados[15];
                        var cashdronome = dados[16];
                        var mostraLigDireta = dados[17];
                        var nrClientesParceiro = dados[18];


                        $("#ddlTipo option[value=" + id_tipo + "]").attr('selected', 'selected');
                        $('#txtNome').val(nome);
                        $('#txtEmail').val(email);
                        $('#txtPassword').val(password);
                        $('#txtTelemovel').val(telemovel);
                        $('#txtNotas').val(notas);
                        $('#txtPartner').val(parceiro);
                        $('#txtPin').val(pin);
                        $('#txtNMaquinas').val(nmaquinas);
                        $('#txtNIF').val(nif);
                        $('#txtMorada').val(morada);
                        $('#txtNomeEstab').val(nomeestabelecimento);
                        $('#txtPessoaContacto').val(pessoaresponsavel);
                        $('#txtCashDroNome').val(cashdronome);
                        $('#txtNClientesParceiro').val(nrClientesParceiro);


                        if (ativo == "false")
                            $('#chkAtivo').attr('checked', false);
                        else
                            $('#chkAtivo').attr('checked', true);

                        if (suspenso == "false")
                            $('#chkSuspenso').attr('checked', false);
                        else
                            $('#chkSuspenso').attr('checked', true);

                        if (mostraLigDireta == "false")
                            $('#chkMostraLigDireta').attr('checked', false);
                        else
                            $('#chkMostraLigDireta').attr('checked', true);

                        getPartnersList();
                        verifyUserType();
                    }
                });
            }
            else {
                $('#chkAtivo').attr('checked', true);
                $('#chkMostraLigDireta').attr('checked', true);
                getPartnersList();
                verifyUserType();
            }
        }

        function getPartnersList() {
            $.ajax({
                type: "POST",
                url: "config_ficha_utilizador.aspx/getPartnerList",
                //data: '{"id":"' + id + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var dados = res.d.split('<#SEP#>');

                    autocomplete(document.getElementById("txtPartner"), dados);
                }
            });
        }

        function saveData() {
            var id = $('#txtAux').val();

            var nome = $('#txtNome').val();
            var tipo = $('#ddlTipo').val();
            var telemovel = $('#txtTelemovel').val();
            var email = $('#txtEmail').val();
            var password = $('#txtPassword').val();
            var notas = $('#txtNotas').val();
            var parceiro = $('#txtPartner').val();
            var pin = $('#txtPin').val();
            var nmaquinas = $('#txtNMaquinas').val();
            var nclientesparceiro = $('#txtNClientesParceiro').val().trim();

            var nif = $('#txtNIF').val();
            var morada = $('#txtMorada').val();
            var nomeestabelecimento = $('#txtNomeEstab').val();
            var pessoaresponsavel = $('#txtPessoaContacto').val();
            var cashdronome = $('#txtCashDroNome').val();

            var ativo = $('#chkAtivo').is(":checked");
            if (ativo) ativo = 1;
            else ativo = 0;

            var suspenso = $('#chkSuspenso').is(":checked");
            if (suspenso) suspenso = 1;
            else suspenso = 0;

            var ligDireta = $('#chkMostraLigDireta').is(":checked");
            if (ligDireta) ligDireta = 1;
            else ligDireta = 0;

            if (nome == '' || nome == null || nome == undefined) {
                sweetAlertWarning('Nome', 'Por favor indique o nome do utilizador');
                return;
            }
            else if (email == '' || email == null || email == undefined) {
                sweetAlertWarning('Email', 'Por favor indique o endereço de email');
                return;
            }
            else if (password == '' || password == null || password == undefined) {
                sweetAlertWarning('Password', 'Por favor indique a password de acesso');
                return;
            }
            else if (tipo == '' || tipo == null || tipo == undefined) {
                sweetAlertWarning('Tipo', 'Por favor indique o tipo de utilizador');
                return;
            }
            else if (pin == '' || pin == null || pin == undefined) {
                pin = '';
            }

            if (nome != '' && nome != null && nome != undefined &&
                email != '' && email != null && email != undefined &&
                password != '' && password != null && password != undefined) {
                $.ajax({
                    type: "POST",
                    url: "config_ficha_utilizador.aspx/saveData",
                    data: '{"id":"' + id + '","nome":"' + nome + '","tipo":"' + tipo + '","telemovel":"' + telemovel + '","email":"' + email
                        + '","password":"' + password + '","notas":"' + notas + '","ativo":"' + ativo + '","parceiro":"' + parceiro
                        + '","suspenso":"' + suspenso + '","pin":"' + pin + '","nmaquinas":"' + nmaquinas
                        + '","nif":"' + nif + '","morada":"' + morada + '","nomeestabelecimento":"' + nomeestabelecimento
                        + '","pessoaresponsavel":"' + pessoaresponsavel + '","cashdronome":"' + cashdronome + '","mostraligdireta":"' + ligDireta + '","nclientesparceiro":"' + nclientesparceiro + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        var dados = res.d.split('@');
                        if (dados[0] > 0)
                            window.location = "config_lista_utilizadores.aspx";
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

        function retroceder() {
            alertify.confirm("Tem a certeza que pretende sair?!<br />Todas os dados serão perdidos.",
                function () {
                    loadUrl("config_lista_utilizadores.aspx");
                },
                function () {

                }).set('labels', { ok: 'Sim', cancel: 'Não' }).set('title', 'Utilizador');
        }

        function sweetAlertWarning(subject, msg) {
            swal(
                subject,
                msg,
                'warning'
            )
        }

        function verifyUserType() {
            var type = $('#ddlTipo option:selected').text();

            if (type == 'Cliente' || type == 'Técnico Parceiro') {
                $('#rowPartner').fadeIn();
            }
            else {
                $('#rowPartner').fadeOut();
            }

            if (type == 'Administrador' || type == 'Parceiro') {
                $('#rowNMaq').fadeIn();
            }
            else {
                $('#rowNMaq').fadeOut();
            }

            // Se for cliente mostra os campos especificos de cliente, senao esconde
            if (type == 'Cliente') {
                $('#opcoesCashDrow1').fadeIn();
                $('#opcoesCashDrow2').fadeIn();
                $('#opcoesCashDrow3').fadeIn();
            }
            else {
                $('#opcoesCashDrow1').fadeOut();
                $('#opcoesCashDrow2').fadeOut();
                $('#opcoesCashDrow3').fadeOut();
            }

            getUserType();
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
                closeAllLists(e.target);
            });
        }
    </script>
</body>

</html>
