using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using VidaCamara.SBS.Entity;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.Mantenimiento
{
    public partial class General : System.Web.UI.Page
    {
        static int total;
        static String[] mes = { "-", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        static int totalContrato;
        bValidarAcceso accesso = new bValidarAcceso();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["pagina"] = "OTROS";
            if (Session["username"] == null)
                Response.Redirect("Login?go=0");
            else
            {
                if (!accesso.GetValidarAcceso(Request.QueryString["go"]))
                {
                    Response.Redirect("Error");
                }
            }

            if (!IsPostBack){
                bTablaVC concepto = new bTablaVC();
                ParametroList();
                concepto.SetEstablecerDataSourceConcepto(ddl_ramo_prima_c,"05");
                concepto.SetEstablecerDataSourceConcepto(ddl_seniestro_c,"04");
                concepto.SetEstablecerDataSourceConcepto(ddl_moneda_c,"10");
                concepto.SetEstablecerDataSourceConcepto(ddl_contratante_c,"14");
                SetLLenadoContrato();
                concepto.SetEstablecerDataSourceConcepto(ddl_reasegurador_r,"01");
                concepto.SetEstablecerDataSourceConcepto(ddl_modalidad_c,"06");
                concepto.SetEstablecerDataSourceConcepto(ddl_tipcont_det_r,"16");
                concepto.SetEstablecerDataSourceConcepto(ddl_tipcon_c,"07");
                concepto.SetEstablecerDataSourceConcepto(ddl_calificadora_r,"02");
                concepto.SetEstablecerDataSourceConcepto(ddl_crediticia_r,"11");
                llenarEstado("09","U");
            }
        }

        //LLENADO DE PARAMETROS
        private void ParametroList() {
            bGeneralVC bg = new bGeneralVC();
            List<eGeneral> list = bg.GetSelecionarGeneral();
            if (list.Count > 0)
            {
                txt_idempresa.Value = list[0]._idEmpresa.ToString();
                txt_empresa.Text = list[0]._descripcion.ToString();
                txt_ruc.Text = list[0]._rucEmpresa.ToString();
                txt_vigente.Text = list[0]._anoVigente.ToString();
                txt_mes_vig.Text = list[0]._mesVigente.ToString();
                txt_ruta_archivo.Text = list[0]._Ruta_Archivo;
                txt_cantidad_decimal.Text = list[0]._Nro_Decimal.ToString();
                txt_tcamesCont.Text = list[0]._tcaMes.ToString();
                txt_tcaCierre.Text = list[0]._tcaAno.ToString();
                bg.ParametroListSession();
            }
        }
        //LLENADO DE LISTA CONTRATO DETALLE
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object ContratoDetalleList(int jtStartIndex, int jtPageSize, string jtSorting, String WhereBy)
        {
            int total;
            int indexPage;
            if (jtStartIndex != 0)
            {
                indexPage = jtStartIndex / jtPageSize;
            }
            else
            {
                indexPage = jtStartIndex;
            }
            eContratoDetalleVC o = new eContratoDetalleVC();
            o._inicio = indexPage;
            o._fin = jtPageSize;
            o._orderby = jtSorting.Substring(1).ToUpper();
            o._nro_Contrato = WhereBy.Trim();

            bContratoDetalleVC tb = new bContratoDetalleVC();
            List<eContratoDetalleVC> list = tb.GetSelecionarContratoDetalle(o,out total);
            return new { Result = "OK", Records = list, TotalRecordCount = total };
        }
        //LLENADO DE CONTRATOO
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object ContratoList(int jtStartIndex, int jtPageSize, string jtSorting, String WhereBy)
        {
            int total;
            int indexPage;
            if (jtStartIndex != 0)
            {
                indexPage = jtStartIndex / jtPageSize;
            }
            else
            {
                indexPage = jtStartIndex;
            }
            eContratoVC o = new eContratoVC();
            o._inicio = indexPage;
            o._fin = jtPageSize;
            o._orderby = jtSorting.Substring(1).ToUpper();
            o._nro_Contrato = WhereBy.Trim();
            o._estado = "R";

            bContratoVC tb = new bContratoVC();
            List<eContratoVC> list = tb.GetSelecionarContrato(o, out total);
            return new { Result = "OK", Records = list, TotalRecordCount = total };
        }

        protected void btnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            int tab = Convert.ToInt16(menuTabs.SelectedValue);
            if (tab == 0)
            {
                   SetInsertarGeneral();
            }
            else if (tab == 1)
            {
                SetInsertarActualizarContrato();
            }
            else if (tab == 2)
            {
                SetInsertarActualizarContratoDetalle();
            }
        }
        //botton de borrar
        protected void btn_borrar_Click(object sender, ImageClickEventArgs e)
        {
            int tab = Convert.ToInt16(menuTabs.SelectedValue);
            if (tab == 0)
            {
                MessageBox("Formulario no Borrable");
            }
            else if (tab == 1)
            {
                SetEliminarParamentro("CONTRATO", txt_idContrato_c.Value);
            }
            else if (tab == 2)
            {
                SetEliminarParamentro("CONTRATO_DETALLE", txt_idContratoDetalle_c.Value);
            }
        }
        //#region funciones
        private void SetInsertarGeneral() {
            try
            {
                if (Int32.Parse(txt_idempresa.Value) == 0)
                {
                    eGeneral o = new eGeneral();
                    o._descripcion = txt_empresa.Text;
                    o._rucEmpresa = txt_ruc.Text;
                    o._anoVigente = Convert.ToInt32(txt_vigente.Text);
                    o._mesVigente = Convert.ToInt32(txt_mes_vig.Text);
                    o._Ruta_Archivo = txt_ruta_archivo.Text;
                    o._Nro_Decimal = Convert.ToInt32(txt_cantidad_decimal.Text);
                    o._tcaMes = Decimal.Parse(txt_tcamesCont.Text);
                    o._tcaAno = Decimal.Parse(txt_tcaCierre.Text);
                    o._estado = "A";
                    o._usureg = Session["username"].ToString();

                    bGeneralVC control = new bGeneralVC();
                    Int32 resp = control.SetInsertarGeneral(o);
                    if (resp != 0)
                    {
                        MessageBox("Registro Grabado Correctamente!");
                        ParametroList();
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
                else
                {
                    SetActualizarGeneral();
                }
            }
            catch (Exception e) {
                MessageBoxcCatch("ERROR =>" + e.Message);
            }
        }
        //funcion de insertar contrato
        private void SetActualizarGeneral() {
            try
            {
                eGeneral o = new eGeneral();
                o._idEmpresa = Int32.Parse(txt_idempresa.Value);
                o._descripcion = txt_empresa.Text;
                o._rucEmpresa = txt_ruc.Text;
                o._anoVigente = Convert.ToInt32(txt_vigente.Text);
                o._mesVigente = Convert.ToInt32(txt_mes_vig.Text);
                o._Ruta_Archivo = txt_ruta_archivo.Text;
                o._Nro_Decimal = Convert.ToInt32(txt_cantidad_decimal.Text);
                o._tcaMes = Decimal.Parse(txt_tcamesCont.Text);
                o._tcaAno = Decimal.Parse(txt_tcaCierre.Text);
                o._estado = "A";
                o._usumod = Session["username"].ToString();

                bGeneralVC control = new bGeneralVC();
                Int32 resp = control.SetActualizarGeneral(o);
                if (resp != 0)
                {
                    MessageBox("Registro Actualizado Correctamente!");
                    ParametroList();
                }
                else
                {
                    MessageBox("Ocurrio un Error en el Servidor!");
                }
            }
            catch (Exception e) { 
                MessageBoxcCatch("ERROR =>" +e.Message);
            }
        }
        //actualiza y inserta contrato
        private void SetInsertarActualizarContrato() {

            try
            {
                Int32 resp = 0;
                eContratoVC c = new eContratoVC();
                c._id_Empresa = Convert.ToInt32(Session["idempresa"]);
                c._ide_Contrato = Convert.ToInt32(txt_idContrato_c.Value);
                c._nro_Contrato = txt_nrocont_c.Text;
                c._cod_Ramo_Sin = ddl_seniestro_c.SelectedItem.Value;
                c._cod_Ramo_pri = ddl_ramo_prima_c.SelectedItem.Value;
                c._cla_Contrato = ddl_clasecontrato_c.SelectedItem.Value;
                c._fec_Ini_Vig = DateTime.Parse(txt_fecini_c.Text);
                c._fec_Fin_Vig = DateTime.Parse(txt_fecfin_c.Text);
                c._tip_Contrato = ddl_tipcon_c.SelectedItem.Value;
                c._cod_Moneda = ddl_moneda_c.SelectedItem.Value;
                c._cod_Contratante = ddl_contratante_c.SelectedItem.Value;
                c._por_Participa_Cia = Convert.ToDecimal(txt_participacion_cia_c.Text);
                c._por_Tasa_Riesgo = Convert.ToDecimal(txt_tasariesgo_c.Text);
                c._por_Tasa_Reaseguro = Convert.ToDecimal(txt_tasareaseguro_c.Text);
                c._por_Impuesto = Convert.ToDecimal(txt_impuesto_c.Text);
                c._Centro_Costo = txt_centro_costo.Text;
                c._des_Contrato = txt_descrip_contrato.Text;
                c._estado = ddl_estado_c.SelectedItem.Value;
                c._mod_Contrato = ddl_modalidad_c.SelectedItem.Value;
                c._por_Retencion = Convert.ToDecimal(txt_retencion_c.Text);
                c._por_Cesion = Convert.ToDecimal(txt_cesion_c.Text);
                c._mto_Max_Retencion = Convert.ToDecimal(txt_montomax_retenc_c.Text);
                c._mto_Max_Cesion = Convert.ToDecimal(txt_montomax_cesion_c.Text);
                c._mto_Pleno = Convert.ToDecimal(txt_montopleno_c.Text);
                c._nro_Linea_Mult = Convert.ToInt32(txt_multiplo_c.Text);
                c._mto_Max_Cubertura = Convert.ToDecimal(txt_mto_max_cubert_c.Text);
                c._nro_Capa_Xl1 = Convert.ToInt32(txt_nrocapaxl_c1.Text);
                c._Prioridad1 = Convert.ToDecimal(txt_prioridad_c1.Text);
                c._Cesion_Exc_Prioridad1 = Convert.ToDecimal(txt_excesoprio_c1.Text);
                c._mto_Max_Cap_Lim_Sup1 = Convert.ToDecimal(txt_mto_max_lim_sup_c1.Text);
                c._prima_Min_Deposito1 = Convert.ToDecimal(txt_primaminima_deposit_c1.Text);

                c._nro_Capa_Xl2 = Convert.ToInt32(txt_nrocapaxl_c2.Text);
                c._Prioridad2 = Convert.ToDecimal(txt_prioridad_c2.Text);
                c._Cesion_Exc_Prioridad2 = Convert.ToDecimal(txt_excesoprio_c2.Text);
                c._mto_Max_Cap_Lim_Sup2 = Convert.ToDecimal(txt_mto_max_lim_sup_c2.Text);
                c._prima_Min_Deposito2 = Convert.ToDecimal(txt_primaminima_deposit_c2.Text);

                c._usu_reg = Session["username"].ToString();
                c._usu_mod = Session["username"].ToString();
                
                bContratoVC control = new bContratoVC();
                if (c._ide_Contrato == 0)
                {
                    resp = control.SetInsertarContrato(c);
                    if (resp != 0)
                    {
                        MessageBox("Registro Grabado Correctamente");
                        SetLLenadoContrato();
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
                else {
                    resp = control.SetActualizarContrato(c);
                    if (resp != 0)
                    {
                        MessageBox("Registro Actualizado Correctamente");
                        SetLLenadoContrato();
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
            }
            catch (Exception e) {
                MessageBoxcCatch("ERROR =>" + e.Message);
            }
        }
        //funcion de insertar  reaseguradores
        private void SetInsertarActualizarContratoDetalle(){
            try
            {
                Int32 resp = 0;
                eContratoDetalleVC d = new eContratoDetalleVC();
                d._ide_Contrato_Det = Convert.ToInt32(txt_idContratoDetalle_c.Value);
                d._id_Empresa = Convert.ToInt32(Session["idempresa"]);
                d._nro_Contrato = ddl_contrato_r.SelectedItem.Value;
                d._ide_Reasegurador = ddl_reasegurador_r.SelectedItem.Value;
                d._cod_Reasegurador = ddl_reasegurador_r.SelectedItem.Value;
                d._cod_Empresa_Califica = ddl_calificadora_r.SelectedItem.Value;
                d._cal_Crediticia = ddl_crediticia_r.SelectedItem.Value;
                d._mod_Contrato = ddl_tipcont_det_r.SelectedItem.Value;
                d._prc_Retencion = Convert.ToDecimal(txt_retencion_r.Text);
                d._prc_Cesion = Convert.ToDecimal(txt_cesion_r.Text);
                d._prc_participacion_rea = Convert.ToDecimal(txt_participacion_cesion.Text);
                d._nombre_Rea = txt_nombre_rea.Text;
                d._nro_Registro_Rea = Convert.ToInt32(txt_nro_registro_rea.Text);
                d._estado = "A";
                d._usu_reg = Session["username"].ToString();
                d._usu_mod = Session["username"].ToString();

                bContratoDetalleVC icd = new bContratoDetalleVC();
                if(d._ide_Contrato_Det == 0){
                    resp = icd.SetInsertarContratoDetalle(d);
                    if (resp != 0)
                    {
                        MessageBox("Registro Grabado Correctamente!");
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
                else
                {
                    resp = icd.SetActualizarContratoDetalle(d);
                    if (resp != 0)
                    {
                        MessageBox("Registro Actualizado Correctamente");
                    }
                    else {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
            }
            catch (Exception e) {
                MessageBoxcCatch("ERROR =>" + e.Message);
            }
        }
        public void SetEliminarParamentro(String tabla, String indice)
        {
            try
            {
                if (tabla.Equals("CONTRATO") && indice != "0")
                {
                    bContratoVC bc = new bContratoVC();
                    Int32 resp = bc.SetEliminarContrato(Int32.Parse(indice));
                    if (resp != 0)
                    {
                        MessageBox(resp + "Registro Eliminado Correctamente!");
                        SetLLenadoContrato();
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
                else if (tabla.Equals("CONTRATO_DETALLE") && indice != "0")
                {
                    bContratoDetalleVC bcd = new bContratoDetalleVC();
                    Int32 resp = bcd.SetEliminarContratoDetalle(Int32.Parse(indice));
                    if (resp != 0)
                    {
                        MessageBox(resp + "Registro Eliminado Correctamente!");
                    }
                    else
                    {
                        MessageBox("Ocurrio un Error en el Servidor!");
                    }
                }
            }
            catch (Exception) {
                MessageBoxcCatch("ERROR => Selecione un Registro");
            }
        }

        private void SetLLenadoContrato() {
            eContratoVC o = new eContratoVC();
            o._inicio = 0;
            o._fin = 10000;
            o._orderby = "IDE_CONTRATO ASC";
            o._estado = "R";
            o._nro_Contrato = "NO";

            bContratoVC tb = new bContratoVC();

            ddl_contrato_r.DataSource = tb.GetSelecionarContrato(o, out totalContrato);
            ddl_contrato_r.DataTextField = "_des_Contrato";
            ddl_contrato_r.DataValueField = "_nro_Contrato";
            ddl_contrato_r.DataBind();
            ddl_contrato_r.Items.Insert(0, new ListItem("Selecione ----", "0"));
        }
        private void llenarEstado(String est,String tip)
        {

            eTabla o = new eTabla();
            bTablaVC tb = new bTablaVC();
            o._id_Empresa = 0;
            o._tipo_Tabla = est;
            o._descripcion = "NULL";
            o._valor = "N";
            o._estado = "A";
            o._tipo = tip;
            o._inicio = 0;
            o._fin = 10000000;
            o._order = "DESCRIPCION ASC";
            ddl_estado_c.DataSource = tb.GetSelectConcepto(o, out total); ;
            ddl_estado_c.DataTextField = "_descripcion";
            ddl_estado_c.DataValueField = "_codigo";
            ddl_estado_c.DataBind();
            ddl_estado_c.Items.Insert(0, new ListItem("Seleccione ----", "0"));
        }
        private void MessageBox(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:160,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
        private void MessageBoxcCatch(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div style=\"font-size:14px;text-align:center;\">" + text + "</div>').dialog({title:'Error',modal:true,width:400,height:160,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
    }
}