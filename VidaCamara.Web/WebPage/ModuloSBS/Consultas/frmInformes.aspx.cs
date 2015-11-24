﻿using System;
using System.Data;
using System.IO;
using System.Web.UI;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using VidaCamara.SBS.Negocio;

namespace VidaCamara.Web.WebPage.ModuloSBS.Consultas
{
    public partial class frmInformes : System.Web.UI.Page
    {
        static HSSFWorkbook hssfworkbook;
        static String formato_moneda;
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
            if (!IsPostBack) {
                bContratoVC contrato = new bContratoVC();
                contrato.SetEstablecerDataSourceContrato(ddl_contrato_i);
                formato_moneda = Session["formatomoneda"].ToString();
            }
        }
  
        protected void btnExcel_Click(object sender, ImageClickEventArgs e)
        {

            string value = ddl_anexo_i.SelectedValue;
            string text = ddl_anexo_i.SelectedItem.Text;

            if (value == "0") return;

            string filename = "REPORTE SBS " + text + " "+ System.DateTime.Today.ToShortDateString() + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            Response.Clear();

            MemoryStream ms = new MemoryStream();

            if (value == "1")
                ms = Anexos(formato_moneda,Convert.ToDateTime(txt_fecha_creacion.Text),Convert.ToDateTime(txt_hasta.Text));
            else if (value == "2")
                ms = ES_18A(ddl_contrato_i.SelectedItem.Value,Convert.ToDateTime(txt_fecha_creacion.Text),Convert.ToDateTime(txt_hasta.Text),formato_moneda);
            else if (value == "3")
                ms = ES_18B(ddl_contrato_i.SelectedItem.Value);
            else if (value == "4")
                ms = ES_18C(ddl_contrato_i.SelectedItem.Value, formato_moneda);
            else if (value == "5")
                ms = ES_18D(ddl_contrato_i.SelectedItem.Value,formato_moneda);
            else if (value == "6")
                ms = ES_18E(ddl_contrato_i.SelectedItem.Value,Convert.ToDateTime(txt_fecha_creacion.Text),Convert.ToDateTime(txt_hasta.Text),formato_moneda);
            else if (value == "7")
                ms = ES_18F(ddl_contrato_i.SelectedItem.Value, Convert.ToDateTime(txt_fecha_creacion.Text), Convert.ToDateTime(txt_hasta.Text), formato_moneda);
            else if (value == "8")
                ms = Modelos(ddl_contrato_i.SelectedItem.Value,Convert.ToDateTime(txt_fecha_creacion.Text),Convert.ToDateTime(txt_hasta.Text),formato_moneda,1);
            else if (value == "9")
                ms = Modelos(ddl_contrato_i.SelectedItem.Value, Convert.ToDateTime(txt_fecha_creacion.Text), Convert.ToDateTime(txt_hasta.Text), formato_moneda,2);

            Response.BinaryWrite(ms.GetBuffer());
            Response.End();
        }

        public MemoryStream Anexos(String formato_moneda,DateTime fecha_inicio,DateTime fecha_hasta) {
            //seccionde variables
            Int32 count = 1;

            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            String[] Cabecera1 = { "CODIGOS SBS","EMPRESAS \n REASEGURADORAS Y/O \n COASEGURADORAS", "PRIMAS POR \n PAGAR \n REASEGUROS \n CEDIDOS", "PRIMAS POR \n COBRAR \n REASEGUROS \n ACEPTADOS", "SINIESTROS \n POR COBRAR \n REASEGUROS \n CEDIDOS",
                                    "SINIESTROS \n POR PAGAR \n REASEGUROS \n ACEPTADOS", "OTRAS \n CUENTAS POR \n COBRAR \n REASEGUROS \n CEDIDOS", "OTRAS \n CUENTAS POR \n PAGAR \n REASEGUROS \n ACEPTADOS", "DESCUENOTS Y \n COMISIONES DE \n REASEGUROS",
                                    "SALDO \n DEUDOR", "SALDO \n ACREEDOR","SALDO \n DEUDOR \n COMPENSADOS","SALDO \n ACREEDOR \n COMPENSADOS" };

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ANEXO ES - 2A");

            String[] Cabecera2 = { "CODIGOS SBS", "NOMBRE DE LAS \n EMPRESAS Y DETALLE \n DE CONTRATOS", "CUENTAS POR COBRAR A \n REASEGURADORES Y/O \n COASEGURADORES", "6  ó  \n MAS MESES DE ANTIGÜEDAD",
                                    "12  ó  MAS  MESES \n DE ANTIGÜEDAD", "TOTAL DE \n PARTIDAS \n A PROVISIONAR" };

            ISheet hojaTrabajo2 = hssfworkbook.CreateSheet("ANEXO ES - 2B");

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;

            ICellStyle styleCabecera = this.SetStyleHeader(this.SetFontTitle());

            IRow FilaTitulo = hojaTrabajo.CreateRow(4);
            ICell CeldaTitulo;
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ANEXO ES - 2A");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(4, 4, 1, 13));

            IRow FilaDesc1 = hojaTrabajo.CreateRow(6);
            ICell CeldaDesc1;
            CeldaDesc1 = FilaDesc1.CreateCell(1);
            CeldaDesc1.SetCellValue("CUENTAS CORRIENTES CON REASEGURADORAS Y/O COASEGURADORAS");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(6, 6, 1, 13));

            IRow FilaDesc2 = hojaTrabajo.CreateRow(7);
            ICell CeldaDesc2;
            CeldaDesc2 = FilaDesc2.CreateCell(1);
            CeldaDesc2.SetCellValue("DETALLE DE SALDOS REPORTADOS EN EL BALANCE DE COMPROBACIÓN DE SALDOS");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(7, 7, 1, 13));

            IRow filaDescF = hojaTrabajo.CreateRow(8);
            ICell celdaDescF;
            celdaDescF = filaDescF.CreateCell(1);
            celdaDescF.SetCellValue("AL " + Convert.ToDateTime(txt_fecha_creacion.Text));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(8, 8, 1, 12));

            IRow FilaDesc3 = hojaTrabajo.CreateRow(9);
            ICell CeldaDesc3;
            CeldaDesc3 = FilaDesc3.CreateCell(1);
            CeldaDesc3.SetCellValue("(EN NUEVOS SOLES)");
            CeldaDesc3.CellStyle.Alignment = HorizontalAlignment.Center;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(9, 9, 1, 12));

            IRow FilaCabecera = hojaTrabajo.CreateRow(11);
            IRow FilaCabecera2 = hojaTrabajo.CreateRow(12);
            IRow FilaCabecera3 = hojaTrabajo2.CreateRow(12);

            ICell CeldaCabecera, CeldaCabecera2;

            for (int cl = 1; cl <= 13; cl++) {
                hojaTrabajo.SetColumnWidth(cl,300*15);
            }

            for (int c = 0; c < Cabecera1.Length; c++)
            {
                CeldaCabecera = FilaCabecera.CreateCell(c + 1);
                CeldaCabecera.SetCellValue(Cabecera1[c]);
                CeldaCabecera.CellStyle = styleCabecera;
                CeldaCabecera.CellStyle.WrapText = true;
                CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                CeldaCabecera.CellStyle.Alignment = HorizontalAlignment.Center;

                CeldaCabecera2 = FilaCabecera2.CreateCell(c + 1);
                CeldaCabecera2.SetCellValue(c.ToString());
                CeldaCabecera2.CellStyle = styleCabecera;
            }
            bExportarData be = new bExportarData();
            DataTable dtanexo = be.GetSelecionarAnexo(ddl_contrato_i.SelectedItem.Value, formato_moneda,fecha_inicio,fecha_hasta);
            for (int r = 0; r < dtanexo.Rows.Count; r++) {
                switch (Convert.ToString(dtanexo.Rows[r][0]).Trim())
                {
                    case "PCP":
                        IRow filaData = hojaTrabajo.CreateRow(13 + count);
                        ICell dataCell;
                        IRow filaData2HJ2 = hojaTrabajo2.CreateRow(13 + count);
                        ICell dataCell2HJ2;
                        for (int c = 1; c < dtanexo.Columns.Count; c++)
                        {
                            dataCell = filaData.CreateCell(c);
                            dataCell.SetCellValue(dtanexo.Rows[r][c].ToString());
                            dataCell.CellStyle = styleCabecera;
                            if (c < 4)
                            {
                                dataCell2HJ2 = filaData2HJ2.CreateCell(c);
                                dataCell2HJ2.SetCellValue(dtanexo.Rows[r][c].ToString());
                                dataCell2HJ2.CellStyle = styleCabecera;
                            }

                        }
                        count++;
                        break;
                    case "PF":
                        IRow filaData1 = hojaTrabajo.CreateRow(15 + count);
                        ICell dataCell1;
                        IRow filaData3HJ2 = hojaTrabajo2.CreateRow(15 + count);
                        ICell dataCell3HJ2;
                        for (int c = 1; c < dtanexo.Columns.Count; c++)
                        {
                            dataCell1 = filaData1.CreateCell(c);
                            dataCell1.SetCellValue(dtanexo.Rows[r][c].ToString());
                            dataCell1.CellStyle = styleCabecera;
                            if (c < 4)
                            {
                                dataCell3HJ2 = filaData3HJ2.CreateCell(c);
                                dataCell3HJ2.SetCellValue(dtanexo.Rows[r][c].ToString());
                                dataCell3HJ2.CellStyle = styleCabecera;
                            }
                        }
                        count++;
                        break;
                    case "NPA":
                        IRow filaData2 = hojaTrabajo.CreateRow(17 + count);
                        ICell dataCell2;
                        IRow filaData4HJ2 = hojaTrabajo2.CreateRow(17 + count);
                        ICell dataCell4HJ2;
                        for (int c = 1; c < dtanexo.Columns.Count; c++)
                        {
                            dataCell2 = filaData2.CreateCell(c);
                            dataCell2.SetCellValue(dtanexo.Rows[r][c].ToString());
                            dataCell2.CellStyle = styleCabecera;
                            if (c < 4)
                            {
                                dataCell4HJ2 = filaData4HJ2.CreateCell(c);
                                dataCell4HJ2.SetCellValue(dtanexo.Rows[r][c].ToString());
                                dataCell4HJ2.CellStyle = styleCabecera;
                            }
                        }
                        count++;
                        break;
                    case "NPF":
                        IRow filaData3 = hojaTrabajo.CreateRow(19 + count);
                        ICell dataCell3;
                        IRow filaData5HJ2 = hojaTrabajo2.CreateRow(19 + count);
                        ICell dataCell5HJ2;
                        for (int c = 1; c < dtanexo.Columns.Count; c++)
                        {
                            dataCell3 = filaData3.CreateCell(c);
                            dataCell3.SetCellValue(dtanexo.Rows[r][c].ToString());
                            dataCell3.CellStyle = styleCabecera;
                            if (c < 4)
                            {
                                dataCell5HJ2 = filaData5HJ2.CreateCell(c);
                                dataCell5HJ2.SetCellValue(dtanexo.Rows[r][c].ToString());
                                dataCell5HJ2.CellStyle = styleCabecera;
                            }
                        }
                        count++;
                        break;
                }
            }

            //columnas para nombre de aseguradores PARA ANEXO 2A
            IRow FilaAutomatico = hojaTrabajo.CreateRow(13);
            IRow FilaFacultivo = hojaTrabajo.CreateRow(15 + (count-1));
            IRow FilaNoAutomatico = hojaTrabajo.CreateRow(17 + (count - 1));
            IRow FilaNoFacultivos = hojaTrabajo.CreateRow(19 + (count - 1));
            IRow FilaOtraModalidad = hojaTrabajo.CreateRow(21 + (count - 1));
            IRow FilaCoaseguros = hojaTrabajo.CreateRow(23 + (count - 1));
            IRow FilaCoaRecibido = hojaTrabajo.CreateRow(25 + (count - 1));
            IRow FilaTotales = hojaTrabajo.CreateRow(26 + (count - 1));

            //COLUMNAS PARa nombre de aseguradores NEXO2B
            IRow FilaAutomatico2 = hojaTrabajo2.CreateRow(13);
            IRow FilaFacultivo2 = hojaTrabajo2.CreateRow(15 + (count - 1));
            IRow FilaNoAutomatico2 = hojaTrabajo2.CreateRow(17 + (count - 1));
            IRow FilaNoFacultivos2 = hojaTrabajo2.CreateRow(19 + (count - 1));
            IRow FilaOtraModalidad2 = hojaTrabajo2.CreateRow(21 + (count - 1));
            IRow FilaCoaseguros2 = hojaTrabajo2.CreateRow(23 + (count - 1));
            IRow FilaCoaRecibido2 = hojaTrabajo2.CreateRow(25 + (count - 1));
            IRow FilaTotales2 = hojaTrabajo2.CreateRow(26 + (count - 1));

            ICell CellAutomatico = FilaAutomatico.CreateCell(2);
            CellAutomatico.SetCellValue("CONTRATOS \n PROPORCIONALES  \nAUTOMÁTICOS");
            CellAutomatico.CellStyle = styleCabecera;
            CellAutomatico.CellStyle.WrapText = true;

            ICell CellFacultivo = FilaFacultivo.CreateCell(2);
            CellFacultivo.SetCellValue("CONTRATOS \n PROPORCIONALES \n FACULTATIVOS");
            CellFacultivo.CellStyle = styleCabecera;
            CellFacultivo.CellStyle.WrapText = true;

            ICell CellNoAutomatico = FilaNoAutomatico.CreateCell(2);
            CellNoAutomatico.SetCellValue("CONTRATOS NO \n PROPORCIONALES \n AUTOMÁTICOS");
            CellNoAutomatico.CellStyle = styleCabecera;
            CellNoAutomatico.CellStyle.WrapText = true;

            ICell CellNoFacultivo = FilaNoFacultivos.CreateCell(2);
            CellNoFacultivo.SetCellValue("CONTRATOS NO \n PROPORCIONALES \n FACULTATIVOS");
            CellNoFacultivo.CellStyle = styleCabecera;
            CellNoFacultivo.CellStyle.WrapText = true;

            ICell CellOtraModalidad = FilaOtraModalidad.CreateCell(2);
            CellOtraModalidad.SetCellValue("CONTRATO EN \n OTRAS \n MODALIDADES");
            CellOtraModalidad.CellStyle = styleCabecera;
            CellOtraModalidad.CellStyle.WrapText = true;

            ICell Coaseguros = FilaCoaseguros.CreateCell(2);
            Coaseguros.SetCellValue("CONTRATO DE \n COASEGUROS \n CEDIDOS");
            Coaseguros.CellStyle = styleCabecera;
            Coaseguros.CellStyle.WrapText = true;

            ICell CellCoaRecibido = FilaCoaRecibido.CreateCell(2);
            CellCoaRecibido.SetCellValue("CONTRATO DE \n COASEGUROS \n RECIBIDOS");
            CellCoaRecibido.CellStyle = styleCabecera;
            CellCoaRecibido.CellStyle.WrapText = true;

            ICell CellTotalesTittle = FilaTotales.CreateCell(2);
            CellTotalesTittle.SetCellValue("TOTALES");
            CellTotalesTittle.CellStyle = styleCabecera;
            CellTotalesTittle.CellStyle.WrapText = true;

            //CELDAS DE SHEET 2B
            ICell CellAutomatico2 = FilaAutomatico2.CreateCell(2);
            CellAutomatico2.SetCellValue("CONTRATOS \n PROPORCIONALES  \nAUTOMÁTICOS");
            CellAutomatico2.CellStyle = styleCabecera;
            CellAutomatico2.CellStyle.WrapText = true;

            ICell CellFacultivo2 = FilaFacultivo2.CreateCell(2);
            CellFacultivo2.SetCellValue("CONTRATOS \n PROPORCIONALES \n FACULTATIVOS");
            CellFacultivo2.CellStyle = styleCabecera;
            CellFacultivo2.CellStyle.WrapText = true;

            ICell CellNoAutomatico2 = FilaNoAutomatico2.CreateCell(2);
            CellNoAutomatico2.SetCellValue("CONTRATOS NO \n PROPORCIONALES \n AUTOMÁTICOS");
            CellNoAutomatico2.CellStyle = styleCabecera;
            CellNoAutomatico2.CellStyle.WrapText = true;

            ICell CellNoFacultivo2 = FilaNoFacultivos2.CreateCell(2);
            CellNoFacultivo2.SetCellValue("CONTRATOS NO \n PROPORCIONALES \n FACULTATIVOS");
            CellNoFacultivo2.CellStyle = styleCabecera;
            CellNoFacultivo2.CellStyle.WrapText = true;

            ICell CellOtraModalidad2 = FilaOtraModalidad2.CreateCell(2);
            CellOtraModalidad2.SetCellValue("CONTRATO EN \n OTRAS \n MODALIDADES");
            CellOtraModalidad2.CellStyle = styleCabecera;
            CellOtraModalidad2.CellStyle.WrapText = true;

            ICell Coaseguros2 = FilaCoaseguros2.CreateCell(2);
            Coaseguros2.SetCellValue("CONTRATO DE \n COASEGUROS \n CEDIDOS");
            Coaseguros2.CellStyle = styleCabecera;
            Coaseguros2.CellStyle.WrapText = true;

            ICell CellCoaRecibido2 = FilaCoaRecibido2.CreateCell(2);
            CellCoaRecibido2.SetCellValue("CONTRATO DE \n COASEGUROS \n RECIBIDOS");
            CellCoaRecibido2.CellStyle = styleCabecera;
            CellCoaRecibido2.CellStyle.WrapText = true;

            ICell CellTotalesTittle2 = FilaTotales2.CreateCell(2);
            CellTotalesTittle2.SetCellValue("TOTALES");
            CellTotalesTittle2.CellStyle = styleCabecera;
            CellTotalesTittle2.CellStyle.WrapText = true;

            for (int cl = 3; cl < dtanexo.Columns.Count; cl++) {
                decimal totales = 0;
                ICell celldatatotal;
                ICell celldatatotal2;
                for (int rw = 0; rw < dtanexo.Rows.Count; rw++) {
                    totales += Convert.ToDecimal(dtanexo.Rows[rw][cl]);
                }
                celldatatotal = FilaTotales.CreateCell(cl);
                celldatatotal.SetCellValue(String.Format(formato_moneda, totales).Substring(3));
                celldatatotal.CellStyle = styleCabecera;
                if (cl == 3) {
                    celldatatotal2 = FilaTotales2.CreateCell(cl);
                    celldatatotal2.SetCellValue(String.Format(formato_moneda, totales).Substring(3));
                    celldatatotal2.CellStyle = styleCabecera;
                }
            }
            //segunda ventana
            ICellStyle styleData = this.SetFontData(dataFont);

            for (int cl = 2; cl <= 7; cl++) {
                hojaTrabajo2.SetColumnWidth(cl,300*25);
            }

            IRow FilaTitulo2 = hojaTrabajo2.CreateRow(4);
            ICell CeldaTitulo2;
            CeldaTitulo2 = FilaTitulo.CreateCell(1);
            CeldaTitulo2.SetCellValue("ANEXO ES - 2B");
            hojaTrabajo2.AddMergedRegion(new CellRangeAddress(4, 4, 1, 7));
            CeldaTitulo2.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo2 = hojaTrabajo2.CreateRow(6);
            CeldaTitulo2 = FilaTitulo2.CreateCell(1);
            CeldaTitulo2.SetCellValue("CUENTAS CORRIENTES CON REASEGURADORAS Y/O COASEGURADORAS");
            hojaTrabajo2.AddMergedRegion(new CellRangeAddress(6, 6, 1, 7));
            CeldaTitulo2.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo2 = hojaTrabajo2.CreateRow(7);
            CeldaTitulo2 = FilaTitulo2.CreateCell(1);
            CeldaTitulo2.SetCellValue("ANÁLISIS DE LAS PARTIDAS DEUDORAS POR ANTIGÜEDAD DE SALDOS Y DETERMINACIÓN DE  PROVISIONES");
            hojaTrabajo2.AddMergedRegion(new CellRangeAddress(7, 7, 1, 7));
            CeldaTitulo2.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo2 = hojaTrabajo2.CreateRow(9);
            CeldaTitulo2 = FilaTitulo2.CreateCell(1);
            CeldaTitulo2.SetCellValue("(EN NUEVOS SOLES)");
            hojaTrabajo2.AddMergedRegion(new CellRangeAddress(9, 9, 1, 7));
            CeldaTitulo2.CellStyle.Alignment = HorizontalAlignment.Center;

            IRow FilaCabecera22 = hojaTrabajo2.CreateRow(11);
            ICell celdaSheet2;
            for (int c = 0; c < Cabecera2.Length; c++)
            {
                celdaSheet2 = FilaCabecera22.CreateCell(c + 1);
                celdaSheet2.SetCellValue(Cabecera2[c]);
                celdaSheet2.CellStyle = this.SetFontData(this.SetFontDataText());
                celdaSheet2.CellStyle = styleCabecera;
                celdaSheet2.CellStyle.WrapText = true;
            }

            dtanexo.Rows.Clear();
            dtanexo.Columns.Clear();
            dtanexo.Clear();

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;
        
        }

        public MemoryStream ES_18A(String contrato, DateTime fecha_inicio, DateTime fecha_hasta,String formato_moneda)
        {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Andean System";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Powered by JLevano";

            String[] Cabecera = { "Nº", "EMPRESA DE REASEGUROS","EMPRESA CORREDORA DE SEGUROS", "INFORMACION SOBRE CONTRATOS DE REASEGUROS","OBSERVACIONES"};

            String[] SubCabecera = { "CODIGO", "NOMBRE", "PAIS DE ORIGEN", "EMPRESA CALIFICADORA", "CLASIFICACION",
                                    "Nº DE REGISTRO", "NOMBRE", "TIPO DE \n CONTRATO","RAMO","INICIO DE \n VIGENCIA",
                                    "PRIMA CEDITAS \n BRUTAS", "PRIMAS CEDITAS \n NETAS" };

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18A");

            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            IRow FilaTitulo = hojaTrabajo.CreateRow(7);
            ICell CeldaTitulo;

            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ANEXO ES-18A");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(7, 7, 1, 14));

            FilaTitulo = hojaTrabajo.CreateRow(8);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("PRIMAS CEDIDAS POR REASEGURADOR");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(8, 8, 1, 14));

            FilaTitulo = hojaTrabajo.CreateRow(9);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("(MONTO EN US$)");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(9, 9, 1, 14));

            FilaTitulo = hojaTrabajo.CreateRow(10);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("DEL : " +txt_fecha_creacion.Text + " AL : " + txt_hasta.Text);
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(10, 10, 1, 14));

            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 14, 1, 1));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 2, 6));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 7, 8));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 9, 13));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 14, 14, 14));

            IRow FilaCabecera = hojaTrabajo.CreateRow(13);
                 FilaCabecera.Height = 100 * 8;
            ICell CeldaCabecera;
            for (int c = 0; c < Cabecera.Length; c++)
            {
                hojaTrabajo.AutoSizeColumn(c);
                if (c == 0)
                {
                    CeldaCabecera = FilaCabecera.CreateCell(c + 1);
                    CeldaCabecera.SetCellValue(Cabecera[c]);
                    CeldaCabecera.CellStyle = styleCabecera;
                    CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                }
                else if (c == 1)
                {
                    CeldaCabecera = FilaCabecera.CreateCell(c + 1);
                    CeldaCabecera.SetCellValue(Cabecera[c]);
                    CeldaCabecera.CellStyle = styleCabecera;
                    CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                }
                else if (c == 2)
                {
                    CeldaCabecera = FilaCabecera.CreateCell(7);
                    CeldaCabecera.SetCellValue(Cabecera[c]);
                    CeldaCabecera.CellStyle = styleCabecera;
                    CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                }
                else if (c == 3)
                {
                    CeldaCabecera = FilaCabecera.CreateCell(9);
                    CeldaCabecera.SetCellValue(Cabecera[c]);
                    CeldaCabecera.CellStyle = styleCabecera;
                    CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                }
                else if (c == 4)
                {
                    CeldaCabecera = FilaCabecera.CreateCell(14);
                    CeldaCabecera.SetCellValue(Cabecera[c]);
                    CeldaCabecera.CellStyle = styleCabecera;
                    CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                }
            }
            IRow FilaSubCabecera = hojaTrabajo.CreateRow(14);
            ICell CeldaSubCabecera;

            for (int c = 0; c < SubCabecera.Length; c++)
            {
                CeldaSubCabecera = FilaSubCabecera.CreateCell(c + 2);
                CeldaSubCabecera.SetCellValue(SubCabecera[c]);
                CeldaSubCabecera.CellStyle = styleCabecera;
                CeldaSubCabecera.CellStyle.WrapText = true;
                hojaTrabajo.AutoSizeColumn(c);
            }

            //DATA DE DB
            bExportarData ed = new bExportarData();
            DataTable table18 = ed.GetSelecionarEs18A(contrato,fecha_inicio,fecha_hasta,formato_moneda);
            for (int r = 0; r < table18.Rows.Count; r++) {
                IRow filaData = hojaTrabajo.CreateRow(15+r);
                ICell CeldaData;
                for (int c = 0; c < table18.Columns.Count; c++) {

                    CeldaData = filaData.CreateCell(c + 1);
                    CeldaData.SetCellValue(table18.Rows[r][c].ToString());
                    CeldaData.CellStyle = this.SetFontData(this.SetFontDataText());
                    hojaTrabajo.AutoSizeColumn(table18.Columns[c].Ordinal);
                }
            }
            table18.Rows.Clear();
            table18.Columns.Clear();
            table18.Clear();

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;

        }

        public MemoryStream ES_18B(String contratos)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18B");

            /*HSSFPatriarch patriarch = (HSSFPatriarch)hojaTrabajo.CreateDrawingPatriarch();
            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 100, 3, 1, 4, 4);
            anchor.AnchorType = 2;
            String imageUrl = Directory.GetCurrentDirectory();
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage("/../../ISL/im_titulo.jpg", hssfworkbook));
            picture.Resize();
            //picture.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL;*/


            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            hojaTrabajo.SetColumnWidth(1, 300 * 26);
            hojaTrabajo.SetColumnWidth(2, 300 * 20);
            hojaTrabajo.SetColumnWidth(3, 300 * 25);
            hojaTrabajo.SetColumnWidth(4, 300 * 25);
            hojaTrabajo.SetColumnWidth(5, 300 * 25);
            hojaTrabajo.SetColumnWidth(6, 300 * 25);

            IRow FilaTitulo = hojaTrabajo.CreateRow(10);
            ICell CeldaTitulo;

            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ANEXO ES-18B");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(10, 10, 1, 5));

            FilaTitulo = hojaTrabajo.CreateRow(12);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("INFORMACIÓN SOBRE DISTRIBUCIÓN DEL REASEGURO");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(12, 12, 1, 5));

            FilaTitulo = hojaTrabajo.CreateRow(13);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("(CONTRATOS PROPORCIONALES Y NO PROPORCIONALES)");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 1, 5));

            FilaTitulo = hojaTrabajo.CreateRow(14);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("(EXPRESADO EN PORCENTAJES)");
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 1, 5));

            FilaTitulo = hojaTrabajo.CreateRow(15);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("POR LOS CONTRATOS VIGENTES AL "+System.DateTime.Today.ToShortDateString());
            CeldaTitulo.CellStyle = styleTitulo;
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(15, 15, 1, 5));
            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            ICellStyle styleData = hssfworkbook.CreateCellStyle();
            styleData.SetFont(dataFont);
            styleData.Alignment = HorizontalAlignment.Left;

            FilaTitulo = hojaTrabajo.CreateRow(17);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("Empresa : Compañía de Seguros de Vida Cámara");
            CeldaTitulo.CellStyle = styleData;

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17,17, 1, 5));

            IRow FilaCabecera17 = hojaTrabajo.CreateRow(19);
            IRow FilaCabecera18 = hojaTrabajo.CreateRow(20);
                 FilaCabecera18.Height = 100 * 4;
            IRow FilaCabecera19 = hojaTrabajo.CreateRow(21);
                 FilaCabecera19.Height = 100 * 4;
            ICell CeldaCabecera;
            
            //DATA DB
            bExportarData ed = new bExportarData();
            DataTable tableB = ed.GetSelecionarEs18B(contratos);

            CeldaCabecera = FilaCabecera17.CreateCell(1);
            CeldaCabecera.SetCellValue("NOMBRE DEL REASEGURADOR");
            CeldaCabecera.CellStyle = styleCabecera;
            CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;

            CeldaCabecera = FilaCabecera17.CreateCell(2);
            CeldaCabecera.SetCellValue("CODIGO SBS DEL \n REASEGURADOR");
            CeldaCabecera.CellStyle = styleCabecera;
            CeldaCabecera.CellStyle.WrapText = true;

            CeldaCabecera = FilaCabecera17.CreateCell(3);
            CeldaCabecera.SetCellValue("PROPORCIONAL AUTOMATICO");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera17.CreateCell(4);
            CeldaCabecera.SetCellValue("NO PROPORCIONAL AUTOMATICO");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera17.CreateCell(5);
            CeldaCabecera.SetCellValue("PROPORCIONAL FACULTATIVO");
            CeldaCabecera.CellStyle = styleCabecera;


            hojaTrabajo.AddMergedRegion(new CellRangeAddress(19, 21, 1, 1));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(19, 21, 2, 2));

            for (int r = 0; r < tableB.Rows.Count; r++) {

                if (tableB.Rows[r][0].ToString().Trim().Equals("PCP"))
                {
                    CeldaCabecera = FilaCabecera18.CreateCell(3);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][1].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;

                    CeldaCabecera = FilaCabecera19.CreateCell(3);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][2].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;
                }
                else if (tableB.Rows[r][0].ToString().Trim().Equals("NPA"))
                {

                    CeldaCabecera = FilaCabecera18.CreateCell(4);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][1].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;

                    CeldaCabecera = FilaCabecera19.CreateCell(4);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][2].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;
                }
                else if (tableB.Rows[r][0].ToString().Trim().Equals("PF"))
                {

                    CeldaCabecera = FilaCabecera18.CreateCell(5);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][1].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;

                    CeldaCabecera = FilaCabecera19.CreateCell(5);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][2].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;
                }
                else
                {
                    CeldaCabecera = FilaCabecera17.CreateCell(6);
                    CeldaCabecera.SetCellValue("NO PROPORCIONAL FACULTATIVO");
                    CeldaCabecera.CellStyle = styleCabecera;

                    CeldaCabecera = FilaCabecera18.CreateCell(6);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][1].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;

                    CeldaCabecera = FilaCabecera19.CreateCell(6);
                    CeldaCabecera.SetCellValue(tableB.Rows[r][2].ToString());
                    CeldaCabecera.CellStyle = styleCabecera;
                }
                IRow dataRow = hojaTrabajo.CreateRow(22+r);
                ICell dataCell;
                for (int c = 0; c < tableB.Columns.Count-3; c++) {

                    if (c == 2)
                    {
                        if (tableB.Rows[r][0].ToString().Trim().Equals("PCP"))
                        {
                            dataCell = dataRow.CreateCell(3);
                            dataCell.SetCellValue(Convert.ToString(Convert.ToDecimal(tableB.Rows[r][c + 3])/100));
                            dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                        }
                        else if (tableB.Rows[r][0].ToString().Trim().Equals("NPA"))
                        {
                            dataCell = dataRow.CreateCell(4);
                            dataCell.SetCellValue(Convert.ToString(Convert.ToDecimal(tableB.Rows[r][c + 3]) / 100));
                            dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                        }
                        else if (tableB.Rows[r][0].ToString().Trim().Equals("PF")) {

                            dataCell = dataRow.CreateCell(5);
                            dataCell.SetCellValue(Convert.ToString(Convert.ToDecimal(tableB.Rows[r][c + 3]) / 100));
                            dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                        }
                        else
                        {
                            dataCell = dataRow.CreateCell(6);
                            dataCell.SetCellValue("0");
                            dataCell.CellStyle = this.SetFontData(this.SetFontDataText());

                            dataCell = dataRow.CreateCell(6);
                            dataCell.SetCellValue(Convert.ToString(Convert.ToDecimal(tableB.Rows[r][c + 3]) / 100));
                            dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                        }
                    }
                    else
                    {
                        dataCell = dataRow.CreateCell(c + 1);
                        dataCell.SetCellValue(Convert.ToString(tableB.Rows[r][c + 3].ToString()));
                        dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                    }

                }
            }

            tableB.Rows.Clear();
            tableB.Columns.Clear();
            tableB.Clear();

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;
        }

        public MemoryStream ES_18C(String contrato, String formato_moneda)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Andean System";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Powered by JLevano";

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18C");

            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();
            styleTitulo.Alignment = HorizontalAlignment.Center;

            ICellStyle styleOtro = hssfworkbook.CreateCellStyle();
            styleOtro.Alignment = HorizontalAlignment.Left;
            styleOtro.SetFont(titleFont);

            IRow FilaTitulo = hojaTrabajo.CreateRow(11);
            ICell CeldaTitulo;
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ANEXO ES - 18C");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(11, 11, 1, 13));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo  = hojaTrabajo.CreateRow(12);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ESTRUCTURA DE LOS CONTRATOS DE REASEGUROS PROPORCIONALES");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(12, 12, 1, 13));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(13);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("(Expresado en US$)");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 1, 13));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(14);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("AL "+System.DateTime.Today.ToShortDateString());
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 1, 13));

            CeldaTitulo.CellStyle = styleTitulo;

            IRow FilaDesc4 = hojaTrabajo.CreateRow(15);
            ICell CeldaDesc4;
            CeldaDesc4 = FilaDesc4.CreateCell(1);
            CeldaDesc4.SetCellValue("Empresa : Compañía de Seguros de Vida Cámara");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(15, 15, 1, 4));

            for (int ce = 1; ce <= 13; ce++) {
                hojaTrabajo.SetColumnWidth(ce,300 * 17);
            }

            IRow FilaCabecera17 = hojaTrabajo.CreateRow(17);
            IRow FilaCabecera18 = hojaTrabajo.CreateRow(18);
                 FilaCabecera18.Height = 100 * 10;
            IRow FilaCabecera19 = hojaTrabajo.CreateRow(19);
       
            ICell CeldaCabecera;

            CeldaCabecera = FilaCabecera17.CreateCell(1);
            CeldaCabecera.SetCellValue("RAMOS (1)");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera17.CreateCell(2);
            CeldaCabecera.SetCellValue("VIGENCIA");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera18.CreateCell(2);
            CeldaCabecera.SetCellValue("INICIO");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera18.CreateCell(3);
            CeldaCabecera.SetCellValue("FIN");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera17.CreateCell(4);
            CeldaCabecera.SetCellValue("CONTRATO CUOTA PARTE");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera18.CreateCell(4);
            CeldaCabecera.SetCellValue("RETENCION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(4);
            CeldaCabecera.SetCellValue("%");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(5);
            CeldaCabecera.SetCellValue("MONTO \n MAXIMO DE \n RETENCION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera18.CreateCell(6);
            CeldaCabecera.SetCellValue("CESION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(6);
            CeldaCabecera.SetCellValue("%");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(7);
            CeldaCabecera.SetCellValue("MONTO \n MAXIMO \n CESION");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera17.CreateCell(8);
            CeldaCabecera.SetCellValue("CONTRATO EXCEDENTE");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera18.CreateCell(8);
            CeldaCabecera.SetCellValue("PRIMER EXCEDENTE  \n SEGUNDO EXCEDENTE  \n TERCER EXCEDENTE");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(8);
            CeldaCabecera.SetCellValue("MONTO DEL \n PLENO");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(9);
            CeldaCabecera.SetCellValue("Nº DE LINEAS \n MULTIPLOS DE");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(10);
            CeldaCabecera.SetCellValue("MONTO \n MAXIMO DE \n CESION");
            CeldaCabecera.CellStyle = styleCabecera;



            CeldaCabecera = FilaCabecera17.CreateCell(11);
            CeldaCabecera.SetCellValue("FACULTATIVO \n OBLIGATORIO");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(11);
            CeldaCabecera.SetCellValue("% DE \n RETENCION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(12);
            CeldaCabecera.SetCellValue("% DE \n CESION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera19.CreateCell(13);
            CeldaCabecera.SetCellValue("MONTO \n MAXIMO DE \n COBERTURA");
            CeldaCabecera.CellStyle = styleCabecera;
            CeldaCabecera.CellStyle.WrapText = true;
            CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;


            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17, 19, 1, 1));

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17, 17, 2, 3));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 2, 2));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 3, 3));

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17, 17, 4, 7));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 18, 4, 5));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 18, 6, 7));

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17, 17, 8, 10));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 18, 8, 10));

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(17, 18, 11, 13));
            //DATA DB
            bExportarData ed = new bExportarData();
            DataTable tableC = ed.GetSelecionarEs18C(contrato, formato_moneda);
            for (int r = 0; r < tableC.Rows.Count; r++)
            {
                IRow dataRow = hojaTrabajo.CreateRow(20 + r);
                ICell dataCell;
                for (int c = 0; c < tableC.Columns.Count; c++)
                {
                    dataCell = dataRow.CreateCell(c + 1);
                    dataCell.SetCellValue(tableC.Rows[r][c].ToString());
                    dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                }
            }

            tableC.Rows.Clear();
            tableC.Columns.Clear();
            tableC.Clear();

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;
        }

        public MemoryStream ES_18D(String contrato,String formato_moneda)
        {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Andean System";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Powered by JLevano";

            String[] Cabecera = { "Nº", "TIPO DE \n CONTRATO (1)", "RAMOS (2)", "REASEGURADORA LIDER", " ","CODIGO SBS","INICIO DE \n VIGENCIA","FIN DE \n VIGENCIA",
                                "CAPA Nº (XL)", "PRIORIDAD", "CESION EN \n EXCESO DE LA \n PRIORIDAD", "MONTO MAXIMO \n DE LA CAPA O \n LIMITE SUPERIOR", "PRIMA MINIMA \n O PRIMA \n DEPOSITO" };

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18D");

            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 14;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 14;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);

            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();

            ICellStyle styleOtro = hssfworkbook.CreateCellStyle();
            styleOtro.Alignment = HorizontalAlignment.Left;
            styleOtro.SetFont(titleFont);
            
            IRow FilaTitulo = hojaTrabajo.CreateRow(12);
            ICell CeldaTitulo;

            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ANEXO ES - 18D");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(12, 12, 1, 13));
            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo = hojaTrabajo.CreateRow(13);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("ESTRUCTURA DE LOS CONTRATOS DE REASEGUROS  NO - PROPORCIONALES");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 1, 13));
            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo = hojaTrabajo.CreateRow(14);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("(Expresado en S/.)");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 1, 13));
            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            FilaTitulo = hojaTrabajo.CreateRow(15);
            CeldaTitulo = FilaTitulo.CreateCell(1);
            CeldaTitulo.SetCellValue("AL : "+System.DateTime.Today.ToShortDateString());
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(15, 15, 1, 13));
            CeldaTitulo.CellStyle.Alignment = HorizontalAlignment.Center;

            IRow FilaDesc4 = hojaTrabajo.CreateRow(16);
            ICell CeldaDesc4;
            CeldaDesc4 = FilaDesc4.CreateCell(1);
            CeldaDesc4.SetCellValue("Empresa : Compañía de Seguros de Vida Cámara");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(16, 16, 1, 4));

            IRow FilaCabecera = hojaTrabajo.CreateRow(18);
                 FilaCabecera.Height = 100 * 10;
            ICell CeldaCabecera;

            for (int c = 0; c < Cabecera.Length; c++)
            {
                CeldaCabecera = FilaCabecera.CreateCell(c + 1);
                CeldaCabecera.SetCellValue(Cabecera[c]);
                CeldaCabecera.CellStyle = styleCabecera;
                CeldaCabecera.CellStyle.WrapText = true;
                CeldaCabecera.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                hojaTrabajo.SetColumnWidth(c, 300 * 15);
            }

            IRow FilaCabecera2 = hojaTrabajo.CreateRow(19);
            ICell CeldaCabecera2;

            CeldaCabecera2 = FilaCabecera2.CreateCell(4);
            CeldaCabecera2.SetCellValue("NOMBRE");
            CeldaCabecera2.CellStyle = styleCabecera;

            CeldaCabecera2 = FilaCabecera2.CreateCell(5);
            CeldaCabecera2.SetCellValue("%");
            CeldaCabecera2.CellStyle = styleCabecera;

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 1, 1));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 2, 2));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 3, 3));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 18, 4, 5));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 6, 6));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 7, 7));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 8, 8));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 9, 9));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 10, 10));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 11, 11));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 12, 12));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(18, 19, 13, 13));


            ICellStyle styleData = this.SetFontData(this.SetFontDataText());

            bExportarData ed = new bExportarData();
            DataTable tableD = ed.GetSelecionarEs18D(contrato, formato_moneda);
            for (int r = 0; r < tableD.Rows.Count; r++)
            {
                IRow dataRow = hojaTrabajo.CreateRow(20 + r);
                ICell dataCell;
                for (int c = 0; c < tableD.Columns.Count; c++)
                {
                    dataCell = dataRow.CreateCell(c + 1);
                    dataCell.SetCellValue(tableD.Rows[r][c].ToString());
                    dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                }
            }
            tableD.Rows.Clear();
            tableD.Columns.Clear();
            tableD.Clear();

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;

        }

        public MemoryStream ES_18E(String contrato,DateTime fecha_inicio,DateTime fecha_hasta,String formato_moneda)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Andean System";
            hssfworkbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Powered by JLevano";

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18C");

            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);
            styleCabecera.VerticalAlignment = VerticalAlignment.Center;
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();
                       styleTitulo.Alignment = HorizontalAlignment.Center;
            ICellStyle styleOtro = hssfworkbook.CreateCellStyle();
            styleOtro.Alignment = HorizontalAlignment.Left;
            styleOtro.SetFont(titleFont);

            IRow FilaTitulo = hojaTrabajo.CreateRow(6);
            ICell CeldaTitulo;
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("ANEXO ES - 18E");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(6, 6, 2, 15));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(8);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("SINIESTROS POR COBRAR- REASEGUROS PROPORCIONALES Y SINIESTROS POR PAGAR ");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(8, 8, 2, 15));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(9);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("(Expresado en US$)");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(9, 9, 2, 15));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(10);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("DEL : "+txt_fecha_creacion.Text+" AL : "+ txt_hasta.Text);
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(10, 10, 2, 15));
            CeldaTitulo.CellStyle = styleTitulo;

            IRow FilaDesc4 = hojaTrabajo.CreateRow(13);
            ICell CeldaDesc4;
            CeldaDesc4 = FilaDesc4.CreateCell(2);
            CeldaDesc4.SetCellValue("Empresa : Compañía de Seguros de Vida Cámara");

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 2, 5));

            for (int cl = 2; cl <= 14; cl++) {
                if (cl == 6)
                    hojaTrabajo.SetColumnWidth(6, 300 * 50);
                else
                    hojaTrabajo.SetColumnWidth(cl, 300 * 20);
            }

            IRow FilaCabecera14 = hojaTrabajo.CreateRow(14);
            IRow FilaCabecera15 = hojaTrabajo.CreateRow(15);
            ICell CeldaCabecera;

            CeldaCabecera = FilaCabecera14.CreateCell(2);
            CeldaCabecera.SetCellValue("Nº");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera14.CreateCell(3);
            CeldaCabecera.SetCellValue("EMPRESA REASEGURADORA");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera14.CreateCell(5);
            CeldaCabecera.SetCellValue("INFORMACION SOBRE EL SINIESTRO");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera14.CreateCell(8);
            CeldaCabecera.SetCellValue("SINIESTROS POR COBRAR A REASEGURADORES  POR MERCADO");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera14.CreateCell(11);
            CeldaCabecera.SetCellValue("SITUACION DE LOS SINIESTROS POR PAGAR");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera15.CreateCell(2);
            CeldaCabecera.SetCellValue("");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(3);
            CeldaCabecera.SetCellValue("CODIGO (1)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(4);
            CeldaCabecera.SetCellValue("NOMBRE");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(5);
            CeldaCabecera.SetCellValue("RAMO (2)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(6);
            CeldaCabecera.SetCellValue("ASEGURADO O CONTRANTANTE DE LA POLIZA");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(7);
            CeldaCabecera.SetCellValue("FECHA DE \n OCURRENCIA");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(8);
            CeldaCabecera.SetCellValue("MERCADO \n LOCAL");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(9);
            CeldaCabecera.SetCellValue("MERCADO \n DEL \n EXTERIOR");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(10);
            CeldaCabecera.SetCellValue("TOTAL \n (3)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(11);
            CeldaCabecera.SetCellValue("PENDIENTE \n DE \n LIQUIDACION");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(12);
            CeldaCabecera.SetCellValue("LIQUIDADOS");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera15.CreateCell(13);
            CeldaCabecera.SetCellValue("TOTAL \n (4)");
            CeldaCabecera.CellStyle = styleCabecera;
            CeldaCabecera.CellStyle.WrapText = true;


            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 3, 4));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 5, 7));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 8, 10));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(14, 14, 11, 13));
           // hojaTrabajo.AddMergedRegion(new CellRangeAddress(15, 15, 6, 8));

            bExportarData ed = new bExportarData();
            DataTable tableE = ed.GetSelecionarEs18E(contrato,fecha_inicio,fecha_hasta,formato_moneda);
            for (int r = 0; r < tableE.Rows.Count; r++)
            {
                IRow dataRow = hojaTrabajo.CreateRow(16 + r);
                ICell dataCell;
                for (int c = 0; c < tableE.Columns.Count; c++)
                {
                    dataCell = dataRow.CreateCell(c + 2);
                    dataCell.SetCellValue(tableE.Rows[r][c].ToString());
                    dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                }
            }

            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;

        }

        public MemoryStream ES_18F(String contrato,DateTime fecha_inicio,DateTime fecha_hasta,String formato_moneda)
        {
            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            ISheet hojaTrabajo = hssfworkbook.CreateSheet("ES-18F");

            IFont titleFont = this.SetFontTitle();

            IFont titleFontBlack = hssfworkbook.CreateFont();
            titleFontBlack.FontName = "Calibri";
            titleFontBlack.Boldweight = (short)FontBoldWeight.Bold;
            titleFontBlack.Color = (IndexedColors.Black.Index);
            titleFontBlack.FontHeightInPoints = 11;

            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            ICellStyle styleTitulo = hssfworkbook.CreateCellStyle();
                       styleTitulo.Alignment = HorizontalAlignment.Center;
            ICellStyle styleOtro = hssfworkbook.CreateCellStyle();
            styleOtro.Alignment = HorizontalAlignment.Left;
            styleOtro.SetFont(titleFont);

            IRow FilaTitulo = hojaTrabajo.CreateRow(5);
            ICell CeldaTitulo;
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("ANEXO ES - 18F");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(5, 5, 2, 9));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(7);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("INFORMACION ESTADISTICA DE OPERACIONES DE REASEGUROS CON EL MERCADO EXTRANJERO ");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(7, 7, 2, 9));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(8);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("(Cifras Netas Acumuladas trimestralmente expresadas en US$)");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(8, 8, 2, 9));
            CeldaTitulo.CellStyle = styleTitulo;

            FilaTitulo = hojaTrabajo.CreateRow(9);
            CeldaTitulo = FilaTitulo.CreateCell(2);
            CeldaTitulo.SetCellValue("DEL : "+txt_fecha_creacion.Text+" AL: "+txt_hasta.Text);
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(9, 9, 2, 9));
            CeldaTitulo.CellStyle = styleTitulo;

            IRow FilaDesc4 = hojaTrabajo.CreateRow(11);
            ICell CeldaDesc4;
            CeldaDesc4 = FilaDesc4.CreateCell(2);
            CeldaDesc4.SetCellValue("Empresa : ............................");
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(11, 11, 2, 4));


            for (int cl = 3; cl <= 10; cl++)
            {
                hojaTrabajo.SetColumnWidth(cl, 300 * 18);
            }
            hojaTrabajo.SetColumnWidth(2, 300 * 25);

            IRow FilaCabecera12 = hojaTrabajo.CreateRow(13);
            IRow FilaCabecera13 = hojaTrabajo.CreateRow(14);
            ICell CeldaCabecera;

            CeldaCabecera = FilaCabecera12.CreateCell(2);
            CeldaCabecera.SetCellValue("RAMOS DE SEGUROS (1) \n (Solo riesgos Reasegurados)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera12.CreateCell(3);
            CeldaCabecera.SetCellValue("EGRESOS (E)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera12.CreateCell(6);
            CeldaCabecera.SetCellValue("INGRESOS (I)");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera12.CreateCell(9);
            CeldaCabecera.SetCellValue("SALDO (I)  -  (E)");
            CeldaCabecera.CellStyle = styleCabecera;


            CeldaCabecera = FilaCabecera13.CreateCell(3);
            CeldaCabecera.SetCellValue("Primas de \n Reaseguros \n Cedidos");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera13.CreateCell(4);
            CeldaCabecera.SetCellValue("Siniestros de \n Reaseguros \n Recibidos");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera13.CreateCell(5);
            CeldaCabecera.SetCellValue("Comisiones de \n Reaseguros \n Recibidos");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera13.CreateCell(6);
            CeldaCabecera.SetCellValue("Primas de \n Reaseguros \n Recibidos");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera13.CreateCell(7);
            CeldaCabecera.SetCellValue("Siniestros de \n Reaseguros \n Cedidos ");
            CeldaCabecera.CellStyle = styleCabecera;

            CeldaCabecera = FilaCabecera13.CreateCell(8);
            CeldaCabecera.SetCellValue("Comisiones de \n Reaseguros \n Cedidos");
            CeldaCabecera.CellStyle = styleCabecera;
            CeldaCabecera.CellStyle.WrapText = true;

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 14, 2, 2));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 3, 5));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 13, 6, 8));
            hojaTrabajo.AddMergedRegion(new CellRangeAddress(13, 14, 9, 9));
            //DATA DESDE DB
            bExportarData ed = new bExportarData();
            DataTable tableF = ed.GetSelecionarEs18F(contrato,fecha_inicio,fecha_hasta,formato_moneda);
            for (int r = 0; r < tableF.Rows.Count; r++)
            {
                IRow dataRow = hojaTrabajo.CreateRow(15 + r);
                ICell dataCell;
                for (int c = 0; c < tableF.Columns.Count; c++)
                {
                    dataCell = dataRow.CreateCell(c + 2);
                    dataCell.SetCellValue(tableF.Rows[r][c].ToString());
                    dataCell.CellStyle = this.SetFontData(this.SetFontDataText());
                }
            }
            IRow FilaTotal = hojaTrabajo.CreateRow(15 + tableF.Rows.Count);
            ICell celldataTittle = FilaTotal.CreateCell(2);
            celldataTittle.SetCellValue("TOTALES");
            celldataTittle.CellStyle = this.SetFontData(this.SetFontDataText());
            //sumalisar el reporte
            for (int cl = 1; cl < tableF.Columns.Count; cl++) {
                decimal sumatotal = 0;
                ICell sumadatatotal;
                for (int rw = 0; rw < tableF.Rows.Count; rw++) {
                    sumatotal += Convert.ToDecimal(tableF.Rows[rw][cl]);
                }
                sumadatatotal = FilaTotal.CreateCell(cl+2);
                sumadatatotal.SetCellValue(String.Format(formato_moneda, sumatotal).Substring(3));
                sumadatatotal.CellStyle = this.SetFontData(this.SetFontDataText());
            }
            hssfworkbook.Write(output);
            writer.Flush();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;

        }
        private MemoryStream Modelos(String nro_contrato,DateTime fecha_inicio,DateTime fecha_hasta,String formato_moneda,Int32 token) {

            var output = new MemoryStream();
            var writer = new StreamWriter(output);

            hssfworkbook = new HSSFWorkbook();

            String[] cabecera = { "REASEGURADOR", "TIPO \n COMPROBANTE", "N° COMPROBANTE", "FECHA \n COMPROBANTE", "NOMBRE \n ASEGURADO ", "N° CONTRATO", "RAMO","MONEDA", "PRIMAS POR PAGAR \n REASEGUROS CEDIDOS \n (4)", "PRIMAS  POR COBRAR \n REASEGUROS ACEPTADOS \n (5)",
                                 "SINIESTROS POR COBRAR \n REASEGUROS CEDIDOS (6)","SINIESTROS POR PAGAR \n REASEGUROS ACEPTADOS (7)","OTRAS CUENTAS POR COBRAR \n REASEGUROS CEDIDOS (8)"," OTRAS CUENTAS POR PAGAR \n REASEGUROS ACEPTADOS (9)",
                                 "DESCUENTOS Y COMISIONES \n DE REASEGUROS (10)","SALDO DEUDOR \n (11)","SALDO ACREEDOR \n (12)","SALDO DEUDOR \n COMPENSADO \n (13)","SALDO ACREEDOR COMPENSADO\n (14)"};
            ISheet hojaTrabajo = hssfworkbook.CreateSheet("Modelo");

            IFont titleFont = this.SetFontTitle();

            ICellStyle styleCabecera = this.SetStyleHeader(titleFont);
            styleCabecera.Alignment = HorizontalAlignment.Center;
            styleCabecera.VerticalAlignment = VerticalAlignment.Center;
            styleCabecera.SetFont(titleFont);

            //DATA DESDE DB PARA TITULO SI ES PROPORCIONAL Y NO PROPORCIONAL
            bExportarData ed = new bExportarData();
            DataTable tbmodelo = ed.GetSelecionarModelo(nro_contrato, fecha_inicio, fecha_hasta,formato_moneda,token);
            
            IRow title = hojaTrabajo.CreateRow(3);
            if (token == 1){
                title.CreateCell(1, CellType.String).SetCellValue("Modelo: Contratos Facultativos - Reasegurador Local y del Exterior");
            }else {
                title.CreateCell(1, CellType.String).SetCellValue("Modelo: Contratos Autómaticos - Reasegurador Local y del Exterior");
            }

            hojaTrabajo.AddMergedRegion(new CellRangeAddress(3,3,1,4));

            IRow rowheader =  hojaTrabajo.CreateRow(6);
            ICell rowcell;
            for (int c = 1; c < cabecera.Length; c++) {
                hojaTrabajo.SetColumnWidth(c,300*20);
                rowcell = rowheader.CreateCell(c);
                rowcell.SetCellValue(cabecera[c].ToString());
                rowcell.CellStyle = styleCabecera;
                rowcell.CellStyle.WrapText = true;
            }
            //for en la data de la tabla 
            for (int r = 0; r < tbmodelo.Rows.Count; r++)
            {
                IRow dataRow1 = hojaTrabajo.CreateRow(7 + r);
                ICell dataCell1;
                for (int c = 1; c < tbmodelo.Columns.Count; c++)
                {
                    dataCell1 = dataRow1.CreateCell(c);
                    dataCell1.SetCellValue(tbmodelo.Rows[r][c].ToString());
                    dataCell1.CellStyle = this.SetFontData(this.SetFontDataText());
                }
            }
            IRow Filasuma = hojaTrabajo.CreateRow(7+tbmodelo.Rows.Count);
            ICell CelldataTittle;
            CelldataTittle = Filasuma.CreateCell(7);
            CelldataTittle.SetCellValue("TOTALES");
            CelldataTittle.CellStyle = this.SetFontData(this.SetFontDataText());

            for (int cl = 8; cl < tbmodelo.Columns.Count; cl++) {
                decimal costtotal = 0;
                ICell sumadatatotal;
                for (int rw = 0; rw < tbmodelo.Rows.Count; rw++) {
                    costtotal += Convert.ToDecimal(tbmodelo.Rows[rw][cl]);
                }
                sumadatatotal = Filasuma.CreateCell(cl);
                sumadatatotal.SetCellValue(String.Format(formato_moneda, costtotal).Substring(3));
                sumadatatotal.CellStyle = this.SetFontData(this.SetFontDataText());
            }

            hssfworkbook.Write(output);
            writer.Flush();

            tbmodelo.Columns.Clear();
            tbmodelo.Rows.Clear();
            tbmodelo.Clear();

            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);

            return output;
        }
        private ICellStyle SetStyleHeader(IFont SetFontTitle)
        {
            ICellStyle styleCabecera = hssfworkbook.CreateCellStyle();
            styleCabecera.SetFont(SetFontTitle);
            styleCabecera.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleCabecera.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleCabecera.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleCabecera.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            return styleCabecera;
        }

        private IFont SetFontDataText()
        {
            IFont dataFont = hssfworkbook.CreateFont();
            dataFont.FontName = "Calibri";
            dataFont.Color = (IndexedColors.Black.Index);
            dataFont.FontHeightInPoints = 10;
            return dataFont;
        }
        private ICellStyle SetFontData(IFont SetFontDataText)
        {
            ICellStyle styleData = hssfworkbook.CreateCellStyle();
            styleData.SetFont(SetFontDataText);
            styleData.Alignment = HorizontalAlignment.Center;
            styleData.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleData.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleData.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleData.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            return styleData;
        }
        private IFont SetFontTitle()
        {

            IFont titleFont = hssfworkbook.CreateFont();
            titleFont.FontName = "Calibri";
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleFont.Color = (IndexedColors.Black.Index);
            titleFont.FontHeightInPoints = 10;

            return titleFont;
        }
        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }
        private void MessageBox(String text)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "$('<div>" + text + "</div>').dialog({title:'Confirmación',modal:true,width:400,height:170,buttons: [{id: 'aceptar',text: 'Aceptar',icons: { primary: 'ui-icon-circle-check' },click: function () {$(this).dialog('close');}}]})", true);
        }
    }
}