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
    //AGREGUE (CPD A SPONSOR):
    public class Dg_tarifa_forma_uso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public static Dg_tarifa_forma_uso getFormaUso(int indice)
        {
            if ((indice <0) || (indice > 4))
            {
                return null;
            }
            else
            {
                List<Dg_tarifa_forma_uso> col = new List<Dg_tarifa_forma_uso>();
                Dg_tarifa_forma_uso item = new Dg_tarifa_forma_uso();
                item.Id = 0;
                item.Descripcion = "CPM";
                col.Add(item);
                item = new Dg_tarifa_forma_uso();
                item.Id = 1;
                item.Descripcion = "CPD (Sponsor)";
                col.Add(item);
                item = new Dg_tarifa_forma_uso();
                item.Id = 2;
                item.Descripcion = "Post";
                col.Add(item);
                item = new Dg_tarifa_forma_uso();
                item.Id = 3;
                item.Descripcion = "CPC";
                col.Add(item);
                item = new Dg_tarifa_forma_uso();
                item.Id = 4;
                item.Descripcion = "CPA";
                col.Add(item);

                return col[indice];
            }
        }
        public static List<Dg_tarifa_forma_uso> getAll()
        {
            List<Dg_tarifa_forma_uso> col = new List<Dg_tarifa_forma_uso>();
            Dg_tarifa_forma_uso item = new Dg_tarifa_forma_uso();
            item.Id = 0;
            item.Descripcion = "CPM";
            col.Add(item);
            item = new Dg_tarifa_forma_uso();
            item.Id = 1;
            item.Descripcion = "CPD (Sponsor)";
            col.Add(item);
            item = new Dg_tarifa_forma_uso();
            item.Id = 2;
            item.Descripcion = "Posteo";
            col.Add(item);
            item = new Dg_tarifa_forma_uso();
            item.Id = 3;
            item.Descripcion = "CPC";
            col.Add(item);
            item = new Dg_tarifa_forma_uso();
            item.Id = 4;
            item.Descripcion = "CPA";
            col.Add(item);
            return col;
        }
    }

    public class Dg_tarifas
    {
        public int Id_tarifa_dg { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_desde { get; set; }
        public DateTime Fecha_hasta { get; set; }
        public Dg_tarifa_forma_uso Forma_uso { get; set; }
        public float Precio_unitario { get; set; }
        public bool Es_borrado { get; set; }
        public int Id_red { get; set; }

        public List<Medio> Medios;
        public List<Dg_tipos_avisos> Tipos_Avisos;
        //AGREGUE (emplaza, med, area):
        public List<Dg_emplazamientos> Emplazamientos;
        public List<Dg_medidas> Medidas;
        public Dg_areas_geo Area_geo { get; set; }

        public static Dg_tarifas getById(int Id)
        {
            string sqlCommand = " select id_tarifa_dg, descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario, es_borrado, id_red " +
                                " from dg_tarifas where id_tarifa_dg = " + Id.ToString();
            Dg_tarifas resultado;
            resultado = new Dg_tarifas();
            DataTable t = DB.Select(sqlCommand);
            DataTable det = new DataTable();
            if (t.Rows.Count == 1)
            {
                resultado.Id_tarifa_dg = int.Parse(t.Rows[0]["id_tarifa_dg"].ToString());
                resultado.Descripcion = t.Rows[0]["descripcion"].ToString();
                resultado.Fecha_desde = DateTime.Parse(t.Rows[0]["fecha_desde"].ToString());
                resultado.Fecha_hasta = DateTime.Parse(t.Rows[0]["fecha_hasta"].ToString());
                resultado.Forma_uso =  Dg_tarifa_forma_uso.getFormaUso(DB.DInt(t.Rows[0]["forma_uso"].ToString()));
                resultado.Precio_unitario = float.Parse(t.Rows[0]["precio_unitario"].ToString());
                resultado.Es_borrado = (t.Rows[0]["Es_borrado"].ToString()=="1");
                resultado.Id_red = int.Parse(t.Rows[0]["id_red"].ToString());
                // Medios
                resultado.Medios = new List<Medio>();                
                det = DB.Select("select tm.id_tarifa_dg, m.* from dg_tarifas_medios tm inner join medios m on m.id_medio = tm.id_medio where id_tarifa_dg = " + t.Rows[0]["id_tarifa_dg"].ToString());                
                foreach (DataRow item in det.Rows)
                {
                    Medio elem = Medio.getMedio(item);
                    resultado.Medios.Add(elem);
                }
                // Tipos de Aviso
                resultado.Tipos_Avisos = new List<Dg_tipos_avisos>();
                det = DB.Select("select dta.id_tarifa_dg, dta.id_tipo_aviso_dg, ta.* from dg_tarifas_tipos_avisos_dg dta inner join dg_tipos_avisos ta on dta.id_tipo_aviso_dg = ta.id_tipo_aviso_dg where dta.id_tarifa_dg =" + t.Rows[0]["id_tarifa_dg"].ToString());
                foreach (DataRow item in det.Rows)
                {
                    Dg_tipos_avisos elem = Dg_tipos_avisos.getDg_tipos_avisos(item);
                    resultado.Tipos_Avisos.Add(elem);
                }
                //AGREGUE:
                // Emplazamientos
                resultado.Emplazamientos = new List<Dg_emplazamientos>();
                det = DB.Select(@"select te.id_tarifa_dg, e.* 
                                  from dg_tarifas_emplazamientos te 
                                  inner join dg_emplazamientos e on e.id_emplazamiento = te.id_emplazamiento
                                  where te.id_tarifa_dg = " + t.Rows[0]["id_tarifa_dg"].ToString());
                foreach (DataRow item in det.Rows)
                {
                    Dg_emplazamientos elem = Dg_emplazamientos.getDg_emplazamientos(item);
                    resultado.Emplazamientos.Add(elem);
                }
                //AGREGUE:
                // Medidas
                resultado.Medidas = new List<Dg_medidas>();
                det = DB.Select(@"select tme.id_tarifa_dg, m.* 
                                  from dg_tarifas_medidas tme 
                                  inner join dg_medidas m on m.id_medidadigital = tme.id_medidadigital 
                                  where tme.id_tarifa_dg = " + t.Rows[0]["id_tarifa_dg"].ToString());
                foreach (DataRow item in det.Rows)
                {
                    Dg_medidas elem = Dg_medidas.getDg_medidas(item);
                    resultado.Medidas.Add(elem);
                }
                //AGREGUE:
                // Areas
                resultado.Area_geo = new Dg_areas_geo();
                det = DB.Select(@"select ta.id_tarifa_dg, ag.* 
                                  from dg_tarifas_areas ta 
                                  inner join dg_areas_geo ag on ag.id_area = ta.id_area
                                  where ta.id_tarifa_dg = " + t.Rows[0]["id_tarifa_dg"].ToString());
                foreach (DataRow item in det.Rows)
                {
                    resultado.Area_geo = Dg_areas_geo.getDg_areas_geo(item);
                }
            }
            return resultado;
        }

        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_tarifa_dg == 0)
            {
                sql = "INSERT INTO dg_tarifas (descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario, es_borrado, id_red) " +
                    " values (@descripcion, @fecha_desde, @fecha_hasta, @forma_uso, @precio_unitario, 0, @id_red) " +
                    " SELECT SCOPE_IDENTITY(); ";
            }
            else
            {
                sql = " update dg_tarifas set descripcion = @descripcion, fecha_desde=@fecha_desde, fecha_hasta=@fecha_hasta, forma_uso=@forma_uso, " +
                      "precio_unitario=@precio_unitario, id_red=@id_red" +
                      " where Id_tarifa_dg = @Id_tarifa_dg";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@Id_tarifa_dg",SqlDbType = SqlDbType.Int, Value = Id_tarifa_dg },
                    new SqlParameter()
                    { ParameterName="@descripcion", SqlDbType = SqlDbType.NVarChar, Value = Descripcion },
                    new SqlParameter()
                    { ParameterName="@fecha_desde", SqlDbType = SqlDbType.DateTime, Value = Fecha_desde },
                    new SqlParameter()
                    { ParameterName="@fecha_hasta", SqlDbType = SqlDbType.DateTime, Value = Fecha_hasta },
                    new SqlParameter()
                    { ParameterName="@forma_uso", SqlDbType = SqlDbType.Int, Value = Forma_uso.Id },
                    new SqlParameter()
                    { ParameterName="@precio_unitario", SqlDbType = SqlDbType.Float, Value = Precio_unitario },
                     new SqlParameter()
                    { ParameterName="@id_red",SqlDbType = SqlDbType.Int, Value = Id_red }
            };
            try
            {
                using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                {
                    // Grabo la Tarifa (Cabecera)
                    DB.Execute(sql, parametros);
                    //Obtengo el ID
                    if (Id_tarifa_dg == 0)
                    {
                            DataTable t = DB.Select("select max(id_tarifa_dg) as ULTIMO from dg_tarifas ");
                            if (t.Rows.Count == 1)
                            {
                                Id_tarifa_dg = int.Parse(t.Rows[0]["ULTIMO"].ToString());
                            }
                    }
                    // Grabo los Medios relacionados...
                    DB.Execute("delete from dg_tarifas_medios where Id_tarifa_dg = " + Id_tarifa_dg.ToString());
                    foreach (Medio elem in Medios)
                    {
                        sql = "insert into dg_tarifas_medios (Id_tarifa_dg, Id_medio) values  (" + Id_tarifa_dg.ToString() + ", " + elem.Id_medio.ToString() + ")";
                        DB.Execute(sql);
                    }
                    // Grabo los tipos de avisos relacionados...
                    DB.Execute("delete from dg_tarifas_tipos_avisos_dg where id_tarifa_dg = " + Id_tarifa_dg.ToString());
                    foreach (Dg_tipos_avisos elem in Tipos_Avisos)
                    {
                        sql = "insert into dg_tarifas_tipos_avisos_dg (Id_tarifa_dg,id_tipo_aviso_dg) values (" + Id_tarifa_dg.ToString() + "," + elem.Id_tipo_aviso_dg + ")";
                        DB.Execute(sql);
                    }
                    //AGREGUE:
                    // Grabo los Emplazamientos relacionados...
                    DB.Execute("delete from dg_tarifas_emplazamientos where id_tarifa_dg = " + Id_tarifa_dg.ToString());
                    foreach (Dg_emplazamientos elem in Emplazamientos)
                    {
                        sql = "insert into dg_tarifas_emplazamientos (id_tarifa_dg, id_emplazamiento) values  (" + Id_tarifa_dg.ToString() + ", " + elem.Id_emplazamiento.ToString() + ")";
                        DB.Execute(sql);
                    }
                    //AGREGUE:
                    // Grabo las Medidas relacionadas...
                    DB.Execute("delete from dg_tarifas_medidas where id_tarifa_dg = " + Id_tarifa_dg.ToString());
                    foreach (Dg_medidas elem in Medidas)
                    {
                        sql = "insert into dg_tarifas_medidas (id_tarifa_dg, id_medidadigital) values  (" + Id_tarifa_dg.ToString() + ", " + elem.Id_medidadigital.ToString() + ")";
                        DB.Execute(sql);
                    }
                    //AGREGUE:
                    // Grabo el AreaGeo relacionada...
                    DB.Execute("delete from dg_tarifas_areas where id_tarifa_dg = " + Id_tarifa_dg.ToString());
                    sql = "insert into dg_tarifas_areas (id_tarifa_dg, id_area) values  (" + Id_tarifa_dg.ToString() + ", " + Area_geo.Id_area.ToString() + ")";
                    DB.Execute(sql);

                    transaccion.Complete();
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static List<Dg_tarifas> getAll()
        {
            string sqlCommand = " select id_tarifa_dg, descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario, es_borrado, id_red " +
                                " from dg_tarifas where es_borrado = 0 " ;

            List<Dg_tarifas> col = new List<Dg_tarifas>();
            Dg_tarifas elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                elem = new Dg_tarifas
                {
                    Id_tarifa_dg = int.Parse(item["id_tarifa_dg"].ToString()),
                    Descripcion = item["descripcion"].ToString(),
                    Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
                    Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
                    Forma_uso = Dg_tarifa_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString())),
                    Precio_unitario = float.Parse(item["precio_unitario"].ToString()),
                    Es_borrado = (item["Es_borrado"].ToString()=="1"),
                    Id_red = int.Parse(t.Rows[0]["id_red"].ToString())
            };
                col.Add(elem);
            }
            return col;
        }

        public bool remove()
        {
            string sql = "";

            if (Id_tarifa_dg != 0)
            {
                sql = "update dg_tarifas set es_borrado = 1 where Id_tarifa_dg = " + Id_tarifa_dg.ToString();
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

        //AGREGUE (modifique las fechas por config sql y agregué emplazamientos, medidas y areasgeo):
        public static List<Dg_tarifas> filter(List<Parametro> parametros)
        {
            string sqlCommand = " select id_tarifa_dg, descripcion, fecha_desde, fecha_hasta, forma_uso, precio_unitario, es_borrado, id_red " +
                                " from dg_tarifas where es_borrado = 0 ";
            string mifiltro = "";

            string idMedidas = "";
            int contMedidas = 0;
            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "descripcion") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and descripcion like '%" + p.Value + "%'";

                    //if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and fecha_desde >='" + formatted + "'";
                    //}
                    //if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                    //{
                    //    DateTime fecha = DateTime.Parse(p.Value);
                    //    string formatted = fecha.ToString("dd-MM-yyyy");
                    //    mifiltro = mifiltro + " and fecha_hasta <='" + formatted + "'";
                    //}
                    if ((p.ParameterName == "fecha_desde") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and fecha_desde >= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "fecha_hasta") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and fecha_hasta <= '" + p.Value.ToString() + "'";
                    if ((p.ParameterName == "forma_uso") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and forma_uso = " + p.Value.ToString();
                    if ((p.ParameterName == "id_medio") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_tarifas_medios where dg_tarifas_medios.id_tarifa_dg = dg_tarifas.id_tarifa_dg and id_medio = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_tipo_aviso_dg") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_tarifas_tipos_avisos_dg where dg_tarifas_tipos_avisos_dg.id_tarifa_dg = dg_tarifas.id_tarifa_dg and id_tipo_aviso_dg = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_emplazamiento") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_tarifas_emplazamientos where dg_tarifas_emplazamientos.id_tarifa_dg = dg_tarifas.id_tarifa_dg and id_emplazamiento = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "id_medidadigital") && (p.Value.ToString() != ""))
                    {
                        if (contMedidas == 0)
                        {
                            idMedidas = p.Value;
                        }
                        else
                        {
                            idMedidas += "," + p.Value;
                        }
                        contMedidas++;
                    }
                    if ((p.ParameterName == "id_areaGeo") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and exists (select * from dg_tarifas_areas where dg_tarifas_areas.id_tarifa_dg = dg_tarifas.id_tarifa_dg and id_area = " + p.Value.ToString() + ")";
                    if ((p.ParameterName == "vigente") && (p.Value.ToString() == "1"))
                        mifiltro = mifiltro + " and (getdate() between fecha_desde and fecha_hasta) ";
                    if ((p.ParameterName == "id_red") && (p.Value.ToString() != ""))
                        mifiltro = mifiltro + " and id_red = " + p.Value.ToString() + ")";
                }
            }
            if (idMedidas != "")
            {
                mifiltro = mifiltro + " and exists (select * from dg_tarifas_medidas where dg_tarifas_medidas.id_tarifa_dg = dg_tarifas.id_tarifa_dg and id_medidadigital IN (" + idMedidas + "))";
            }

            List<Dg_tarifas> col = new List<Dg_tarifas>();
            Dg_tarifas elem;
            DataTable t = DB.Select(sqlCommand + mifiltro);

            foreach (DataRow item in t.Rows)
            {
                elem = new Dg_tarifas
                {
                    Id_tarifa_dg = int.Parse(item["id_tarifa_dg"].ToString()),
                    Descripcion = item["descripcion"].ToString(),
                    Fecha_desde = DateTime.Parse(item["fecha_desde"].ToString()),
                    Fecha_hasta = DateTime.Parse(item["fecha_hasta"].ToString()),
                    Forma_uso = Dg_tarifa_forma_uso.getFormaUso(DB.DInt(item["forma_uso"].ToString())),
                    Precio_unitario = float.Parse(item["precio_unitario"].ToString()),
                    Es_borrado = (item["Es_borrado"].ToString() == "1"),
                    Id_red = int.Parse(item["id_red"].ToString())
            };
                col.Add(elem);
            }
            return col;
        }
    }

}
