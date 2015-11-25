<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="VidaCamara.Web.WebPage.Inicio.frmLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"/>

    <link rel="stylesheet" href="../../Content/bootstrap.min.css" />
    <link rel="stylesheet" href="../../Content/material.min.css" />
    <link rel="stylesheet" href="../../Content/ripples.min.css" />
    <link rel="stylesheet" href="../../Content/roboto.min.css" />
    <link rel="stylesheet" href="../../Content/scss/login.min.css" />

    <link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.2/themes/redmond/jquery-ui.css" />
    
    <script src="../../Scripts/jquery-2.1.4.min.js"></script>
    <script src="../../Scripts/bootstrap.min.js" async defer></script>
    <script src="../../Scripts/jquery-ui-1.10.0.min.js"></script>
    <script src="../../Scripts/material.min.js"></script>
    <script src="../../Scripts/ripples.min.js" async defer></script>
    <script src="Resources/js/j-valida-login.js"></script>

    <title>Vida Cámara - Login </title>

    <style type="text/css">
        #login_error{color:red;text-align:center;font-size:1.1em;font-family:'Segoe UI Semilight','Open Sans',Verdana,Arial,Helvetica,sans-serif;}
    </style>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <div class="container-fluid">
            <div class="row">
                <div class="xol-sm-3 col-md-4"></div>
                <div class="col-sm-6 col-md-4 box_login">
                    <figure>
                        <img src="../../Resources/img/im_titulo.jpg" class="img-responsive" />
                    </figure>
                    <div class="form-group">
                        <label>Usuario</label>
                        <asp:TextBox runat="server" ID="txtUsuario" CssClass="form-control"  MaxLength="25" />
                    </div>
                    <div class="form-group">
                        <label>Password</label>
                        <asp:TextBox runat="server" ID="txtContrasena" CssClass="form-control" TextMode="Password" MaxLength="25"  />
                    </div>

                    <div class="form-group">
                        <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary pull-right" onclick="btnLogin_Click" Text="Aceptar" />
                        <button id="btnCancelar" class="btn btn-default pull-right"> Cancelar</button>
                    </div>
                </div>
                <div class="col-sm-3 col-md-4"></div>
            </div>
        </div>
        <div id="login_error"><asp:Label ID="lbl_error_login" runat="server" Text="..."></asp:Label></div>
    </form>
</body>
</html>
