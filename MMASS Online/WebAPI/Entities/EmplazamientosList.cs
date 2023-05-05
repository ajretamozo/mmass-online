using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class EmplazamientosList
    {
        public List<Dg_emplazamientos> Emplazamientos;

        public bool save()
        {
            int idRed = Emplazamientos[1].Id_red;
            DB.Execute("update dg_emplazamientos set es_borrado = 1 where id_red = " + idRed);
            bool ret = true;
            //DB.Execute("update dg_emplazamientos set es_borrado = 1");
            foreach (Dg_emplazamientos emp in Emplazamientos)
            {
                if (Dg_emplazamientos.getByCodigo(emp.Codigo_emplazamiento, idRed) == false)
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
