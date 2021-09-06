using System;
using System.Collections.Generic;
using System.Data;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Contacto
    {
        public int  Id { get; set; }
        public int Id_contacto { get; set; }
        public string RazonSocial { get; set; }
        public string IdContactoDigital { get; set; }
        public bool No_facturable { get; set; }
        public string Token { get; set; }
        
        public static Contacto getContactoById(int Id)
        {
            string sqlCommand = "Select c.id_contacto, c.no_facturable ,razon_social,id_contactodigital  from contactos c where c.id_contacto = " + Id.ToString();
            
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
                    No_facturable = !(item["no_facturable"].ToString() == "1")


                };                
            }
            return contacto;
        }


        public static List<Contacto> GetAgenciasPorAnunciante(int IdAnunciante)
        {
            string sqlCommand = "select v.id_contacto_padre as id_contacto, c.razon_social, c.id_contactodigital  from contactos c " +
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
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        //public static List<Contacto> GetContactosPorAgencia(int idAgencia)
        //{
        //    string sqlCommand = "Select c.id_contacto, c.razon_social,id_contactodigital from contactos c , roles r, vinculos v where r.id_contacto = c.id_contacto  and es_borrado = 0 and r.id_tipo_rol = 1 and c.id_contacto = v.id_contacto and v.id_tipo_rol_padre= 0 and v.id_contacto_padre=" + idAgencia.ToString()+" order by razon_social ";
        //    List<Contacto> col = new List<Contacto>();
        //    Contacto contact;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        contact = new Contacto
        //        {
        //            Id = int.Parse(item["id_contacto"].ToString()),
        //            RazonSocial = item["razon_social"].ToString(),
        //            Id_contacto = int.Parse(item["id_contacto"].ToString()),
        //            IdContactoDigital = item["id_contactodigital"].ToString()
        //        };
        //        col.Add(contact);
        //    }
        //    return col;
        //}


        public static List<Contacto> GetContactosPorAgencia(int idAgencia)
        {
            string sqlCommand = "Select c.id_contacto, c.razon_social,id_contactodigital from contactos c , roles r, vinculos v where r.id_contacto = c.id_contacto  and es_borrado = 0 and r.tipo_rol = 1 and c.id_contacto = v.id_contacto and v.tipo_rol_padre= 0 and v.id_contacto_padre=" + idAgencia.ToString() + " order by razon_social ";
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static List<Contacto> GetAnunciantesPorProducto(int idProducto)
        {
            string sqlCommand = "select a.id_contacto, a.razon_social, a.id_contactodigital from productos p inner join anun_prod pa on p.id_producto = pa.id_producto inner join contactos a on pa.id_anunciante = a.id_contacto where p.es_borrado = 0 and p.id_producto = " + idProducto.ToString() + " order by a.razon_social ";
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        //public static List<Contacto> GetContactosPorTipo(string tipo)
        //{
        //    string sqlCommand = "Select c.id_contacto, razon_social,id_contactodigital from contactos c , roles r where r.id_contacto = c.id_contacto  and es_borrado = 0 and r.id_tipo_rol = " + tipo + " order by razon_social ";
        //    List<Contacto> col = new List<Contacto>();
        //    Contacto contact;
        //    DataTable t = DB.Select(sqlCommand);

        //    foreach (DataRow item in t.Rows)
        //    {
        //        contact = new Contacto
        //        {
        //            Id = int.Parse(item["id_contacto"].ToString()),
        //            RazonSocial = item["razon_social"].ToString(),
        //            Id_contacto = int.Parse(item["id_contacto"].ToString()),
        //            IdContactoDigital = item["id_contactodigital"].ToString()
        //        };
        //        col.Add(contact);
        //    }
        //    return col;
        //}

        public static List<Contacto> GetContactosPorTipo(string tipo)
        {
            string sqlCommand = "Select c.id_contacto, razon_social,id_contactodigital from contactos c , roles r where r.id_contacto = c.id_contacto  and es_borrado = 0 and r.tipo_rol = " + tipo + " order by razon_social ";
            List<Contacto> col = new List<Contacto>();
            Contacto contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Contacto
                {
                    Id = int.Parse(item["id_contacto"].ToString()),
                    RazonSocial = item["razon_social"].ToString(),
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
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
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    IdContactoDigital = item["id_contactodigital"].ToString()
                };
                col.Add(contact);
            }
            return col;
        }

        public static bool saveASRelation(int Id_contacto, string Id_ContactoDigital)
        {
            string sql = "";
            if ((Id_contacto != 0) && (Id_ContactoDigital != ""))
            {
                sql = "update contactos set id_contactodigital = " + Id_ContactoDigital + " where id_contacto = " + Id_contacto.ToString();
                try
                {
                    DB.Execute(sql);
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
    }
}