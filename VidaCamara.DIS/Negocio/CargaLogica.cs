using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using VidaCamara.DIS.Modelo;

namespace VidaCamara.DIS.Negocio
{
    public class CargaLogica
    {
        public string FullNombreArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public ObjectResult Resultado { get; set; }
        public bool ValidaNombre { get; set; }
        public StringCollection TipoLinea { get; set; }
        public int IdArchivo { get; set; }
        public int Errores { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Vigente { get; set; }
        public string ExtensionArchivo { get; set; }
        public List<Regla> ReglaLinea { get; set; }
        public List<int> LargoLinea { get; set; }
        public string Errors { get; set; }
        public string MensajeExcepcion { get; set; }
        public string Observacion { get; set; }
        public int Estado { get; set; }
        public string Correo { get; set; }
        protected string CampoActual { get; set; }

        public CargaLogica(string archivo)
        {
            FullNombreArchivo = archivo;
            NombreArchivo = Path.GetFileName(archivo);
            string nombreArchivo = null;
            string extensionArchivo = null;
            nombreArchivo = Path.GetFileNameWithoutExtension(NombreArchivo);
            extensionArchivo = Path.GetExtension(NombreArchivo);

            if (extensionArchivo.Contains("CAM"))
            {
                if (NombreArchivo.Split('_')[0] == "NOMINA" | NombreArchivo.Split('_')[0] == "INOMINA")
                {
                    extensionArchivo = ".CSV";
                    NombreArchivo = nombreArchivo + extensionArchivo;
                }
                ValidaNombre = ValidaNombreArchivo(NombreArchivo);
            }
            else
            {
                ValidaNombre = false;
            }
            MensajeExcepcion = "";
        }

        public void CargarArchivo()
        {
            if (!NombreArchivo.Distinct().Any()) return;

            if (ValidaNombre)
            {
                ObtieneTipoLinea(NombreArchivo.Split('_')[0]);
                var idestado = 0;
                idestado = LeeArchivo(NombreArchivo.Split('_')[0], TipoLinea);
                if (idestado > 2)
                {
                    Errors = "No se puede procesar archivo por estar aprobado/pagado";
                    Errores = Errores + 1;
                }
                else
                {
                    if (idestado == 2)
                    {
                        Errors = "No se puede procesar archivo por tener Checklists";
                        Errores = Errores + 1;
                    }
                }
            }
            else
            {
                Errors = "Nombre de archivo no cumple formato";
                Errores = Errores + 1;

                NombreArchivo = string.Empty;
            }
        }

        public bool ValidaNombreArchivo(string nombreArchivo)
        {
            StringCollection archivo;
            string tipoArchivo = null;
            using (var context = new DISEntities())
            {
                var resultado = (object) context.pa_file_ObtieneTipoArchivos();
                archivo = ObtieneColeccion(resultado);
            }
            tipoArchivo = nombreArchivo.Split('_')[0];

            return archivo.Contains(tipoArchivo);
        }

        private StringCollection ObtieneColeccion(object dt)
        {
            var coleccion = new StringCollection();

            foreach (var iLoopVariable in (IEnumerable) dt)
            {
                var i = iLoopVariable;
                coleccion.Add(i == null ? "" : i.ToString());
            }
            return coleccion;
        }

        public void ObtieneTipoLinea(string tipoArchivo)
        {
            using (var context = new DISEntities())
            {
                var resultado = (object) context.pa_file_ObtienePrimerCaracterLineaPorTipoArchivo(tipoArchivo);
                TipoLinea = ObtieneColeccion(resultado);
            }
        }

        public string[] LineaArchivo()
        {
            Errores = 0;
            var sr = new StreamReader(FullNombreArchivo,
                System.Text.Encoding.GetEncoding(437));

            var texto = sr.ReadToEnd();
            var text = texto.Split('\n');
            sr.Close();
            return text;
        }

        public int LeeArchivo(string tipoArchivo, StringCollection tipoLinea)
        {
            //Dim texto As String
            string[] text = null;
            var resultadoValor = default(StringCollection);
            var exitoLinea = 0;
            string caracterInicial = null;
            var res1 = 0;
            var res2 = 0;

            text = LineaArchivo();

            //Consultar en tabla el estado del archivo
            //entregara 0 si no existe
            using (var context = new DISEntities())
            {
                var m = context.pa_file_ConsultaEstadoArchivo(NombreArchivo);
                Estado = m.FirstOrDefault() == null ? 0 : m.FirstOrDefault().Value;
            }
            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Consulta estado archivo",
                "pa_file_ConsultaEstadoArchivo '" + NombreArchivo + "'", 0);

            //valor anterior = 3
            //Si el valor es menor que 2, significa 2 cosas:
            //1.- que la nomina no esta aprobada y puede ser cargado nuevos archivos.
            //2.- que no hay checklist sobre el o los cuspp de la liquidacion
            if (Estado < 2)
            {
                //Insertar en tabla
                if (Estado == 1)
                {
                    Errors = "Archivo ya cargado previamente";
                }
                using (var context = new DISEntities())
                {
                    var m = context.pa_file_InsertaReferenciaArchivo(NombreArchivo, UsuarioModificacion);
                    IdArchivo = m.FirstOrDefault().Value;
                }
                //InsertaAuditoria(Me.UsuarioModificacion, "Inserta Referencia Archivo", "pa_file_InsertaReferenciaArchivo '" + Me.NombreArchivo + "'", Me.idArchivo)

                for (var x = 0; x <= text.Length - 1; x++)
                {
                    caracterInicial = Mid(text[x].Trim(), 0, 1);
                    var carterInicialNumer = 0;
                    if (int.TryParse(caracterInicial, out carterInicialNumer))
                    {
                        caracterInicial = "*";
                    }
                    if (tipoLinea.Contains(caracterInicial))
                    {
                        using (var context = new DISEntities())
                        {
                            var resultado = context.pa_file_ObtieneReglasArchivoPorLinea(tipoArchivo, caracterInicial);
                            ReglaLinea = ObtieneReglaLinea(resultado);
                        }
                        //InsertaAuditoria(Me.UsuarioModificacion, "Obtiene Regla de archivo por línea", "pa_file_ObtieneReglasArchivoPorLinea '" + tipoArchivo + "', " + CaracterInicial, Me.idArchivo)
                        try
                        {
                            foreach (var rLoopVariable in ReglaLinea)
                            {
                                try
                                {
                                    var r = rLoopVariable;
                                    CampoActual = Mid(text[x].Trim(), r.CaracterInicial - 1, r.LargoCampo);
                                    exitoLinea = 0;
                                    string valor = null;
                                    string[] inString = null;
                                    var j = 0;
                                    switch (r.TipoValidacion)
                                    {
                                        case "EQUAL":
                                            exitoLinea = EvaluarEqual(r, exitoLinea);
                                            break;

                                        case "BOOL_SP":
                                            exitoLinea = EvaluarBoolSp(tipoArchivo, r, x, exitoLinea);
                                            break;

                                        case "BOOL_IF_SP":
                                            exitoLinea = EvaluarBoolIfSp(r, x, exitoLinea);
                                            break;

                                        case "IN_QUERY":
                                            exitoLinea = EvaluarInQuery(r, exitoLinea);
                                            break;

                                        case "IN":
                                            exitoLinea = EvaluarIn(r, text, x, exitoLinea);
                                            break;

                                        case "FILLER":
                                            exitoLinea = EvaluarFiller(r, exitoLinea);
                                            break;

                                        case "":
                                            exitoLinea = 1;
                                            break;
                                    }
                                    using (var context = new DISEntities())
                                    {
                                        context.pa_file_InsertaHistorialCarga(IdArchivo, r.idRegla,
                                            text[x].Trim().Substring(0, 1), x + 1, r.CaracterInicial, r.LargoCampo,
                                            CampoActual, exitoLinea);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                } 
                                
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            MensajeExcepcion = ex.Message;
                            return 0;
                        }
                    }
                    else
                    {
                        if (text[x].Trim().Any())
                        {
                            using (var context = new DISEntities())
                            {
                                context.pa_file_InsertaHistorialCarga(IdArchivo, 451, "#", x + 1, 1,
                                    text[x].Trim().Count(), text[x], 0);
                            }
                            //InsertaAuditoria(Me.UsuarioModificacion, "Inserta Historial de CargaLogica", "pa_file_InsertaHistorialCarga 451" + ", " + "'#'" + ", " + (x + 1).ToString() + ", " + "1" + ", " + text[x].Trim()().Count().ToString() + ", '" + Me.campoActual + "', " + "0", Me.idArchivo)
                            Errores = Errores + 1;
                        }
                    }
                }

                try
                {
                    TraspasaArchivo(tipoArchivo);

                    if (ValidacionesArchivo(tipoArchivo, 2) == false)
                    {
                        using (var context = new DISEntities())
                        {
                            Resultado = context.pa_file_ObtieneErrorArchivo(IdArchivo);
                            var result = context.pa_file_ObtieneErrorArchivo(IdArchivo);

                            var nombre = "";
                            var largo = 0;
                            foreach (var datoLoopVariable in result)
                            {
                                var dato = datoLoopVariable;
                                if (dato.NumeroLinea.Value > 0)
                                {
                                    nombre = dato.NombreArchivo;
                                    largo = dato.LargoCampo.Value;
                                }
                            }
                            if (nombre != string.Empty & largo != null)
                            {
                                //If largo = 25 Then
                                var valor1 = context.pa_valida_CodigoTransferenciaNomina(nombre, IdArchivo, largo);
                                var resultado = 0;
                                resultado = valor1.FirstOrDefault().Value;
                                if (resultado == 0)
                                {
                                    Resultado = null;
                                    Observacion =
                                        "No existe liquidación, debe cargar liquidación y despúes la nómina";
                                    //End If
                                }
                            }
                        }
                    }

                    if (Errores == 0)
                    {
                        using (var context = new DISEntities())
                        {
                            var cantidad = context.pa_file_CantidadRegistroArchivo(IdArchivo);
                            var cant = 0;
                            cant = cantidad.FirstOrDefault().Value;
                            Observacion = "cantidad de registros cargados: " + cant;
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion),
                                "Archivo cargado correctamente, cantidad de registros cargados: " + cant,
                                NombreArchivo, IdArchivo);
                        }
                    }

                    //esto válida que los montos por cuspp no sean mayor a lo establecido
                    //en la entidad: negocio.MontoAlto
                    if (NombreArchivo.Substring(0, 3).ToLower() == "liq")
                    {
                        using (var context = new DISEntities())
                        {
                            var monto = context.pa_valida_MontoAlto(IdArchivo,
                                Convert.ToInt32(UsuarioModificacion));
                            string montoAlto = null;
                            montoAlto = monto.ToString();
                            if (montoAlto == "1")
                            {
                                dynamic monto1 = context.pa_devuelveresultado(IdArchivo);
                                var correo = "";
                                foreach (var registroLoopVariable in monto1)
                                {
                                    var registro = registroLoopVariable;
                                    correo = registro.correo;
                                    Observacion = Observacion + "\\n Monto alto cargado al CUSPP: " +
                                                  registro.Cuspp + ", por valor = " + registro.Valor.ToString;
                                    InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), Observacion,
                                        NombreArchivo, IdArchivo);
                                }
                                Correo = correo;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Observacion = ex.Message + "// TraspasaArchivo...!";
                }
            }
            else
            {
                Observacion = "ya está aprobado";
                return Estado;
            }
            return 0;
        }

        public string EnviarCorreo(string Para, string CC, string CCO, string Asunto, string Cuerpo, string FormatoCuerpo, string Archivos)
        {
            using (var repositorio = new DISEntities())
            {
                return repositorio.pa_envioCorreo_Procesos(Para, CC, CCO, Asunto, Cuerpo, FormatoCuerpo, Archivos).ToString();
            }
        }

        private static string Mid(string text, int startIndex, int length)
        {
            return text.Substring(startIndex, Math.Min(text.Length - startIndex, length));
        }

        private int EvaluarFiller(Regla r, int exitoLinea)
        {
            if (CampoActual == "".PadLeft(r.LargoCampo))
            {
                exitoLinea = 1;
            }
            else
            {
                Errores = Errores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarIn(Regla r, string[] text, int x, int exitoLinea)
        {
            string[] inString;
            int j;
            inString = r.ReglaValidacion.Split(',');
            var existe = 0;
            for (j = 0; j <= inString.Count() - 1; j++)
            {
                if (text[x].Trim().Substring(r.CaracterInicial - 1, r.LargoCampo) == inString[j])
                {
                    existe = 1;
                }
            }

            if (existe == 1)
            {
                exitoLinea = 1;
            }
            else
            {
                Errores = Errores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarInQuery(Regla r, int exitoLinea)
        {
            StringCollection resultadoValor;
            resultadoValor = ExecQuery(r.ReglaValidacion);

            if (resultadoValor.Contains(CampoActual))
            {
                exitoLinea = 1;
            }
            else
            {
                Errors = "No existe: " + CampoActual;
                Errores = Errores + 1;
            }
            return exitoLinea;
        }

        private int EvaluarBoolIfSp(Regla r, int x, int exitoLinea)
        {
            string valor;
            int res1;
            int res2;
            StringCollection resultadoValor;
            string[] inString;
            int j;
            string[] conditionString = null;
            valor = r.ReglaValidacion;
            valor = valor.Replace("@valor", "'" + CampoActual + "'");
            valor = valor.Replace("@IdArchivo", IdArchivo.ToString());
            valor = valor.Replace("@NumeroLinea", (x + 1).ToString());
            valor = valor.Replace("@CampoInicial", r.CaracterInicial.ToString());
            valor = valor.Replace("@LargoCampo", r.LargoCampo.ToString());
            conditionString = valor.Split(';');

            //InsertaAuditoria(Me.UsuarioModificacion, "Validación BOOL_IF_SP", valor, Me.idArchivo)

            switch (conditionString[0])
            {
                case "AND":
                case "OR":
                    res1 = 0;
                    res2 = 0;
                    switch (conditionString[1].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[1].Substring(3)))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[1].Substring(3), Me.idArchivo)
                        case "EQ#":
                            if (CampoActual == conditionString[1].Substring(3))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[1].Substring(3), Me.idArchivo)
                        case "IQ#":
                            resultadoValor = ExecQuery(conditionString[1].Substring(3));
                            if (resultadoValor.Contains(CampoActual))
                            {
                                res1 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[1].Substring(3) + ")", Me.idArchivo)
                        case "IN#":
                            inString = conditionString[1].Substring(3).Split(',');
                            for (j = 0; j <= inString.Count() - 1; j++)
                            {
                                if (CampoActual == inString[j])
                                {
                                    res1 = 1;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[1].Substring(3) + ")", Me.idArchivo)
                    }
                    switch (conditionString[2].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[2].Substring(3)))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                        case "EQ#":
                            if (CampoActual == conditionString[2].Substring(3))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                        case "IQ#":
                            resultadoValor = ExecQuery(conditionString[2].Substring(3));
                            if (resultadoValor.Contains(CampoActual))
                            {
                                res2 = 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                        case "IN#":
                            inString = conditionString[1].Substring(3).Split(',');
                            for (j = 0; j <= inString.Count() - 1; j++)
                            {
                                if (CampoActual == inString[j])
                                {
                                    res2 = 1;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                    }

                    if (conditionString[0] == "AND")
                    {
                        if (res1 == 1 & res2 == 1)
                        {
                            exitoLinea = 1;
                        }
                        else
                        {
                            Errores = Errores + 1;
                        }
                    }
                    else
                    {
                        if (res1 == 1 | res2 == 1)
                        {
                            exitoLinea = 1;
                        }
                        else
                        {
                            Errores = Errores + 1;
                        }
                    }
                    break;
                case "IF":
                    switch (conditionString[1].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[1].Substring(3)))
                            {
                                switch (conditionString[2].Substring(0, 3))
                                {
                                    case "SP#":
                                        if (ExecSpBool(conditionString[2].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                                    case "EQ#":
                                        if (
                                            CampoActual.Equals(
                                                conditionString[2].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                                    case "IQ#":
                                        resultadoValor =
                                            ExecQuery(conditionString[2].Substring(3));
                                        if (resultadoValor.Contains(CampoActual))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                    case "IN#":
                                        inString = conditionString[2].Substring(3)
                                            .Split(',');
                                        for (j = 0; j <= inString.Count() - 1; j++)
                                        {
                                            if (CampoActual == inString[j])
                                            {
                                                exitoLinea = 1;
                                                break;
                                                // TODO: might not be correct. Was : Exit For
                                            }
                                        }

                                        if (exitoLinea == 0)
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                }
                            }
                            else
                            {
                                switch (conditionString[3].Substring(0, 3))
                                {
                                    case "SP#":
                                        if (ExecSpBool(conditionString[3].Substring(3)))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP SP#", ConditionString[2].Substring(3), Me.idArchivo)
                                    case "EQ#":
                                        if (CampoActual ==
                                            conditionString[3].Substring(3))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                                    case "IQ#":
                                        resultadoValor =
                                            ExecQuery(conditionString[3].Substring(3));
                                        if (resultadoValor.Contains(CampoActual))
                                        {
                                            exitoLinea = 1;
                                        }
                                        else
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IQ#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                    case "IN#":
                                        inString = conditionString[3].Substring(3)
                                            .Split(',');
                                        for (j = 0; j <= inString.Count() - 1; j++)
                                        {
                                            if (CampoActual == inString[j])
                                            {
                                                exitoLinea = 1;
                                                break;
                                                // TODO: might not be correct. Was : Exit For
                                            }
                                        }

                                        if (exitoLinea == 0)
                                        {
                                            Errores = Errores + 1;
                                        }
                                        break;
                                    //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP IN#", Me.campoActual + "IN(" + ConditionString[2].Substring(3) + ")", Me.idArchivo)
                                }
                            }
                            break;
                        case "EQ#":
                            if (CampoActual.Equals(conditionString[2].Substring(3)))
                            {
                                exitoLinea = 1;
                            }
                            else
                            {
                                Errores = Errores + 1;
                            }
                            break;
                        //InsertaAuditoria(Me.UsuarioModificacion, "BOOL_IF_SP EQ#", Me.campoActual + "=" + ConditionString[2].Substring(3), Me.idArchivo)
                    }
                    break;
            }
            return exitoLinea;
        }

        private int EvaluarBoolSp(string tipoArchivo, Regla r, int x, int exitoLinea)
        {
            string valor;
            StringCollection resultadoValor;
            valor = r.ReglaValidacion;
            valor = valor.Replace("@valor", "'" + CampoActual + "'");
            valor = valor.Replace("@IdArchivo", IdArchivo.ToString());
            valor = valor.Replace("@NumeroLinea", (x + 1).ToString());
            valor = valor.Replace("@CampoInicial", r.CaracterInicial.ToString());
            valor = valor.Replace("@LargoCampo", r.LargoCampo.ToString());
            using (var context = new DISEntities())
            {
                var resultado = context.pa_valida_EjecutaProcedimientoAlmacenado(valor);
                resultadoValor = ObtieneColeccion(resultado);
            }

            if (resultadoValor[0] == "1")
            {
                exitoLinea = 1;
                //start:
                //1.- valida que exista una PRIMAPAG
                if (tipoArchivo == "PRIMDCUA")
                {
                    if (r.Tabladestino != string.Empty)
                    {
                        valor = r.Tabladestino;
                        valor = valor.Replace("@valor", "'" + CampoActual + "'");
                        using (var context = new DISEntities())
                        {
                            ObjectResult resultado =
                                context.pa_valida_EjecutaProcedimientoAlmacenado(valor);
                            resultadoValor = ObtieneColeccion(resultado);
                        }
                        if (resultadoValor[0] == "1")
                        {
                            exitoLinea = 1;
                            //
                        }
                        else
                        {
                            Errores = Errores + 1;
                            exitoLinea = 0;
                        }
                    }
                }
                //end
            }
            else
            {
                Errores = Errores + 1;
                exitoLinea = 0;
            }
            return exitoLinea;
        }

        private int EvaluarEqual(Regla r, int exitoLinea)
        {
            if (CampoActual == r.ReglaValidacion)
            {
                exitoLinea = 1;
            }
            else
            {
                Errores = Errores + 1;
            }
            return exitoLinea;
        }


        private void InsertaAuditoria(int idUsuario, string descripcion, string comando, int idarchivo)
        {
            using (var context = new DISEntities())
            {
                context.pa_audit_InsertaBitacora(idUsuario, descripcion, comando, idarchivo);
            }
        }

        private void TraspasaArchivo(string tipoArchivo)
        {
            try
            {
                var directorioArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivos"] +
                                        tipoArchivo + "\\";
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), directorioArchivo, NombreArchivo,
                    IdArchivo);

                if (!Directory.Exists(directorioArchivo))
                {
                    Directory.CreateDirectory(directorioArchivo);
                }
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "despues de validar directorio",
                    NombreArchivo, IdArchivo);

                var rutaArchivos = directorioArchivo + NombreArchivo;

                if (File.Exists(rutaArchivos))
                {
                    File.Delete(rutaArchivos);
                }
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion),
                    "despues de validar si el archivo existe en el directorio: " + rutaArchivos, NombreArchivo,
                    IdArchivo);

                File.Copy(FullNombreArchivo, rutaArchivos);
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "despues de copiar archivo en directorio",
                    NombreArchivo, IdArchivo);

                if (tipoArchivo == "PRIMAPAG")
                {
                    var nombre = FullNombreArchivo.Replace("PRIMAPAG", "PRIMPAGA");
                    dynamic nombreArch = Path.GetFileName(nombre);

                    directorioArchivo = System.Configuration.ConfigurationManager.AppSettings["RutaArchivos"] +
                                        "PRIMPAGA\\";

                    if (!Directory.Exists(directorioArchivo))
                    {
                        Directory.CreateDirectory(directorioArchivo);
                    }

                    rutaArchivos = directorioArchivo + nombreArch;

                    if (File.Exists(rutaArchivos))
                    {
                        File.Delete(rutaArchivos);
                    }

                    Errores = 0;

                    using (var writer = new StreamWriter(rutaArchivos))
                    {
                        using (var sr = new StreamReader(FullNombreArchivo))
                        {
                            var texto = sr.ReadLine() + "\r";

                            while ((sr.Peek() >= 0))
                            {
                                if (texto.Contains("PAP"))
                                {
                                    var linea = texto.Substring(0, 1) + "PRE" + texto.Substring(4);
                                    texto = linea;
                                }
                                writer.Write(texto);
                                texto = sr.ReadLine() + "\r";
                            }
                            if (texto != string.Empty)
                            {
                                writer.Write(texto);
                            }
                        }
                    }
                    //Insertar en tabla
                    using (var context = new DISEntities())
                    {
                        var m = context.pa_file_InsertaReferenciaArchivo(nombreArch, UsuarioModificacion);
                        IdArchivo = m;

                        InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se genero archivo(PrimPaga) en Servidor",
                            nombreArch, IdArchivo);
                    }
                }

                File.Delete(FullNombreArchivo);

                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se guarda archivo en Servidor",
                    NombreArchivo, IdArchivo);

                return;
            }
            catch (Exception ex)
            {
                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), ex.Message, NombreArchivo, IdArchivo);
                Observacion = ex.Message;
                return;
            }
        }

        public List<Regla> ObtieneReglaLinea(ObjectResult<pa_file_ObtieneReglasArchivoPorLinea_Result> dt)
        {
            var reglaLinea = new List<Regla>();
            foreach (var iLoopVariable in dt)
            {
                var i = iLoopVariable;
                var regla = new Regla
                {
                    idRegla = i.IdReglaArchivo,
                    CaracterInicial = i.CaracterInicial.Value,
                    LargoCampo = i.LargoCampo.Value,
                    TipoCampo = i.TipoCampo,
                    TipoValidacion = i.TipoValidacion,
                    ReglaValidacion = i.ReglaValidacion,
                    Tabladestino = i.TablaDestino
                };
                reglaLinea.Add(regla);
            }
            return reglaLinea;
        }

        private bool ExecSpBool(string procedure)
        {
            var resultadovalor = default(StringCollection);
            using (var context = new DISEntities())
            {
                ObjectResult resultado = context.pa_valida_EjecutaProcedimientoAlmacenado(procedure);
                resultadovalor = ObtieneColeccion(resultado);
            }
            return resultadovalor[0] == "1";
        }

        private StringCollection ExecQuery(string query)
        {
            var resultadovalor = default(StringCollection);
            using (var context = new DISEntities())
            {
                var resultado = context.pa_valida_EjecutaQuery(query);
                resultadovalor = ObtieneColeccion(resultado);
            }
            return resultadovalor;
        }

        private StringCollection ExecSpCollection(string procedure)
        {
            var resultadovalor = default(StringCollection);
            using (var context = new DISEntities())
            {
                ObjectResult resultado = context.pa_valida_EjecutaProcedimientoAlmacenado(procedure);
                resultadovalor = ObtieneColeccion(resultado);
            }
            return resultadovalor;
        }

        private bool ValidacionesArchivo(string tipoArchivo, int tipoRegla)
        {
            var exitoValidacion = 0;

            try
            {
                string regla = null;
                using (var context = new DISEntities())
                {
                    if (tipoRegla == 2 & Errores == 0)
                    {
                        var valorRetorno = context.pa_file_ArchivoValido(IdArchivo);
                        InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), "Se valida archivo",
                            "pa_file_ArchivoValido", IdArchivo);
                        exitoValidacion = int.Parse(valorRetorno.ToString());
                        if (exitoValidacion == 0)
                        {
                            Errores = Errores + 1;
                            Errors = "Error en Cuadratura de primas";
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), Errors, NombreArchivo,
                                IdArchivo);
                            return false;
                        }
                    }
                    var resultado = context.pa_file_ObtieneReglavalidacionArchivo(tipoArchivo, tipoRegla);
                    regla = resultado.FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(regla))
                {
                    string[] conditionString = null;
                    regla = regla.Replace("@NombreArchivo", NombreArchivo);
                    regla = regla.Replace("@IdArchivo", IdArchivo.ToString());
                    conditionString = regla.Split(';');

                    switch (conditionString[0].Substring(0, 3))
                    {
                        case "SP#":
                            if (ExecSpBool(conditionString[0].Substring(3)))
                            {
                                exitoValidacion = 1;
                            }
                            else
                            {
                                exitoValidacion = 0;
                            }
                            InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[0].Substring(3),
                                conditionString[0].Substring(3), IdArchivo);
                            break;
                    }

                    if (exitoValidacion == 1)
                    {
                        switch (conditionString[1].Substring(0, 3))
                        {
                            case "SP#":
                                if (ExecSpBool(conditionString[1].Substring(3)))
                                {
                                    exitoValidacion = 1;
                                }
                                else
                                {
                                    exitoValidacion = 0;
                                }
                                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[1].Substring(3),
                                    conditionString[1].Substring(3), IdArchivo);
                                break;
                        }
                    }
                    else
                    {
                        switch (conditionString[2].Substring(0, 3))
                        {
                            case "SP#":
                                if (ExecSpBool(conditionString[2].Substring(3)))
                                {
                                    exitoValidacion = 1;
                                }
                                else
                                {
                                    exitoValidacion = 0;
                                }
                                InsertaAuditoria(Convert.ToInt32(UsuarioModificacion), conditionString[2].Substring(3),
                                    conditionString[2].Substring(3), IdArchivo);
                                break;
                        }
                    }

                    return exitoValidacion == 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                Observacion = ex.Message;
                return false;
            }
        }
    }
}
