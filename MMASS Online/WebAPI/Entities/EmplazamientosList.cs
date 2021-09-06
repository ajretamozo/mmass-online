using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class EmplazamientosList
    {
        public List<Dg_emplazamientos> Emplazamientos;


        public bool save()
        {
            bool ret = true;
            DB.Execute("update dg_emplazamientos set es_borrado = 1");
            foreach (Dg_emplazamientos emp in Emplazamientos)
            {
                if (Dg_emplazamientos.getByCodigo(emp.Codigo_emplazamiento) == false)
                {
                    emp.save();
                }
                else
                {
                    emp.updateEmplaza();
                }
            }
            return ret;
        }

    }
}
