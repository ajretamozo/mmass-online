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
using Google.Api.Ads.AdManager.Util.v202111;
using Google.Api.Ads.AdManager.v202111;

namespace WebApi.Services
{
    public interface IGoogleAdManagerService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Contacto> GetAnunciantes(string desc);
        String GetOrderDetails(long idGAM);
        String GetOrderDetails2(Dg_orden_pub_ap orden);
        Dg_orden_pub_ap GetOrderById(long idGAM);
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
        IEnumerable<Dg_orden_pub_ap> GetOpNuevas();
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

        public Dg_orden_pub_ap GetOrderById(long idGAM)
        {
            Order ordenGam = new Order();
            ordenGam = GoogleAdManager.GetOrderById(idGAM);
            Dg_orden_pub_ap ordenNueva = new Dg_orden_pub_ap();
            long codRed = GetRedActual();
            Dg_red_GAM red = new Dg_red_GAM();
            red = Dg_red_GAM.getByCodigo(codRed);
            List<Dg_orden_pub_ejecutivos> ejecutivos = new List<Dg_orden_pub_ejecutivos>();
            List<Dg_orden_pub_pagos> formasPago = new List<Dg_orden_pub_pagos>();
            List<Dg_orden_pub_as> detalles = new List<Dg_orden_pub_as>();

            ordenNueva.Id_red = red.Id_red;
            ordenNueva.Id_Google_Ad_Manager = ordenGam.id;
            ordenNueva.Observ = ordenGam.name;
            ordenNueva.anunciante = Contacto.getContactoByIdGAMyRed(ordenGam.advertiserId.ToString(), red.Id_red);
            ordenNueva.Seg_neto = (ordenGam.totalBudget.microAmount) / 1000000;
            ordenNueva.FormasPago = formasPago;
            ordenNueva.Ejecutivos = ejecutivos;
            string start = DateTimeUtilities.ToString(ordenGam.startDateTime, "yyyy/MM/dd");
            string end = DateTimeUtilities.ToString(ordenGam.endDateTime, "yyyy/MM/dd");

            if (start != "0")
            {
                ordenNueva.Fecha = System.DateTime.Parse(start);
            }
            if (end != "0")
            {
                ordenNueva.Fecha_expiracion = System.DateTime.Parse(end);
            }

            //Se traen las Lineas de Pedido
            Dg_orden_pub_as detalle = new Dg_orden_pub_as();
            List<LineItem> lineasGAM = new List<LineItem>();
            lineasGAM = GoogleAdManager.getLineItemsByOrder(idGAM);
            List<Dg_orden_pub_emplazamientos> emplazamientos = new List<Dg_orden_pub_emplazamientos>();
            List<Dg_orden_pub_medidas> medidas = new List<Dg_orden_pub_medidas>();
            int contId = 0;

            foreach (LineItem linea in lineasGAM)
            {
                detalle.Id_detalle = contId;
                detalle.Id_Google_Ad_Manager = linea.id;
                detalle.Descripcion = linea.name;
                switch (linea.costType)
                {
                    case CostType.CPM:
                        detalle.Tipo_tarifa = 0;
                        break;
                    case CostType.CPD:
                        detalle.Tipo_tarifa = 1;
                        break;
                    case CostType.CPC:
                        detalle.Tipo_tarifa = 3;
                        break;
                    case CostType.CPA:
                        detalle.Tipo_tarifa = 4;
                        break;
                }
                Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                foreach(long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                {
                    emplaza.Codigo_emplazamiento = idEmpla;
                    emplaza.Id_emplazamiento = Dg_emplazamientos.getByCodigo2(idEmpla, red.Id_red).Id_emplazamiento;
                    emplazamientos.Add(emplaza);
                }
                detalle.Emplazamientos = emplazamientos;
                Dg_orden_pub_medidas medida = new Dg_orden_pub_medidas();
                foreach(CreativePlaceholder cph in linea.creativePlaceholders)
                {
                    string desc = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                    medida.Id_medidadigital = Dg_medidas.getByDescripcion(desc).Id_medidadigital;
                    medida.Ancho = cph.size.width;
                    medida.Alto = cph.size.height;
                    medidas.Add(medida);
                }
                detalle.Medidas = medidas;
                detalle.Tarifa_manual = 1;
                detalle.Importe_unitario = (linea.costPerUnit.microAmount) / 1000000;
                detalle.Porc_dto = (float)linea.discount;
                detalle.Cantidad = (int)linea.primaryGoal.units;
                detalle.Monto_neto = (linea.budget.microAmount) / 1000000;
                string startD = DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd");
                string endD = DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd");

                if (startD != "0")
                {
                    detalle.Fecha_desde = System.DateTime.Parse(startD);
                }
                if (endD != "0")
                {
                    detalle.Fecha_hasta = System.DateTime.Parse(endD);
                }

                detalles.Add(detalle);
                contId++;
            }
            ordenNueva.Detalles = detalles;

            return ordenNueva;
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

        public IEnumerable<Dg_orden_pub_ap> GetOpNuevas()
        {
            long codRed = GetRedActual();
            Dg_red_GAM red = new Dg_red_GAM();
            red = Dg_red_GAM.getByCodigo(codRed);
            List<Order> ordenesGAM = new List<Order>();
            List<Dg_orden_pub_ap> ordenesNuevas = new List<Dg_orden_pub_ap>();
            ordenesGAM = GoogleAdManager.GetAllOrders();

            foreach (Order order in ordenesGAM)
            {
                Dg_orden_pub_ap ordenNueva = new Dg_orden_pub_ap();
                if (Dg_orden_pub_ap.existeOpGAMenBD(order.id, red.Id_red) == false)
                {
                    Contacto anun = new Contacto();
                    ordenNueva.Id_red = red.Id_red;
                    ordenNueva.Id_Google_Ad_Manager = order.id;
                    ordenNueva.Observ = order.name;
                    ordenNueva.Anunciante_nombre = GoogleAdManager.GetAnunciantePorId(order.advertiserId);
                    anun.IdContactoDigital = order.advertiserId.ToString();
                    ordenNueva.anunciante = anun;
                    ordenNueva.Seg_neto = (order.totalBudget.microAmount)/1000000;
                    string start = DateTimeUtilities.ToString(order.startDateTime, "yyyy/MM/dd");
                    string end = DateTimeUtilities.ToString(order.endDateTime, "yyyy/MM/dd");

                    if (start != "0")
                    {
                        ordenNueva.Fecha = System.DateTime.Parse(start);
                    }
                    if (end != "0")
                    {
                        ordenNueva.Fecha_expiracion = System.DateTime.Parse(end);
                    }
                    ordenesNuevas.Add(ordenNueva);
                }             
            }
            return ordenesNuevas;
        }

    }
}