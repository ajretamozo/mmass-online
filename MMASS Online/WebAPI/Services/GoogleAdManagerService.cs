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
using Google.Api.Ads.AdManager.v202105;

namespace WebApi.Services
{
    public interface IGoogleAdManagerService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Contacto> GetAnunciantes(string desc);
        String GetOrderDetails(long idGAM);
        String GetOrderDetails2(Dg_orden_pub_ap orden);
        Order GetOrder(long idGAM);
        long CreateOrder(long idOrden);
        //AGREGUE:
        long CreateLineItems(Dg_orden_pub_as det);
        List<long> GetLineItemCreatives(long lineItemId);
        //AGREGUE:
        void RunAdExchangeReport();
        //TEST
        void reporteTest();
        //AGREGUE:
        //IEnumerable<Dg_emplazamientos> GetEmplazamientos(string redGAM);
        IEnumerable<Dg_emplazamientos> GetEmplazamientos();
        IEnumerable<Dg_medidas> GetMedidas();
        IEnumerable<Dg_medidas> GetMedidasTodasRedes(List<Parametro> parametros);
        long ArchivarLineItems(long lineItemIdt);
        void CambiarRed(string netCode);
        long GetRedActual();
    }

    public class GoogleAdManagerService : IGoogleAdManagerService
    {
        

        public IEnumerable<Contacto> GetAnunciantes(string desc)
        {            
            return GoogleAdManager.getAnunciantes(desc);
        }
        public String GetOrderDetails(long idGAM )
        {
            return GoogleAdManager.GetOrderDetails(idGAM);
        }

        public Order GetOrder(long idGAM)
        {
            return GoogleAdManager.GetOrder(idGAM);
        }

        //AGREGUE:
        public long CreateOrder(long idOrden)
        {

            Dg_orden_pub_ap op = Dg_orden_pub_ap.getById(Convert.ToInt32(idOrden));

            if (op.anunciante.IdContactoDigital == null)
            {
                return -2; //El contacto no esta sincronizado
            }
            else if (DB.DLong(op.anunciante.IdContactoDigital) < 1)
            {
                return -2; //El contacto no esta sincronizado
            }

            string nombreOrden = op.Producto_nombre + " " + op.Nro_orden.ToString();

            long result = GoogleAdManager.CreateOrder(nombreOrden, DB.DLong(op.anunciante.IdContactoDigital));

            if (result > 1)
            {
                //op.Id_Google_Ad_Manager = result;
                //op.save();
                //op.saveId_Google_Ad_Manager(idOrden , result);
                Dg_orden_pub_ap.saveId_Google_Ad_Manager(idOrden, result);
            }
            return result;

        }
        //AGREGUE:
        public long CreateLineItems(Dg_orden_pub_as det)
        {
            Dg_orden_pub_ap dg = Dg_orden_pub_ap.getById(det.Id_op_dg);
            long result = 0;

            if (det.Id_Google_Ad_Manager > 0)
            {
                result = GoogleAdManager.UpdateLineItem(det.Descripcion, det.Importe_unitario, det.Cantidad, det.Porc_dto, det.Fecha_desde, det.Fecha_hasta, det.Medidas, det.areaGeo, det.Emplazamientos, det.Tipo_tarifa, det.Id_Google_Ad_Manager);
            }
            else
            {
                result = GoogleAdManager.CreateLineItems(det.Descripcion, dg.Id_Google_Ad_Manager, det.Importe_unitario, det.Cantidad, det.Porc_dto, det.Fecha_desde, det.Fecha_hasta, det.Medidas, det.areaGeo, det.Emplazamientos, det.Tipo_tarifa);
            }

            if (result > 1)
            {
                foreach (var item in dg.Detalles)
                {
                    if (item.Id_detalle == det.Id_detalle)
                    {
                        //item.Id_Google_Ad_Manager = result;
                        //dg.save();
                        Dg_orden_pub_as.saveId_Google_Ad_Manager(item.Id_op_dg, item.Id_detalle, result);
                    }
                }
            }

            return result;
        }
        //AGREGUE:
        public void RunAdExchangeReport()
        {
            GoogleAdManager.RunAdExchangeReport();
        }
        //TEST
        public void reporteTest()
        {
            //GoogleAdManager.GetTamaños();
        }

        public String GetOrderDetails2(Dg_orden_pub_ap orden)
        {
            string res = "";
            //var sitios2 = new List<string>();
            List<string> sitios = new List<string>();

            foreach (var detalle in orden.Detalles)
            {
                if (detalle.Medios.Count() > 1)
                {
                    sitios.Add("RON");
                }
                else
                {
                    Medio med = Medio.getById(detalle.Medios[0].Id_medio);
                    sitios.Add(med.Desc_medio);
                }
            }

            res += GoogleAdManager.GetOrderDetails(orden.Id_Google_Ad_Manager, orden.Anunciante_nombre, sitios);
            res += "<div id='divAdjuntos'></div>";
            return res;
        }

        public List<long> GetLineItemCreatives(long lineItemId)
        {
            return GoogleAdManager.GetLineItemCreatives(lineItemId);
        }

        //AGREGUE:
        //public IEnumerable<Dg_emplazamientos> GetEmplazamientos(string redGAM)
        //{
        //    return GoogleAdManager.getEmplazamientos(redGAM);
        //}

        public IEnumerable<Dg_emplazamientos> GetEmplazamientos()
        {
            return GoogleAdManager.getEmplazamientos();
        }

        public IEnumerable<Dg_medidas> GetMedidas()
        {
            return GoogleAdManager.GetMedidas();
        }

        public IEnumerable<Dg_medidas> GetMedidasTodasRedes(List<Parametro> parametros)
        {
            return GoogleAdManager.GetMedidasTodasRedes(parametros);
        }

        public long ArchivarLineItems(long lineItemId)
        {
            long result = 0;

            result = GoogleAdManager.ArchivarLineItem(lineItemId);

            return result;
        }

        public void CambiarRed(string netCode)
        {          
            GoogleAdManager.CambiarRed(netCode);
        }

        public long GetRedActual()
        {
            long result = 0;

            result = GoogleAdManager.GetRedActual();

            return result;
        }

    }
}