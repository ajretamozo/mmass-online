using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace WebApi.Entities
{
    public class Empresa
    {
        public int Id_empresa { get; set; }
        public string Nombre { get; set; }
        public string Razon_social { get; set; }
        public string Cuit { get; set; }
        public string Dgr { get; set; }
        public DateTime? Inicio { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public DateTime? Ingreso { get; set; }
        public string Cliente { get; set; }
        public string Ciemail { get; set; }
        public string Citel { get; set; }
        public string Cifax { get; set; }
        public string Ciweb { get; set; }
        public string Version { get; set; }
        public string Denom { get; set; }
        public int Minutos_hora { get; set; }
        public bool Es_borrado { get; set; }
        public string Kla { get; set; }
        public string Installdate { get; set; }
        public string Lastaccessdate { get; set; }
        public string Cod_sociedad_sap { get; set; }
        public string Direc_exten { get; set; }
        public int Id_cond_iva { get; set; }
        public int Id_localidad { get; set; }
        public int Id_cond_ib { get; set; }
        public string Logo_url { get; set; }
        public string Cod_cbl { get; set; }
        public bool Es_agente_percepcion { get; set; }
        public bool Usa_certif { get; set; }


        public static Empresa getById(int Id)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = @"select codigo,nombre,razon,cuit,dgr,inicio,domicilio,localidad,provincia,pais,telefono,fax,email,web,
                    ingreso,cliente,ciemail,citel,cifax,ciweb,version,denom,minutos_hora,es_borrado,kla,installdate,lastaccessdate,
	                cod_sociedad_sap,direc_exten,cond_iva,id_localidad,Id_cond_ib,logo,usa_certif from empresa
                    where codigo = " + Id.ToString();
            }
            else if (BD == 2)
            {
                sqlCommand = @"select id_empresa,nombre,razon_social,cuit,dgr,inicio,domicilio,localidad,provincia,pais,telefono,fax,email,web,
                    ingreso,cliente,ciemail,citel,cifax,ciweb,version,denom,minutos_hora,es_borrado,kla,installdate,lastaccessdate,
	                cod_sociedad_sap,direc_exten,id_cond_iva,id_localidad,Id_cond_ib,logo from empresa
                    where id_empresa = " + Id.ToString();
            }

            Empresa resultado;
            resultado = new Empresa();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                if (BD == 1)
                {
                    resultado.Id_empresa = DB.DInt(t.Rows[0]["codigo"].ToString());
                }
                else if (BD == 2)
                {
                    resultado.Id_empresa = DB.DInt(t.Rows[0]["id_empresa"].ToString());
                }
                resultado.Nombre = t.Rows[0]["nombre"].ToString();
                if (BD == 1)
                {
                    resultado.Razon_social = t.Rows[0]["razon"].ToString();
                }
                else if (BD == 2)
                {
                    resultado.Razon_social = t.Rows[0]["razon_social"].ToString();
                }
                resultado.Cuit = t.Rows[0]["cuit"].ToString();
                resultado.Dgr = t.Rows[0]["dgr"].ToString();
                resultado.Inicio = DB.DFecha(t.Rows[0]["inicio"].ToString());
                resultado.Domicilio = t.Rows[0]["domicilio"].ToString();
                resultado.Localidad = t.Rows[0]["localidad"].ToString();
                resultado.Provincia = t.Rows[0]["provincia"].ToString();
                resultado.Pais = t.Rows[0]["pais"].ToString();
                resultado.Telefono = t.Rows[0]["telefono"].ToString();
                resultado.Fax = t.Rows[0]["fax"].ToString();
                resultado.Email = t.Rows[0]["email"].ToString();
                resultado.Web = t.Rows[0]["web"].ToString();
                resultado.Ingreso = DB.DFecha(t.Rows[0]["ingreso"].ToString());
                resultado.Cliente = t.Rows[0]["cliente"].ToString();
                resultado.Ciemail = t.Rows[0]["ciemail"].ToString();
                resultado.Citel = t.Rows[0]["citel"].ToString();
                resultado.Cifax = t.Rows[0]["cifax"].ToString();
                resultado.Ciweb = t.Rows[0]["ciweb"].ToString();
                resultado.Version = t.Rows[0]["version"].ToString();
                resultado.Denom = t.Rows[0]["denom"].ToString();
                resultado.Minutos_hora = DB.DInt(t.Rows[0]["minutos_hora"].ToString());
                resultado.Es_borrado = (t.Rows[0]["es_borrado"].ToString() == "1");
                resultado.Kla = t.Rows[0]["kla"].ToString();
                resultado.Installdate = (t.Rows[0]["installdate"].ToString());
                resultado.Lastaccessdate = (t.Rows[0]["lastaccessdate"].ToString());
                resultado.Cod_sociedad_sap = t.Rows[0]["cod_sociedad_sap"].ToString();
                resultado.Direc_exten = t.Rows[0]["direc_exten"].ToString();
                if (BD == 1)
                {
                    resultado.Id_cond_iva = DB.DInt(t.Rows[0]["cond_iva"].ToString());
                }
                else if (BD == 2)
                {
                    resultado.Id_cond_iva = DB.DInt(t.Rows[0]["id_cond_iva"].ToString());
                }
                resultado.Id_localidad = DB.DInt(t.Rows[0]["id_localidad"].ToString());
                resultado.Id_cond_ib = DB.DInt(t.Rows[0]["Id_cond_ib"].ToString());
                resultado.Logo_url = t.Rows[0]["logo"].ToString();
                if (BD == 2)
                {
                    resultado.Usa_certif = (t.Rows[0]["usa_certif"].ToString() == "1");
                }
            }
            return resultado;
        }

        public bool save()
        {
            string sql = "";
            // Si es nuevo va insert, sino update
            if (Id_empresa == 0)
            {
                sql = "insert into empresa (id_empresa,nombre,razon_social,cuit,dgr,inicio,domicilio,localidad,provincia,pais,telefono,fax,email,web," +
                    "ingreso,cliente,ciemail,citel,cifax,ciweb,version,denom,minutos_hora,es_borrado,kla,installdate,lastaccessdate," +
                    "cod_sociedad_sap,direc_exten,id_cond_iva,id_localidad,Id_cond_ib,logo_url,cod_cbl,es_agente_percepcion,usa_certif )" +
                    " values (@id_empresa,@nombre,@razon_social,@cuit,@dgr,@inicio,@domicilio,@localidad,@provincia,@pais,@telefono,@fax,@email,@web," +
                    "@ingreso,@cliente,@ciemail,@citel,@cifax,@ciweb,@version,@denom,minutos_hora,@es_borrado,@kla,@installdate,@lastaccessdate," +
                    "@cod_sociedad_sap,@direc_exten,@id_cond_iva,@id_localidad,@Id_cond_ib,@logo_url,@cod_cbl,@es_agente_percepcion,@usa_certif )";


                DataTable t = DB.Select("select max(id_empresa) as maximo from empresa");

                if (t.Rows.Count == 1)
                {
                    Id_empresa = int.Parse(t.Rows[0]["maximo"].ToString()) + 1;
                }
            }
            else
            {
                sql = "update empresa  set nombre = @nombre,razon_social = @razon_social,cuit = @cuit,dgr=@dgr,inicio=@inicio,domicilio=@domicilio,localidad=@localidad,provincia=@provincia,pais=@pais,telefono=@telefono,fax=@fax,email=@email,web=@web," +
                    "ingreso=@ingreso,cliente=@cliente,ciemail=@ciemail,citel=@citel,cifax=@cifax,ciweb=@ciweb,version=@version,denom=@denom,minutos_hora=@minutos_hora,es_borrado=@es_borrado,kla=@kla,installdate=@installdate,lastaccessdate=@lastaccessdate," +
                    "cod_sociedad_sap=@cod_sociedad_sap,direc_exten=@direc_exten,id_cond_iva=@id_cond_iva,id_localidad=@id_localidad,Id_cond_ib=@Id_cond_ib,logo_url=@logo_url,cod_cbl=@cod_cbl,es_agente_percepcion=@es_agente_percepcion,usa_certif=@usa_certif " +
                    " where id_empresa = @id_empresa";
            }
            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_empresa",SqlDbType = SqlDbType.Int, Value = Id_empresa},
                    new SqlParameter()
                    { ParameterName="@nombre",SqlDbType = SqlDbType.NVarChar, Value = Nombre },
                    new SqlParameter()
                    { ParameterName = "@razon_social", SqlDbType = SqlDbType.NVarChar, Value = Razon_social },
                    new SqlParameter()
                    { ParameterName = "@cuit", SqlDbType = SqlDbType.NVarChar, Value =  Cuit},
                    new SqlParameter()
                    { ParameterName = "@dgr", SqlDbType = SqlDbType.NVarChar, Value = Dgr },
                    new SqlParameter()
                    { ParameterName = "@inicio", SqlDbType = SqlDbType.DateTime, Value = Inicio},
                    new SqlParameter()
                    { ParameterName = "@domicilio", SqlDbType = SqlDbType.NVarChar, Value = Domicilio },
                    new SqlParameter()
                    { ParameterName = "@localidad", SqlDbType = SqlDbType.NVarChar, Value = Localidad },
                    new SqlParameter()
                    { ParameterName = "@provincia", SqlDbType = SqlDbType.NVarChar, Value = Provincia},
                    new SqlParameter()
                    { ParameterName = "@pais", SqlDbType = SqlDbType.NVarChar, Value = Pais},
                    new SqlParameter()
                    { ParameterName = "@telefono", SqlDbType = SqlDbType.NVarChar, Value = Telefono },
                    new SqlParameter()
                    { ParameterName = "@fax", SqlDbType = SqlDbType.NVarChar, Value = Fax },
                    new SqlParameter()
                    { ParameterName = "@email", SqlDbType = SqlDbType.NVarChar, Value = Email},
                    new SqlParameter()
                    { ParameterName = "@web", SqlDbType = SqlDbType.NVarChar, Value = Web },
                    new SqlParameter()
                    { ParameterName = "@ingreso", SqlDbType = SqlDbType.DateTime, Value = Ingreso },
                    new SqlParameter()
                    { ParameterName = "@cliente", SqlDbType = SqlDbType.NVarChar, Value = Cliente },
                    new SqlParameter()
                    { ParameterName = "@ciemail", SqlDbType = SqlDbType.NVarChar, Value = Ciemail },
                    new SqlParameter()
                    { ParameterName = "@citel", SqlDbType = SqlDbType.NVarChar, Value = Citel },
                    new SqlParameter()
                    { ParameterName = "@cifax", SqlDbType = SqlDbType.NVarChar, Value = Cifax },
                    new SqlParameter()
                    { ParameterName = "@ciweb", SqlDbType = SqlDbType.NVarChar, Value = Ciweb},
                    new SqlParameter()
                    { ParameterName = "@version", SqlDbType = SqlDbType.NVarChar, Value = Version },
                    new SqlParameter()
                    { ParameterName = "@denom", SqlDbType = SqlDbType.NVarChar, Value = Denom },
                    new SqlParameter()
                    { ParameterName = "@minutos_hora", SqlDbType = SqlDbType.Int, Value = Minutos_hora },
                    new SqlParameter()
                    { ParameterName = "@es_borrado", SqlDbType = SqlDbType.Int, Value = Convert.ToInt32(Es_borrado) },
                    new SqlParameter()
                    { ParameterName = "@kla", SqlDbType = SqlDbType.NVarChar, Value = Kla },
                    new SqlParameter()
                    { ParameterName = "@installdate", SqlDbType = SqlDbType.NVarChar, Value = Installdate },
                    new SqlParameter()
                    { ParameterName = "@lastaccessdate", SqlDbType = SqlDbType.NVarChar, Value = Lastaccessdate },
                    new SqlParameter()
                    { ParameterName = "@cod_sociedad_sap", SqlDbType = SqlDbType.NVarChar, Value = Cod_sociedad_sap},
                    new SqlParameter()
                    { ParameterName = "@direc_exten", SqlDbType = SqlDbType.NVarChar, Value = Direc_exten },
                    new SqlParameter()
                    { ParameterName = "@id_cond_iva", SqlDbType = SqlDbType.Int, Value = Id_cond_iva },
                    new SqlParameter()
                    { ParameterName = "@id_localidad", SqlDbType = SqlDbType.Int, Value = Id_localidad },
                    new SqlParameter()
                    { ParameterName = "@Id_cond_ib", SqlDbType = SqlDbType.Int, Value = Id_cond_ib },
                    new SqlParameter()
                    { ParameterName = "@es_agente_percepcion", SqlDbType = SqlDbType.Int, Value = Es_agente_percepcion },
                    new SqlParameter()
                    { ParameterName = "@logo_url", SqlDbType = SqlDbType.NVarChar,Value =Logo_url },
                    new SqlParameter()
                    { ParameterName = "@cod_cbl", SqlDbType = SqlDbType.NVarChar, Value = Cod_cbl },
                    new SqlParameter()
                    { ParameterName = "@usa_certif", SqlDbType = SqlDbType.Int, Value = Convert.ToInt32(Usa_certif) }
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

        private static Empresa getEmpresa1(DataRow item)
        {
            Empresa miEmpresa = new Empresa
            {
                Id_empresa = int.Parse(item["codigo"].ToString()),
                Nombre = item["nombre"].ToString(),
                Razon_social = item["razon"].ToString(),
                Cuit = item["cuit"].ToString(),
                Dgr = item["dgr"].ToString(),
                Inicio = DB.DFecha(item["inicio"].ToString()),
                Domicilio = item["domicilio"].ToString(),
                Localidad = item["localidad"].ToString(),
                Provincia = item["provincia"].ToString(),
                Pais = item["pais"].ToString(),
                Telefono = item["telefono"].ToString(),
                Fax = item["fax"].ToString(),
                Email = item["email"].ToString(),
                Web = item["web"].ToString(),
                Ingreso = DB.DFecha(item["ingreso"].ToString()),
                Cliente = item["cliente"].ToString(),
                Ciemail = item["ciemail"].ToString(),
                Citel = item["citel"].ToString(),
                Cifax = item["cifax"].ToString(),
                Ciweb = item["ciweb"].ToString(),
                Version = item["version"].ToString(),
                Denom = item["denom"].ToString(),
                Minutos_hora = DB.DInt(item["minutos_hora"].ToString()),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Kla = item["kla"].ToString(),
                Installdate = item["installdate"].ToString(),
                Lastaccessdate = item["lastaccessdate"].ToString(),
                Cod_sociedad_sap = item["cod_sociedad_sap"].ToString(),
                Direc_exten = item["direc_exten"].ToString(),
                Id_cond_iva = DB.DInt(item["cond_iva"].ToString()),
                Id_localidad = DB.DInt(item["id_localidad"].ToString()),
                Id_cond_ib = DB.DInt(item["Id_cond_ib"].ToString()),
                Logo_url = item["logo"].ToString(),
            };
            return miEmpresa;
        }

        private static Empresa getEmpresa2(DataRow item)
        {
            Empresa miEmpresa = new Empresa
            {
                Id_empresa = int.Parse(item["id_empresa"].ToString()),
                Nombre = item["nombre"].ToString(),
                Razon_social = item["razon_social"].ToString(),
                Cuit = item["cuit"].ToString(),
                Dgr = item["dgr"].ToString(),
                Inicio = DB.DFecha(item["inicio"].ToString()),
                Domicilio = item["domicilio"].ToString(),
                Localidad = item["localidad"].ToString(),
                Provincia = item["provincia"].ToString(),
                Pais = item["pais"].ToString(),
                Telefono = item["telefono"].ToString(),
                Fax = item["fax"].ToString(),
                Email = item["email"].ToString(),
                Web = item["web"].ToString(),
                Ingreso = DB.DFecha(item["ingreso"].ToString()),
                Cliente = item["cliente"].ToString(),
                Ciemail = item["ciemail"].ToString(),
                Citel = item["citel"].ToString(),
                Cifax = item["cifax"].ToString(),
                Ciweb = item["ciweb"].ToString(),
                Version = item["version"].ToString(),
                Denom = item["denom"].ToString(),
                Minutos_hora = DB.DInt(item["minutos_hora"].ToString()),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Kla = item["kla"].ToString(),
                Installdate = item["installdate"].ToString(),
                Lastaccessdate = item["lastaccessdate"].ToString(),
                Cod_sociedad_sap = item["cod_sociedad_sap"].ToString(),
                Direc_exten = item["direc_exten"].ToString(),
                Id_cond_iva = DB.DInt(item["id_cond_iva"].ToString()),
                Id_localidad = DB.DInt(item["id_localidad"].ToString()),
                Id_cond_ib = DB.DInt(item["Id_cond_ib"].ToString()),
                Logo_url = item["logo_url"].ToString(),
                Usa_certif = (item["usa_certif"].ToString() == "1")
            };
            return miEmpresa;
        }

        public static List<Empresa> getAll()
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = @"select codigo,nombre,razon,cuit,dgr,inicio,domicilio,localidad,provincia,pais,telefono,fax,email,web,
                             ingreso,cliente,ciemail,citel,cifax,ciweb,version,denom,minutos_hora,es_borrado,kla,installdate,lastaccessdate,
	                         cod_sociedad_sap,direc_exten,cond_iva,id_localidad,Id_cond_ib,logo from empresa
							 where es_borrado=0 and (select COUNT(*) from MEDIOS m where m.ID_EMPRESA = codigo and m.tipo_medio=2) > 0";
            }
            else if (BD == 2)
            {
                sqlCommand = @" select e.id_empresa,nombre,razon_social,cuit,dgr,inicio,domicilio,localidad,provincia,pais,telefono,fax,email,web,
                             ingreso,cliente,ciemail,citel,cifax,ciweb,version,denom,minutos_hora,e.es_borrado,kla,installdate,lastaccessdate,
                             cod_sociedad_sap,direc_exten,id_cond_iva,e.id_localidad,Id_cond_ib,logo_url,usa_certif 
                             from empresa e
                             where e.es_borrado = 0 and(select COUNT(*) from MEDIOS m where m.ID_EMPRESA = e.id_empresa and m.tipo_medio = 2) > 0";
            }

            List<Empresa> col = new List<Empresa>();
            Empresa elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                if (BD == 1)
                {
                    elem = getEmpresa1(item);
                    col.Add(elem);
                }
                else if (BD == 2)
                {
                    elem = getEmpresa2(item);
                    col.Add(elem);
                }
            }
            return col;
        }
    }
}
