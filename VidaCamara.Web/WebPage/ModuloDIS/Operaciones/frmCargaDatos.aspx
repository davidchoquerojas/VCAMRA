<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/Inicio/mpFEPCMAC.Master" AutoEventWireup="true" CodeBehind="frmCargaDatos.aspx.cs" Inherits="VidaCamara.Web.WebPage.ModuloDIS.Operaciones.CargaDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaDatos.js"></script>
<script src="/WebPage/ModuloDIS/Operaciones/js/CargaRsp.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--Comienzo de los Tabs-->
   <script runat="server">
        protected void menuTabs_MenuItemClick(object sender, MenuEventArgs e)
        {
            multiTabs.ActiveViewIndex = Int32.Parse(menuTabs.SelectedValue);
        }
    </script>
    <div class="button_box">
        <asp:LinkButton ID="btn_borrar" runat="server" CssClass="btn btn-default btn-fab btn-raised mdi-editor-insert-drive-file pull-right" ToolTip="Nuevo"></asp:LinkButton>
        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-default btn-fab btn-raised mdi-content-save pull-right" ToolTip="Guardar" OnClientClick="SomeMethod()== true;showLoading();"></asp:LinkButton>
        <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-default btn-fab btn-raised mdi-action-delete pull-right" ToolTip="Eliminar"></asp:LinkButton>
        <asp:HyperLink ID="HyperLink1" CssClass="btn btn-default btn-fab btn-raised mdi-navigation-arrow-back pull-right"  ToolTip="Inicio" runat="server" NavigateUrl="~/Inicio"></asp:HyperLink>
    </div>
        <%--<div class="btn_crud">
            <asp:HyperLink CssClass="btn_crud_button"  ToolTip="Inicio" runat="server" ImageUrl="~/Resources/Imagenes/u158_normal.png" NavigateUrl="~/Inicio"></asp:HyperLink>
            <asp:ImageButton  CssClass="btn_crud_button" ID="btnGuardar" runat="server" ToolTip="Guardar" ImageUrl="~/Resources/Imagenes/upload.png" OnClick="btnGuardar_Click" />
        </div>--%>
        <asp:Menu id="menuTabs" CssClass="menuTabs" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                    Orientation="Horizontal" OnMenuItemClick="menuTabs_MenuItemClick" Runat="server">
                <Items >
                    <asp:MenuItem  Text="Carga" Value="0" Selected="true" />
                    <asp:MenuItem  Text="Detalle" Value="1"/>
                    <asp:MenuItem Text="Información Archivo" Value="2"/>
                </Items>
            <StaticMenuItemStyle CssClass="tab"></StaticMenuItemStyle>
            <StaticSelectedStyle CssClass="selectedTab" BackColor="#006666"></StaticSelectedStyle>
        </asp:Menu>
      
    <!--Cuerpo de los tabs-->
    <div class="tabBody">
        <asp:MultiView id="multiTabs" ActiveViewIndex="0" Runat="server">
            <asp:View ID="view1" runat="server">                
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3" for="fileUpload">Contrato (*)</label>
                            <div class="col-sm-9 col-md-9">
                                <asp:DropDownList runat="server" CssClass="form-control">
                                    <asp:ListItem>Contrato de febrero del 2015</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3" for="">Nombre del Archivo (*)</label>
                            <div class="col-sm-9 col-md-9">
                                <asp:FileUpload ID="fileUpload"  runat="server"/>
                                <div class="input-group">
                                    <input type="text" readonly="" class="form-control" placeholder="Elija el archivo a cargar">
                                    <span class="input-group-btn input-group-sm">
                                        <button type="button" class="btn btn-fab btn-fab-mini mdi-content-archive"></button>
                                    </span>
                                </div>
                                <span class="material-input"></span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Tipo de Archivo</label>
                            <div class="col-sm-9 col-md-9">
                                <asp:DropDownList runat="server" CssClass="form-control">
                                    <asp:ListItem>Liquidacion Aporte Adicional</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Moneda</label>
                            <div class="col-sm-9 col-md-9">
                                Soles
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Total Importe</label>
                            <div class="col-sm-9 col-md-9">
                                20,000
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Registros Procesados</label>
                            <div class="col-sm-9 col-md-9">
                                300
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Registros Observados</label>
                            <div class="col-sm-9 col-md-9">
                                40
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="view2" runat="server">               
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Nombre del Archivo</label>
                            <div class="col-sm-9 col-md-9">
                                <asp:DropDownList runat="server" CssClass="form-control">
                                    <asp:ListItem Text="LIQPSEP_20150905_1_1_086.CAM" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="col-sm-3 col-md-3">Tipo de Archivo</label>
                            <div class="col-sm-9 col-md-9">
                                Liquidaciones de Pago de Sepelio
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responsive" id="CargaRSP">
                      <asp:GridView ID="GridView1" runat="server" Width="100%" HorizontalAlign="center"
                            CssClass="CSSTableGenerator">
                      </asp:GridView>
                </div> 
                <div class="table-responsive">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>FILA</th>
                                <th>COLUMNA</th> 
                                <th>CAMPO</th>
                                <th>CONTENIDO</th> 
                                <th>OBSERVACION</th>
                            </tr>
                        </thead>
                    </table>
                </div>             
            </asp:View>
            <asp:View ID="view3" runat="server">
                <div class="table-responsive">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>FILA</th>
                                <th>COLUMNA</th> 
                                <th>CAMPO</th>
                                <th>CONTENIDO</th> 
                                <th>OBSERVACION</th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </asp:View>
        </asp:MultiView>

        <asp:HiddenField ID="hdf_excel_error" runat="server" Value="0"/>
        <asp:HiddenField ID="hdf_estado_borrar" runat="server" Value="A"/>

        <div id="alert" class="modal fade" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <img src="../../../Resources/img/loading19.gif" alt="" height="60" style="margin:auto;display:table-cell;" />
            </div>
        </div>
    </div>
</asp:Content>
