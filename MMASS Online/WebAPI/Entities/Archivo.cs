using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using WebApi.Helpers;
using System.IO;
using System.Transactions;

namespace WebApi.Entities
{
    public class Archivo
    {
        public IFormFile File { get; set; }
        public int Id_archivo { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public int Id_op { get; set; }

        public static List<Archivo> getArchivosPorOP(int id_op)
        {
            string sqlCommand = "select id_adjunto_dg, nombre_archivo, path, id_op_dg, es_borrado from dg_adjuntos where es_borrado != 1 and id_op_dg = " + id_op.ToString();

            List<Archivo> col = new List<Archivo>();
            Archivo elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Archivo
                {
                    Id_archivo = DB.DInt(item["id_adjunto_dg"].ToString()),
                    Id_op = DB.DInt(item["id_op_dg"].ToString()),
                    Nombre = item["nombre_archivo"].ToString(),
                    Ruta = item["path"].ToString()
                };
                col.Add(elem);
            }
            return col;
        }

        public bool save1()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_archivo == 0)
            {
                sql = " insert into dg_adjuntos(nombre_archivo, path, id_op_dg, es_borrado) " +
                      " values(@nombre_archivo, @path, @id_op_dg, 0)";

                DataTable t = DB.Select("SELECT IDENT_CURRENT('dg_adjuntos') AS ULTIMO ");
                if (t.Rows.Count == 1)
                {
                    Id_archivo = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
                }
            }
            else
            {
                sql = " update dg_adjuntos set nombre_archivo = @nombre_archivo, path = @path, id_op_dg = @id_op_dg " +
                      " where id_adjunto_dg = @id_adjunto_dg";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_adjunto_dg",SqlDbType = SqlDbType.Int, Value = Id_archivo },
                    new SqlParameter()
                    { ParameterName="@id_op_dg",SqlDbType = SqlDbType.Int, Value = Id_op },
                    new SqlParameter()
                    { ParameterName="@nombre_archivo",SqlDbType = SqlDbType.NVarChar, Value = Nombre },
                    new SqlParameter()
                    { ParameterName="@path", SqlDbType = SqlDbType.NVarChar, Value = Ruta }
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
        public static bool deleteFile(int id_file)
        {

            string sql = "";

            if (id_file != 0)
            {
                sql = "update dg_adjuntos set es_borrado = 1 where id_adjunto_dg = " + id_file.ToString();
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
        public int save()
        {
            char[] separator = { '.' };
            var nameExt = Nombre.Split(separator);
            var file_ext = nameExt[1];
            var prox = 1;
            string sql = "";
            // Si es nuevo va insert, sino update
                sql = "insert into dg_adjuntos (nombre_archivo, path, id_op_dg, es_borrado)" +
                    " values ( @nombre_archivo, @path , @id_op_dg,0)";
            DataTable t = DB.Select("SELECT IDENT_CURRENT('dg_adjuntos') AS ULTIMO ");
            DataTable t2 = DB.Select("SELECT count(*) as COUNT from dg_adjuntos");
            var count = int.Parse(t2.Rows[0]["COUNT"].ToString());
            if (t.Rows.Count == 1 && count > 0)
            {
                prox = int.Parse(t.Rows[0]["ULTIMO"].ToString()) + 1;
            }
            var ruta = "/files/"+ prox + "." + file_ext;
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@nombre_archivo",SqlDbType = SqlDbType.NVarChar, Value = Nombre },
                    new SqlParameter()
                    { ParameterName="@path",SqlDbType = SqlDbType.NVarChar, Value = ruta },
                    new SqlParameter()
                    { ParameterName="@id_op_dg", SqlDbType = SqlDbType.SmallInt, Value = Id_op }
              };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return prox;
        }
    }
}
