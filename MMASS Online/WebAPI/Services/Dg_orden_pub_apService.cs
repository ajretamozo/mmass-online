using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class Dg_orden_pub_apR {
        public Dg_orden_pub_ap op { get; set; }
        public int result { get; set; }
        public string message { get; set; }
    }
    public interface IDg_orden_pub_apService
    {
        //Contacto GetAgencia(string username, string password);
        //Dg_tarifas getById(int Id);
        //IEnumerable<Dg_tarifas> getAll();
        Dg_orden_pub_apR save(Dg_orden_pub_ap miobj);
        IEnumerable<Dg_orden_pub_ap> getAll();
        String getAllHTML();
        Dg_orden_pub_ap getById(int id);
        Dg_orden_pub_ap getOpRadioById(int id);
        IEnumerable<DatoGenerico> getOrdenesRadio(int Id_agencia, int Id_anunciante);
        IEnumerable<Dg_orden_pub_ap> filter(List<Parametro> parametros);
        bool bloquearOP(Dg_orden_pub_bloqueo opb);
        bool desbloquearOP(Dg_orden_pub_bloqueo opb);
        bool desbloquearTodas(int id);
        IEnumerable<Dg_orden_pub_as> getSponsorsPorFecha(Dg_orden_pub_as det);
        bool anularOrden(int id);
        bool existeOpNombre(int id, string nom);
        void grabarLog(List<Parametro> datosLog);
    }
    public class Dg_orden_pub_apM : Dg_orden_pub_ap {
        public int result { get; set; }
        public string message { get; set; }
    }
    public class Dg_orden_pub_apService : IDg_orden_pub_apService
    {

        /*  public Dg_tarifas getById(int Id)
          {
              return Dg_tarifas.getById(Id);
          }
          */
        public IEnumerable<DatoGenerico> getOrdenesRadio(int Id_agencia, int Id_anunciante)
        {
            return Dg_orden_pub_ap.getOrdenesRadio(Id_agencia, Id_anunciante);
        }
        public IEnumerable<Dg_orden_pub_ap> getAll()
        {
            return Dg_orden_pub_ap.getAll();
        }
        public String getAllHTML()
        {
            return Dg_orden_pub_ap.getAllHTML();
        }
        public Dg_orden_pub_apR save(Dg_orden_pub_ap miobj)
        {
            Dg_orden_pub_apR ret = new Dg_orden_pub_apR();
            Contacto c = Contacto.getContactoById(miobj.Id_facturar);
            
            if (!c.No_facturable && miobj.Facturar_a != 2)
            {
                ret.op = null;
                ret.result = -3;
                ret.message = "El Contacto seleccionado para facturar no es facturable";
            }
            else
            {
                ret.op = miobj.save();
                if (ret.op != null)
                {
                    ret.result = 1;
                    ret.message = "La Orden Nro: " + miobj.Anio + "-" + miobj.Mes + "-" + miobj.Nro_orden + " se generó con éxito";
                }
                else {
                    ret.result = -1;
                    ret.message = "Ocurrió un error al intentar guardar la orden";
                }
            }
            return ret;
        }
        public Dg_orden_pub_ap getById(int id)
        {
            return Dg_orden_pub_ap.getById(id);
        }
        public Dg_orden_pub_ap getOpRadioById(int id)
        {
            return Dg_orden_pub_ap.getOpRadioById(id);
        }
        public IEnumerable<Dg_orden_pub_ap> filter(List<Parametro> parametros) 
        {
            return Dg_orden_pub_ap.filter(parametros);
        }


        /*
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
        */

        //AGREGUE:
        //public IEnumerable<Dg_orden_pub_as> getSponsorsPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    return Dg_orden_pub_as.getSponsorsPorFecha(fechaDesde, fechaHasta);
        //}
        public IEnumerable<Dg_orden_pub_as> getSponsorsPorFecha(Dg_orden_pub_as det)
        {
            return Dg_orden_pub_as.getSponsorsPorFecha(det);
        }

        public bool anularOrden(int id)
        {
            return Dg_orden_pub_ap.anularOrden(id);
        }

        public bool bloquearOP(Dg_orden_pub_bloqueo opb)
        {
            return opb.bloquear();
        }

        public bool desbloquearOP(Dg_orden_pub_bloqueo opb)
        {
            return opb.desbloquear();
        }

        public bool desbloquearTodas(int idUsuario)
        {
            return Dg_orden_pub_bloqueo.desbloquearTodas(idUsuario);
        }

        public bool existeOpNombre(int id, string nom)
        {
            return Dg_orden_pub_ap.existeOpNombre(id, nom);
        }

        public void grabarLog(List<Parametro> datosLog)
        {
            Dg_orden_pub_ap.grabarLog(datosLog);
        }

    }
}