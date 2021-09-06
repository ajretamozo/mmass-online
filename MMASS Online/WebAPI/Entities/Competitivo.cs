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
    public class Competitivo
    {
        public int Id_competitivo { get; set; }
        public string Desc_competitivo { get; set; }
        public bool Es_borrado { get; set; }
        public int Id_categoria_producto { get; set; }
	    public bool Usar_tope { get; set; }
	    public bool No_permite_neto { get; set; }
        public int Id_rubroproducto { get; set; }
        public bool Inv_seg { get; set; }
        public static Competitivo getById(int Id)
        {
            string sqlCommand = " select id_competitivo, desc_competitivo, es_borrado, id_categoria_producto, usar_tope, " +
                                " no_permite_neto, id_rubroproducto, inv_seg from competitivos where id_competitivo = " + Id.ToString();
            Competitivo resultado;
            resultado = new Competitivo();
            DataTable t = DB.Select(sqlCommand);            

            if (t.Rows.Count == 1)
            {
                resultado.Id_competitivo = int.Parse(t.Rows[0]["id_competitivo"].ToString());
                resultado.Desc_competitivo = t.Rows[0]["desc_competitivo"].ToString();
                resultado.Id_categoria_producto = int.Parse(t.Rows[0]["id_categoria_producto"].ToString());
                resultado.Id_rubroproducto = int.Parse(t.Rows[0]["id_rubroproducto"].ToString());
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
                resultado.Usar_tope = (t.Rows[0]["usar_tope"].ToString() == "1");
                resultado.No_permite_neto = (t.Rows[0]["no_permite_neto"].ToString() == "1");
                resultado.Inv_seg =  (t.Rows[0]["inv_seg"].ToString() == "1");
            }
            return resultado;
        }

        public static List<Competitivo> getAll()
        {
            string sqlCommand = " select id_competitivo, desc_competitivo, es_borrado, id_categoria_producto, usar_tope, no_permite_neto, id_rubroproducto, inv_seg " +
                                " from competitivos";

            List<Competitivo> col = new List<Competitivo>();
            Competitivo elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Competitivo
                {
                    Id_competitivo = int.Parse(item["id_competitivo"].ToString()),
                    Desc_competitivo = item["desc_competitivo"].ToString(),
                    Id_categoria_producto = DB.DInt(item["id_categoria_producto"]),
                    Usar_tope = (item["usar_tope"].ToString()=="1"),
                    No_permite_neto = (item["no_permite_neto"].ToString() == "1"),
                    Id_rubroproducto = DB.DInt(item["Id_rubroproducto"]),
                    Inv_seg = (item["inv_seg"].ToString() == "1"),
                    Es_borrado = (item["Es_borrado"].ToString() == "1")
                };
                col.Add(elem);
            }
            return col;
        }
        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_competitivo == 0)
            {
                sql = " insert into competitivos (id_competitivo,desc_competitivo,es_borrado,usar_tope, no_permite_neto,id_rubroproducto, inv_seg) " +
                                 " values(@id_competitivo, @desc_competitivo, @es_borrado, @usar_tope, @no_permite_neto, @id_rubroproducto, @inv_seg)";
                DataTable t = DB.Select("select max(id_competitivo) as maximo from competitivos");

                if (t.Rows.Count == 1)
                {
                    Id_competitivo = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = " update competitivos set desc_competitivo = @desc_competitivo, es_borrado = @es_borrado, usar_tope = @usar_tope, no_permite_neto = @no_permite_neto, id_rubroproducto = @id_rubroproducto, inv_seg = @inv_seg " +
                                 " where id_competitivo = @id_competitivo";
            }

            List<SqlParameter> parametros = new List<SqlParameter>()
                 {
                     new SqlParameter()
                     { ParameterName="@id_competitivo",SqlDbType = SqlDbType.Int, Value = Id_competitivo },
                     new SqlParameter()
                     { ParameterName = "@desc_competitivo", SqlDbType = SqlDbType.NVarChar, Value = Desc_competitivo },
                     new SqlParameter()
                     { ParameterName = "@es_borrado",SqlDbType = SqlDbType.Int,Value = Es_borrado },
                     new SqlParameter()
                     { ParameterName = "@usar_tope",SqlDbType = SqlDbType.Int,Value = Usar_tope },
                     new SqlParameter()
                     { ParameterName = "@no_permite_neto",SqlDbType = SqlDbType.Int,Value = Id_rubroproducto },
                     new SqlParameter()
                     { ParameterName = "@no_permite_neto",SqlDbType = SqlDbType.Int,Value = Id_rubroproducto },
                     new SqlParameter()
                     { ParameterName = "@inv_seg",SqlDbType = SqlDbType.Int,Value = Inv_seg }
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
    }
}
