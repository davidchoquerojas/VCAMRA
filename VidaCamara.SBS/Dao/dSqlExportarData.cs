﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VidaCamara.SBS.Dao
{
    public class dSqlExportarData
    {
        private dbConexion _db = new dbConexion();

        static SqlConnection conexion = new SqlConnection(ConfigurationManager.AppSettings.Get("CnnBD").ToString());

        public DataTable GetSelecionarAnexo(String contrato, String formato_moneda, DateTime fecha_inicio, DateTime fecha_hasta)
        {
            DataTable dt = new DataTable();
            String[] column = {"TIPO_CONT", "COD_REASG", "REASEG", "PRIMAX_PAG_CED", "PRIMA_X_COB_ACE", "SIN_X_PAG_CED", "SIN_X_COB_ACE", "OTR_X_PAG_CED", "OTR_X_PAG_ACE", "DESCUENTO", "DEUDOR", "ACREEDOR","COMP_1","COMP_2"};
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectAnexo;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                sqlcmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = fecha_inicio;
                sqlcmd.Parameters.Add("@FECHA_HASTA", SqlDbType.Date).Value = fecha_hasta;
                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[column.Length];

                    obj[0] = dr.GetString(0);
                    obj[1] = dr.GetString(1);
                    obj[2] = dr.GetString(2);
                    obj[3] = String.Format(formato_moneda, dr.GetDecimal(3)).Substring(3);
                    obj[4] = String.Format(formato_moneda,dr.GetDecimal(4)).Substring(3);
                    obj[5] = String.Format(formato_moneda,dr.GetDecimal(5)).Substring(3);
                    obj[6] = String.Format(formato_moneda,dr.GetDecimal(6)).Substring(3);
                    obj[7] = String.Format(formato_moneda,dr.GetDecimal(7)).Substring(3);
                    obj[8] = String.Format(formato_moneda,dr.GetDecimal(8)).Substring(3);
                    obj[9] = String.Format(formato_moneda,dr.GetDecimal(9)).Substring(3);
                    obj[10] = String.Format(formato_moneda,dr.GetDecimal(10)).Substring(3);
                    obj[11] = String.Format(formato_moneda, dr.GetDecimal(11)).Substring(3);
                    obj[12] = String.Format(formato_moneda, dr.GetDecimal(12)).Substring(3);
                    obj[13] = String.Format(formato_moneda, dr.GetDecimal(13)).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarEs18A(String contrato, DateTime fecha_inicio, DateTime fecha_hasta,String formato_moneda) {
 
            DataTable dt = new DataTable();
            String[] column = {"NRO","CODIGO","NOMBRE","PAIS","CALIFICADORA","CLASIFICACION","N_REGISTRO","NOMBRE_1","TIP_CONTRATO","RAMO","INI_VIG","P_CEDITAS_B","P_CEDITAS_N","OBSERVACION"};
            for(int i= 0;i<column.Length;i++){
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18A;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                sqlcmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = fecha_inicio;
                sqlcmd.Parameters.Add("@FECHA_HASTA", SqlDbType.Date).Value = fecha_hasta;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[14];

                    obj[0] = dr.GetString(0);
                    obj[1] = dr.GetString(1);
                    obj[2] = dr.GetString(2);
                    obj[3] = dr.GetString(3);
                    obj[4] = dr.GetString(4);
                    obj[5] = dr.GetString(5);
                    obj[6] = dr.GetInt32(6);
                    obj[7] = dr.GetString(7);
                    obj[8] = dr.GetString(8);
                    obj[9] = dr.GetString(9);
                    obj[10] = dr.GetDateTime(10).ToShortDateString();
                    obj[11] = String.Format(formato_moneda, dr.GetDecimal(11)).Substring(3);
                    obj[12] = String.Format(formato_moneda,dr.GetDecimal(12)).Substring(3);
                    obj[13] = dr.GetString(13);
                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarEs18B(String contrato) {

            DataTable dt = new DataTable();
            String[] column = {"TIPO","RAMO","REASEGURADOR", "MODALIDAD","CODIGO","PORCENTAJE"};
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18B;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[6];

                    obj[0] = dr.GetString(0);
                    obj[1] = dr.GetString(1);
                    obj[2] = dr.GetString(2);
                    obj[3] = dr.GetString(3);
                    obj[4] = dr.GetString(4);
                    obj[5] = dr.GetDecimal(5);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarEs18C(String contrato, String formato_moneda){
            DataTable dt = new DataTable();
            String[] column = { "","","RAMO", "INICIO", "FIN", "CIENTO" ,"MTO_MAX_SEC","MTO_PLE_A","NRO_L_MUL","MTO_MAX_S2","CIENTO_RET","CIENTO_SEC","MTO_MX_COB"};
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18C;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[13];

                    obj[0] = dr.GetString(0);
                    obj[1] = dr.GetDateTime(1).ToShortDateString();
                    obj[2] = dr.GetDateTime(2).ToShortDateString();
                    obj[3] = dr.GetDecimal(3);
                    obj[4] = String.Format(formato_moneda,dr.GetDecimal(4)).Substring(3);
                    obj[5] = dr.GetDecimal(5);
                    obj[6] = String.Format(formato_moneda,dr.GetDecimal(6)).Substring(3);
                    obj[7] = String.Format(formato_moneda,dr.GetDecimal(7)).Substring(3);
                    obj[8] = dr.GetInt32(8);
                    obj[9] = String.Format(formato_moneda,dr.GetDecimal(9)).Substring(3);
                    obj[10] = String.Format(formato_moneda,dr.GetDecimal(10)).Substring(3);
                    obj[11] = String.Format(formato_moneda, dr.GetDecimal(11)).Substring(3);
                    obj[12] = String.Format(formato_moneda, dr.GetDecimal(12)).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarEs18D(String contrato, String formato_moneda)
        {
            DataTable dt = new DataTable();
            String[] column = { "NRO", "TIP_CONT", "RAMO", "NOMBRE", "CIENTO", "SBS", "INI_VIG", "FIN_VIG", "CAPA_XL", "PRIORIDAD", "SEC_EX_PRIO", "MTO_MAX_CAP_LIM_SUP", "PRIMA MIN" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18D;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[13];

                    obj[0] = dr.GetString(0);
                    obj[1] = dr.GetString(1);
                    obj[2] = dr.GetString(2);
                    obj[3] = dr.GetString(3);
                    obj[4] = String.Format(formato_moneda,dr.GetDecimal(4)).Substring(3);
                    obj[5] = dr.GetString(5);
                    obj[6] = dr.GetDateTime(6).ToShortDateString();
                    obj[7] = dr.GetDateTime(7).ToShortDateString();
                    obj[8] = dr.GetInt32(8);
                    obj[9] = String.Format(formato_moneda,dr.GetDecimal(9)).Substring(3);
                    obj[10] = String.Format(formato_moneda,dr.GetDecimal(10)).Substring(3);
                    obj[11] = String.Format(formato_moneda,dr.GetDecimal(11)).Substring(3);
                    obj[12] = String.Format(formato_moneda,dr.GetDecimal(12)).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;

        }
        public DataTable GetSelecionarEs18E(String contrato,DateTime fecha_inicio,DateTime fecha_hasta,String formato_menada)
        {
            DataTable dt = new DataTable();
            String[] column = { "NRO", "CODIGO", "NOMBRE", "RAMO", "ASEGURADO", "INI_VIG", "MERCADO_L", "MERCADO _EXT", "TOTAL_0", "PEN_LIQ", "LIQUIDAC", "TOTAL_1" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18E;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                sqlcmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = fecha_inicio;
                sqlcmd.Parameters.Add("@FECHA_HASTA", SqlDbType.Date).Value = fecha_hasta;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[12];

                    obj[0] = dr[0];
                    obj[1] = dr[1];
                    obj[2] = dr[2];
                    obj[3] = dr[3];
                    obj[4] = dr[4];
                    obj[5] = Convert.ToDateTime(dr[5]).ToShortDateString();
                    obj[6] = String.Format(formato_menada,dr[6]).Substring(3);
                    obj[7] = String.Format(formato_menada,dr[7]).Substring(3);
                    obj[8] = String.Format(formato_menada,dr[8]).Substring(3);
                    obj[9] = String.Format(formato_menada,dr[9]).Substring(3);
                    obj[10] = String.Format(formato_menada,dr[10]).Substring(3);
                    obj[11] = String.Format(formato_menada, dr[11]).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarEs18F(String contrato,DateTime fecha_inicio,DateTime fecha_hasta,String formato_moneda)
        {
            DataTable dt = new DataTable();
            String[] column = {"REASEGURADOR","PRIMA_CED", "SENIESTRO", "COMISION", "PRIMA_REASG", "SIN_REA_CED", "COM_REA_CED", "SALDO" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.ePselectEs18F;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                sqlcmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = fecha_inicio;
                sqlcmd.Parameters.Add("@FECHA_HASTA", SqlDbType.Date).Value = fecha_hasta;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[column.Length];

                    obj[0] = dr[0];
                    obj[1] = String.Format(formato_moneda, dr.GetDecimal(1)).Substring(3);
                    obj[2] = String.Format(formato_moneda,dr.GetDecimal(2)).Substring(3);
                    obj[3] = String.Format(formato_moneda,dr.GetDecimal(3)).Substring(3);
                    obj[4] = String.Format(formato_moneda,dr.GetDecimal(4)).Substring(3);
                    obj[5] = String.Format(formato_moneda,dr.GetDecimal(5)).Substring(3);
                    obj[6] = String.Format(formato_moneda,dr.GetDecimal(6)).Substring(3);
                    obj[7] = String.Format(formato_moneda, dr.GetDecimal(7)).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarModelo(String contrato, DateTime fecha_inicio, DateTime fecha_hasta, String formato_moneda,Int32 token)
        {
            DataTable dt = new DataTable();
            String[] column = { "REASEGURADOR","TIPO","NRO","FECHA","ASEGURADO","CONTRATO","RAMO","MONEDA","PRI_XPAG_REA_CED", "PRI_XCOB_REA_ACE", "SIN_XCOB_REA_CED", "SIN_XPAG_REA_ACE", "OTR_CTA_XCOB_REA_CED", "OTR_CTA_XPAG_REA_ACE", "DSCTO_COMIS_REA", "SALDO_DEUDOR", "SALDO_ACREEDOR","COMP_1","COMP_2" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                if(token == 1)
                    sqlcmd.CommandText = _db.ePselectModelo1;
                else
                    sqlcmd.CommandText = _db.ePselectModelo2;

                sqlcmd.Parameters.Clear();
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                sqlcmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = fecha_inicio;
                sqlcmd.Parameters.Add("@FECHA_HASTA", SqlDbType.Date).Value = fecha_hasta;

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    Object[] obj = new Object[column.Length];
                    obj[0] = dr[0];
                    obj[1] = dr[1];
                    obj[2] = dr[2];
                    obj[3] = Convert.ToDateTime(dr[3]).ToShortDateString();;
                    obj[4] = dr[4];
                    obj[5] = dr[5];
                    obj[6] = dr[6];
                    obj[7] = dr[7];
                    obj[8] = String.Format(formato_moneda, dr.GetDecimal(8)).Substring(3);
                    obj[9] = String.Format(formato_moneda,dr.GetDecimal(9)).Substring(3);
                    obj[10] = String.Format(formato_moneda,dr.GetDecimal(10)).Substring(3);
                    obj[11] = String.Format(formato_moneda,dr.GetDecimal(11)).Substring(3);
                    obj[12] = String.Format(formato_moneda,dr.GetDecimal(12)).Substring(3);
                    obj[13] = String.Format(formato_moneda,dr.GetDecimal(13)).Substring(3);
                    obj[14] = String.Format(formato_moneda,dr.GetDecimal(14)).Substring(3);
                    obj[15] = String.Format(formato_moneda,dr.GetDecimal(15)).Substring(3);
                    obj[16] = String.Format(formato_moneda, dr.GetDecimal(16)).Substring(3);
                    obj[17] = String.Format(formato_moneda, dr.GetDecimal(17)).Substring(3);
                    obj[18] = String.Format(formato_moneda, dr.GetDecimal(18)).Substring(3);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarAnexoEs2A(String contrato)
        {
            DataTable dt = new DataTable();
            String[] column = { "COD_SBS", "REASEGURADOR", "PRIMAS_X_PAG", "PRIMAS_X_COB", "SIN_X_COB", "SIN_X_PAGAR", "CUENTAS_X_PAG", "DES_COM_REASG", "SALDO_DEU", "SALDO_ACREED" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            /*DataSet ds = new DataSet();
            ds.Tables.Add(dt);*/
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.sSelectDatoM;

                sqlcmd.Parameters.Clear();
                /*sqlcmd.Parameters.Add("@TIPO_INFO", SqlDbType.Char).Value = tipo;
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.Char).Value = contrato;
                sqlcmd.Parameters.Add("@PAGE_INDEX", SqlDbType.Char).Value = index;
                sqlcmd.Parameters.Add("@PAGE_SIZE", SqlDbType.Char).Value = size;
                sqlcmd.Parameters.Add("@TOTALROW", SqlDbType.Int).Direction = ParameterDirection.Output;*/

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[11];

                    obj[0] = dr.GetDateTime(0);
                    obj[1] = dr.GetDateTime(1);
                    obj[2] = dr.GetInt32(2);
                    obj[3] = dr.GetInt32(3);
                    obj[4] = dr.GetDateTime(0);
                    obj[5] = dr.GetDateTime(1);
                    obj[6] = dr.GetInt32(2);
                    obj[7] = dr.GetInt32(3);
                    obj[8] = dr.GetDateTime(0);
                    obj[9] = dr.GetDateTime(1);
                    obj[10] = dr.GetInt32(2);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
        public DataTable GetSelecionarAnexoEs2B(String contrato)
        {
            DataTable dt = new DataTable();
            String[] column = { "COD_SBS", "NOMBRE_EMP/DET_CONT", "CUENTAS_X_COB/REASG_COASEG", "6_+_MES_ANTIG", "12_+_MES_ANTIG", "TOTAL" };
            for (int i = 0; i < column.Length; i++)
            {
                dt.Columns.Add(column[i]);
            }
            /*DataSet ds = new DataSet();
            ds.Tables.Add(dt);*/
            try
            {
                conexion.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = conexion;
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = _db.sSelectDatoM;

                sqlcmd.Parameters.Clear();
                /*sqlcmd.Parameters.Add("@TIPO_INFO", SqlDbType.Char).Value = tipo;
                sqlcmd.Parameters.Add("@NRO_CONTRATO", SqlDbType.Char).Value = contrato;
                sqlcmd.Parameters.Add("@PAGE_INDEX", SqlDbType.Char).Value = index;
                sqlcmd.Parameters.Add("@PAGE_SIZE", SqlDbType.Char).Value = size;
                sqlcmd.Parameters.Add("@TOTALROW", SqlDbType.Int).Direction = ParameterDirection.Output;*/

                SqlDataReader dr = sqlcmd.ExecuteReader();
                while (dr.Read())
                {
                    object[] obj = new object[6];

                    obj[0] = dr.GetDateTime(0);
                    obj[1] = dr.GetDateTime(1);
                    obj[2] = dr.GetInt32(2);
                    obj[3] = dr.GetInt32(3);
                    obj[4] = dr.GetDateTime(0);
                    obj[5] = dr.GetDateTime(1);

                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }
    }
}