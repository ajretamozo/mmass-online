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
    }
}
