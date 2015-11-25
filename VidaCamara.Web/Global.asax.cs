using System;
using System.Web.Routing;

namespace VidaCamara.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RoutingData(RouteTable.Routes);
        }
        private void RoutingData(RouteCollection coleccion) 
        {
            coleccion.MapPageRoute("login", "Login", "~/WebPage/Inicio/frmLogin.aspx");
            coleccion.MapPageRoute("Inicio", "Inicio", "~/WebPage/Inicio/frmInicio.aspx");
            coleccion.MapPageRoute("hahah", "Parametros", "~/WebPage/Mantenimiento/frmGeneral.aspx");
            coleccion.MapPageRoute("Error", "Error", "~/WebPage/Inicio/frmError.aspx");
            coleccion.MapPageRoute("CargaDatos", "CargaDatos", "~/WebPage/ModuloSBS/Operaciones/frmCargaDatos.aspx");
            coleccion.MapPageRoute("OperacionManual", "OperacionManual", "~/WebPage/ModuloSBS/Operaciones/frmOperacionManual.aspx");
            coleccion.MapPageRoute("RegistroDatos", "RegistroDatos", "~/WebPage/ModuloSBS/Operaciones/frmRegistroDatos.aspx");
            coleccion.MapPageRoute("ProcesoOperacion", "ProcesoOperacion", "~/WebPage/ModuloSBS/Operaciones/frmProcesaOperacion.aspx");
            coleccion.MapPageRoute("CierreOperacion", "CierreOperacion", "~/WebPage/ModuloSBS/Operaciones/frmCierreProceso.aspx");
            coleccion.MapPageRoute("InterfaceContable", "InterfaceContable", "~/WebPage/ModuloSBS/Operaciones/frmInterfaceContable.aspx");
            coleccion.MapPageRoute("Usuarios", "Usuarios", "~/WebPage/Mantenimiento/frmUsuario.aspx");
            coleccion.MapPageRoute("Tablas", "Tablas", "~/WebPage/Mantenimiento/frmTabla.aspx");
            coleccion.MapPageRoute("Operaciones", "Operaciones", "~/WebPage/ModuloSBS/Consultas/frmConsultaOperaciones.aspx");
            coleccion.MapPageRoute("Comprobantes", "Comprobantes", "~/WebPage/ModuloSBS/Consultas/frmConsultaComprobante.aspx");
            coleccion.MapPageRoute("Informes", "Informes", "~/WebPage/ModuloSBS/Consultas/frmInformes.aspx");

            coleccion.MapPageRoute("CargaDatosDIS", "CargaDatosDIS", "~/WebPage/ModuloDIS/Operaciones/frmCargaDatos.aspx");
            coleccion.MapPageRoute("AprobacionCarga", "AprobacionCarga", "~/WebPage/ModuloDIS/Operaciones/frmAprobacionCarga.aspx");
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Abandon();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}