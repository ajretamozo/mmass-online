using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using Google.Api.Ads.AdManager.Lib;
using Google.Api.Ads.AdManager.Util.v202302;
using Google.Api.Ads.AdManager.v202302;
using Google.Api.Ads.Common.Util.Reports;
using System.Diagnostics;
using System.Configuration;
using User = Google.Api.Ads.AdManager.v202302.User;
using Google.Api.Ads.Common.Util;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Helpers
{
    public class GoogleAdManager
    {
        //Parámretro global para mantener el usuario GAM
        public static AdManagerUser user = new AdManagerUser();

        public static void CambiarRed(string netCode)
        {
            AdManagerAppConfig config = (AdManagerAppConfig)user.Config;
            config.NetworkCode = netCode;
        }

        public static long GetRedActual()
        {
            long netCode = 0;

            //AdManagerUser user = new AdManagerUser();

            using (NetworkService networkService = user.GetService<NetworkService>())
            {
                try
                {
                    // Get the current network.
                    Network network = networkService.getCurrentNetwork();

                    Console.WriteLine(
                        "Current network has network code \"{0}\" and display name \"{1}\".",
                        network.networkCode, network.displayName);
                        netCode =  long.Parse(network.networkCode);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to get current network. Exception says \"{0}\"",
                        e.Message);
                }
            }

            return netCode;
        }

        public static long GetUserActual()
        {
            long traffId = 0;

            using (UserService userService = user.GetService<UserService>())
            {
                // Create a statement to select users.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("email = :email")
                    .OrderBy("id ASC")
                    .Limit(pageSize)
                    .AddValue("email", "admanagertest@admanagertest.iam.gserviceaccount.com");

                // Retrieve a small amount of users at a time, paging through until all
                // users have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    UserPage page = userService.getUsersByStatement(statementBuilder.ToStatement());

                    // Print out some information for each user.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (User usr in page.results)
                        {
                            Console.WriteLine("{0}) User with ID {1} and name \"{2}\" was found.",
                                i++, usr.id, usr.name);
                            traffId = usr.id;
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
            }

            return traffId;
        }

        public static List<Contacto> getAnunciantes(string nombre)
        {
            List<Contacto> Anunciantes = new List<Contacto>();
            Contacto Anunciante = null;

            string where = "";
            if (nombre == "")
            {
                where = "type = :type";
            }
            else
            {
                where = "type = :type and name like '%" + nombre + "%'";
            }

            //AdManagerUser user = new AdManagerUser();
            using (CompanyService companyService = user.GetService<CompanyService>())
            {
                // Create a statement to select companies.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()

                           .Where(where)
                           .OrderBy("name ASC")
                           .Limit(pageSize)
                           .AddValue("type", CompanyType.ADVERTISER.ToString());

                // Retrieve a small amount of companies at a time, paging through until all
                // companies have been retrieved.
                int totalResultSetSize = 0;

                try
                {
                    do
                    {
                        CompanyPage page =
                            companyService.getCompaniesByStatement(statementBuilder.ToStatement());

                        // Print out some information for each company.
                        if (page.results != null)
                        {
                            totalResultSetSize = page.totalResultSetSize;
                            int i = page.startIndex;
                            foreach (Company company in page.results)
                            {
                                Anunciante = new Contacto();
                                Anunciante.IdContactoDigital = company.id.ToString();
                                Anunciante.Nombre_com = company.name;
                                Anunciantes.Add(Anunciante);
                            }
                        }

                        statementBuilder.IncreaseOffsetBy(pageSize);
                    } while (statementBuilder.GetOffset() < totalResultSetSize);
                }
                catch (AdManagerApiException e)
                {
                    ApiException innerException = e.ApiException as ApiException;
                    string msj = "Ocurrio un error al intentar guardar la Orden en Google Ad Manager: " + innerException.message;
                }

                //do
                //{
                //    CompanyPage page =
                //        companyService.getCompaniesByStatement(statementBuilder.ToStatement());

                //    // Print out some information for each company.
                //    if (page.results != null)
                //    {
                //        totalResultSetSize = page.totalResultSetSize;
                //        int i = page.startIndex;
                //        foreach (Company company in page.results)
                //        {
                //            Anunciante = new Contacto();
                //            Anunciante.IdContactoDigital = company.id.ToString();
                //            Anunciante.RazonSocial = company.name;
                //            Anunciantes.Add(Anunciante);
                //        }
                //    }

                //    statementBuilder.IncreaseOffsetBy(pageSize);
                //} while (statementBuilder.GetOffset() < totalResultSetSize);

            }
    
            return Anunciantes;
        }

        public static Order GetOrderById(long orderId)
        {
            Order result = new Order();
            using (OrderService orderService = user.GetService<OrderService>())
            {
                // Create a statement to select orders.             
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder =
                    new StatementBuilder().Where("id = :oID").OrderBy("id ASC").Limit(pageSize).AddValue("oID", orderId);

                // Retrieve a small amount of orders at a time, paging through until all
                // orders have been retrieved.
                int totalResultSetSize = 0;

                OrderPage page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                // Print out some information for each order.
                if (page.results != null)
                {
                    totalResultSetSize = page.totalResultSetSize;
                    int i = page.startIndex;
                    foreach (Order order in page.results)
                    {
                        Debug.WriteLine("{0}) Order with ID {1} and name \"{2}\" was found.",
                            i++, order.id, order.name);
                        result = order;
                    }
                } else
                {
                    result = null;
                }
                statementBuilder.IncreaseOffsetBy(pageSize);
            }
            return result;
        }

        public static List<LineItem> getLineItemsByOrder(long idOrder)
        {
            List<LineItem> Lineas = new List<LineItem>();

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Create a statement to select placements.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                   //.Where("OrderId = :oID and isArchived = false").OrderBy("id ASC")
                   .Where("OrderId = :oID and isArchived = false").OrderBy("id ASC")
                   .Limit(pageSize)
                   .AddValue("oID", idOrder);

                // Retrieve a small amount of placements at a time, paging through until all
                // placements have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    LineItemPage page =
                        lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    // Print out some information for each placement.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {
                            Lineas.Add(lineItem);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

            }
            return Lineas;
        }

        public static List<LineItem> FilterLineItems(List<Parametro> parametros, string listaOP, int idRed)
        {
            List<LineItem> Lineas = new List<LineItem>();

            //string mifiltro = "isArchived = false";
            string mifiltro = "1 = 1";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if ((p.ParameterName == "idDet"))
                        mifiltro = mifiltro + " and Id = " + p.Value;
                    if ((p.ParameterName == "descripcionDet"))
                        mifiltro = mifiltro + " and name like '%" + p.Value + "%'";
                    if (p.ParameterName == "fecha_desde")
                        mifiltro = mifiltro + " and startDateTime >= '" + p.Value.ToString() + "'";
                    if (p.ParameterName == "fecha_hasta")
                        mifiltro = mifiltro + " and endDateTime <= '" + p.Value.ToString() + "'";
                }
            }
            mifiltro = mifiltro + " and OrderId IN (" + listaOP + ")";

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Create a statement to select placements.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                   .Where(mifiltro).OrderBy("CreationDateTime DESC")
                   .Limit(pageSize);
                   //.AddValue("oID");

                // Retrieve a small amount of placements at a time, paging through until all
                // placements have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    LineItemPage page =
                        lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    // Print out some information for each placement.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {
                            lineItem.externalId = idRed.ToString(); //uso este campo para guardar el idRed por practicidad
                            Lineas.Add(lineItem);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

            }
            return Lineas;
        }

        public static Order GetOrder(long orderId)
        {
            using (OrderService orderService = user.GetService<OrderService>())
            {
                // Create a statement to select orders.
                Order result = new Order();
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder =
                    new StatementBuilder().Where("id = :oID").OrderBy("id ASC").Limit(pageSize).AddValue("oID", orderId);

                // Retrieve a small amount of orders at a time, paging through until all
                // orders have been retrieved.
                int totalResultSetSize = 0;

                OrderPage page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                // Print out some information for each order.
                if (page.results != null)
                {
                    totalResultSetSize = page.totalResultSetSize;
                    int i = page.startIndex;
                    foreach (Order order in page.results)
                    {
                        Debug.WriteLine("{0}) Order with ID {1} and name \"{2}\" was found.",
                            i++, order.id, order.name);

                    }
                }
                else
                {
                    return null;
                }
                statementBuilder.IncreaseOffsetBy(pageSize);
                return page.results[0];
            }
        }

        public static string GetOrderDetails(long orderId, string anunciante = null, List<string> sitios = null)
        {
            //test
            //CambiarRed("22474715684");
            //orderId = 2934737245;

            //AdManagerUser user = new AdManagerUser();
            Order camp = GetOrder(orderId);
            string result = @"<div class='breakBefore'><div class='divImgCert add-mb-6'></div><p style='font-family: Roboto'><span style='font-weight: bold;'>Orden Publicitaria: </span>" + camp.name + "</p><p class='add-mb-6' style='font-family: Roboto'><span style='font-weight: bold;'>Anunciante: </span ><span id='txtAnunciante'>" + anunciante + "</span></p>";
            result = result + "<table id='detailsTable' class='table add-mb-6' style='font-family: Roboto'>";
            //result = result + " <thead><tr><th class='certTableHeader'> Detalle </th> <th class='sitio certTableHeader'> Sitio </th> <th class='fecha certTableHeader'> Fecha </th> <th class='pautado certTableHeader headerObjetivo'> Objetivo </th><th class='impreso certTableHeader'> Impresiones </th><th class='certTableHeader'> Clicks </th><th class='ctr pautado certTableHeader'> CTR </th> <th class='ctr impreso certTableHeader'> CTR </th><th class='certTableHeader' width='1px'></th>";
            result = result + " <thead><tr><th class='certTableHeader'> Detalle </th> <th class='sitio certTableHeader'> Sitio </th> <th class='fecha certTableHeader'> Fecha </th> <th class='pautado certTableHeader headerObjetivo'> Objetivo </th><th class='impreso certTableHeader'> Impresiones </th><th class='certTableHeader'> Clicks </th> <th class='ctr impreso certTableHeader'> CTR </th><th class='certTableHeader' width='1px'></th>";
            result = result + "</tr></thead><tbody>";
            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Create a statement to select line items.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder2 =
                    new StatementBuilder().OrderBy("id ASC").Limit(pageSize);
                StatementBuilder statementBuilder = new StatementBuilder()
                  .Where("OrderId = :oID and isArchived = false").OrderBy("id ASC")
                  .Limit(pageSize)
                  .AddValue("oID", orderId);
                // Retrieve a small amount of line items at a time, paging through until all
                // line items have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    LineItemPage page =
                        lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    // Print out some information for each line item.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        float totalImpresionesPautadas = 0;
                        float totalImpresionesEmitidas = 0;
                        float totalClicks = 0;
                        //long totalCTRPautado = 0;
                        float totalCTRImpreso = 0;
                        int cont = 0;
                        foreach (LineItem lineItem in page.results)
                        {
                            Debug.WriteLine("{0}) Line item with ID {1} and name \"{2}\" was found.", i++, lineItem.id, lineItem.name);
   
                            result += "<tr>";
                            result += "<td>" + lineItem.name + "</td>";
                            if (sitios != null)
                            {
                                result += "<td class='sitio' id='txtSitio'>" + sitios[cont] + "</td>";
                            }
                            
                            result += "<td class='fecha'>" + DateTimeUtilities.ToString(lineItem.startDateTime, "dd/MM/yyyy") + " - " + DateTimeUtilities.ToString(lineItem.endDateTime, "dd/MM/yyyy") + "</td>";
                            
                            if (lineItem.lineItemType != LineItemType.SPONSORSHIP)
                            {
                                totalImpresionesPautadas += lineItem.primaryGoal.units;
                                result += "<td class='pautado'>" + lineItem.primaryGoal.units.ToString() + "</td>";
                            }
                            else
                            {
                                result += "<td class='pautado'> </td>";
                            }

                            if (lineItem.stats != null)
                            {
                                totalImpresionesEmitidas += lineItem.stats.impressionsDelivered;
                                totalClicks += lineItem.stats.clicksDelivered;
                                result += "<td class='impreso'>" + lineItem.stats.impressionsDelivered.ToString() + "</td>";
                                result += "<td>" + lineItem.stats.clicksDelivered.ToString() + "</td>";
                                //if (lineItem.primaryGoal.units != 0)
                                //{
                                //    result += "<td class='ctr pautado'>" + (float.Parse(lineItem.stats.clicksDelivered.ToString()) / float.Parse(lineItem.primaryGoal.units.ToString())) * 100 + "%</td>";
                                //}
                                //else
                                //{
                                //    result += "<td class='ctr pautado'>" + "0" + "%</td>";
                                //}
                                //float ctr = (float.Parse(lineItem.stats.clicksDelivered.ToString()) / float.Parse(lineItem.stats.impressionsDelivered.ToString())) * 100;
                                if (lineItem.stats.impressionsDelivered != 0)
                                {
                                    float ctr = (float.Parse(lineItem.stats.clicksDelivered.ToString()) / float.Parse(lineItem.stats.impressionsDelivered.ToString())) * 100;
                                    result += "<td class='ctr impreso'>" + Math.Round(ctr, 2) + "%</td>";
                                } else
                                {
                                    result += "<td class='ctr impreso'>" + "0" + "%</td>";
                                }
                            } else
                            {
                                result += "<td class='impreso'>" + "0" + "</td>";
                                result += "<td>" + "0" + "</td>";
                                //result += "<td class='ctr pautado'>" + "0" + "%</td>";
                                result += "<td class='ctr impreso'>" + "0" + "%</td>";
                            }
                            result += "<td></td></tr>";
                            cont++;
                        }
                        //if (totalImpresionesPautadas != 0)
                        //{
                        //    totalCTRPautado = totalClicks / totalImpresionesPautadas;
                        //}
                        if (totalImpresionesEmitidas != 0)
                        {
                            totalCTRImpreso = (totalClicks / totalImpresionesEmitidas) * 100;
                        }
                        result += "<tr>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' colspan='3' class='totales colspan2'> Totales </td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales pautado'>" + totalImpresionesPautadas.ToString() + "</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales impreso'>" + totalImpresionesEmitidas.ToString() + "</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales' >" + totalClicks.ToString() + "</td>";
                        //result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='ctr pautado totales'>" + totalCTRPautado + "%</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='ctr impreso totales'>" + Math.Round(totalCTRImpreso, 2).ToString() + "%</td>";
                        result += "<td class='totales'></td></tr>";
                    }


                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);
                result = result + "</tbody></table></div>";
                Debug.WriteLine("Number of results found: {0}", totalResultSetSize);

                return result.Replace("\r", string.Empty).Replace("\n", string.Empty);
            }
        }

        public static Parametro CreateOrder(String name, long advertiserId)
        {
            Parametro resultado = new Parametro();
            long result = -1;
            string msj = "";

            //AdManagerUser user = new AdManagerUser();
            using (OrderService orderService = user.GetService<OrderService>())
            {
                Order[] orders = new Order[1];
                // Create an array to store local order objects.                              
                Order order = new Order();
                order.name = name;//string.Format("Order #{0}", i);
                order.advertiserId = advertiserId;//advertiserId - 4747697929;
                order.traffickerId = GetUserActual();
                //order.traffickerId = long.Parse(ConfigurationManager.AppSettings["traffickerId"]);// traffickerId  - User;

                Debug.WriteLine("user client id:" + user.Config.OAuth2ClientId.ToString());
                orders[0] = order;

                try
                {
                    // Create the orders on the server.
                    orders = orderService.createOrders(orders);

                    if (orders != null)
                    {
                        foreach (Order op in orders)
                        {
                            Debug.WriteLine(
                                "An order with ID ='{0}' and named '{1}' was created.", op.id,
                                op.name);
                            result = op.id;
                            msj = "La Orden en Google Ad Manager se ha guardado con éxito con el ID: " + op.id;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No orders created.");
                    }
                }
                catch (AdManagerApiException e)
                {
                    ApiException innerException = e.ApiException as ApiException;
                    if (innerException.message.Contains("UniqueError.NOT_UNIQUE"))
                    {
                        msj = "Ya existe un Pedido con la misma Descripción en la Red seleccionada";
                    }
                    else
                    {
                        msj = "Ocurrio un error al intentar guardar la Orden en Google Ad Manager: " + innerException.message;
                    }
                }
            }
            resultado.ParameterName = msj;
            resultado.Value = result.ToString();

            return resultado;
        }

        public static Parametro UpdateOrder(String name, long advertiserId, long orderId)
        {
            Parametro resultado = new Parametro();
            long result = -1;
            string msj = "";

            using (OrderService orderService = user.GetService<OrderService>())
            {
                // Create a statement to get the order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("id = :id")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("id", orderId);

                try
                {
                    // Get orders by statement.
                    OrderPage page =
                        orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    Order order = page.results[0];

                    // Update the order object.
                    order.name = name;
                    order.advertiserId = advertiserId;

                    // Update the orders on the server.
                    Order[] orders = orderService.updateOrders(new Order[]
                    {
                        order
                    });

                    if (orders != null)
                    {
                        foreach (Order updatedOrder in orders)
                        {
                            Console.WriteLine(
                                "Order with ID = '{0}', name = '{1}', advertiser ID = '{2}' " +
                                "was updated.", updatedOrder.id,
                                updatedOrder.name, updatedOrder.advertiserId);

                            result = updatedOrder.id;
                            msj = "La Orden en Google Ad Manager se ha guardado con éxito con el ID: " + updatedOrder.id;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No orders updated.");
                    }
                }
                catch (AdManagerApiException e)
                {
                    ApiException innerException = e.ApiException as ApiException;
                    msj = "Ocurrio un error al intentar guardar la Orden en Google Ad Manager: " + innerException.message;
                }
            }
            resultado.ParameterName = msj;
            resultado.Value = result.ToString();

            return resultado;
        }

        public static Parametro CreateLineItems(int tipoAviso, String name, long orderId, double cost, long units, double discount, System.DateTime? fechaDesde, System.DateTime? fechaHasta, List<Dg_orden_pub_medidas> medidas, Dg_areas_geo areaGeo, List<Dg_orden_pub_emplazamientos> emplazamientos, int tipoTarifa)
        {
            Parametro resultado = new Parametro();
            long result = -1;
            string msj = "";
            string rootId = "";

            //AdManagerUser user = new AdManagerUser();

            using (NetworkService networkService = user.GetService<NetworkService>())
            {
                    Network network = networkService.getCurrentNetwork();
                    rootId = network.effectiveRootAdUnitId;
            }

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Target only video platforms
                RequestPlatformTargeting requestPlatformTargeting = new RequestPlatformTargeting()
                {
                    targetedRequestPlatforms = new RequestPlatform[] {
                    RequestPlatform.VIDEO_PLAYER
                    }
                };

                // Create inventory targeting.
                InventoryTargeting inventoryTargeting = new InventoryTargeting();

                if (emplazamientos.Count != 0 && emplazamientos != null)
                {
                    //Emplazamientos (Placeholders)
                    List<long> listEmp = new List<long>();

                    foreach (Dg_orden_pub_emplazamientos elem in emplazamientos)
                    {
                        long idEmplaza = elem.Codigo_emplazamiento;       
                        listEmp.Add(idEmplaza);
                    }
                    long[] listEmpLong = listEmp.ToArray();
                    inventoryTargeting.targetedPlacementIds = listEmpLong;
                }

                else
                {
                    //Bloques de anuncio (Ad Units) - En toda la red
                    AdUnitTargeting adUnitTargeting = new AdUnitTargeting();
                    adUnitTargeting.adUnitId = rootId;
                    adUnitTargeting.includeDescendants = true;
                    AdUnitTargeting[] targetPlacementIds = new AdUnitTargeting[]
                    {
                        adUnitTargeting
                    };
                    inventoryTargeting.targetedAdUnits = targetPlacementIds;
                }              

                //// Create geographical targeting.
                GeoTargeting geoTargeting = new GeoTargeting();

                if (areaGeo.Tipo > 0)
                {
                    Location locacion = new Location();
                    locacion.id = areaGeo.Codigo_area;
        
                    geoTargeting.targetedLocations = new Location[]
                    {
                        locacion
                    };
                }

                //// Exclude domains that are not under the network's control.
                //UserDomainTargeting userDomainTargeting = new UserDomainTargeting();
                //userDomainTargeting.domains = new String[]
                //{
                //    "usa.gov"
                //};
                //userDomainTargeting.targeted = false;

                // Create day-part targeting.
                //DayPartTargeting dayPartTargeting = new DayPartTargeting();
                //dayPartTargeting.timeZone = DeliveryTimeZone.BROWSER;

                //// Target only the weekend in the browser's timezone.
                //DayPart saturdayDayPart = new DayPart();
                //saturdayDayPart.dayOfWeek = Google.Api.Ads.AdManager.v202005.DayOfWeek.SATURDAY;

                //saturdayDayPart.startTime = new TimeOfDay();
                //saturdayDayPart.startTime.hour = 0;
                //saturdayDayPart.startTime.minute = MinuteOfHour.ZERO;

                //saturdayDayPart.endTime = new TimeOfDay();
                //saturdayDayPart.endTime.hour = 24;
                //saturdayDayPart.endTime.minute = MinuteOfHour.ZERO;

                //DayPart sundayDayPart = new DayPart();
                //sundayDayPart.dayOfWeek = Google.Api.Ads.AdManager.v202005.DayOfWeek.SUNDAY;

                //sundayDayPart.startTime = new TimeOfDay();
                //sundayDayPart.startTime.hour = 0;
                //sundayDayPart.startTime.minute = MinuteOfHour.ZERO;

                //sundayDayPart.endTime = new TimeOfDay();
                //sundayDayPart.endTime.hour = 24;
                //sundayDayPart.endTime.minute = MinuteOfHour.ZERO;

                //dayPartTargeting.dayParts = new DayPart[]
                //{
                //    saturdayDayPart,
                //    sundayDayPart
                //};


                // Create technology targeting.
                //TechnologyTargeting technologyTargeting = new TechnologyTargeting();

                //// Create browser targeting.
                //BrowserTargeting browserTargeting = new BrowserTargeting();
                //browserTargeting.isTargeted = true;

                //// Target just the Chrome browser.
                //Technology browserTechnology = new Technology();
                //browserTechnology.id = 500072L;
                //browserTargeting.browsers = new Technology[]
                //{
                //    browserTechnology
                //};
                //technologyTargeting.browserTargeting = browserTargeting;

                // Create an array to store local line item objects.
                LineItem[] lineItems = new LineItem[1];
                LineItem lineItem = new LineItem();

                lineItem.name = name;
                lineItem.orderId = orderId;
                lineItem.targeting = new Targeting();

                lineItem.targeting.inventoryTargeting = inventoryTargeting;
                lineItem.targeting.geoTargeting = geoTargeting;
                if (tipoAviso == 2)
                {
                    lineItem.targeting.requestPlatformTargeting = requestPlatformTargeting;
                    lineItem.environmentType = EnvironmentType.VIDEO_PLAYER;
                    // Set the maximum video creative length for this line item to 10 min.
                    lineItem.videoMaxDuration = 600000L;
                }
                //lineItem.targeting.userDomainTargeting = userDomainTargeting;
                //lineItem.targeting.dayPartTargeting = dayPartTargeting;
                //lineItem.targeting.technologyTargeting = technologyTargeting;

                if (tipoTarifa == 1)
                {
                    lineItem.lineItemType = LineItemType.SPONSORSHIP;
                }
                else
                {
                    lineItem.lineItemType = LineItemType.STANDARD;
                }
                
                //lineItem.allowOverbook = true;

                // Set the creative rotation type to even.
                lineItem.creativeRotationType = CreativeRotationType.EVEN;

                // Tamaños de creatividades

                if (medidas.Count() > 0)
                {
                    List<CreativePlaceholder> listCPH = new List<CreativePlaceholder>();

                    foreach (Dg_orden_pub_medidas elem in medidas)
                    {
                        Size size = new Size();

                        size.width = elem.Ancho;
                        size.height = elem.Alto;
                        size.isAspectRatio = false;

                        CreativePlaceholder creativePlaceholder = new CreativePlaceholder();
                        creativePlaceholder.size = size;

                        listCPH.Add(creativePlaceholder);                   
                    }

                    CreativePlaceholder[] creativePlaces = listCPH.ToArray();
                    lineItem.creativePlaceholders = creativePlaces;
                }

                else
                {
                    Size size = new Size();
                    size.width = 300;
                    size.height = 250;
                    size.isAspectRatio = false;

                    CreativePlaceholder creativePlaceholder = new CreativePlaceholder();
                    creativePlaceholder.size = size;

                    lineItem.creativePlaceholders = new CreativePlaceholder[]
                    {
                            creativePlaceholder
                    };
                }


                // Vigencia
                //si fechadesde es hoy: lineItem.startDateTimeType = StartDateTimeType.IMMEDIATELY;
                //System.DateTime fecha = System.DateTime.Now.Date;
                if (fechaDesde == System.DateTime.Now.Date)
                {
                    lineItem.startDateTimeType = StartDateTimeType.IMMEDIATELY;
                }
                else
                {
                    lineItem.startDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires");
                }
                string fechaHStg = fechaHasta.ToString();
                string[] arrFechaH = fechaHStg.Split(" ");
                System.DateTime fechaHastaFormat = System.DateTime.Parse(arrFechaH[0] + " 23:59:59");
                lineItem.endDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaHastaFormat, "America/Argentina/Buenos_Aires");

                // Costos
                if (tipoTarifa == 0)
                {
                    lineItem.costType = CostType.CPM;
                }
                else if(tipoTarifa == 1)
                {
                    lineItem.costType = CostType.CPD;
                }
                else if (tipoTarifa == 3)
                {
                    lineItem.costType = CostType.CPC;
                }

                lineItem.costPerUnit = new Money();
                lineItem.costPerUnit.currencyCode = "ARS";
                lineItem.costPerUnit.microAmount = (long)(cost * 1000000);
                lineItem.discountType = LineItemDiscountType.PERCENTAGE;
                lineItem.discount = discount;

                // Cantidad de impresiones objetivo
                Goal goal = new Goal();
                
                if (tipoTarifa == 0)
                {
                    goal.goalType = GoalType.LIFETIME;
                    goal.unitType = UnitType.IMPRESSIONS;
                    goal.units = units;
                }
                if (tipoTarifa == 1)
                {
                    goal.goalType = GoalType.DAILY;
                    goal.unitType = UnitType.IMPRESSIONS;
                    goal.units = 100;
                }
                if (tipoTarifa == 3)
                {
                    goal.goalType = GoalType.LIFETIME;
                    goal.unitType = UnitType.CLICKS;
                    goal.units = units;
                }
                
                lineItem.primaryGoal = goal;

                lineItems[0] = lineItem;

                try
                {
                    // Create the line items on the server.
                    lineItems = lineItemService.createLineItems(lineItems);

                    if (lineItems != null)
                    {
                        foreach (LineItem li in lineItems)
                        {
                            Console.WriteLine(
                                "A line item with ID \"{0}\", belonging to order ID \"{1}\", and" +
                                " named \"{2}\" was created.", li.id, li.orderId,
                                li.name);

                            result = li.id;
                            msj = "La Línea de pedido en Google Ad Manager se ha guardado con éxito con el ID: " + li.id;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No line items created.");
                    }

                }
                catch (AdManagerApiException e)
                {
                    ApiException innerException = e.ApiException as ApiException;
                    if (innerException.message.Contains("UniqueError.NOT_UNIQUE"))
                    {
                        msj = "Ya existe una Línea de Pedido con la misma Descripción para el Pedido seleccionado";
                    }                   
                    else if (innerException.message.Contains("LineItemError.FRACTIONAL_PERCENTAGE_NOT_ALLOWED"))
                    {
                        msj = "El AdServer no permite un porcentaje de Descuento fraccionario";
                    }
                    else
                    {
                        msj = "Ocurrio un error al intentar guardar la Línea de pedido en Google Ad Manager: " + innerException.message;
                    }
                }
            }
            resultado.ParameterName = msj;
            resultado.Value = result.ToString();
          
            return resultado;
        }

        public static Dg_orden_pub_as UpdateLineItem(string tipoAviso, String name, double cost, long units, double discount, System.DateTime? fechaDesde, System.DateTime? fechaHasta, List<Dg_orden_pub_medidas> medidas, Dg_areas_geo areaGeo, List<Dg_orden_pub_emplazamientos> emplazamientos, int tipoTarifa, long Id, int paramSinc)
        {
            Dg_orden_pub_as resultado = new Dg_orden_pub_as();
            string rootId = "";

            using (NetworkService networkService = user.GetService<NetworkService>())
            {
                Network network = networkService.getCurrentNetwork();
                rootId = network.effectiveRootAdUnitId;
            }

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Set the ID of the line item.
                long lineItemId = Id;

                // Create a statement to get the line item.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("id = :lineItemId")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("lineItemId", lineItemId);

                // Target only video platforms
                RequestPlatformTargeting requestPlatformTargeting = new RequestPlatformTargeting()
                {
                    targetedRequestPlatforms = new RequestPlatform[] {
                        RequestPlatform.VIDEO_PLAYER
                    }
                };

                // Create inventory targeting.
                InventoryTargeting inventoryTargeting = new InventoryTargeting();

                if (emplazamientos.Count != 0 && emplazamientos != null)
                {
                    //Emplazamientos (Placeholders)
                    List<long> listEmp = new List<long>();

                    foreach (Dg_orden_pub_emplazamientos elem in emplazamientos)
                    {
                        long idEmplaza = elem.Codigo_emplazamiento;
                        listEmp.Add(idEmplaza);
                    }
                    long[] listEmpLong = listEmp.ToArray();
                    inventoryTargeting.targetedPlacementIds = listEmpLong;
                }

                else
                {
                    //Bloques de anuncio (Ad Units) - En toda la red
                    AdUnitTargeting adUnitTargeting = new AdUnitTargeting();
                    adUnitTargeting.adUnitId = rootId;
                    adUnitTargeting.includeDescendants = true;
                    AdUnitTargeting[] targetPlacementIds = new AdUnitTargeting[]
                    {
                        adUnitTargeting
                    };
                    inventoryTargeting.targetedAdUnits = targetPlacementIds;
                }

                // Create geographical targeting.
                GeoTargeting geoTargeting = new GeoTargeting();

                if (areaGeo.Tipo > 0)
                {
                    Location locacion = new Location();
                    locacion.id = areaGeo.Codigo_area;

                    geoTargeting.targetedLocations = new Location[]
                    {
                        locacion
                    };
                }

                try
                {
                    // Get line items by statement.
                    LineItemPage page =
                        lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    LineItem lineItem = page.results[0];

                    // Update line item object.
                    lineItem.name = name;
                    lineItem.targeting = new Targeting();
                    lineItem.targeting.inventoryTargeting = inventoryTargeting;
                    lineItem.targeting.geoTargeting = geoTargeting;
                    if (tipoAviso == "Video")
                    {
                        lineItem.targeting.requestPlatformTargeting = requestPlatformTargeting;
                        lineItem.environmentType = EnvironmentType.VIDEO_PLAYER;
                        // Set the maximum video creative length for this line item to 10 min.
                        lineItem.videoMaxDuration = 600000L;
                    }

                    if (tipoTarifa == 1)
                    {
                        lineItem.lineItemType = LineItemType.SPONSORSHIP;
                    }
                    else
                    {
                        lineItem.lineItemType = LineItemType.STANDARD;
                    }

                    // Tamaños de creatividades
                    if (medidas.Count() > 0)
                    {
                        List<CreativePlaceholder> listCPH = new List<CreativePlaceholder>();

                        foreach (Dg_orden_pub_medidas elem in medidas)
                        {
                            Size size = new Size();

                            size.width = elem.Ancho;
                            size.height = elem.Alto;
                            size.isAspectRatio = false;

                            CreativePlaceholder creativePlaceholder = new CreativePlaceholder();
                            creativePlaceholder.size = size;

                            listCPH.Add(creativePlaceholder);
                        }

                        CreativePlaceholder[] creativePlaces = listCPH.ToArray();
                        lineItem.creativePlaceholders = creativePlaces;
                    }

                    else
                    {
                        Size size = new Size();
                        size.width = 300;
                        size.height = 250;
                        size.isAspectRatio = false;

                        CreativePlaceholder creativePlaceholder = new CreativePlaceholder();
                        creativePlaceholder.size = size;

                        lineItem.creativePlaceholders = new CreativePlaceholder[]
                        {
                            creativePlaceholder
                        };
                    }

                    // Vigencia 
                    Google.Api.Ads.AdManager.v202302.DateTime desde = DateTimeUtilities.FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires");
                    if (lineItem.startDateTime.date.day != desde.date.day || lineItem.startDateTime.date.month != desde.date.month || lineItem.startDateTime.date.year != desde.date.year)
                    {
                        if (fechaDesde <= System.DateTime.Now.Date)
                        {
                            lineItem.startDateTimeType = StartDateTimeType.IMMEDIATELY;
                        }
                        else
                        {
                            lineItem.startDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires");
                        }
                    }
                    Google.Api.Ads.AdManager.v202302.DateTime hasta = DateTimeUtilities.FromDateTime((System.DateTime)fechaHasta, "America/Argentina/Buenos_Aires");
                    if (lineItem.endDateTime.date.day != hasta.date.day || lineItem.endDateTime.date.month != hasta.date.month || lineItem.endDateTime.date.year != hasta.date.year)
                    {
                        string fechaHStg = fechaHasta.ToString();
                        string[] arrFechaH = fechaHStg.Split(" ");
                        System.DateTime fechaHastaFormat = System.DateTime.Parse(arrFechaH[0] + " 23:59:59");
                        lineItem.endDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaHastaFormat, "America/Argentina/Buenos_Aires");
                    }


                    // Costos
                    int divD = 1;
                    if (tipoTarifa == 0)
                    {
                        lineItem.costType = CostType.CPM;
                        divD = 1000;
                    }
                    else if (tipoTarifa == 1)
                    {
                        lineItem.costType = CostType.CPD;
                    }
                    else if (tipoTarifa == 3)
                    {
                        lineItem.costType = CostType.CPC;
                    }

                    lineItem.costPerUnit = new Money();
                    lineItem.costPerUnit.currencyCode = "ARS";
                    lineItem.costPerUnit.microAmount = (long)(cost * 1000000);
                    lineItem.discountType = LineItemDiscountType.PERCENTAGE;
                    lineItem.discount = discount;

                    double brutoD = cost * (units / divD);
                    double total = brutoD - (brutoD * discount / 100);
                    lineItem.budget.microAmount = (long)(total * 1000000);

                    // Cantidad de impresiones objetivo
                    Goal goal = new Goal();

                    if (tipoTarifa == 0)
                    {
                        goal.goalType = GoalType.LIFETIME;
                        goal.unitType = UnitType.IMPRESSIONS;
                        goal.units = units;
                    }
                    if (tipoTarifa == 1)
                    {
                        goal.goalType = GoalType.DAILY;
                        goal.unitType = UnitType.IMPRESSIONS;
                        goal.units = 100;
                    }
                    if (tipoTarifa == 3)
                    {
                        goal.goalType = GoalType.LIFETIME;
                        goal.unitType = UnitType.CLICKS;
                        goal.units = units;
                    }

                    lineItem.primaryGoal = goal;

                    //si el param sinc auto con adserver esta desactivado, se pausa la linea en el caso de estar activa
                    if (paramSinc == 0 && lineItem.status == ComputedStatus.DELIVERING)
                    {
                        //// Create action.
                        PauseLineItems action = new PauseLineItems();

                        //// Perform action.
                        UpdateResult uResult =
                            lineItemService.performLineItemAction(action, statementBuilder.ToStatement());
                    }

                    // Update the line item on the server.
                    LineItem[] lineItems = lineItemService.updateLineItems(new LineItem[]
                    {
                        lineItem
                    });

                    if (lineItems != null)
                    {
                        foreach (LineItem updatedLineItem in lineItems)
                        {
                            Console.WriteLine(
                                "A line item with ID = '{0}', belonging to order ID = '{1}', " +
                                "named '{2}', and having delivery rate = '{3}' was updated.",
                                updatedLineItem.id, updatedLineItem.orderId, updatedLineItem.name,
                                updatedLineItem.deliveryRateType);

                            resultado.Id_Google_Ad_Manager = updatedLineItem.id ;
                            resultado.UsuarioSesion = updatedLineItem.status.ToString(); //se guarda el estado
                            //resultado.UsuarioSesion = "La Línea de pedido en Google Ad Manager se ha actualizado con éxito con el ID: " + updatedLineItem.id;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No line items updated.");
                    }
                }

                catch (AdManagerApiException e)
                {
                    ApiException innerException = e.ApiException as ApiException;
                    resultado.Id_Google_Ad_Manager = -1;
                    if (innerException.message.Contains("UniqueError.NOT_UNIQUE"))
                    {
                        resultado.UsuarioSesion = "Ya existe una Línea de Pedido con la misma Descripción para el Pedido seleccionado";
                    }
                    else if (innerException.message.Contains("LineItemError.FRACTIONAL_PERCENTAGE_NOT_ALLOWED"))
                    {
                        resultado.UsuarioSesion = "El AdServer no permite un porcentaje de Descuento fraccionario";
                    }
                    else
                    {
                        resultado.UsuarioSesion = "Ocurrio un error al intentar guardar la Línea de pedido en Google Ad Manager: " + innerException.message;
                    }
                }
            }

            return resultado;
        }

        public static List<long> GetLineItemCreatives(long lineItemId)
        {
            List<long> lista = new List<long>();
            //AdManagerUser user = new AdManagerUser();
            using (LineItemCreativeAssociationService lineItemCreativeAssociationService = user.GetService<LineItemCreativeAssociationService>())
            {
                // Create a statement to select line item creative associations.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("lineItemId = :lineItemId").OrderBy("lineItemId ASC, creativeId ASC")
                    .Limit(pageSize)
                    .AddValue("lineItemId", lineItemId);

                // Retrieve a small amount of line item creative associations at a time, paging
                // through until all line item creative associations have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    LineItemCreativeAssociationPage page =
                        lineItemCreativeAssociationService
                            .getLineItemCreativeAssociationsByStatement(
                                statementBuilder.ToStatement());

                    // Print out some information for each line item creative association.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (LineItemCreativeAssociation lica in page.results)
                        {
                            if (lica.creativeSetId != 0)
                            {
                                lista.Add(lica.creativeId);
                                Console.WriteLine(
                                    "{0}) Line item creative association with line item ID {1} " +
                                    "and creative set ID {2} was found.", i++, lica.lineItemId,
                                    lica.creativeSetId);
                            }
                            else
                            {
                                lista.Add(lica.creativeId);
                                Console.WriteLine(
                                    "{0}) Line item creative association with line item ID {1} " +
                                    "and creative ID {2} was found.", i++, lica.lineItemId,
                                    lica.creativeId);
                            }
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);
                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
                return lista;
            }
        }
        //AGREGUE:
        //public static void RunAdExchangeReport()
        //{
        //    //if (user == null)
        //    //{
        //    //    user = new AdManagerUser();
        //    //}
        //    using (ReportService reportService = user.GetService<ReportService>())
        //    {
        //        // Set the file path where the report will be saved.
        //        String filePath = (@"C:\Users\linov\Desktop\reporteAE.xlsx");

        //        // Create report query.
        //        ReportQuery reportQuery = new ReportQuery();
        //        reportQuery.dimensions = new Dimension[]
        //        {
        //            Dimension.AD_EXCHANGE_DATE,
        //            Dimension.AD_EXCHANGE_COUNTRY_NAME
        //        };
        //        reportQuery.columns = new Column[]
        //        {
        //            Column.AD_EXCHANGE_AD_REQUESTS,
        //            Column.AD_EXCHANGE_IMPRESSIONS,
        //            Column.AD_EXCHANGE_ESTIMATED_REVENUE
        //        };

        //        reportQuery.dateRangeType = DateRangeType.LAST_WEEK;

        //        // Run in pacific time.
        //        reportQuery.timeZoneType = TimeZoneType.AD_EXCHANGE;
        //        //reportQuery.adxReportCurrency = "ARS";
        //        reportQuery.adxReportCurrency = null;

        //        // Create report job.
        //        ReportJob reportJob = new ReportJob();
        //        reportJob.reportQuery = reportQuery;

        //        try
        //        {
        //            // Run report.
        //            reportJob = reportService.runReportJob(reportJob);

        //            ReportUtilities reportUtilities =
        //                new ReportUtilities(reportService, reportJob.id);

        //            // Set download options.
        //            ReportDownloadOptions options = new ReportDownloadOptions();
        //            options.exportFormat = ExportFormat.XLSX;
        //            options.useGzipCompression = false;
        //            reportUtilities.reportDownloadOptions = options;

        //            // Download the report.
        //            using (ReportResponse reportResponse = reportUtilities.GetResponse())
        //            {
        //                reportResponse.Save(filePath);
        //            }

        //            Console.WriteLine("Report saved to \"{0}\".", filePath);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Failed to run Ad Exchange report. Exception says \"{0}\"",
        //                e.Message);
        //        }
        //    }
        //}

        public static string printCertExcel(long orderId, System.DateTime? fechaDesde, System.DateTime? fechaHasta)
        {

            //CambiarRed("5491998");
            //CambiarRed("22474715684");
            //orderId = 2934737245;

            using (ReportService reportService = user.GetService<ReportService>())
            {
                // Create report job.
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[]
                {
                    Dimension.ORDER_ID,
                    Dimension.ORDER_NAME,
                    Dimension.LINE_ITEM_ID,
                    Dimension.LINE_ITEM_NAME,
                    Dimension.DATE
                };
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[]
                {
                    //DimensionAttribute.ORDER_TRAFFICKER,
                    //DimensionAttribute.ORDER_START_DATE_TIME,
                    //DimensionAttribute.ORDER_END_DATE_TIME
                };
                reportJob.reportQuery.columns = new Column[]
                {
                    Column.AD_SERVER_IMPRESSIONS,
                    Column.AD_SERVER_CLICKS,
                    Column.AD_SERVER_CTR
                    //Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                    //Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM
                };

                // Set a custom date range
                reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                if (fechaDesde > System.DateTime.Now.Date)
                {
                    fechaDesde = System.DateTime.Now.Date;
                }
                if (fechaHasta > System.DateTime.Now.Date)
                {
                    fechaHasta = System.DateTime.Now.Date;
                }
                reportJob.reportQuery.startDate = DateTimeUtilities
                    .FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires").date;
                reportJob.reportQuery.endDate = DateTimeUtilities
                    .FromDateTime((System.DateTime)fechaHasta, "America/Argentina/Buenos_Aires").date;

                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder().Where("ORDER_ID = :id")
                    .AddValue("id", orderId);
                reportJob.reportQuery.statement = statementBuilder.ToStatement();

                try
                {
                    // Run report job.
                    reportJob = reportService.runReportJob(reportJob);

                    ReportUtilities reportUtilities =
                        new ReportUtilities(reportService, reportJob.id);
                  
                    // Set download options.
                    ReportDownloadOptions options = new ReportDownloadOptions();
                    options.exportFormat = ExportFormat.XLSX;
                    options.useGzipCompression = false;
                    reportUtilities.reportDownloadOptions = options;

                    //Esperamos a que el reporte esté listo
                    ReportJobStatus estadoRep = ReportJobStatus.FAILED;
                    while(estadoRep != ReportJobStatus.COMPLETED)
                    {
                        estadoRep = reportService.getReportJobStatus(reportJob.id);
                    }

                    //Obtenemos la URL del archivo
                    string linkReporte = reportService.getReportDownloadUrlWithOptions(reportJob.id, reportUtilities.reportDownloadOptions);

                    return linkReporte;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to run delivery report. Exception says \"{0}\"",
                        e.Message);
                    return "";
                }
            }
        }

        public static string printLineItemCSV(long lineId, System.DateTime? fechaDesde, System.DateTime? fechaHasta)
        {
            using (ReportService reportService = user.GetService<ReportService>())
            {
                string fecha = System.DateTime.Now.ToString();
                string fechaFormat1 = fecha.Replace("/", "-");
                string fechaFormat2 = fechaFormat1.Replace(":", "");
                Random random = new Random();
                string num = random.Next(1000).ToString();

                // Set the file path where the report will be saved.
                string nombreArch = fechaFormat2 + "_" + num + ".csv";
                var folder = System.IO.Directory.GetCurrentDirectory();
                var parent = System.IO.Directory.GetParent(folder);
                var uploads = Path.Combine(parent.FullName, Startup.StaticConfig.GetSection("AppSettings").GetSection("FrontendFolder").Value, "files");
                String filePath = Path.Combine(uploads, nombreArch);

                // Create report job.
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[]
                {
                    //Dimension.ORDER_ID,
                    //Dimension.ORDER_NAME,
                    //Dimension.LINE_ITEM_ID,
                    //Dimension.LINE_ITEM_NAME,
                    Dimension.DATE
                };
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[]
                {
                    //DimensionAttribute.ORDER_TRAFFICKER,
                    //DimensionAttribute.ORDER_START_DATE_TIME,
                    //DimensionAttribute.ORDER_END_DATE_TIME
                };
                reportJob.reportQuery.columns = new Column[]
                {
                    Column.AD_SERVER_IMPRESSIONS,
                    Column.AD_SERVER_CLICKS,
                    Column.AD_SERVER_CTR
                    //Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                    //Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM
                };

                // Set a custom date range
                reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                if (fechaDesde > System.DateTime.Now.Date)
                {
                    fechaDesde = System.DateTime.Now.Date;
                }
                if (fechaHasta > System.DateTime.Now.Date)
                {
                    fechaHasta = System.DateTime.Now.Date;
                }
                reportJob.reportQuery.startDate = DateTimeUtilities
                    .FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires").date;
                reportJob.reportQuery.endDate = DateTimeUtilities
                    .FromDateTime((System.DateTime)fechaHasta, "America/Argentina/Buenos_Aires").date;

                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder().Where("LINE_ITEM_ID = :id")
                    .AddValue("id", lineId);
                reportJob.reportQuery.statement = statementBuilder.ToStatement();

                try
                {
                    // Run report job.
                    reportJob = reportService.runReportJob(reportJob);

                    ReportUtilities reportUtilities =
                        new ReportUtilities(reportService, reportJob.id);

                    // Set download options.
                    ReportDownloadOptions options = new ReportDownloadOptions();
                    options.exportFormat = ExportFormat.CSV_DUMP;
                    options.useGzipCompression = false;
                    reportUtilities.reportDownloadOptions = options;

                    // Download the report.
                    using (ReportResponse reportResponse = reportUtilities.GetResponse())
                    {
                        reportResponse.Save(filePath);
                    }

                    Console.WriteLine("Report saved to \"{0}\".", filePath);
                    return filePath;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to run delivery report. Exception says \"{0}\"",
                        e.Message);
                    return "";
                }
            }
        }

        public static List<Dg_emplazamientos> getEmplazamientos()
        {
            List<Dg_emplazamientos> Emplazamientos = new List<Dg_emplazamientos>();
            Dg_emplazamientos Emplazamiento = null;

            //AdManagerUser user = CambiarRed(redGAM);
            using (PlacementService placementService = user.GetService<PlacementService>())
            {
                // Create a statement to select placements.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("status = :status")
                    .OrderBy("id ASC")
                    .Limit(pageSize)
                    .AddValue("status", InventoryStatus.ACTIVE.ToString());

                // Retrieve a small amount of placements at a time, paging through until all
                // placements have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    PlacementPage page = placementService.getPlacementsByStatement(statementBuilder.ToStatement());

                    // Print out some information for each placement.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (Placement placement in page.results)
                        {
                            Emplazamiento = new Dg_emplazamientos();
                            Emplazamiento.Descripcion = placement.name;
                            Emplazamiento.Codigo_emplazamiento = placement.id;
                            Emplazamientos.Add(Emplazamiento);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

            }
            return Emplazamientos;
        }


        //GET TAMAÑOS BANNER
        public static List<Dg_medidas> GetMedidas()
        {

            List<Dg_medidas> Tamaños = new List<Dg_medidas>();
            Dg_medidas Tamaño = null;

            //AdManagerUser user = new AdManagerUser();

            using (InventoryService inventoryService = user.GetService<InventoryService>())
            {
                // Create a statement to select ad unit sizes.
                StatementBuilder statementBuilder = new StatementBuilder();
                //.OrderBy("size.height ASC");

                AdUnitSize[] adUnitSizes = inventoryService.getAdUnitSizesByStatement(statementBuilder.ToStatement());

                // Print out some information for each ad unit size.
                int i = 0;
                foreach (AdUnitSize adUnitSize in adUnitSizes)
                {
                    //Controla que sea banner
                    if(adUnitSize.environmentType.ToString() == "BROWSER" && adUnitSize.isAudio == false && adUnitSize.size.isAspectRatio == false && adUnitSize.fullDisplayString != null && adUnitSize.size.height != 0)
                    {
                        Tamaño = new Dg_medidas();
                        Tamaño.Alto = adUnitSize.size.height;
                        Tamaño.Ancho = adUnitSize.size.width;
                        Tamaño.Descripcion = adUnitSize.fullDisplayString;
                        Tamaño.Tipo = 1;
                        Tamaños.Add(Tamaño);
                        Console.WriteLine("{0}) Ad unit size with dimensions \"{1}\" was found.", i++, adUnitSize.fullDisplayString);
                    }
                }

                Console.WriteLine("Number of results found: {0}", adUnitSizes.Length);
            }
            List<Dg_medidas> TamOrdenados = Tamaños.OrderBy(tamaño => tamaño.Ancho).ThenBy(tamaño => tamaño.Alto).ToList();
            return TamOrdenados;
        }

        public static List<Dg_medidas> GetMedidasTodasRedes(List<Parametro> redes)
        {
            List<Dg_medidas> Tamaños = new List<Dg_medidas>();

            foreach (Parametro red in redes)
            {
                CambiarRed(red.Value);

                Dg_medidas Tamaño = null;

                using (InventoryService inventoryService = user.GetService<InventoryService>())
                {
                    // Create a statement to select ad unit sizes.
                    StatementBuilder statementBuilder = new StatementBuilder();
                    //.OrderBy("size.height ASC");

                    AdUnitSize[] adUnitSizes = inventoryService.getAdUnitSizesByStatement(statementBuilder.ToStatement());

                    // Print out some information for each ad unit size.
                    int i = 0;
                    foreach (AdUnitSize adUnitSize in adUnitSizes)
                    {
                        //Controla que sea banner
                        if (adUnitSize.environmentType.ToString() == "BROWSER" && adUnitSize.isAudio == false && adUnitSize.size.isAspectRatio == false && adUnitSize.fullDisplayString != null && adUnitSize.size.height != 0)
                        {
                            Tamaño = new Dg_medidas();
                   
                            Tamaño.Descripcion = adUnitSize.fullDisplayString;
                            
                            Console.WriteLine("{0}) Ad unit size with dimensions \"{1}\" was found.", i++, adUnitSize.fullDisplayString);

                            bool existe = false;
                            foreach (Dg_medidas tam in Tamaños)
                            {
                                if (Tamaño.Descripcion == tam.Descripcion)
                                {
                                    existe = true;                           
                                }
                            }
                            if (existe == false)
                            {
                                Tamaño.Alto = adUnitSize.size.height;
                                Tamaño.Ancho = adUnitSize.size.width;
                                Tamaño.Tipo = 1;
                                Tamaños.Add(Tamaño);
                            }
                        }
                    }
                    Console.WriteLine("Number of results found: {0}", adUnitSizes.Length);
                }
            }
            List<Dg_medidas> TamOrdenados = Tamaños.OrderBy(tamaño => tamaño.Ancho).ThenBy(tamaño => tamaño.Alto).ToList();
            return TamOrdenados;
        }

        public static List<Dg_medidas> GetMedidasVideoTodasRedes(List<Parametro> redes)
        {
            List<Dg_medidas> Tamaños = new List<Dg_medidas>();

            foreach (Parametro red in redes)
            {
                CambiarRed(red.Value);

                Dg_medidas Tamaño = null;

                using (InventoryService inventoryService = user.GetService<InventoryService>())
                {
                    // Create a statement to select ad unit sizes.
                    StatementBuilder statementBuilder = new StatementBuilder();
                    //.OrderBy("size.height ASC");

                    AdUnitSize[] adUnitSizes = inventoryService.getAdUnitSizesByStatement(statementBuilder.ToStatement());

                    // Print out some information for each ad unit size.
                    int i = 0;
                    foreach (AdUnitSize adUnitSize in adUnitSizes)
                    {
                        //Controla que sea video
                        if (adUnitSize.environmentType.ToString() == "VIDEO_PLAYER" && adUnitSize.isAudio == false && adUnitSize.size.isAspectRatio == false && adUnitSize.fullDisplayString != null && adUnitSize.size.height != 0)
                        {
                            Tamaño = new Dg_medidas();

                            Tamaño.Descripcion = adUnitSize.fullDisplayString;

                            Console.WriteLine("{0}) Ad unit size with dimensions \"{1}\" was found.", i++, adUnitSize.fullDisplayString);

                            bool existe = false;
                            foreach (Dg_medidas tam in Tamaños)
                            {
                                if (Tamaño.Descripcion == tam.Descripcion)
                                {
                                    existe = true;
                                }
                            }
                            if (existe == false)
                            {
                                Tamaño.Alto = adUnitSize.size.height;
                                Tamaño.Ancho = adUnitSize.size.width;
                                Tamaño.Tipo = 2;
                                Tamaños.Add(Tamaño);
                            }
                        }
                    }
                    Console.WriteLine("Number of results found: {0}", adUnitSizes.Length);
                }
            }
            List<Dg_medidas> TamOrdenados = Tamaños.OrderBy(tamaño => tamaño.Ancho).ThenBy(tamaño => tamaño.Alto).ToList();
            return TamOrdenados;
        }

        //REPORTE INVENTARIO (error en el archivo que descarga)
        public static void ReporteInventario()
        {
            //if (user == null)
            //{
            //    user = new AdManagerUser();
            //}

                using (ReportService reportService = user.GetService<ReportService>())
                {
                    // Set the file path where the report will be saved.
                    String filePath = @"C:\Users\devagustin\Desktop\ReporteInv.xml";

                    // Create report query.
                    ReportQuery reportQuery = new ReportQuery();
                    reportQuery.dimensions = new Dimension[]
                    {
                    Dimension.AD_UNIT_ID,
                    Dimension.AD_UNIT_NAME
                    };
                    reportQuery.columns = new Column[]
                    {
                    Column.AD_SERVER_IMPRESSIONS,
                    Column.AD_SERVER_CLICKS,
                    Column.ADSENSE_LINE_ITEM_LEVEL_IMPRESSIONS,
                    Column.ADSENSE_LINE_ITEM_LEVEL_CLICKS,
                    Column.TOTAL_LINE_ITEM_LEVEL_IMPRESSIONS,
                    Column.TOTAL_LINE_ITEM_LEVEL_CPM_AND_CPC_REVENUE
                    };

                    reportQuery.adUnitView = ReportQueryAdUnitView.HIERARCHICAL;
                    reportQuery.dateRangeType = DateRangeType.LAST_WEEK;

                    // Create report job.
                    ReportJob reportJob = new ReportJob();
                    reportJob.reportQuery = reportQuery;

                    try
                    {
                        // Run report.
                        reportJob = reportService.runReportJob(reportJob);

                        ReportUtilities reportUtilities =
                            new ReportUtilities(reportService, reportJob.id);

                        // Set download options.
                        ReportDownloadOptions options = new ReportDownloadOptions();
                        options.exportFormat = ExportFormat.XML;
                        options.useGzipCompression = true;
                        reportUtilities.reportDownloadOptions = options;

                        // Download the report.
                        using (ReportResponse reportResponse = reportUtilities.GetResponse())
                        {
                            reportResponse.Save(filePath);
                        }

                        Console.WriteLine("Report saved to \"{0}\".", filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to run inventory report. Exception says \"{0}\"",
                            e.Message);
                    }
                }
        }

        public static int ArchivarLineItem(long Id)
        {
            int result = -1;
            //AdManagerUser user = new AdManagerUser();

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Set the ID of the line item.
                long lineItemId = Id;

                // Create a statement to get the line item.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("id = :lineItemId")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("lineItemId", lineItemId);

                try
                {
                    // Get line items by statement.
                    LineItemPage page =
                        lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    LineItem lineItem = page.results[0];

                    ////si el param sinc auto con adserver esta desactivado, se pausa la linea en el caso de estar activa
                    //if (paramSinc == 0 && lineItem.status == ComputedStatus.DELIVERING)
                    //{
                    //    //// Create action.
                    //    PauseLineItems action = new PauseLineItems();

                    //    //// Perform action.
                    //    UpdateResult uResult =
                    //        lineItemService.performLineItemAction(action, statementBuilder.ToStatement());
                        
                    //    result = 1;
                    //}
                    //else
                    //{
                        //// Create action.
                        ArchiveLineItems action = new ArchiveLineItems();

                        //// Perform action.
                        UpdateResult uResult =
                            lineItemService.performLineItemAction(action,statementBuilder.ToStatement());

                        result = 2;
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to archive line items. Exception says \"{0}\"",
                        e.Message);
                }
            }
            return result;
        }

        public static List<Order> FilterOrders(List<Parametro> parametros)
        {
            List<Order> OrdenesGAM = new List<Order>();

            string where = "isArchived = false";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if (p.ParameterName == "descripcionOp")
                        where = where + " and name like '%" + p.Value + "%'";
                    if (p.ParameterName == "idOp")
                        where = where + " and id = " + p.Value;
                    if (p.ParameterName == "Id_contacto")     
                        where = where + " and advertiserId = " + p.Value;
                    //if (p.ParameterName == "fecha_desde")
                    //    where = where + " and lastModifiedDateTime >= '" + p.Value.ToString() + "'";
                    //if (p.ParameterName == "fecha_hasta")
                    //    where = where + " and lastModifiedDateTime <= '" + p.Value.ToString() + "'";
                }
            }          

            using (OrderService orderService = user.GetService<OrderService>())
            {
                // Create a statement to select orders.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where(where)
                    .OrderBy("lastModifiedDateTime DESC")
                    .Limit(pageSize)
                    .AddValue("now", DateTimeUtilities.FromDateTime(System.DateTime.Now, "America/Argentina/Buenos_Aires"));
                // Retrieve a small amount of orders at a time, paging through until all
                // orders have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    OrderPage page =
                        orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    // Print out some information for each order.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (Order order in page.results)
                        {
                            Console.WriteLine("{0}) Order with ID {1} and name \"{2}\" was found.",
                                i++, order.id, order.name);
                            OrdenesGAM.Add(order);
                        }
                    }
                    
                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
            }
            return OrdenesGAM;
        }

        public static string GetAnunciantePorId(long anunId)
        {
            string result = "";

            using (CompanyService companyService = user.GetService<CompanyService>())
            {
                // Create a statement to select companies.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("type = :type and id = :aID")
                    .OrderBy("id ASC")
                    .Limit(pageSize)
                    .AddValue("type", CompanyType.ADVERTISER.ToString())
                    .AddValue("aID", anunId);

                // Retrieve a small amount of companies at a time, paging through until all
                // companies have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    CompanyPage page =
                        companyService.getCompaniesByStatement(statementBuilder.ToStatement());

                    // Print out some information for each company.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (Company company in page.results)
                        {
                            Console.WriteLine(
                                "{0}) Company with ID {1}, name \"{2}\", and type \"{3}\" was " +
                                "found.",
                                i++, company.id, company.name, company.type);
                            result = company.name;
                        }
                    }
                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
            }

            return result;
        }

        public static LineItem GetLineItemById(long lineaId)
        {
            LineItem result = new LineItem();
            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Create a statement to get the line item.
                StatementBuilder statementBuilder = new StatementBuilder()
                    //.Where("id = :lineItemId and isArchived = false")
                    .Where("id = :lineItemId")
                    .OrderBy("id ASC")
                    .Limit(1)
                    .AddValue("lineItemId", lineaId);

                // Get line items by statement.
                LineItemPage page =
                    lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                
                if (page.results != null)
                {
                    result = page.results[0];
                }
            }
            return result;
        }

        //public static List<long> GetOrderByName(string nombre)
        //{
        //    List<long> result = new List<long>();
        //    using (OrderService orderService = user.GetService<OrderService>())
        //    {
        //        // Create a statement to select orders.             
        //        int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
        //        StatementBuilder statementBuilder =
        //            new StatementBuilder().Where("name like '%" + nombre + "%'").OrderBy("id ASC").Limit(pageSize);

        //        // Retrieve a small amount of orders at a time, paging through until all
        //        // orders have been retrieved.
        //        int totalResultSetSize = 0;

        //        OrderPage page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

        //        // Print out some information for each order.
        //        if (page.results != null)
        //        {
        //            totalResultSetSize = page.totalResultSetSize;
        //            int i = page.startIndex;
        //            foreach (Order order in page.results)
        //            {
        //                Debug.WriteLine("{0}) Order with ID {1} and name \"{2}\" was found.",
        //                    i++, order.id, order.name);
        //                result.Add(order.id);
        //            }
        //        }
        //        else
        //        {
        //            result = null;
        //        }
        //        statementBuilder.IncreaseOffsetBy(pageSize);
        //    }
        //    return result;
        //}

        public static string GetIdOrderByFilters(List<Parametro> parametros)
        {
            string result = "";

            //string where = "isArchived = false";
            string where = "1 = 1";

            foreach (Parametro p in parametros)
            {
                if (p.Value.ToString() != "")
                {
                    if (p.ParameterName == "descripcionOp")
                        where = where + " and name like '%" + p.Value + "%'";
                    if (p.ParameterName == "idOp")
                        where = where + " and id = " + p.Value;
                    if (p.ParameterName == "Id_contacto")
                        where = where + " and advertiserId = " + p.Value;
                }
            }

            using (OrderService orderService = user.GetService<OrderService>())
            {
                    // Create a statement to select orders.
                    int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                    StatementBuilder statementBuilder = new StatementBuilder()
                        .Where(where)
                        .OrderBy("lastModifiedDateTime DESC")
                        .Limit(pageSize)
                        .AddValue("now", DateTimeUtilities.FromDateTime(System.DateTime.Now, "America/Argentina/Buenos_Aires"));

                //    // Create a statement to select orders.             
                //    int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                //StatementBuilder statementBuilder =
                //    new StatementBuilder().Where("name like '%" + nombre + "%'").OrderBy("id ASC").Limit(pageSize);

                // Retrieve a small amount of orders at a time, paging through until all
                // orders have been retrieved.
                int totalResultSetSize = 0;

                OrderPage page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                // Print out some information for each order.
                if (page.results != null)
                {
                    totalResultSetSize = page.totalResultSetSize;
                    int i = page.startIndex;
                    foreach (Order order in page.results)
                    {
                        Debug.WriteLine("{0}) Order with ID {1} and name \"{2}\" was found.",
                            i++, order.id, order.name);
                        if (i == 1)
                        {
                            result += order.id.ToString();
                        }
                        else
                        {
                            result+=", " + order.id.ToString();
                        }
                    }
                }
                statementBuilder.IncreaseOffsetBy(pageSize);
            }
            return result;
        }


    }
}
