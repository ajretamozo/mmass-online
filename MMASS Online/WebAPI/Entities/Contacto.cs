using System;
using System.Collections.Generic;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.Identity.Client;

namespace WebApi.Entities
{
    public class Contacto
    {
        public int Id { get; set; }
        public int Id_contacto { get; set; }
        public string RazonSocial { get; set; }
        public string IdContactoDigital { get; set; }
        public string Nombre_com { get; set; }
        public bool No_facturable { get; set; }
        public string Token { get; set; }
        public int IdRed { get; set; }
        public List<string> IdContactosDigitales;
        //Usados para guardar los clientes potenciales
        public int Tipo_contacto { get; set; }
        public int Id_usuario_alta { get; set; }
        public int Id_contacto_vinculado { get; set; }
        public string Email { get; set; }

        public static Contacto getContactoById(int Id)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 3)
            {
                sqlCommand = @"Select c.id_contacto, c.no_facturable, razon_social, nombre_com from contactos c
                                  where c.id_contacto = " + Id.ToString();
            }
            else
            {

                sqlCommand = @"Select c.id_contacto, c.no_facturable, razon_social, nombre_com, g.id_contactodigital from contactos c
                                  left join dg_contacto_red_GAM g on c.id_contacto = g.id_contacto
                                  where c.id_contacto = " + Id.ToString();
            }

            Contacto contacto = new Contacto();
            contacto.IdContactosDigitales = new List<string>();
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                //contacto = new Contacto
                {
                    contacto.Id = int.Parse(item["id_contacto"].ToString());
                    contacto.Id_contacto = int.Parse(item["id_contacto"].ToString());
                    if (BD != 3)
                    {
                        contacto.IdContactosDigitales.Add(item["id_contactodigital"].ToString());
                    }
                    contacto.RazonSocial = item["razon_social"].ToString();
                    contacto.Nombre_com = item["nombre_com"].ToString();
                    contacto.No_facturable = !(item["no_facturable"].ToString() == "1");
                };
            }
            return contacto;
        }

        public static Contacto getContactoByIdyRed(int Id, int idRed)
        {
            string sqlCommand = @"Select c.id_contacto, c.no_facturable, razon_social, nombre_com, g.id_contactodigital from contactos c
                                  left join dg_contacto_red_GAM g on c.id_contacto = g.id_contacto
                                  where c.id_contacto = " + Id.ToString() + " and g.id_red = " + idRed.ToString();

            Contacto contacto = new Contacto();
            DataTable t = DB.Select(sqlCommand);

            if(t.Rows.Count > 0)
            {
                foreach (DataRow item in t.Rows)
                {
                    contacto = new Contacto
                    {
                        Id = int.Parse(item["id_contacto"].ToString()),
                        Id_contacto = int.Parse(item["id_contacto"].ToString()),
                        IdContactoDigital = item["id_contactodigital"].ToString(),
                        RazonSocial = item["razon_social"].ToString(),
                        Nombre_com = item["nombre_com"].ToString(),
                        No_facturable = !(item["no_facturable"].ToString() == "1")
                    };
                }
            }
            else
            {
                contacto = new Contacto
                {
                    IdContactoDigital = "0"
                };
            }
           
            return contacto;
        }

        public static Contacto getContactoByIdGAMyRed(string idGam, int idRed)
        {
            string sqlCommand = @"Select c.id_contacto, c.no_facturable, razon_social, nombre_com, g.id_contactodigital from contactos c
                                  left join dg_contacto_red_GAM g on c.id_contacto = g.id_contacto
                                  where g.id_contactodigital = '" + idGam+ "' and g.id_red = " + idRed.ToString();

            Contacto contacto = new Contacto();
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contacto = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString(),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    No_facturable = !(item["no_facturable"].ToString() == "1")
                };
            }
            return contacto;
        }

        public static List<Contacto> GetAgenciasPorAnunciante(int IdAnunciante)
        {
            string sqlCommand = "select v.id_contacto_padre as id_contacto, c.razon_social, c.nombre_com from contactos c " +
                                " inner join vinculos v on v.id_contacto_padre = c.id_contacto " +
                                " inner join roles r on r.id_contacto = v.id_contacto_padre " +
                                " where v.id_contacto = " + IdAnunciante.ToString();
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString())
                    //IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static List<Contacto> GetContactosPorAgencia(int idAgencia)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = @"Select c.id_contacto, c.razon_social, c.nombre_com from contactos c , roles r, vinculos v where r.id_contacto = c.id_contacto  and es_borrado = 0 and es_bloqueado = 0  
                                      and r.tipo_rol = 1 and c.id_contacto = v.id_contacto and v.tipo_rol_padre= 0 
                                      and v.id_contacto_padre=" + idAgencia.ToString() + " order by razon_social ";
            }
            else
            {
                sqlCommand = @"Select c.id_contacto, c.razon_social, c.nombre_com from contactos c , roles r, vinculos v where r.id_contacto = c.id_contacto  and es_borrado = 0 and es_bloqueado = 0  
                                      and r.id_tipo_rol = 1 and c.id_contacto = v.id_contacto and v.id_tipo_rol_padre= 0 
                                      and v.id_contacto_padre=" + idAgencia.ToString() + " order by razon_social ";
            }

            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString())
                    //IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static List<Contacto> GetAnunciantesPorProducto(int idProducto)
        {
            string sqlCommand = "select a.id_contacto, a.razon_social, nombre_com from productos p inner join anun_prod pa on p.id_producto = pa.id_producto inner join contactos a on pa.id_anunciante = a.id_contacto where p.es_borrado = 0 and p.id_producto = " + idProducto.ToString() + " order by a.razon_social ";
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString())
                    //IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static List<Contacto> GetContactosPorTipo(string tipo)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = @"Select c.id_contacto, razon_social, nombre_com from contactos c
                                    inner join roles r on r.id_contacto = c.id_contacto
                                    where r.id_contacto = c.id_contacto
                                    and es_borrado = 0 and (es_bloqueado = 0 or es_bloqueado is null) and r.tipo_rol = " + tipo + " order by razon_social";
            }
            else
            {
                sqlCommand = @"Select c.id_contacto, razon_social, nombre_com from contactos c
                                    inner join roles r on r.id_contacto = c.id_contacto
                                    where r.id_contacto = c.id_contacto
                                    and es_borrado = 0 and (es_bloqueado = 0 or es_bloqueado is null) and r.id_tipo_rol = " + tipo + " order by razon_social";
            }

            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString())
                    //IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }


        public static List<Contacto> getAll()
        {
            string sqlCommand = "select * from contactos ";
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString())
                    //IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static bool saveASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red)
        {
            string sql = "";
            if ((Id_contacto != 0) && (Id_ContactoDigital != "") && (Id_Red != 0))
            {
                sql = "insert into dg_contacto_red_GAM (id_contacto, id_contactodigital, id_red) " +
                      "values ( @id_contacto, @id_contactodigital, @id_red)";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_contacto",SqlDbType = SqlDbType.Int, Value = Id_contacto },
                    new SqlParameter()
                    { ParameterName="@id_contactodigital",SqlDbType = SqlDbType.NVarChar, Value = Id_ContactoDigital },
                    new SqlParameter()
                    { ParameterName="@id_red", SqlDbType = SqlDbType.Int, Value = Id_Red }
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
            else return false;
        }

        public static bool deleteASRelation(int Id_contacto, string Id_ContactoDigital, int Id_Red)
        {
            string sql = "";
            if ((Id_contacto != 0) && (Id_ContactoDigital != "") && (Id_Red != 0))
            {
                sql = "DELETE FROM dg_contacto_red_GAM " +
                      "WHERE id_contacto=@id_contacto AND id_contactodigital=@id_contactodigital AND id_red=@id_red";

                List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@id_contacto",SqlDbType = SqlDbType.Int, Value = Id_contacto },
                    new SqlParameter()
                    { ParameterName="@id_contactodigital",SqlDbType = SqlDbType.NVarChar, Value = Id_ContactoDigital },
                    new SqlParameter()
                    { ParameterName="@id_red", SqlDbType = SqlDbType.Int, Value = Id_Red }
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
            else return false;
        }

        public static List<Contacto> GetAnunSincro(int idRed)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = @"Select c.id_contacto, razon_social, nombre_com, g.id_contactodigital, g.id_red
                                from contactos c
                                inner join roles r on r.id_contacto = c.id_contacto
                                left join  dg_contacto_red_GAM g on g.id_contacto = c.id_contacto 
                                and g.id_red = " + idRed.ToString() + "where r.id_contacto = c.id_contacto and es_borrado = 0 and es_bloqueado = 0 and r.tipo_rol = 1 order by razon_social";
            }
            else
            {
                sqlCommand = @"Select c.id_contacto, razon_social, nombre_com, g.id_contactodigital, g.id_red
                                from contactos c
                                inner join roles r on r.id_contacto = c.id_contacto
                                left join  dg_contacto_red_GAM g on g.id_contacto = c.id_contacto 
                                and g.id_red = " + idRed.ToString() + "where r.id_contacto = c.id_contacto and es_borrado = 0 and es_bloqueado = 0 and r.id_tipo_rol = 1 order by razon_social";
            }

            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Nombre_com = item["nombre_com"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public List<string> GetEmailsPorContacto()
        {
            string sqlCommand = "select email from emails where id_contacto = " + Id_contacto.ToString();
            
            DataTable t = DB.Select(sqlCommand);

            List<string> emails = new List<string>();
            foreach (DataRow item in t.Rows)
            {
                emails.Add(item["email"].ToString());
            }
            return emails;
        }

        public Contacto saveCliPotencial()
        {
            DataTable t = DB.Select("select IsNull(max(id_contacto),0) as ultimo from contactos");
            if (t.Rows.Count == 1)
            {
                Id_contacto = int.Parse(t.Rows[0]["ultimo"].ToString()) + 1;
            }

            string sqlCommand = "INSERT [dbo].[contactos] ([id_contacto], [contacto], [ing_contacto], [razon_social], [nombre_com], [tip_doc], [nro_doc], [comfer], [id_cond_iva], [cuit], [ib], [observa], [fecha_baja], [es_borrado], [es_solorutina], [id_contactoenlace], [pesodeorden], [es_exportable], [cobra_comision_vend], [porc_comision_vend], [cobra_comision_cob], [porc_comision_cob], [dias_credito], [monto_credito], [id_rubro], [es_bloqueado], [id_plazo_pago], [es_grupo], [no_facturable], [id_origen], [id_tipo_precio], [id_tipo], [id_usuario_vinc], [provisorio], [id_tipotarifa], [id_tipoorganismo], [convenioanualprecio], [no_vinculable], [solorutina], [porc_grav_coproductor], [dias_actual], [credito_actual], [xctacte], [id_usuario_alta], [noprintfc], [id_contactoenlace2], [id_cond_IB], [id_tipo_doc], [fecha_modificacion], [transferido], [account_id], [Porc_agencia], [recibeFC], [id_formapago], [id_uc], [req_exp_fact], [cod_sociedad], [id_reg_fiscal]) " +
                                " VALUES (@id_contacto, @contacto, GETDATE(), @razon_social, @nombre_com, NULL, N'', N'', NULL, N'', N'', N'', NULL, 0, 0, N'', 0, 0, 0, 0, 0, 0, 0, 0, NULL, 0, NULL, 0, 0, NULL, NULL, NULL, NULL, 1, 0, NULL, 0, 0, 0, 0, NULL, NULL, NULL, @id_usuario_alta, 0, NULL, NULL, NULL, GETDATE(), NULL, NULL, 0, 0, NULL, NULL, 0, NULL, NULL)";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@id_contacto",SqlDbType = SqlDbType.Int, Value = Id_contacto },
                new SqlParameter()
                { ParameterName="@contacto",SqlDbType = SqlDbType.Int, Value = Id_contacto },
                new SqlParameter()
                { ParameterName="@razon_social",SqlDbType = SqlDbType.NVarChar, Value = RazonSocial },
                new SqlParameter()
                { ParameterName="@nombre_com",SqlDbType = SqlDbType.NVarChar, Value = Nombre_com },
                new SqlParameter()
                { ParameterName="@id_usuario_alta",SqlDbType = SqlDbType.Int, Value = Id_usuario_alta }
            };

            try
            {
                DB.Execute(sqlCommand, parametros);

                DB.Execute("insert roles (id_contacto, id_tipo_rol) values (" + Id_contacto + ", " + Tipo_contacto + ")");

                if (Email != "")
                {
                    int id_email = 0;
                    DataTable dt = DB.Select("select IsNull(max(id_email),0) as ultimo from emails");
                    if (dt.Rows.Count == 1)
                    {
                        id_email = int.Parse(dt.Rows[0]["ultimo"].ToString()) + 1;
                    }

                    List<SqlParameter> parametrosMail = new List<SqlParameter>()
                    {
                        new SqlParameter()
                        { ParameterName="@email",SqlDbType = SqlDbType.NVarChar, Value = Email }
                    };

                    DB.Execute("insert emails (id_contacto, id_email, desc_email, email, es_general, es_certifica, es_factura) " +
                               "values (" + Id_contacto + ", " + id_email + ", '', @email, 1, 0, 0)", parametrosMail);
                }

                if (Id_contacto_vinculado > 0)
                {
                    int id_vinculo = 0;
                    DataTable dt = DB.Select("select IsNull(max(id_vinculo),0) as ultimo from vinculos");
                    if (dt.Rows.Count == 1)
                    {
                        id_vinculo = int.Parse(dt.Rows[0]["ultimo"].ToString()) + 1;
                    }

                    if (Tipo_contacto == 0)
                    {
                        DB.Execute("insert vinculos (id_contacto, id_tipo_rol, id_contacto_padre, id_tipo_rol_padre, xctacte, id_vinculo) values (" + Id_contacto_vinculado + ", 1, " + Id_contacto + ", 0, 0, " + id_vinculo + ")");
                    }
                    else
                    {
                        DB.Execute("insert vinculos (id_contacto, id_tipo_rol, id_contacto_padre, id_tipo_rol_padre, xctacte, id_vinculo) values (" + Id_contacto + ", 1, " + Id_contacto_vinculado + ", 0, 0, " + id_vinculo + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return this;
        }

    }
}