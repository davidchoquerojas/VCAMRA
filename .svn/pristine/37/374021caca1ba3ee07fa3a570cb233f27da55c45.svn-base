using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using VidaCamara.SBS.Dao;
using VidaCamara.SBS.Dao.Interface;
using VidaCamara.SBS.Entity;

namespace VidaCamara.SBS.Negocio
{
    public class bTablaVC : ITablaVC
    {
        Int32 total;
        public Int32 SetInsertarConcepto(eTabla o) { 
            dSqlTablaVC dg = new dSqlTablaVC();
            return dg.SetInsertarConcepto(o);
        }
        //actualizar
        public Int32 SetActualizarConcepto(eTabla o) {
            dSqlTablaVC dg = new dSqlTablaVC();
            return dg.SetActualizarConcepto(o);
        }
        public Int32 SetEliminarConcepto(int indice) {
            dSqlTablaVC dg = new dSqlTablaVC();
            return dg.SetEliminarConcepto(indice);
        }
        public List<eTabla> GetSelectConcepto(eTabla o,out int total)
        {
            dSqlTablaVC dg = new dSqlTablaVC();
            return dg.GetSelectConcepto(o,out total);
        }
        public DropDownList SetEstablecerDataSourceConcepto(DropDownList control,String filtro1,String descripcion = "NULL") {
                eTabla o = new eTabla();
                o._id_Empresa = 0;
                o._tipo_Tabla = filtro1;
                o._descripcion = descripcion;
                o._valor = "N";
                o._estado = "A";
                o._inicio = 0;
                o._fin = 1000000;
                o._order = "DESCRIPCION ASC";

                dSqlTablaVC dg = new dSqlTablaVC();
                control.DataSource = dg.GetSelectConcepto(o, out total);
                control.DataTextField = "_descripcion";
                control.DataValueField = "_codigo";
                control.DataBind();
                control.Items.Insert(0, new ListItem("Seleccione ----", "0"));
            return control;
        }
        public String GetConceptoByCodigo(String codigo) {
            dSqlTablaVC t = new dSqlTablaVC();
            return t.GetConceptoByCodigo(codigo);
        }
    }
}