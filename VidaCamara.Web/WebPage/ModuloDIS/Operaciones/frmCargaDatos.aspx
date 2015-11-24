<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmCargaDatos.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Operaciones.CargaDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/Resources/CSS/Progressbar.css" rel="stylesheet" />
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaDatos.js"></script>
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaRsp.js"></script>

<script type="text/javascript">

    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            loading.css({ top: '40%', left: '45%' });
        }, 200);
    }
    $('form').live("submit", function () {
        var excelerror = $("#ctl00_ContentPlaceHolder1_hdf_excel_error").val();
        if (excelerror == 0) {
            ShowProgress();
        }
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--Comienzo de los Tabs-->
   <script runat="server">
        protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
        {
            multiTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);
        }
    </script>

        <div class="btn_crud">
            <asp:HyperLink CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btnGuardar" runat="server" ToolTip="Guardar" ImageUrl="~/Resources/Imagenes/upload.png" OnClick="btnGuardar_Click" />
        </div>
        <asp:Menu id="menuTabs" CssClass="menuTabs" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                    Orientation="Horizontal" OnMenuItemClick="menuTabs_MenuItemClick" Runat="server">
                <Items >
                    <asp:MenuItem  Text="CargaLogica de Datos" Value="0" Selected="true" />
                    <asp:MenuItem  Text="Resultado" Value="1"/>
                </Items>
            <StaticMenuItemStyle CssClass="tab"></StaticMenuItemStyle>
            <StaticSelectedStyle CssClass="selectedTab" BackColor="#006666"></StaticSelectedStyle>
        </asp:Menu>
      
    <!--Cuerpo de los tabs-->
    <div class="tabBody">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <!--VISTA CARGA DE DATOS-->
            <asp:View ID="view1" runat="server">                

                <label class="label_to" for="fileUpload">Archivo (*)</label>
                <asp:FileUpload CssClass="input_to" ID="fileUpload" ToolTip="Selecione el archivo a subir" runat="server" Height="25px" Width="48.4%" />

            </asp:View>

            <!--seccion de RSP-->
            <asp:View ID="view2" runat="server">               

                <div class="iframe" id="CargaRSP">
                      <asp:GridView ID="GridView1" runat="server" Width="100%" HorizontalAlign="center"
                            CssClass="CSSTableGenerator">
                      </asp:GridView>
                </div>               
               
            </asp:View>
        </asp:MultiView>

        <asp:HiddenField ID="hdf_excel_error" runat="server" Value="0"/>
        <asp:HiddenField ID="hdf_estado_borrar" runat="server" Value="A"/>

        <div class="loading">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Resources/Imagenes/loading19.gif" />
        </div>
    </div>
</asp:Content>
