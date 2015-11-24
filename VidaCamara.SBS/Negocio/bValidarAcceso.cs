using System;
using System.Linq;

namespace VidaCamara.SBS.Negocio
{
    public class bValidarAcceso : System.Web.UI.Page
    {
        public Boolean GetValidarAcceso(String QueryParam)
        {
            String listaPagina = Session["accesos"].ToString();
            String[] listSearch = listaPagina.Split(',');
            String httpPost = QueryParam;
            return listSearch.Contains(httpPost);
        }
    }
}