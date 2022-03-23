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
        IEnumerable<Contacto> GetAnunciantes(Parametro nombre);
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
        IEnumerable<Dg_orden_pub_ap> GetOpNuevas(Parametro nombre);
        ListaParametro ComprobarModificaciones(Dg_orden_pub_ap order);
        ListaParametro ComprobarModificacionesD(long idGam);
    }

    public class GoogleAdManagerService : IGoogleAdManagerService
    {
        

        public IEnumerable<Contacto> GetAnunciantes(Parametro nombre)
        {            
            return GoogleAdManager.getAnunciantes(nombre);
        }
        public String GetOrderDetails(long idGAM )
        {
            return GoogleAdManager.GetOrderDetails(idGAM);
        }

        public Dg_orden_pub_ap GetOrderById(long idGAM)
        {
            Order ordenGam = new Order();
            ordenGam = GoogleAdManager.GetOrderById(idGAM);
            return OrderGamAOrdenAp(ordenGam);
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

            //string nombreOrden;
            //if (op.Bitacora == "")
            //{
            //    nombreOrden = op.Producto_nombre + " " + op.Anio.ToString() + " " + op.Mes.ToString() + " " + op.Nro_orden.ToString();
            //}
            //else
            //{
            //    nombreOrden = op.Bitacora;
            //}
            long result = 0;

            if (op.Id_Google_Ad_Manager > 0)
            {
                result = GoogleAdManager.UpdateOrder(op.Bitacora, DB.DLong(op.anunciante.IdContactoDigital), op.Id_Google_Ad_Manager);

            }
            else
            {
                result = GoogleAdManager.CreateOrder(op.Bitacora, DB.DLong(op.anunciante.IdContactoDigital));

            }

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
            //GoogleAdManager.GetTama�os();
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

        public IEnumerable<Dg_orden_pub_ap> GetOpNuevas(Parametro nombre)
        {
            long codRed = GetRedActual();
            Dg_red_GAM red = new Dg_red_GAM();
            red = Dg_red_GAM.getByCodigo(codRed);
            List<Order> ordenesGAM = new List<Order>();
            List<Dg_orden_pub_ap> ordenesNuevas = new List<Dg_orden_pub_ap>();
            ordenesGAM = GoogleAdManager.GetAllOrders(nombre);

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
                    string end = "";
                    if (order.endDateTime != null)
                    {
                        end = DateTimeUtilities.ToString(order.endDateTime, "yyyy/MM/dd");
                    }

                    if (start != "0")
                    {
                        ordenNueva.Fecha = System.DateTime.Parse(start);
                    }
                    if (end != "")
                    {
                        ordenNueva.Fecha_expiracion = System.DateTime.Parse(end);
                    }
                    ordenesNuevas.Add(ordenNueva);
                }             
            }
            return ordenesNuevas;
        }

        //public Dg_orden_pub_ap ComprobarModificaciones(Dg_orden_pub_ap order)
        //{
        //    Order ordenGam = new Order();
        //    ordenGam = GoogleAdManager.GetOrderById(order.Id_Google_Ad_Manager);
        //    Dg_orden_pub_ap orden = new Dg_orden_pub_ap();
        //    orden = Dg_orden_pub_ap.getById(order.Id_op_dg);
        //    List<LineItem> lineasGAM = new List<LineItem>();
        //    lineasGAM = GoogleAdManager.getLineItemsByOrder(ordenGam.id);

        //    //se buscan diferencias entre la orden gam y la orden ap; si se encuentran, se devuelve la orden gam
        //    //orden.Observ = ordenGam.name;
        //    if (long.Parse(orden.anunciante.IdContactoDigital) != ordenGam.advertiserId)
        //    {
        //        orden = OrderGamAOrdenAp(ordenGam);
        //    }

        //    else if(ordenGam.totalBudget.microAmount / 1000000 != orden.Seg_neto)
        //    {
        //        orden = OrderGamAOrdenAp(ordenGam);
        //    }

        //    else if(System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.startDateTime, "yyyy/MM/dd")) != orden.Fecha)
        //    {
        //        orden = OrderGamAOrdenAp(ordenGam);
        //    }

        //    else if (System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.endDateTime, "yyyy/MM/dd")) != orden.Fecha_expiracion)
        //    {
        //        orden = OrderGamAOrdenAp(ordenGam);
        //    }

        //    //Se comparan las Lineas de Pedido
        //    else if (lineasGAM.Count != orden.Detalles.Count)
        //    {
        //        orden = OrderGamAOrdenAp(ordenGam);
        //    }

        //    else if (lineasGAM.Count == orden.Detalles.Count)
        //    {
        //        foreach (Dg_orden_pub_as detalle in orden.Detalles)
        //        {
        //            foreach (LineItem linea in lineasGAM)
        //            {
        //                while (linea.id == detalle.Id_Google_Ad_Manager)
        //                {
        //                    switch (linea.costType)
        //                    {
        //                        case CostType.CPM:
        //                            if (detalle.Tipo_tarifa != 0)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                        case CostType.CPD:
        //                            if (detalle.Tipo_tarifa != 1)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                        case CostType.CPC:
        //                            if (detalle.Tipo_tarifa != 3)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                        case CostType.CPA:
        //                            if (detalle.Tipo_tarifa != 4)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                    }

        //                    if (linea.name != detalle.Descripcion)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }

        //                    //Se comparan emplazamientos
        //                    else if (linea.targeting.inventoryTargeting.targetedPlacementIds.Length != detalle.Emplazamientos.Count)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }
        //                    //else if (linea.targeting.inventoryTargeting.targetedPlacementIds == null && detalle.Emplazamientos != null)
        //                    //{
        //                    //    return OrderGamAOrdenAp(ordenGam);
        //                    //}
        //                    //else if (linea.targeting.inventoryTargeting.targetedPlacementIds == null && detalle.Emplazamientos != null)
        //                    //{
        //                    //    return OrderGamAOrdenAp(ordenGam);
        //                    //}
        //                    else if (linea.targeting.inventoryTargeting.targetedPlacementIds.Length == detalle.Emplazamientos.Count)
        //                    {
        //                        bool existe = false;
        //                        foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
        //                        {
        //                            foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
        //                            {
        //                                if (idEmpla == emp.Codigo_emplazamiento)
        //                                {
        //                                    existe = true;
        //                                }
        //                            }
        //                            if (existe == false)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                        }
        //                    }

        //                    //Se comparan medidas
        //                    else if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }
        //                    else if (linea.creativePlaceholders.Length == detalle.Medidas.Count)
        //                    {
        //                        bool existe = false;
        //                        foreach (Dg_orden_pub_medidas med in detalle.Medidas)
        //                        {
        //                            string medAg = med.Ancho.ToString() + "x" + med.Alto.ToString();

        //                            foreach (CreativePlaceholder cph in linea.creativePlaceholders)
        //                            {
        //                                string medGam = cph.size.width.ToString() + "x" + cph.size.height.ToString();

        //                                if (String.Equals(medGam, medAg))
        //                                {
        //                                    existe = true;
        //                                }
        //                            }
        //                            if (existe == false)
        //                            {
        //                                orden = OrderGamAOrdenAp(ordenGam);
        //                            }
        //                            break;
        //                        }
        //                    }

        //                    else if ((linea.costPerUnit.microAmount / 1000000) != detalle.Importe_unitario)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }

        //                    else if ((float)linea.discount != detalle.Porc_dto)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }

        //                    else if ((int)linea.primaryGoal.units != detalle.Cantidad)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }

        //                    else if ((linea.budget.microAmount / 1000000) != detalle.Monto_neto)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }

        //                    else if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }
        //                    else if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
        //                    {
        //                        orden = OrderGamAOrdenAp(ordenGam);
        //                    }
        //                    //else
        //                    //{
        //                    //    return orden;
        //                    //}
        //                }
        //                break;
        //            }

        //        }
        //        //return orden;
        //    }

        //    //si no se encuentran modificaciones, se devuelve la orden vac�a
        //    //else
        //    //{
        //    //    return orden;
        //    //}
        //    return orden;
        //}

        public ListaParametro ComprobarModificaciones(Dg_orden_pub_ap order)
        {
            ListaParametro ordYdet = new ListaParametro();
            ordYdet.Parametros = new List<Parametro>();
            ordYdet.ListaListaParametros = new List<ListaParametro>();
            Order ordenGam = new Order();
            ordenGam = GoogleAdManager.GetOrderById(order.Id_Google_Ad_Manager);
            Dg_orden_pub_ap orden = new Dg_orden_pub_ap();
            orden = Dg_orden_pub_ap.getById(order.Id_op_dg);
            List<LineItem> lineasGAM = new List<LineItem>();
            lineasGAM = GoogleAdManager.getLineItemsByOrder(ordenGam.id);
            long codRed = GetRedActual();
            Dg_red_GAM red = new Dg_red_GAM();
            red = Dg_red_GAM.getByCodigo(codRed);

            //se buscan diferencias entre la orden gam y la orden ap; si se encuentran, se devuelve la lista de cambios
            //orden.Observ = ordenGam.name;
            //if (orden.Bitacora == "")
            //{
            //    string[] arrNombre = ordenGam.name.Split(" ");
            //    string prod = arrNombre[0];
            //    string anio = arrNombre[1];
            //    string mes = arrNombre[2];
            //    string nro_orden = arrNombre[3];
            //    if (orden.Producto_nombre != prod || orden.Anio != int.Parse(anio) || orden.Mes != int.Parse(mes) || orden.Nro_orden != int.Parse(nro_orden))
            //    {
            //        string nombreOrden = orden.Producto_nombre + " " + orden.Anio.ToString() + " " + orden.Mes.ToString() + " " + orden.Nro_orden.ToString();
            //        Parametro cambioNom = new Parametro();
            //        cambioNom.ParameterName = "Nombre";
            //        cambioNom.Value = nombreOrden + "@@@" + ordenGam.name;
            //        ordYdet.Parametros.Add(cambioNom);
            //    }
            //}
            //else
            //{
                if (orden.Bitacora != ordenGam.name)
                {
                    Parametro cambioNom = new Parametro();
                    cambioNom.ParameterName = "Descripci�n";
                    cambioNom.Value = orden.Bitacora + "@@@" + ordenGam.name;
                    ordYdet.Parametros.Add(cambioNom);
                }
            //}

            //if (long.Parse(orden.anunciante.IdContactoDigital) != ordenGam.advertiserId)
            //{
            //    Parametro cambioAnun = new Parametro();
            //    cambioAnun.ParameterName = "Anunciante";
            //    cambioAnun.Value = orden.Anunciante_nombre + "@@@" + Contacto.getContactoByIdGAMyRed(ordenGam.advertiserId.ToString(), red.Id_red).RazonSocial;
            //    ordYdet.Parametros.Add(cambioAnun);
            //}

            //if (ordenGam.totalBudget.microAmount / 1000000 != orden.Seg_neto)
            //{
            //    Parametro cambioTotal = new Parametro();
            //    cambioTotal.ParameterName = "Seg Neto";
            //    cambioTotal.Value = orden.Seg_neto.ToString() + "@@@" + (ordenGam.totalBudget.microAmount / 1000000).ToString();
            //    ordYdet.Parametros.Add(cambioTotal);
            //}

            if (System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.startDateTime, "yyyy/MM/dd")) != orden.Fecha)
            {
                Parametro cambioDesde = new Parametro();
                cambioDesde.ParameterName = "Vigencia Desde";
                string fechaFormat = formatFecha(orden.Fecha.ToString());
                cambioDesde.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(ordenGam.startDateTime, "dd/MM/yyyy");
                ordYdet.Parametros.Add(cambioDesde);
            }

            if (System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.endDateTime, "yyyy/MM/dd")) != orden.Fecha_expiracion)
            {
                Parametro cambioHasta = new Parametro();
                cambioHasta.ParameterName = "Vigencia Hasta";
                string fechaFormat = formatFecha(orden.Fecha_expiracion.ToString());
                cambioHasta.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(ordenGam.endDateTime, "dd/MM/yyyy");
                ordYdet.Parametros.Add(cambioHasta);
            }

            //Se comparan las Lineas de Pedido
            //if (lineasGAM.Count != orden.Detalles.Count)
            //{
            //    Parametro cambioLCant = new Parametro();
            //    cambioLCant.ParameterName = "cantLineas";
            //    cambioLCant.Value = orden.Detalles.Count.ToString() + "@@@" + lineasGAM.Count.ToString();
            //    cambios.Parametros.Add(cambioLCant);
            //}

            //else
            //{
           
                foreach (Dg_orden_pub_as detalle in orden.Detalles)
                {
                    ListaParametro cambiosL = new ListaParametro();
                    foreach (LineItem linea in lineasGAM)
                    {
                        if (linea.id == detalle.Id_Google_Ad_Manager)
                        {
                            cambiosL.Parametros = new List<Parametro>();
                            switch (detalle.Tipo_tarifa)
                            {
                                case 0:
                                    if (linea.costType != CostType.CPM)
                                    {
                                        Parametro cambioLTipoTar = new Parametro();
                                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                                        cambioLTipoTar.Value = "CPM" + "@@@" + linea.costType.ToString();
                                        cambiosL.Parametros.Add(cambioLTipoTar);
                                    }
                                    break;
                                case 1:
                                    if (linea.costType != CostType.CPD)
                                    {
                                        Parametro cambioLTipoTar = new Parametro();
                                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                                        cambioLTipoTar.Value = "CPD" + "@@@" + linea.costType.ToString();
                                        cambiosL.Parametros.Add(cambioLTipoTar);
                                    }
                                    break;
                                case 2:
                                    {
                                        Parametro cambioLTipoTar = new Parametro();
                                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                                        cambioLTipoTar.Value = "Posteo" + "@@@" + linea.costType.ToString();
                                        cambiosL.Parametros.Add(cambioLTipoTar);
                                    }                                                 
                                    break;
                                case 3:
                                    if (linea.costType != CostType.CPC)
                                    {
                                        Parametro cambioLTipoTar = new Parametro();
                                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                                        cambioLTipoTar.Value = "CPC" + "@@@" + linea.costType.ToString();
                                        cambiosL.Parametros.Add(cambioLTipoTar);
                                    }
                                    break;
                                case 4:
                                    if (linea.costType != CostType.CPA)
                                    {
                                        Parametro cambioLTipoTar = new Parametro();
                                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                                        cambioLTipoTar.Value = "CPA" + "@@@" + linea.costType.ToString();
                                        cambiosL.Parametros.Add(cambioLTipoTar);
                                    }
                                    break;
                            }

                            if (linea.name != detalle.Descripcion)
                            {
                                Parametro cambioLDesc = new Parametro();
                                cambioLDesc.ParameterName = "Descripci�n";
                                cambioLDesc.Value = detalle.Descripcion + "@@@" + linea.name;
                                cambiosL.Parametros.Add(cambioLDesc);
                            }

                            //Se comparan emplazamientos
                            int cantEmpGam = 0;
                            if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
                            {
                                cantEmpGam = linea.targeting.inventoryTargeting.targetedPlacementIds.Length;
                            }
                            if (cantEmpGam != detalle.Emplazamientos.Count)
                            {
                                cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, linea.targeting.inventoryTargeting.targetedPlacementIds, red.Id_red));
                            }
                            else
                            {
                                foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
                                {
                                    bool existeEmp = false;
                                    foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                                    {
                                        if (idEmpla == emp.Codigo_emplazamiento)
                                        {
                                            existeEmp = true;
                                        }
                                    }
                                    if (existeEmp == false)
                                    {
                                        cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, linea.targeting.inventoryTargeting.targetedPlacementIds, red.Id_red));
                                    }
                                    break;
                                }
                            }

                            //Se comparan medidas
                            if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
                            {
                                cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders));
                            }
                            else
                            {
                                bool existe = false;
                                foreach (Dg_orden_pub_medidas med in detalle.Medidas)
                                {
                                    string medAg = med.Ancho.ToString() + "x" + med.Alto.ToString();

                                    foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                                    {
                                        string medGam = cph.size.width.ToString() + "x" + cph.size.height.ToString();

                                        if (String.Equals(medGam, medAg))
                                        {
                                            existe = true;
                                        }
                                    }
                                    if (existe == false)
                                    {
                                        cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders));
                                    }
                                    break;
                                }
                            }

                            if ((linea.costPerUnit.microAmount / 1000000) != detalle.Importe_unitario)
                            {
                                Parametro cambioLImpUni = new Parametro();
                                cambioLImpUni.ParameterName = "Precio Unitario";
                                cambioLImpUni.Value = detalle.Importe_unitario.ToString() + "@@@" + (linea.costPerUnit.microAmount / 1000000).ToString();
                                cambiosL.Parametros.Add(cambioLImpUni);
                            }

                            if ((float)linea.discount != detalle.Porc_dto)
                            {
                                Parametro cambioLDesc = new Parametro();
                                cambioLDesc.ParameterName = "Descuento";
                                cambioLDesc.Value = detalle.Porc_dto.ToString() + "@@@" + ((float)linea.discount).ToString();
                                cambiosL.Parametros.Add(cambioLDesc);
                            }

                            if ((int)linea.primaryGoal.units != detalle.Cantidad)
                            {
                                Parametro cambioLCant = new Parametro();
                                cambioLCant.ParameterName = "Cantidad";
                                cambioLCant.Value = detalle.Cantidad.ToString() + "@@@" + ((int)linea.primaryGoal.units).ToString();
                                cambiosL.Parametros.Add(cambioLCant);
                            }

                            if ((linea.budget.microAmount / 1000000) != detalle.Monto_neto)
                            {
                                Parametro cambioLImpTotal = new Parametro();
                                cambioLImpTotal.ParameterName = "Total";
                                cambioLImpTotal.Value = detalle.Monto_neto.ToString() + "@@@" + (linea.budget.microAmount / 1000000).ToString();
                                cambiosL.Parametros.Add(cambioLImpTotal);
                            }

                            if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
                            {
                                Parametro cambioLDesde = new Parametro();
                                cambioLDesde.ParameterName = "Vigencia Desde";
                                string fechaFormat = formatFecha(detalle.Fecha_desde.ToString());
                                cambioLDesde.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(linea.startDateTime, "dd/MM/yyyy");
                                cambiosL.Parametros.Add(cambioLDesde);
                            }
                            if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
                            {
                                Parametro cambioLHasta = new Parametro();
                                cambioLHasta.ParameterName = "Vigencia Hasta";
                                string fechaFormat = formatFecha(detalle.Fecha_hasta.ToString());
                                cambioLHasta.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(linea.endDateTime, "dd/MM/yyyy");
                                cambiosL.Parametros.Add(cambioLHasta);
                            }
                        }
                    }
                    ordYdet.ListaListaParametros.Add(cambiosL);
                }

            return ordYdet;
        }

        private Dg_orden_pub_ap OrderGamAOrdenAp(Order ordenGam)
        {
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
            ordenNueva.Seg_neto = ordenGam.totalBudget.microAmount / 1000000;
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
            List<LineItem> lineasGAM = new List<LineItem>();
            lineasGAM = GoogleAdManager.getLineItemsByOrder(ordenGam.id);
            int contId = 0;

            foreach (LineItem linea in lineasGAM)
            {
                Dg_orden_pub_as detalle = new Dg_orden_pub_as();
                List<Dg_orden_pub_emplazamientos> emplazamientos = new List<Dg_orden_pub_emplazamientos>();
                List<Dg_orden_pub_medidas> medidas = new List<Dg_orden_pub_medidas>();

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
                if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
                {
                    Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                    foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                    {
                        emplaza.Codigo_emplazamiento = idEmpla;
                        emplaza.Id_emplazamiento = Dg_emplazamientos.getByCodigo2(idEmpla, red.Id_red).Id_emplazamiento;
                        emplazamientos.Add(emplaza);
                    }
                    detalle.Emplazamientos = emplazamientos;
                }
                Dg_orden_pub_medidas medida = new Dg_orden_pub_medidas();
                foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                {
                    string desc = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                    medida.Id_medidadigital = Dg_medidas.getByDescripcion(desc).Id_medidadigital;
                    medida.Ancho = cph.size.width;
                    medida.Alto = cph.size.height;
                    medidas.Add(medida);
                }
                detalle.Medidas = medidas;
                detalle.Tarifa_manual = 1;
                detalle.Importe_unitario = linea.costPerUnit.microAmount / 1000000;
                detalle.Porc_dto = (float)linea.discount;
                detalle.Cantidad = (int)linea.primaryGoal.units;
                detalle.Monto_neto = linea.budget.microAmount / 1000000;
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

        public Parametro ImprimirEmplazas(List<Dg_orden_pub_emplazamientos> emplazaBD, long[] emplazaGam, int idRed)
        {
            Parametro cambioLEmplaza = new Parametro();
            cambioLEmplaza.ParameterName = "Emplazamientos";
            string empBD = "";
            string empGam = "";

            foreach (Dg_orden_pub_emplazamientos emp in emplazaBD)
            {
                if (empBD == "")
                {
                    empBD = emp.Descripcion;
                }
                else
                {
                    empBD += ", " + emp.Descripcion;
                }
            }
            foreach (long idEmpla in emplazaGam)
            {
                Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                string empDesc = Dg_emplazamientos.getByCodigo2(idEmpla, idRed).Descripcion;
                if (empGam == "")
                {
                    empGam = empDesc + "$@%" + idEmpla.ToString();
                }
                else
                {
                    empGam += ", " + empDesc + "$@%" + idEmpla.ToString();
                }
            }

            cambioLEmplaza.Value = empBD + "@@@" + empGam;
            return cambioLEmplaza;
        }

        public Parametro ImprimirMedidas(List<Dg_orden_pub_medidas> medidasBD, CreativePlaceholder[] medidasGam)
        {
            Parametro cambioLMed = new Parametro();
            cambioLMed.ParameterName = "Medidas";
            string medBD = "";
            string medGam = "";

            foreach (Dg_orden_pub_medidas med in medidasBD)
            {
                if (medBD == "")
                {
                    medBD = med.Ancho.ToString() + "x" + med.Alto.ToString();
                }
                else
                {
                    medBD += ", " + med.Ancho.ToString() + "x" + med.Alto.ToString();
                }
            }
            foreach (CreativePlaceholder cph in medidasGam)
            {
                if (medGam == "")
                {
                    medGam = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                }
                else
                {
                    medGam += ", " + cph.size.width.ToString() + "x" + cph.size.height.ToString();
                }
            }

            cambioLMed.Value = medBD + "@@@" + medGam;
            return cambioLMed;
        }

        public string formatFecha(string fecha)
        {
            string fechaFormat = "";
            string[] arrFecha = fecha.Split(" ");
            fechaFormat = arrFecha[0];
            return fechaFormat;
        }

        public ListaParametro ComprobarModificacionesD(long idGam)
        {
            ListaParametro cambiosL = new ListaParametro();
            cambiosL.Parametros = new List<Parametro>();
            long codRed = GetRedActual();
            Dg_red_GAM red = new Dg_red_GAM();
            red = Dg_red_GAM.getByCodigo(codRed);
            Dg_orden_pub_as detalle = new Dg_orden_pub_as();
            detalle = Dg_orden_pub_as.getByIdGam(idGam, red.Id_red);
            LineItem linea = new LineItem();
            linea = GoogleAdManager.GetLineItemById(idGam);

            //se buscan diferencias entre la orden gam y la orden ap; si se encuentran, se devuelve la lista de cambios
            switch (detalle.Tipo_tarifa)
            {
                case 0:
                    if (linea.costType != CostType.CPM)
                    {
                        Parametro cambioLTipoTar = new Parametro();
                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                        cambioLTipoTar.Value = "CPM" + "@@@" + linea.costType.ToString();
                        cambiosL.Parametros.Add(cambioLTipoTar);
                    }
                    break;
                case 1:
                    if (linea.costType != CostType.CPD)
                    {
                        Parametro cambioLTipoTar = new Parametro();
                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                        cambioLTipoTar.Value = "CPD" + "@@@" + linea.costType.ToString();
                        cambiosL.Parametros.Add(cambioLTipoTar);
                    }
                    break;
                case 2:
                    {
                        Parametro cambioLTipoTar = new Parametro();
                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                        cambioLTipoTar.Value = "Posteo" + "@@@" + linea.costType.ToString();
                        cambiosL.Parametros.Add(cambioLTipoTar);
                    }
                    break;
                case 3:
                    if (linea.costType != CostType.CPC)
                    {
                        Parametro cambioLTipoTar = new Parametro();
                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                        cambioLTipoTar.Value = "CPC" + "@@@" + linea.costType.ToString();
                        cambiosL.Parametros.Add(cambioLTipoTar);
                    }
                    break;
                case 4:
                    if (linea.costType != CostType.CPA)
                    {
                        Parametro cambioLTipoTar = new Parametro();
                        cambioLTipoTar.ParameterName = "Tipo Tarifa";
                        cambioLTipoTar.Value = "CPA" + "@@@" + linea.costType.ToString();
                        cambiosL.Parametros.Add(cambioLTipoTar);
                    }
                    break;
            }

            if (linea.name != detalle.Descripcion)
            {
                Parametro cambioLDesc = new Parametro();
                cambioLDesc.ParameterName = "Descripci�n";
                cambioLDesc.Value = detalle.Descripcion + "@@@" + linea.name;
                cambiosL.Parametros.Add(cambioLDesc);
            }

            //Se comparan emplazamientos
            int cantEmpGam = 0;
            if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
            {
                cantEmpGam = linea.targeting.inventoryTargeting.targetedPlacementIds.Length;
            }
            if (cantEmpGam != detalle.Emplazamientos.Count)
            {
                cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, linea.targeting.inventoryTargeting.targetedPlacementIds, red.Id_red));
            }
            else
            {
                foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
                {
                    bool existeEmp = false;
                    foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                    {
                        if (idEmpla == emp.Codigo_emplazamiento)
                        {
                            existeEmp = true;
                            break;
                        }
                    }
                    if (existeEmp == false)
                    {
                        cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, linea.targeting.inventoryTargeting.targetedPlacementIds, red.Id_red));
                    }
                    //break;
                }
            }

            //Se comparan medidas
            if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
            {
                cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders));
            }
            else
            {
                foreach (Dg_orden_pub_medidas med in detalle.Medidas)
                {
                    string medAg = med.Ancho.ToString() + "x" + med.Alto.ToString();
                    bool existe = false;
                    foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                    {
                        string medGam = cph.size.width.ToString() + "x" + cph.size.height.ToString();

                        if (String.Equals(medGam, medAg))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (existe == false)
                    {
                        cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders));
                    }
                    //break;
                }
            }

            if ((linea.costPerUnit.microAmount / 1000000) != detalle.Importe_unitario)
            {
                Parametro cambioLImpUni = new Parametro();
                cambioLImpUni.ParameterName = "Precio Unitario";
                cambioLImpUni.Value = detalle.Importe_unitario.ToString() + "@@@" + (linea.costPerUnit.microAmount / 1000000).ToString();
                cambiosL.Parametros.Add(cambioLImpUni);
            }

            if ((float)linea.discount != detalle.Porc_dto)
            {
                Parametro cambioLDesc = new Parametro();
                cambioLDesc.ParameterName = "Descuento";
                cambioLDesc.Value = detalle.Porc_dto.ToString() + "@@@" + ((float)linea.discount).ToString();
                cambiosL.Parametros.Add(cambioLDesc);
            }

            if ((int)linea.primaryGoal.units != detalle.Cantidad)
            {
                Parametro cambioLCant = new Parametro();
                cambioLCant.ParameterName = "Cantidad";
                cambioLCant.Value = detalle.Cantidad.ToString() + "@@@" + ((int)linea.primaryGoal.units).ToString();
                cambiosL.Parametros.Add(cambioLCant);
            }

            //if ((linea.budget.microAmount / 1000000) != detalle.Monto_neto)
            //{
            //    Parametro cambioLImpTotal = new Parametro();
            //    cambioLImpTotal.ParameterName = "Total";
            //    cambioLImpTotal.Value = detalle.Monto_neto.ToString() + "@@@" + (linea.budget.microAmount / 1000000).ToString();
            //    cambiosL.Parametros.Add(cambioLImpTotal);
            //}

            if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
            {
                Parametro cambioLDesde = new Parametro();
                cambioLDesde.ParameterName = "Vigencia Desde";
                string fechaFormat = formatFecha(detalle.Fecha_desde.ToString());
                cambioLDesde.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(linea.startDateTime, "dd/MM/yyyy");
                cambiosL.Parametros.Add(cambioLDesde);
            }
            if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
            {
                Parametro cambioLHasta = new Parametro();
                cambioLHasta.ParameterName = "Vigencia Hasta";
                string fechaFormat = formatFecha(detalle.Fecha_hasta.ToString());
                cambioLHasta.Value = fechaFormat + "@@@" + DateTimeUtilities.ToString(linea.endDateTime, "dd/MM/yyyy");
                cambiosL.Parametros.Add(cambioLHasta);
            }

            return cambiosL;
        }

    }
}