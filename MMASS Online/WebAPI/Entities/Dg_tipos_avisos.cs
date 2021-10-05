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
            string sqlCommand = "Select * FROM Dg_tipos_avisos where Id_tipo_aviso_dg = " + Id.ToString();
            Dg_tipos_avisos resultado;
            resultado = new Dg_tipos_avisos();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count ==1)
            {
                resultado.Id_tipo_aviso_dg = int.Parse(t.Rows[0]["Id_tipo_aviso_dg"].ToString());
                resultado.Descripcion = t.Rows[0]["Descripcion"].ToString();
                resultado.Permite_envio_ads = (t.Rows[0]["Permite_envio_ads"].ToString() == "1");
                resultado.Es_borrado = (t.Rows[0]["Es_borrado"].ToString() == "1");
            }
            return resultado;
        }
        public static Dg_tipos_avisos getDg_tipos_avisos(DataRow item)
        {
            Dg_tipos_avisos mi = new Dg_tipos_avisos
            {
                Id_tipo_aviso_dg = int.Parse(item["Id_tipo_aviso_dg"].ToString()),
                Descripcion = item["Descripcion"].ToString(),
                Permite_envio_ads = (item["Permite_envio_ads"].ToString() == "1"),
                Es_borrado = (item["Es_borrado"].ToString() == "1")
            };
            return mi;
        }
        public static List<Dg_tipos_avisos> getAll()
        {
            string sql = "Select Id_tipo_aviso_dg, Descripcion, Permite_envio_ads, Es_borrado FROM Dg_tipos_avisos where (Es_borrado = 0) or (Es_borrado is null) ";
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
                sql = "insert into dg_tipos_avisos (Descripcion, Permite_envio_ads,Es_borrado)" +
                    " values ( @Descripcion, @Permite_envio_ads,0)";
                DataTable t = DB.Select("SELECT IDENT_CURRENT('Dg_tipos_avisos') AS ULTIMO ");
                if (t.Rows.Count == 1)
                {
                    Id_tipo_aviso_dg = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update Dg_tipos_avisos set Descripcion = @Descripcion, Permite_envio_ads = @Permite_envio_ads " +
                " where Id_tipo_aviso_dg = @Id_tipo_aviso_dg";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@Id_tipo_aviso_dg",SqlDbType = SqlDbType.Int, Value = Id_tipo_aviso_dg },
                    new SqlParameter()
                    { ParameterName="@Descripcion",SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
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
                sql = "update Dg_tipos_avisos set es_borrado = 1 where Id_tipo_aviso_dg = " + Id_tipo_aviso_dg.ToString();
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
            string sqlCommand = " Select Id_tipo_aviso_dg, Descripcion, Permite_envio_ads, Es_borrado FROM Dg_tipos_avisos where ((Es_borrado = 0) or (Es_borrado is null))";

            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and Descripcion like '%" + p.Value + "%'";
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
