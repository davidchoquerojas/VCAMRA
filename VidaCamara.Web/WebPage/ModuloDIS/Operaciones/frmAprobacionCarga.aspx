<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmAprobacionCarga.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Operaciones.frmAprobacionCarga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="button_box">
        <asp:HyperLink ID="HyperLink1" CssClass="btn btn-default btn-fab btn-raised mdi-navigation-arrow-back pull-right"  ToolTip="Inicio" runat="server" NavigateUrl="~/Inicio"></asp:HyperLink>
    </div>
    <div class="tabBody">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="col-sm-3 col-md-3">Contrato</label>
                    <div class="col-sm-9 col-md-9">
                        <asp:DropDownList runat="server" CssClass="form-control">
                            <asp:ListItem Text="Contrato Febrero a marzo 2014"/>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-6 col-md-6">Tipo de Archivo</label>
                    <div class="col-sm-6 col-md-6">
                        <asp:DropDownList runat="server" CssClass="form-control">
                            <asp:ListItem Text="Todos" />
                            <asp:ListItem Text="Liquidaciones" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                 <div class="form-group">
                     <label class="col-sm-6 col-md-6">Fecha</label>
                     <div class="col-sm-6 col-md-6">
                         <asp:TextBox runat="server" CssClass="form-control" TextMode="Date"/> 
                     </div>
                 </div>
            </div>
        </div>
        <div class="table-responsive">
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <th>Archivo</th>
                        <th>Fecha de Carga</th>
                        <th># Registros</th>
                        <th>Moneda</th>
                        <th>Importe Total</th>
                        <th>Pago VC</th>
                        <th>Fecha Info</th>
                        <th>Usuario</th>
                        <th>Acción</th>
                        <th>Acción</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>LIQAADIC_20150905_1_1_086.CAM</td>
                        <td>05/09/2015 14:00</td>
                        <td>80</td>
                        <td>Soles</td>
                        <td>12,394.00</td>
                        <td>234.00</td>
                        <td>05/09/2015</td>
                        <td>JCamara</td>
                        <td><a>Aprobar</a></td>
                        <td><a>Elimina</a></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
