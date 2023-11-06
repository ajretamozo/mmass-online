using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Services
{
    public interface IR_VentasService
    {
        IEnumerable<R_Ventas> filterBy(List<Parametro> parametros);
        IEnumerable<R_Ventas> getDatosHome(List<Parametro> parametros);
        IEnumerable<List<R_Ventas>> getRankingsHome(List<Parametro> parametros);
    }

    public class R_VentasService : IR_VentasService
    {
        public IEnumerable<R_Ventas> filterBy(List<Parametro> parametros)
        {
            return R_Ventas.filterBy(parametros);
        }

        public IEnumerable<R_Ventas> getDatosHome(List<Parametro> parametros)
        {
            //Obtenemos los datos del último mes 
            List<R_Ventas> datosMeses = new List<R_Ventas>();
            List<R_Ventas> OrdenesUltimoMes = R_Ventas.getDatosHome(parametros[0].Value, parametros[1].Value);
            R_Ventas ultimoMes = new R_Ventas();
            foreach(R_Ventas orden in OrdenesUltimoMes)
            {
                ultimoMes.Impresiones += orden.Impresiones;
                ultimoMes.Importe_total += orden.Importe_total / orden.Cambio;
            }
            ultimoMes.Ecpm = (ultimoMes.Importe_total / ultimoMes.Impresiones) * 1000;
            ultimoMes.Cantidad_ordenes = OrdenesUltimoMes.Count();

            datosMeses.Add(ultimoMes);

            //Obtenemos los datos del penúltimo mes 
            List<R_Ventas> OrdenesPenultimoMes = R_Ventas.getDatosHome(parametros[2].Value, parametros[3].Value);
            R_Ventas penultimoMes = new R_Ventas();
            foreach (R_Ventas orden in OrdenesPenultimoMes)
            {
                penultimoMes.Impresiones += orden.Impresiones;
                penultimoMes.Importe_total += orden.Importe_total / orden.Cambio;
            }
            penultimoMes.Ecpm = (penultimoMes.Importe_total / penultimoMes.Impresiones) * 1000;
            penultimoMes.Cantidad_ordenes = OrdenesPenultimoMes.Count();

            datosMeses.Add(penultimoMes);

            return datosMeses;
        }

        public IEnumerable<List<R_Ventas>> getRankingsHome(List<Parametro> parametros)
        {
            List<List<R_Ventas>> rankings = new List<List<R_Ventas>>();
            //Ranking Anunciantes
            rankings.Add(R_Ventas.getRankingsHome(parametros[0].Value, parametros[1].Value, 1));

            //Ranking Agencias
            rankings.Add(R_Ventas.getRankingsHome(parametros[0].Value, parametros[1].Value, 2));

            //Ranking Medios
            rankings.Add(R_Ventas.getRankingMedios(parametros[0].Value, parametros[1].Value));

            //Ranking Tipos Aviso
            rankings.Add(R_Ventas.getRankingTiposAviso(parametros[0].Value, parametros[1].Value));

            return rankings;
        }

        public IEnumerable<R_Ventas> getRankingMedios(List<Parametro> parametros)
        {
            return R_Ventas.getRankingMedios(parametros[0].Value, parametros[1].Value);
        }
    }

}
