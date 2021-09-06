using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
namespace WebApi.Entities
{
    public class Dg_emplazamientos
    {
        public int Id_emplazamiento { get; set; }
        public string Descripcion { get; set; }    
        public long Codigo_emplazamiento { get; set; }
        public bool Es_borrado { get; set; }

        public static Dg_emplazamientos getDg_emplazamientos(DataRow item)
        {
            Dg_emplazamientos mi = new Dg_emplazamientos();
            mi.Id_emplazamiento = DB.DInt(item["Id_emplazamiento"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            mi.Codigo_emplazamiento = DB.DLong(item["Codigo_emplazamiento"].ToString());
            mi.Es_borrado = (item["Es_borrado"].ToString() == "1");

            return mi;
        }

        public static Dg_emplazamientos getById(int Id_emplazamiento)
        {
            string sqlCommand = " select id_emplazamiento, descripcion, codigo_emplazamiento, es_borrado from dg_emplazamientos" +
                                " where id_emplazamiento = " + Id_emplazamiento.ToString();
            Dg_emplazamientos resultado;
            resultado = new Dg_emplazamientos();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_emplazamientos(t.Rows[0]);
            }
            return resultado;
        }
        public static List<Dg_emplazamientos> getAll()
        {
            string sqlCommand = " select id_emplazamiento, descripcion, codigo_emplazamiento, es_borrado from dg_emplazamientos where es_borrado = 0 ";
            List<Dg_emplazamientos> col = new List<Dg_emplazamientos>();
            Dg_emplazamientos elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_emplazamientos(item);
                col.Add(elem);
            }
            return col;
        }

        public void save()
        {
            string sql = "insert into dg_emplazamientos (descripcion, codigo_emplazamiento, es_borrado)" +
                                 " values ( @descripcion, @codigo_emplazamiento, 0)";

                    List<SqlParameter> parametros = new List<SqlParameter>()
                    {
                        new SqlParameter()
                        { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                        new SqlParameter()
                        { ParameterName="@codigo_emplazamiento", SqlDbType = SqlDbType.BigInt, Value = Codigo_emplazamiento }
                    };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
   
        }

        public void updateEmplaza()
        { 
            string sql = "update dg_emplazamientos set es_borrado = 0 where codigo_emplazamiento = " + Codigo_emplazamiento.ToString();

            try
            {
                DB.Execute(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool getByCodigo(long codigo)
        { 
            bool resultado = false;
            string sqlCommand = " select id_emplazamiento, descripcion, codigo_emplazamiento, es_borrado from dg_emplazamientos" +
                                " where codigo_emplazamiento = " + codigo.ToString();
           
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = true;
            }
            return resultado;
        }

    }
}
