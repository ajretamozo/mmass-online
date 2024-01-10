using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
using System.Transactions;

namespace WebApi.Entities
{
    public class Tarifa_forma_uso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public static Tarifa_forma_uso getFormaUso(int indice)
        {
            if ((indice < 0) || (indice > 3))
            {
                return null;
            }
            else
            {
                List<Tarifa_forma_uso> col = new List<Tarifa_forma_uso>();
                Tarifa_forma_uso item = new Tarifa_forma_uso();
                item.Id = 0;
                item.Descripcion = "x Segundo";
                col.Add(item);
                item = new Tarifa_forma_uso();
                item.Id = 1;
                item.Descripcion = "x Spot";
                col.Add(item);
                item = new Tarifa_forma_uso();
                item.Id = 2;
                item.Descripcion = "x Renglón";
                col.Add(item);
                item = new Tarifa_forma_uso();
                item.Id = 3;
                item.Descripcion = "x Rating";
                col.Add(item);

                return col[indice];
            }
        }
        public static List<Tarifa_forma_uso> getAll()
        {
            List<Tarifa_forma_uso> col = new List<Tarifa_forma_uso>();
            Tarifa_forma_uso item = new Tarifa_forma_uso();
            item.Id = 0;
            item.Descripcion = "x Segundo";
            col.Add(item);
            item = new Tarifa_forma_uso();
            item.Id = 1;
            item.Descripcion = "x Spot";
            col.Add(item);
            item = new Tarifa_forma_uso();
            item.Id = 2;
            item.Descripcion = "x Renglón";
            col.Add(item);
            item = new Tarifa_forma_uso();
            item.Id = 3;
            item.Descripcion = "x Rating";
            col.Add(item);

            return col;
        }
    }

    public class Tarifas
    {
        public int Id_tarifa { get; set; }
        public string Desc_tarifa { get; set; }
        public DateTime Fvigen_desde { get; set; }
        public DateTime? Fvigen_hasta { get; set; }
        public double Importe_tarifa { get; set; }
        public bool Es_borrado { get; set; }
        public int Id_categoria { get; set; }
        public DateTime? Hs_desde { get; set; }
        public DateTime? Hs_hasta { get; set; }
        public Tarifa_forma_uso Forma_uso { get; set; }
        public int Id_moneda { get; set; }
        public float Cambio { get; set; }
        public string SimboloMoneda { get; set; }

        public List<Medio> Medios;


        public static Tarifas getById(int Id)
        {
            string sqlCommand = " select id_tarifa, desc_tarifa, fvigen_desde, fvigen_hasta, importe_tarifa, es_borrado, " +
                                " id_categoria, hs_desde, hs_hasta, formausotarifa, id_moneda, cambio " +
                                " from Tarifas where id_tarifa = " + Id.ToString();
           
            Tarifas resultado = new Tarifas();
            DataTable t = DB.Select(sqlCommand);
            DataTable det = new DataTable();
            if (t.Rows.Count == 1)
            {
                resultado.Id_tarifa = int.Parse(t.Rows[0]["id_tarifa"].ToString());
                resultado.Desc_tarifa = t.Rows[0]["desc_tarifa"].ToString();
                resultado.Fvigen_desde = DateTime.Parse(t.Rows[0]["fvigen_desde"].ToString());
                resultado.Importe_tarifa = double.Parse(t.Rows[0]["importe_tarifa"].ToString());
                resultado.Es_borrado = (t.Rows[0]["Es_borrado"].ToString() == "1");
                resultado.Id_categoria = int.Parse(t.Rows[0]["id_categoria"].ToString());
                resultado.Hs_desde = DateTime.Parse(t.Rows[0]["hs_desde"].ToString());
                resultado.Hs_hasta = DateTime.Parse(t.Rows[0]["hs_hasta"].ToString());
                resultado.Forma_uso = Tarifa_forma_uso.getFormaUso(DB.DInt(t.Rows[0]["formausotarifa"].ToString()));
                resultado.Id_moneda = DB.DInt(t.Rows[0]["id_moneda"].ToString());
                resultado.Cambio = DB.DFloat(t.Rows[0]["cambio"].ToString());

                if (t.Rows[0]["fvigen_hasta"].ToString() != "")
                {
                    resultado.Fvigen_hasta = DateTime.Parse(t.Rows[0]["fvigen_hasta"].ToString());
                }

                // Medios
                resultado.Medios = new List<Medio>();
                det = DB.Select("select tm.id_tarifa, m.* from medios_tarifa tm inner join medios m on m.id_medio = tm.id_medio where id_tarifa = " + t.Rows[0]["id_tarifa"].ToString());
                foreach (DataRow item in det.Rows)
                {
                    Medio elem = Medio.getMedio(item);
                    resultado.Medios.Add(elem);
                }
            }

            return resultado;
        }


        public static List<Tarifas> getAll()
        {
            string sqlCommand = " select top 100  select id_tarifa, desc_tarifa, fvigen_desde, fvigen_hasta, importe_tarifa, es_borrado, " +
                                " id_categoria, hs_desde, hs_hasta, formausotarifa, id_moneda, cambio" +
                                " from tarifas where es_borrado = 0 " +
                                " order by id_tarifa desc";

            List<Tarifas> col = new List<Tarifas>();
            Tarifas elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Tarifas
                {
                    Id_tarifa = int.Parse(item["id_tarifa"].ToString()),
                    Desc_tarifa = item["desc_tarifa"].ToString(),
                    Fvigen_desde = DateTime.Parse(item["fvigen_desde"].ToString()),
                    Importe_tarifa = double.Parse(item["importe_tarifa"].ToString()),
                    Es_borrado = (item["Es_borrado"].ToString() == "1"),
                    Id_categoria = int.Parse(t.Rows[0]["id_categoria"].ToString()),
                    Hs_desde = DateTime.Parse(item["hs_desde"].ToString()),
                    Hs_hasta = DateTime.Parse(item["hs_hasta"].ToString()),
                    Forma_uso = Tarifa_forma_uso.getFormaUso(DB.DInt(item["formausotarifa"].ToString())),
                    Id_moneda = DB.DInt(t.Rows[0]["id_moneda"].ToString()),
                    Cambio = DB.DFloat(t.Rows[0]["cambio"].ToString())
                };
                if (item["fvigen_hasta"].ToString() != "")
                {
                    elem.Fvigen_hasta = DateTime.Parse(item["fvigen_hasta"].ToString());
                }

                col.Add(elem);
            }
            return col;
        }


        public static List<Tarifas> filter(List<Parametro> parametros)
        {
            string sqlCommand = " select top 100 id_tarifa, desc_tarifa, fvigen_desde, fvigen_hasta, importe_tarifa, es_borrado, " +
                                " id_categoria, hs_desde, hs_hasta, formausotarifa, id_moneda, cambio" +
                                " from tarifas where es_borrado = 0 ";

            string mifiltro = "";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    //if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                    //    mifiltro = mifiltro + " and fvigen_desde <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and (fvigen_hasta >= '" + p.Value.ToString() + "' or fvigen_hasta is null)";
                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and fvigen_desde <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "id_medio") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from medios_tarifa where medios_tarifa.id_tarifa = tarifas.id_tarifa and id_medio = " + p.Value.ToString() + ")";                              
                    if ((p.ParameterName == "id_moneda") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and tarifas.id_moneda = " + p.Value.ToString();
                }
            }

            string orderBy = " order by id_tarifa desc";

            List<Tarifas> col = new List<Tarifas>();
            Tarifas elem;
            DataTable t = DB.Select(sqlCommand + mifiltro + orderBy);

            foreach (DataRow item in t.Rows)
            {
                elem = new Tarifas
                {
                    Id_tarifa = int.Parse(item["id_tarifa"].ToString()),
                    Desc_tarifa = item["desc_tarifa"].ToString(),
                    Fvigen_desde = DateTime.Parse(item["fvigen_desde"].ToString()),
                    Importe_tarifa = double.Parse(item["importe_tarifa"].ToString()),
                    Es_borrado = (item["Es_borrado"].ToString() == "1"),
                    Id_categoria = int.Parse(t.Rows[0]["id_categoria"].ToString()),
                    Hs_desde = DateTime.Parse(item["hs_desde"].ToString()),
                    Hs_hasta = DateTime.Parse(item["hs_hasta"].ToString()),
                    Forma_uso = Tarifa_forma_uso.getFormaUso(DB.DInt(item["formausotarifa"].ToString())),
                    Id_moneda = DB.DInt(t.Rows[0]["id_moneda"].ToString()),
                    Cambio = DB.DFloat(t.Rows[0]["cambio"].ToString())
                };
                if (item["fvigen_hasta"].ToString() != "")
                {
                    elem.Fvigen_hasta = DateTime.Parse(item["fvigen_hasta"].ToString());
                }

                col.Add(elem);
            }
            return col;
        }

    }
}
