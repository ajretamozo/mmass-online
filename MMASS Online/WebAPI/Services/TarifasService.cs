using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;
using System.Data;
using System.Transactions;

namespace WebApi.Services
{
    public interface ITarifaService
    {
        //Contacto GetAgencia(string username, string password);
        Dg_tarifas getById(int Id);
        IEnumerable<Dg_tarifas> getAll();
        int saveTarifa(Dg_tarifas miobj);
        bool removeTarifa(Dg_tarifas miobj);
        IEnumerable<Dg_tarifa_forma_uso> getFormasUso();
        IEnumerable<Dg_tarifas> filter(List<Parametro> parametros);
        int actualizarTarifa(List<Parametro> parametros);
    }

    public class TarifaService : ITarifaService
    {
        
        public Dg_tarifas getById(int Id)
        {
            return Dg_tarifas.getById(Id);
        }

        public IEnumerable<Dg_tarifas> getAll()
        {
            return Dg_tarifas.getAll();
        }
        public int saveTarifa(Dg_tarifas miobj)
        {
            return miobj.save();
        }

        public bool removeTarifa(Dg_tarifas miobj)
        {
            return miobj.remove();
        }

        public IEnumerable<Dg_tarifa_forma_uso> getFormasUso()
        {
            return Dg_tarifa_forma_uso.getAll();
        }

        public IEnumerable<Dg_tarifas> filter(List<Parametro> parametros)
        {
            return Dg_tarifas.filter(parametros);
        }

        public int actualizarTarifa(List<Parametro> parametros)
        {
            int respuesta = 0;

            Dg_tarifas tarifa = Dg_tarifas.getById(int.Parse(parametros[0].Value));

            try
            {
                using (TransactionScope transaccion = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 2, 0)))
                {
                    //Actualizo la fechaHasta de la tarifa actual
                    tarifa.Fecha_hasta = DateTime.Parse(parametros[1].Value);
                    tarifa.save();

                    //Creo la Tarifa nueva
                    tarifa.Id_tarifa_dg = 0;
                    tarifa.Fecha_desde = DateTime.Parse(parametros[1].Value);
                    if (parametros[2].Value != "")
                    {
                        tarifa.Fecha_hasta = DateTime.Parse(parametros[2].Value);
                    }
                    else
                    {
                        tarifa.Fecha_hasta = null;
                    }
                    tarifa.Precio_unitario = double.Parse(parametros[3].Value);
                    tarifa.save();

                    transaccion.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = 2;
                return respuesta;
            }

            return respuesta;
        }
    }
}