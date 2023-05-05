using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;

namespace WebApi.Entities
{
    public class Tabla
    {
        public int Id_tabla { get; set; }
        public string Desc_tabla { get; set; }
        public int Id_tipo_tabla { get; set; }
        public bool Es_borrado { get; set; }
        public int Id_externo { get; set; }
        public string Opcion1 { get; set; }
        public string Opcion2 { get; set; }
        public string Opcion3 { get; set; }
        public string Codigo_aux { get; set; }

        public static Tabla getTabla(DataRow item)
        {
            Tabla mi = new Tabla
            {
                Id_tabla = int.Parse(item["id_tabla"].ToString()),
                Desc_tabla = item["desc_tabla"].ToString(),
                Id_tipo_tabla =  DB.DInt(item["id_tipo_tabla"].ToString()),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Id_externo = DB.DInt(item["id_externo"].ToString()),
                Opcion1 = item["opcion1"].ToString(),
                Opcion2 = item["opcion2"].ToString(),
                Opcion3 = item["opcion3"].ToString(),
                Codigo_aux = item["codigo_aux"].ToString()
            };
            return mi;
        }

        public Tabla getById(int Id)
        {
            string sqlCommand = "select id_tabla, desc_tabla, id_tipo_tabla, es_borrado, id_externo, opcion1, opcion2, opcion3, codigo_aux from tabla where Id_tabla = " + Id.ToString();
            Tabla resultado;
            resultado = new Tabla();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getTabla(t.Rows[0]);                
            }
            return resultado;
        }

        public bool Save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_tabla == 0)
            {
                sql = " insert into tabla (id_tabla, desc_tabla, id_tipo_tabla, es_borrado, id_externo, opcion1, opcion2, opcion3, codigo_aux) " +
                      " values (@id_tabla, @desc_tabla, @id_tipo_tabla, @es_borrado, @id_externo, @opcion1, @opcion2, @opcion3, @codigo_aux)";

                DataTable t = DB.Select("select max(id_tabla) as maximo from tabla");

                if (t.Rows.Count == 1)
                {
                    Id_tabla = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update tabla set desc_tabla = @desc_tabla, id_tipo_tabla = @id_tipo_tabla, es_borrado = @es_borrado, id_externo = @id_externo, opcion1 = @opcion1, opcion2 = @opcion2, opcion3 = opcion3, codigo_aux = @codigo_aux " +
                    " where id_tabla = @id_tabla ";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_tabla",SqlDbType = SqlDbType.Int, Value = Id_tabla},
                    new SqlParameter()
                    { ParameterName = "@desc_tabla", SqlDbType = SqlDbType.NVarChar, Value = Desc_tabla},
                    new SqlParameter()
                    { ParameterName = "@id_tipo_tabla", SqlDbType = SqlDbType.Int, Value = Id_tipo_tabla},
                    new SqlParameter()
                    { ParameterName = "@es_borrado", SqlDbType = SqlDbType.SmallInt, Value = Es_borrado},
                    new SqlParameter()
                    { ParameterName = "@id_externo",SqlDbType = SqlDbType.Int, Value = Id_externo },
                    new SqlParameter()
                    { ParameterName = "@opcion1",SqlDbType = SqlDbType.NVarChar,Value = Opcion1 },
                    new SqlParameter()
                    { ParameterName = "@opcion2",SqlDbType = SqlDbType.NVarChar,Value = Opcion2 },
                    new SqlParameter()
                    { ParameterName = "@opcion3",SqlDbType = SqlDbType.NVarChar,Value = Opcion3 },
                    new SqlParameter()
                    { ParameterName = "@codigo_aux",SqlDbType = SqlDbType.NVarChar,Value = Codigo_aux }
            };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static List<Tabla> getAll(string CodigoTipoTabla)
        {
            string sql = "select t.id_tabla, t.desc_tabla, t.id_tipo_tabla, t.es_borrado, t.id_externo, t.opcion1, t.opcion2, t.opcion3, t.codigo_aux from tabla t inner join tipo_tabla tt on tt.id_tipo_tabla = t.id_tipo_tabla " +
                            " where t.es_borrado = 0 and codigo_tipo_tabla = '" + CodigoTipoTabla + "'";
            List<Tabla> col = new List<Tabla>();
            Tabla elem;
            DataTable t = DB.Select(sql);

            foreach (DataRow item in t.Rows)
            {
                elem = getTabla(item);
                col.Add(elem);
            }
            return col;
        }

    }
}
