using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;


namespace WebApi.Entities
{
    public class Moneda
    {
        public int Id_moneda { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public int Decimales { get; set;}
        public float CambioCompra { get; set; }
        public float CambioVenta { get; set; }
        public bool Base { get; set; }
        public bool Borrado { get; set; }

        private static Moneda getMoneda(DataRow item)
        {
            Moneda mi = new Moneda
            {
                Id_moneda = DB.DInt(item["Id_moneda"].ToString()),
                Nombre = item["Nombre"].ToString(),
                Simbolo = item["Simbolo"].ToString(),
                Decimales = DB.DInt(item["Decimales"].ToString()),
                CambioCompra = DB.DFloat(item["CambioCompra"].ToString()),
                CambioVenta = DB.DFloat(item["CambioVenta"].ToString()),
                Base = (item["Base"].ToString()=="1"),
                Borrado = (item["Borrado"].ToString() == "1")
            };
            return mi;
        }
        public static Moneda getById(int Id)
        {
            string sqlCommand = "select Id_moneda, Nombre, Simbolo, Decimales, CambioCompra, CambioVenta, Base, Borrado from moneda where Id_moneda = " + Id.ToString();
            Moneda resultado;
            resultado = new Moneda();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getMoneda(t.Rows[0]);
            }
            return resultado;
        }

        public static Moneda getMonedaBase()
        {
            string sqlCommand = "select Id_moneda, Nombre, Simbolo, Decimales, CambioCompra, CambioVenta, Base, Borrado from moneda where Base = 1";
            Moneda resultado;
            resultado = new Moneda();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getMoneda(t.Rows[0]);
            }
            return resultado;
        }


        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_moneda == 0)
            {
                sql = "insert into moneda (Id_moneda, Nombre, Simbolo, Decimales, CambioCompra, CambioVenta, Base, Borrado) " +
                    "values (@Id_moneda, @Nombre, @Simbolo, @Decimales, @CambioCompra, @CambioVenta, @Base, 0)" ;


                DataTable t = DB.Select("select max(Id_moneda) as maximo from moneda");

                if (t.Rows.Count == 1)
                {
                    Id_moneda = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update moneda set Nombre = @Nombre, Simbolo = @Simbolo, Decimales = @Decimales, CambioCompra = @CambioCompra, CambioVenta = @CambioVenta, Base = @ Base " +
                    " where Id_moneda = @Id_moneda";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@Id_moneda",SqlDbType = SqlDbType.Int, Value = Id_moneda},
                    new SqlParameter()
                    { ParameterName="@nombre",SqlDbType = SqlDbType.NVarChar, Value = Nombre },
                    new SqlParameter()
                    { ParameterName = "@Simbolo", SqlDbType = SqlDbType.NVarChar, Value = Simbolo },
                    new SqlParameter()
                    { ParameterName = "@Decimales", SqlDbType = SqlDbType.Int, Value =  Decimales},
                    new SqlParameter()
                    { ParameterName = "@CambioCompra", SqlDbType = SqlDbType.Float, Value = CambioCompra },
                    new SqlParameter()
                    { ParameterName = "@CambioVenta", SqlDbType = SqlDbType.Float, Value = CambioVenta},
                    new SqlParameter()
                    { ParameterName = "@Base", SqlDbType = SqlDbType.Int, Value = Convert.ToInt32(Base) } 
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

        public static List<Moneda> getAll()
        {
            string sqlCommand = "select Id_moneda, Nombre, Simbolo, Decimales, CambioCompra, CambioVenta, Base, Borrado from moneda where Borrado = 0";

            List<Moneda> col = new List<Moneda>();
            Moneda elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = getMoneda(item);
                col.Add(elem);
            }
            return col;
        }
    }
}
