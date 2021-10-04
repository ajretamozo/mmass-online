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
    public class Dg_red_GAM
    {

        public int Id_red { get; set; }
        public long Codigo_red { get; set; }
        public string Descripcion { get; set; }
        public bool Es_borrado { get; set; }


        public static Dg_red_GAM getDg_red_GAM(DataRow item)
        {
            Dg_red_GAM mi = new Dg_red_GAM();
            mi.Id_red = DB.DInt(item["Id_red"].ToString());
            mi.Codigo_red = DB.DLong(item["Codigo_red"].ToString());
            mi.Descripcion = item["Descripcion"].ToString();
            mi.Es_borrado = (item["Es_borrado"].ToString() == "0");
            return mi;
        }

        public static Dg_red_GAM getByCodigo(long netCode)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
                                " where es_borrado = 0 and codigo_red = " + netCode.ToString();
            Dg_red_GAM resultado;
            resultado = new Dg_red_GAM();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_red_GAM(t.Rows[0]);
            }
            return resultado;
        }

        public static Dg_red_GAM getById(int id)
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM" +
                                " where es_borrado = 0 and id_red = " + id.ToString();
            Dg_red_GAM resultado;
            resultado = new Dg_red_GAM();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getDg_red_GAM(t.Rows[0]);
            }
            return resultado;
        }

        public static List<Dg_red_GAM> getAll()
        {
            string sqlCommand = " select id_red, codigo_red, descripcion from dg_red_GAM where es_borrado = 0";
            List<Dg_red_GAM> col = new List<Dg_red_GAM>();
            Dg_red_GAM elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_red_GAM(item);
                col.Add(elem);
            }
            return col;
        }

        public static List<string> getCodigos()
        {
            string sqlCommand = " select codigo_red from dg_red_GAM where es_borrado = 0";
            List<string> col = new List<string>();
            string elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = item["codigo_red"].ToString();
                col.Add(elem);
            }
            return col;
        }

        public void save()
        {
            String sqlId = "select max(id_red) as maximo from dg_red_GAM";
            int nuevoId = 0;
            DataTable t = DB.Select(sqlId);

            if (t.Rows.Count == 1)
            {
                nuevoId = DB.DInt(t.Rows[0]["maximo"].ToString());
                nuevoId++;
            }

            string sql = "insert into dg_red_GAM (id_red, descripcion, codigo_red, es_borrado)" +
                                 " values (@id_red, @descripcion, @codigo_red, 0)";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_red", SqlDbType = SqlDbType.Int, Value = nuevoId },
                new SqlParameter()
                { ParameterName="@descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                new SqlParameter()
                { ParameterName="@codigo_red", SqlDbType = SqlDbType.BigInt, Value = Codigo_red }
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

        //SEGUIR EL SAVE EN BASE A ESTE DE ABAJO AGREGANDO EL UPDATE

        //public bool save()
        //{

        //    string sql = "";
        //    // Si es nuevo va insert, sino update
        //    if (Id_tipo_aviso_dg == 0)
        //    {
        //        sql = "insert into dg_tipos_avisos (Descripcion, Permite_envio_ads,Es_borrado)" +
        //            " values ( @Descripcion, @Permite_envio_ads,0)";
        //        DataTable t = DB.Select("SELECT IDENT_CURRENT('Dg_tipos_avisos') AS ULTIMO ");
        //        if (t.Rows.Count == 1)
        //        {
        //            Id_tipo_aviso_dg = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
        //        }
        //    }
        //    else
        //    {
        //        sql = "update Dg_tipos_avisos set Descripcion = @Descripcion, Permite_envio_ads = @Permite_envio_ads " +
        //        " where Id_tipo_aviso_dg = @Id_tipo_aviso_dg";
        //    }
        //    List<SqlParameter> parametros = new List<SqlParameter>()
        //        {
        //            new SqlParameter()
        //            { ParameterName="@Id_tipo_aviso_dg",SqlDbType = SqlDbType.Int, Value = Id_tipo_aviso_dg },
        //            new SqlParameter()
        //            { ParameterName="@Descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
        //            new SqlParameter()
        //            { ParameterName="@Permite_envio_ads", SqlDbType = SqlDbType.SmallInt, Value = Permite_envio_ads }
        //      };
        //    try
        //    {
        //        DB.Execute(sql, parametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    return true;
        //}

        public bool deleteRed()
        {
            string sql = "update dg_red_GAM set es_borrado = 1 where id_red = " + Id_red.ToString() ;

            if (Id_red != 0)
            {
                try
                {
                    DB.Execute(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }

        //public static bool getByDesc(string desc)
        //{
        //    bool resultado = false;
        //    string sqlCommand = " select id_medidadigital, descripcion, ancho, alto, tipo from dg_medidas" +
        //                        " where descripcion = '" + desc + "'";

        //    DataTable t = DB.Select(sqlCommand);

        //    if (t.Rows.Count == 1)
        //    {
        //        resultado = true;
        //    }
        //    return resultado;
        //}

        //public bool saveMedidas()
        //{
        //    bool ret = true;
        //    DB.Execute("update dg_medidas set es_borrado = 1");
        //    foreach (Dg_medidas med in Medidas)
        //    {
        //        if (Dg_medidas.getByDesc(med.Descripcion) == false)
        //        {
        //            med.save();
        //        }
        //        else
        //        {
        //            med.updateMedidas();
        //        }
        //    }
        //    return ret;
        //}

    }
}
