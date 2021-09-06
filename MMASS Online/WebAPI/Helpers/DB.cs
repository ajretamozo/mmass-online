using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebApi.Helpers
{
    
    /// <summary>
    /// Clase de acceso a datos.
    /// </summary>
    /// 
    public class DB
    {
        private static readonly object conexionLock = new object();
        private static SqlConnection _conexion = null;
        private static SqlConnection conexion {
            get {
                lock (conexionLock) {
                    if (_conexion == null) {
                        _conexion = new SqlConnection(Startup.StaticConfig.GetSection("AppSettings").GetSection("ConexionDB").Value + " MultipleActiveResultSets=true; ");
                    }
                    return _conexion;
                }
            }
        }
        public static DataTable Select(string sql, List<SqlParameter> parameters = null)
        {
            DataTable dt = new DataTable();            
            //string connString = "Data Source=10.0.0.15\\SQL2014,8034;Initial Catalog=MMASS_METRO4;User ID=sa;Password=mms435;";                       
            
            string query = sql;

           
            SqlCommand cmd = new SqlCommand(query, conexion);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            lock (conexionLock)
            {

                try
                {
                    conexion.Open();
                    // this will query your database and return the result to your datatable
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexion.Close();
                    da.Dispose();
                }                
                
            }
            return dt;
        }
        public static void Execute(string sql)
        {
           
            string query = sql;

            
            SqlCommand cmd = new SqlCommand(query, conexion);
            lock (conexionLock)
            {
                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {                
                    throw ex;
                }
                finally {
                    conexion.Close();
                }

                
            }
        }



        public static void Execute(string sql, List<SqlParameter> parameters = null)
        {
           
            string query = sql;

           
            SqlCommand cmd = new SqlCommand(query, conexion);
            if (parameters != null) {                
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            lock (conexionLock)
            {
                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conexion.Close();
                }
            }
        }


        public static DateTime? DFecha(Object data)
        {
            DateTime result;
            if (data == null) { return null; }
            if (DateTime.TryParse(data.ToString(), out result))
                return result;
            else
                return null;
        }

        public static float DFloat(Object data)
        {
            float result;
            if (float.TryParse(data.ToString(), out result))
                return result;
            else
                return 0;
        }
        public static long DLong(Object data)
        {
            long result;
            if (long.TryParse(data.ToString(), out result))
                return result;
            else
                return 0;
        }
        public static int DInt(Object data)
        {
            int result;
            if (int.TryParse(data.ToString(), out result))
                return result;
            else
                return 0;
        }


    }
}