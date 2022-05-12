using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Data.SqlClient;
using System.Transactions;

namespace WebApi.Entities
{
    public class Dg_tipos_avisos
    {
        public int Id_tipo_aviso_dg { get; set; }
        public string Descripcion { get; set; }
        public bool Permite_envio_ads { get; set; }
        public bool Es_borrado { get; set; }
        
        public static Dg_tipos_avisos getById(int Id)
        {
            string sqlCommand = "Select * FROM categorias where (tipomedio = 0 or tipomedio = 2) and Id_categoria = " + Id.ToString();
            Dg_tipos_avisos resultado;
            resultado = new Dg_tipos_avisos();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count ==1)
            {
                resultado.Id_tipo_aviso_dg = int.Parse(t.Rows[0]["Id_categoria"].ToString());
                resultado.Descripcion = t.Rows[0]["desc_categoria"].ToString();
                resultado.Permite_envio_ads = (t.Rows[0]["Permite_envio_ads"].ToString() == "1");
                resultado.Es_borrado = (t.Rows[0]["Es_borrado"].ToString() == "1");
            }
            return resultado;
        }
        public static Dg_tipos_avisos getDg_tipos_avisos(DataRow item)
        {
            Dg_tipos_avisos mi = new Dg_tipos_avisos
            {
                Id_tipo_aviso_dg = int.Parse(item["Id_categoria"].ToString()),
                Descripcion = item["desc_categoria"].ToString(),
                Permite_envio_ads = (item["Permite_envio_ads"].ToString() == "1"),
                Es_borrado = (item["Es_borrado"].ToString() == "1")
            };
            return mi;
        }
        public static List<Dg_tipos_avisos> getAll()
        {
            string sql = "Select id_categoria, desc_categoria, Permite_envio_ads, Es_borrado FROM categorias where (tipomedio = 0 or tipomedio = 2) and (Es_borrado = 0 or Es_borrado is null) ";
            List<Dg_tipos_avisos> col = new List<Dg_tipos_avisos>();
            Dg_tipos_avisos elem;
            DataTable t = DB.Select(sql);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_tipos_avisos(item);
                col.Add(elem);
            }
            return col;
        }
        public bool save() {

            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_tipo_aviso_dg == 0)
            {
                sql = "insert into categorias (id_categoria, desc_categoria, Permite_envio_ads, Es_borrado, tipomedio)" +
                    " values (@id_categoria, @desc_categoria, @Permite_envio_ads, 0, 2)";
                DataTable t = DB.Select("select IsNull(max(ID_CATEGORIA),0) as ULTIMO from categorias");
                if (t.Rows.Count == 1)
                {
                    Id_tipo_aviso_dg = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update categorias set desc_categoria = @desc_categoria, Permite_envio_ads = @Permite_envio_ads " +
                " where id_categoria = @id_categoria";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_categoria",SqlDbType = SqlDbType.Int, Value = Id_tipo_aviso_dg },
                    new SqlParameter()
                    { ParameterName="@desc_categoria",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                    new SqlParameter()
                    { ParameterName="@Permite_envio_ads", SqlDbType = SqlDbType.SmallInt, Value = Permite_envio_ads }
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

        public bool remove()
        {
            string sql = "";

            if (Id_tipo_aviso_dg != 0)
            {
                sql = "update categorias set es_borrado = 1 where id_categoria = " + Id_tipo_aviso_dg.ToString();
                try
                {
                    using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                    {
                        DB.Execute(sql);
                        transaccion.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return true;
        }

        public static List<Dg_tipos_avisos> filter(List<Parametro> parametros)
        {
            string sqlCommand = " Select id_categoria, desc_categoria, Permite_envio_ads, Es_borrado FROM categorias where (tipomedio = 0 or tipomedio = 2) and (Es_borrado = 0 or Es_borrado is null)";

            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and desc_categoria like '%" + p.Value + "%'";
                }
            }

            List<Dg_tipos_avisos> col = new List<Dg_tipos_avisos>();
            Dg_tipos_avisos elem;
            DataTable t = DB.Select(sqlCommand + mifiltro);

            foreach (DataRow item in t.Rows)
            {
                elem = getDg_tipos_avisos(item);
                col.Add(elem);
            }
            return col;
        }

    }
}
