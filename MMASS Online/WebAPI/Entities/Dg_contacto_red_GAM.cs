using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace WebApi.Entities
{
    public class Dg_contacto_red_GAM
    {
        public int Id_contacto { get; set; }
        public string id_contactodigital { get; set; }
        public int Id_red { get; set; }


        public static List<Dg_contacto_red_GAM> getAll()
        {
            string sqlCommand = "select * from dg_contacto_red_GAM";
            List<Dg_contacto_red_GAM> col = new List<Dg_contacto_red_GAM>();
            Dg_contacto_red_GAM contact;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                contact = new Dg_contacto_red_GAM
                {
                    Id_contacto = int.Parse(item["id_contacto"].ToString()),
                    id_contactodigital = item["id_contactodigital"].ToString(),
                    Id_red = int.Parse(item["id_red"].ToString())
                };
                col.Add(contact);
            }
            return col;
        }
    }
}
