﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using Google.Api.Ads.AdManager.Lib;
using Google.Api.Ads.AdManager.Util.v202105;
using Google.Api.Ads.AdManager.v202105;
using Google.Api.Ads.Common.Util.Reports;
using System.Diagnostics;
using System.Configuration;


namespace WebApi.Helpers
{
    public class GoogleAdManager
    {

        public static AdManagerUser CambiarRed(string netCode)
        {
            AdManagerUser user = new AdManagerUser();
            AdManagerAppConfig config = (AdManagerAppConfig)user.Config;
            config.NetworkCode = netCode;
            return user;
        }

        public static List<Contacto> getAnunciantes(string desc)
        {
            List<Contacto> Anunciantes = new List<Contacto>();
            Contacto Anunciante = null;

            string where = "";
            if (desc == "a")
            {
                where = "type = :type";
            }
            else
            {
                where = "type = :type and name like '%" + desc + "%'";
            }

            AdManagerUser user = new AdManagerUser();
            using (CompanyService companyService = user.GetService<CompanyService>())
            {
                // Create a statement to select companies.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()

                           .Where(where)
                           .OrderBy("id ASC")
                           .Limit(pageSize)
                           .AddValue("type", CompanyType.ADVERTISER.ToString());

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
                            Anunciante = new Contacto();
                            Anunciante.IdContactoDigital = company.id.ToString();
                            Anunciante.RazonSocial = company.name;
                            Anunciantes.Add(Anunciante);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

            }

    
            return Anunciantes;
        }

        public static Order GetOrder(AdManagerUser user, long orderId)
        {
            if (user == null)
            {
                user = new AdManagerUser();
            }

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
                } else
                {
                    return null;
                }
                statementBuilder.IncreaseOffsetBy(pageSize);
                return page.results[0];
            }
        }

        public static string GetOrderDetails(long orderId, string anunciante = null, List<string> sitios = null)
        {
   
            AdManagerUser user = new AdManagerUser();
            Order camp = GetOrder(user, orderId);
            string result = @"<div class='breakBefore'><div class='divImgCert add-mb-6'></div><p><span style='font-weight: bold;'>Orden Publicitaria: </span>" + camp.name + "</p><p class='add-mb-6'><span style='font-weight: bold;'>Anunciante: </span ><span id='txtAnunciante'>" + anunciante + "</span></p>";
            result = result + "<table id='detailsTable' class='table add-mb-6'>";
            result = result + " <thead><tr><th class='certTableHeader'> Campaña </th> <th class='sitio certTableHeader'> Sitio </th> <th class='pautado certTableHeader headerObjetivo'> Objetivo </th><th class='impreso certTableHeader'> Impresiones </th><th class='certTableHeader'> Clicks </th><th class='ctr pautado certTableHeader'> CTR </th> <th class='ctr impreso certTableHeader'> CTR </th><th class='certTableHeader' width='1px'></th>";
            result = result + "</tr></thead><tbody>";
            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
                // Create a statement to select line items.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder2 =
                    new StatementBuilder().OrderBy("id ASC").Limit(pageSize);
                StatementBuilder statementBuilder = new StatementBuilder()
                  .Where("OrderId = :oID").OrderBy("id ASC")
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
                        long totalImpresionesPautadas = 0;
                        long totalImpresionesEmitidas = 0;
                        long totalClicks = 0;
                        long totalCTRPautado = 0;
                        long totalCTRImpreso = 0;
                        int cont = 0;
                        foreach (LineItem lineItem in page.results)
                        {
                            Debug.WriteLine("{0}) Line item with ID {1} and name \"{2}\" was found.", i++, lineItem.id, lineItem.name);
                            totalImpresionesPautadas += lineItem.primaryGoal.units;
                            result += "<tr>";
                            result += "<td>" + lineItem.name + "</td>";
                            if (sitios != null)
                            {
                                result += "<td class='sitio' id='txtSitio'>" + sitios[cont] + "</td>";
                            }
                            result += "<td class='pautado'>" + lineItem.primaryGoal.units.ToString() + "</td>";
                            if (lineItem.stats != null)
                            {
                                totalImpresionesEmitidas += lineItem.stats.impressionsDelivered;
                                totalClicks += lineItem.stats.clicksDelivered;
                                result += "<td class='impreso'>" + lineItem.stats.impressionsDelivered.ToString() + "</td>";
                                result += "<td>" + lineItem.stats.clicksDelivered.ToString() + "</td>";
                                if (lineItem.primaryGoal.units != 0)
                                {
                                    result += "<td class='ctr pautado'>" + lineItem.stats.clicksDelivered / lineItem.primaryGoal.units + "%</td>";
                                }
                                else
                                {
                                    result += "<td class='ctr pautado'>" + "0" + "%</td>";
                                }
                                if (lineItem.stats.impressionsDelivered != 0)
                                {
                                    result += "<td class='ctr impreso'>" + lineItem.stats.clicksDelivered / lineItem.stats.impressionsDelivered + "%</td>";
                                } else
                                {
                                    result += "<td class='ctr impreso'>" + "0" + "%</td>";
                                }
                            } else
                            {
                                result += "<td class='impreso'>" + "0" + "</td>";
                                result += "<td>" + "0" + "</td>";
                                result += "<td class='ctr pautado'>" + "0" + "%</td>";
                                result += "<td class='ctr impreso'>" + "0" + "%</td>";
                            }
                            result += "<td></td></tr>";
                            cont++;
                        }
                        if (totalImpresionesPautadas != 0)
                        {
                            totalCTRPautado = totalClicks / totalImpresionesPautadas;
                        }
                        if (totalImpresionesEmitidas != 0)
                        {
                            totalCTRImpreso = totalClicks / totalImpresionesEmitidas;
                        }
                        result += "<tr>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' colspan='2' class='totales colspan2'> Totales </td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales pautado'>" + totalImpresionesPautadas.ToString() + "</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales impreso'>" + totalImpresionesEmitidas.ToString() + "</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales' >" + totalClicks.ToString() + "</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='ctr pautado totales'>" + totalCTRPautado + "%</td>";
                        result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='ctr impreso totales'>" + totalCTRImpreso + "%</td>";
                        result += "<td class='totales'></td></tr>";
                    }


                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);
                result = result + "</tbody></table></div>";
                Debug.WriteLine("Number of results found: {0}", totalResultSetSize);

                return result.Replace("\r", string.Empty).Replace("\n", string.Empty); ;

            }
        }

        //public static long CreateOrder(String name, long advertiserId)
        //{
        //    long result = -1;
        //    AdManagerUser user = new AdManagerUser();
        //    using (OrderService orderService = user.GetService<OrderService>())
        //    {
        //        Order[] orders = new Order[1];
        //        // Create an array to store local order objects.                              
        //        Order order = new Order();
        //        order.name = name;//string.Format("Order #{0}", i);
        //        order.advertiserId = advertiserId;//advertiserId - 4747697929; 
        //        //order.startDateTime = DateTimeUtilities.FromDateTime(inicio, "America/Buenos_Aires");
        //        //order.endDateTime = DateTimeUtilities.FromDateTime(fin, "America/Buenos_Aires");

        //        order.traffickerId = long.Parse( ConfigurationManager.AppSettings["traffickerId"]);// traffickerId  - User;
        //        //order.traffickerId = 245442393;// traffickerId  - User;
        //        Debug.WriteLine("user client id:" + user.Config.OAuth2ClientId.ToString());
        //        orders[0] = order;

        //        try
        //        {
        //            // Create the orders on the server.
        //            orders = orderService.createOrders(orders);

        //            if (orders != null)
        //            {
        //                foreach (Order op in orders)
        //                {
        //                    Debug.WriteLine(
        //                        "An order with ID ='{0}' and named '{1}' was created.", op.id,
        //                        op.name);
        //                    result = op.id;
        //                }
        //            }
        //            else
        //            {
        //                Debug.WriteLine("No orders created.");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine("Failed to create orders. Exception says \"{0}\"", e.Message);
        //        }
        //    }
        //    return result;
        //}

        //AGREGUE:
        public static long CreateOrder(String name, long advertiserId)
        {
            long result = -1;
            AdManagerUser user = new AdManagerUser();
            using (OrderService orderService = user.GetService<OrderService>())
            {
                Order[] orders = new Order[1];
                // Create an array to store local order objects.                              
                Order order = new Order();
                order.name = name;//string.Format("Order #{0}", i);
                order.advertiserId = advertiserId;//advertiserId - 4747697929;
                order.traffickerId = long.Parse(ConfigurationManager.AppSettings["traffickerId"]);// traffickerId  - User;
                //order.traffickerId = 245442393;// traffickerId  - User;

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
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No orders created.");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Failed to create orders. Exception says \"{0}\"", e.Message);
                }
            }
            return result;
        }
        //AGREGUE:
        public static long CreateLineItems(String name, long orderId, float cost, long units, double discount, System.DateTime? fechaDesde, System.DateTime? fechaHasta, List<Dg_orden_pub_medidas> medidas, Dg_areas_geo areaGeo, List<Dg_orden_pub_emplazamientos> emplazamientos, int tipoTarifa)
        {
            long result = -1;
            string rootId = "";

            AdManagerUser user = new AdManagerUser();

            using (NetworkService networkService = user.GetService<NetworkService>())
            {
                    Network network = networkService.getCurrentNetwork();
                    rootId = network.effectiveRootAdUnitId;
            }

            using (LineItemService lineItemService = user.GetService<LineItemService>())
            {
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
                    adUnitTargeting.includeDescendants= true;
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
                lineItem.startDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires");
                lineItem.endDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaHasta, "America/Argentina/Buenos_Aires");

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
                        }
                    }
                    else
                    {
                        Console.WriteLine("No line items created.");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to create line items. Exception says \"{0}\"", e.Message);
                }
            }
            return result;
        }

        public static List<long> GetLineItemCreatives(long lineItemId)
        {
            List<long> lista = new List<long>();
            AdManagerUser user = new AdManagerUser();
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
        public static void RunAdExchangeReport(AdManagerUser user)
        {
            if (user == null)
            {
                user = new AdManagerUser();
            }
            using (ReportService reportService = user.GetService<ReportService>())
            {
                // Set the file path where the report will be saved.
                String filePath = (@"C:\Users\linov\Desktop");

                // Create report query.
                ReportQuery reportQuery = new ReportQuery();
                reportQuery.dimensions = new Dimension[]
                {
                    Dimension.AD_EXCHANGE_DATE,
                    Dimension.AD_EXCHANGE_COUNTRY_NAME
                };
                reportQuery.columns = new Column[]
                {
                    Column.AD_EXCHANGE_AD_REQUESTS,
                    Column.AD_EXCHANGE_IMPRESSIONS,
                    Column.AD_EXCHANGE_ESTIMATED_REVENUE
                };

                reportQuery.dateRangeType = DateRangeType.LAST_WEEK;

                // Run in pacific time.
                reportQuery.timeZoneType = TimeZoneType.AD_EXCHANGE;
                //reportQuery.adxReportCurrency = "ARS";
                reportQuery.adxReportCurrency = null;

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
                    options.exportFormat = ExportFormat.CSV_DUMP;
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
                    Console.WriteLine("Failed to run Ad Exchange report. Exception says \"{0}\"",
                        e.Message);
                }
            }
        }
        
        public static List<Dg_emplazamientos> getEmplazamientos(string redGAM)
        {
            List<Dg_emplazamientos> Emplazamientos = new List<Dg_emplazamientos>();
            Dg_emplazamientos Emplazamiento = null;

            AdManagerUser user = CambiarRed(redGAM);
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

            AdManagerUser user = new AdManagerUser();

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

        //REPORTE INVENTARIO (error en el archivo que descarga)
        public static void ReporteInventario(AdManagerUser user)
        {
            if (user == null)
            {
                user = new AdManagerUser();
            }

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

        //UPDATE LINE ITEM
        public static long UpdateLineItem(String name, float cost, long units, double discount, System.DateTime? fechaDesde, System.DateTime? fechaHasta, List<Dg_orden_pub_medidas> medidas, Dg_areas_geo areaGeo, List<Dg_orden_pub_emplazamientos> emplazamientos, int tipoTarifa, long Id)
        {
            long result = -1;
            AdManagerUser user = new AdManagerUser();

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
                    adUnitTargeting.adUnitId = "";
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
                    lineItem.startDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaDesde, "America/Argentina/Buenos_Aires");
                    lineItem.endDateTime = DateTimeUtilities.FromDateTime((System.DateTime)fechaHasta, "America/Argentina/Buenos_Aires");

                    // Costos
                    if (tipoTarifa == 0)
                    {
                        lineItem.costType = CostType.CPM;
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

                            result = updatedLineItem.id;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No line items updated.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to update line items. Exception says \"{0}\"",
                        e.Message);
                }
            }
            return result;
        }

        public static long ArchivarLineItem(long Id)
        {
            long result = -1;
            AdManagerUser user = new AdManagerUser();

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
                    //// Create action.
                    ArchiveLineItems action = new ArchiveLineItems();

                    //// Perform action.
                    UpdateResult uResult =
                        lineItemService.performLineItemAction(action,
                            statementBuilder.ToStatement());
                    result = 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to archive line items. Exception says \"{0}\"",
                        e.Message);
                }
            }
            return result;
        }

    }
}