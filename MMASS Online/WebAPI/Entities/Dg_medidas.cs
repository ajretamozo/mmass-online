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
    public class Dg_medidas
    {
        public int Id_medidadigital { get; set; }
        public string Descripcion { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public int Tipo { get; set; }

        public List<Dg_medidas> Medidas;

        public static Dg_medidas getDg_medidas(DataRow item)
        {
            Dg_medidas mi = new Dg_medidas();
            mi.Id_medidadigital = DB.DInt(item["Id_medidadigital"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            mi.Ancho = DB.DInt(item["Ancho"].ToString());
            mi.Alto = DB.DInt(item["Alto"].ToString());
            mi.Tipo = DB.DInt(item["Tipo"].ToString());
            return mi;
        }

        public static Dg_medidas getById(int Id_medidadigital)
        {
            string sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas" +
                                " where id_medidadigital = " + Id_medidadigital.ToString();
            Dg_medidas resultado;
            resultado = new Dg_medidas();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_medidas(t.Rows[0]);
            }
            return resultado;
        }

        public static List<Dg_medidas> getMedidas(int tipo)
        {
            string sqlCommand = "";
            if (tipo == 1)
            {
                sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas where tipo = 1 and es_borrado = 0 order by ancho, alto";
            }
            else if (tipo == 2)
            {
                sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas where tipo = 2 and es_borrado = 0 order by ancho, alto";
            }
            else
            {
                sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas where es_borrado = 0 order by ancho, alto";
            }
            List<Dg_medidas> col = new List<Dg_medidas>();
            Dg_medidas elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_medidas(item);
                col.Add(elem);
            }
            return col;
        }

        public void save()
        {
            String sqlId = "select max(id_medidadigital) as maximo from dg_medidas";
            int nuevoId = 0;
            DataTable t = DB.Select(sqlId);

            if (t.Rows.Count == 1)
            {
                nuevoId = DB.DInt(t.Rows[0]["maximo"].ToString());
                nuevoId++;
            }

            string sql = "insert into dg_medidas (id_medidadigital, descripcion, ancho, alto, tipo, es_borrado)" +
                                 " values (@id_medidadigital, @descripcion, @ancho, @alto, @tipo, 0)";

            List<SqlParameter> parametros = new List<SqlParameter>()
                    {
                        new SqlParameter()
                        { ParameterName="@id_medidadigital", SqlDbType = SqlDbType.Int, Value = nuevoId },
                        new SqlParameter()
                        { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                        new SqlParameter()
                        { ParameterName="@ancho", SqlDbType = SqlDbType.Int, Value = Ancho },
                        new SqlParameter()
                        { ParameterName="@alto", SqlDbType = SqlDbType.Int, Value = Alto },
                        new SqlParameter()
                        { ParameterName="@tipo", SqlDbType = SqlDbType.Int, Value = Tipo },
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

        public void updateMedidas()
        {
            string sql = "update dg_medidas set es_borrado = 0 where descripcion = '" + Descripcion + "'";

            try
            {
                DB.Execute(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool getByDesc(string desc)
        {
            bool resultado = false;
            string sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas" +
                                " where descripcion = '" + desc + "'";

            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = true;
            }
            return resultado;
        }

        public bool saveMedidas()
        {
            bool ret = true;
            DB.Execute("update dg_medidas set es_borrado = 1 where tipo = 1");
            foreach (Dg_medidas med in Medidas)
            {
                if (Dg_medidas.getByDesc(med.Descripcion) == false)
                {
                    med.save();
                }
                else
                {
                    med.updateMedidas();
                }
            }
            return ret;
        }

        public bool saveMedidasV()
        {
            bool ret = true;
            DB.Execute("update dg_medidas set es_borrado = 1 where tipo = 2");
            foreach (Dg_medidas med in Medidas)
            {
                if (Dg_medidas.getByDesc(med.Descripcion) == false)
                {
                    med.save();
                }
                else
                {
                    med.updateMedidas();
                }
            }
            return ret;
        }

        public static Dg_medidas getByDescripcion(string desc)
        {
            string sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas" +
                                " where descripcion = '" + desc + "'";
            Dg_medidas resultado = new Dg_medidas();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_medidas(t.Rows[0]);
            }
            return resultado;
        }
    }
}
