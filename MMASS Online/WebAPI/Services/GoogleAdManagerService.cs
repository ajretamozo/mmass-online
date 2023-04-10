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
using Google.Api.Ads.AdManager.Util.v202208;
using Google.Api.Ads.AdManager.v202208;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebApi.Services
{
    public interface IGoogleAdManagerService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Contacto> GetAnunciantes(Parametro nombre);
        String GetOrderDetails(long idGAM);
        String GetOrderDetails2(Dg_orden_pub_ap orden);
        String printCertExcel(Dg_orden_pub_ap orden);
        Dg_orden_pub_ap GetOrderById(long idGAM);
        //V2
        //Parametro CreateOrder(long idOrden);
        //V3
        Parametro CreateOrder(List<Parametro> parametros);
        //AGREGUE:
        Parametro CreateLineItems(Dg_orden_pub_as det);
        List<long> GetLineItemCreatives(long lineItemId);
        //AGREGUE:
        void RunAdExchangeReport();
        //TEST
        void reporteTest();
        //AGREGUE:
        //IEnumerable<Dg_emplazamientos> GetEmplazamientos(string redGAM);
        IEnumerable<Dg_emplazamientos> GetEmplazamientos(string codRed);
        IEnumerable<Dg_medidas> GetMedidas();
        IEnumerable<Dg_medidas> GetMedidasTodasRedes(List<Parametro> parametros);
        IEnumerable<Dg_medidas> GetMedidasVideoTodasRedes(List<Parametro> parametros);
        long ArchivarLineItems(Dg_orden_pub_ap op);
        void CambiarRed(string netCode);
        long GetRedActual();
        IEnumerable<Dg_orden_pub_ap> GetOpNuevas(List<Parametro> parametros); 
        IEnumerable<Dg_orden_pub_ap> FiltrarPedidos(List<Parametro> parametros);
        //ListaParametro ComprobarModificaciones(Dg_orden_pub_ap order);
        List<ListaParametro> ComprobarModificacionesD(Dg_orden_pub_ap order);
        List<Dg_orden_pub_as> ComprobarModificacionesDSincro(List<Parametro> parametros);
        List<Parametro> obtenerProgresoLineasGam(Dg_orden_pub_ap order);
        IEnumerable<Dg_orden_pub_as> comprobarNuevosDetalles(Dg_orden_pub_ap order);
        IEnumerable<Dg_orden_pub_as> GetDetNuevos(List<Parametro> parametros);
        bool saveMail(Mail mail);
        Mail getMailCta();
        String pruebaMail();
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

        //V2
        //public Parametro CreateOrder(long idOrden)
        //{
        //    Parametro resultado = new Parametro();
        //    Dg_orden_pub_ap op = Dg_orden_pub_ap.getById(Convert.ToInt32(idOrden));

        //    if (op.anunciante.IdContactoDigital == null || DB.DLong(op.anunciante.IdContactoDigital) < 1)
        //    {
        //        resultado.ParameterName = "El Contacto no est� sincronizado con Google Ad Manager. �Desea sincronizarlo?";
        //        resultado.Value = "-2";
        //    }
        //    else
        //    {
        //        if (op.Id_Google_Ad_Manager > 0)
        //        {
        //            resultado = GoogleAdManager.UpdateOrder(op.Bitacora, DB.DLong(op.anunciante.IdContactoDigital), op.Id_Google_Ad_Manager);
        //        }
        //        else
        //        {
        //            resultado = GoogleAdManager.CreateOrder(op.Bitacora, DB.DLong(op.anunciante.IdContactoDigital));
        //            if (long.Parse(resultado.Value) > 1)
        //            {
        //                Dg_orden_pub_ap.saveId_Google_Ad_Manager(idOrden, long.Parse(resultado.Value));
        //            }
        //        }
        //    }

        //    return resultado;
        //}

        //V3
        public Parametro CreateOrder(List<Parametro> parametros)
        {
            Parametro resultado = new Parametro();

            Contacto anun = new Contacto();
            anun = Contacto.getContactoByIdyRed(int.Parse(parametros[2].Value), int.Parse(parametros[0].Value));

            resultado = GoogleAdManager.CreateOrder(parametros[1].Value, DB.DLong(anun.IdContactoDigital));
           
            return resultado;
        }

        public Parametro CreateLineItems(Dg_orden_pub_as det)
        {
            Parametro resultado = new Parametro();

            //se obtiene el param sinc auto para ver el grado de sincronizaci�n con adserver
            int paramSinc = int.Parse(Dg_parametro.getById(2).Valor);

            if (paramSinc == 0 || paramSinc == 4)
            {
                resultado.ParameterName = "La Creaci�n/Modificaci�n de L�neas de Pedido est� desactivada. Si es Administrador, puede activarla desde el Panel de Configuraci�n";
                resultado.Value = "-4";
                return resultado;
            }

            else
            {
                if (det.Tipo_tarifa != 0 && det.Tipo_tarifa != 1 && det.Tipo_tarifa != 3)
                {
                    resultado.ParameterName = "La Forma de Uso debe ser CPM, CPD o CPC";
                    resultado.Value = "-3";
                    return resultado;
                }
                else
                {
                    int paramEnviarMail = int.Parse(Dg_parametro.getById(3).Valor);
                    if (det.Id_Google_Ad_Manager > 0)
                    {
                        if (paramSinc == 2 || paramSinc == 6)
                        {
                            resultado.ParameterName = "La Modificaci�n de L�neas de Pedido est� desactivada. Si es Administrador, puede activarla desde el Panel de Configuraci�n";
                            resultado.Value = "-5";
                            return resultado;
                        }
                        else
                        {
                            Dg_red_GAM red = Dg_red_GAM.getById(det.Id_red);
                            CambiarRed(red.Codigo_red.ToString());
                            Dg_orden_pub_as res = GoogleAdManager.UpdateLineItem(det.tipo_aviso_dg.Descripcion, det.Descripcion, det.Importe_unitario, det.Cantidad, det.Porc_dto, det.Fecha_desde, det.Fecha_hasta, det.Medidas, det.areaGeo, det.Emplazamientos, det.Tipo_tarifa, det.Id_Google_Ad_Manager, paramSinc);
                            resultado.Value = res.Id_Google_Ad_Manager.ToString(); //se devuelve el idGam
                            if (res.Id_Google_Ad_Manager > 0)
                            {
                                resultado.ParameterName = "La L�nea de pedido en Google Ad Manager se ha actualizado con �xito con el ID: " + res.Id_Google_Ad_Manager;

                                //enviar notificaci�n por mail
                                if (paramEnviarMail == 1 || paramEnviarMail == 3 || paramEnviarMail == 5 || paramEnviarMail == 7)
                                {
                                    string asunto = "MMASS Online - Modificaci�n de L�nea de Pedido";
                                    string msj = "Se ha modificado la L�nea de Pedido <b>" + det.Descripcion + "</b>, " +
                                                  "ID: <b>" + det.Id_Google_Ad_Manager + "</b> perteneciente al Pedido <b>" +
                                                  det.Nombre_pedido_Google_Ad_Manager + "</b>, ID: <b>" + det.Id_pedido_Google_Ad_Manager + "</b><br>" +
                                                  "N�mero de Orden MMASS: <b>" + det.Anio + "-" + det.Mes + "-" + det.Nro_orden + "</b><br>" +
                                                  "La L�nea de Pedido ha quedado en el estado: <b>" + res.UsuarioSesion + "</b><br>" +
                                                  "Usuario MMASS Online: <b>" + det.UsuarioSesion + "</b><br>" +
                                                  "--------------------------------------------------------------------" +
                                                  "---------------------------------------------------------------<br>" +
                                                  "<font size=1>No responder este mensaje</font><br>" +
                                                  "<H5>Sistema de Notificaciones MMASS Online</H5>";
                                    enviarMail(asunto, msj);
                                }
                            }
                            else
                            {
                                resultado.ParameterName = res.UsuarioSesion; //se devuelve el mensaje de error
                            }
                            return resultado;
                        }                       
                    }
                    else
                    {
                        if (paramSinc == 3 || paramSinc == 7)
                        {
                            resultado.ParameterName = "La Creaci�n de L�neas de Pedido est� desactivada. Si es Administrador, puede activarla desde el Panel de Configuraci�n";
                            resultado.Value = "-5";
                            return resultado;
                        }
                        else
                        {
                            if (det.Fecha_desde < System.DateTime.Now.Date)
                            {
                                resultado.ParameterName = "La Fecha de Inicio del Detalle no puede estar en el pasado";
                                resultado.Value = "-2";
                                return resultado;
                            }
                            else
                            {
                                Dg_red_GAM red = Dg_red_GAM.getById(det.Id_red);
                                CambiarRed(red.Codigo_red.ToString());
                                resultado = GoogleAdManager.CreateLineItems(det.tipo_aviso_dg.Tipo_aviso_ads, det.Descripcion, det.Id_pedido_Google_Ad_Manager, det.Importe_unitario, det.Cantidad, det.Porc_dto, det.Fecha_desde, det.Fecha_hasta, det.Medidas, det.areaGeo, det.Emplazamientos, det.Tipo_tarifa);
                            }

                            if (long.Parse(resultado.Value) > 0)
                            {
                                Dg_orden_pub_as.saveId_Google_Ad_Manager(det.Id_op_dg, det.Id_detalle, long.Parse(resultado.Value));
                                //grabamos parametro para que OrderList sepa que op pintar de verde
                                Dg_orden_pub_ap.saveId_Google_Ad_Manager(det.Id_op_dg);

                                //enviar notificaci�n por mail
                                if (paramEnviarMail == 1 || paramEnviarMail == 2 || paramEnviarMail == 5 || paramEnviarMail == 6)
                                {
                                    string asunto = "MMASS Online - Creaci�n de L�nea de Pedido";
                                    string msj = @"Se ha creado la L�nea de Pedido <b>" + det.Descripcion + "</b>, " +
                                                  "ID: <b>" + resultado.Value + "</b> perteneciente al Pedido <b>" +
                                                  det.Nombre_pedido_Google_Ad_Manager + "</b>, ID: <b>" + det.Id_pedido_Google_Ad_Manager + "</b><br>" +
                                                  "N�mero de Orden MMASS: <b>" + det.Anio + "-" + det.Mes + "-" + det.Nro_orden + "</b><br>" +
                                                  "La L�nea de Pedido ha quedado en el estado: <b>DRAFT</b><br>" +
                                                  "Usuario MMASS Online: <b>" + det.UsuarioSesion + "</b><br>" +
                                                  "--------------------------------------------------------------------" +
                                                  "---------------------------------------------------------------<br>" +
                                                  "<font size=1>No responder este mensaje</font><br>" +
                                                  "<H5>Sistema de Notificaciones MMASS Online</H5>";
                                    enviarMail(asunto, msj);
                                }
                            }
                            return resultado;
                        }
                    }                    
                }
            }
        }

        //TEST
        public void RunAdExchangeReport()
        {
            //GoogleAdManager.RunAdExchangeReport();
        }
        //TEST
        public void reporteTest()
        {
            //GoogleAdManager.GetTama�os();
            //GoogleAdManager.RunOpReport();
            //procesarReporte();
        }

        public String printCertExcel(Dg_orden_pub_ap orden)
        {
            return GoogleAdManager.printCertExcel(orden.Id_Google_Ad_Manager, orden.Fecha, orden.Fecha_expiracion);
        }

        public String GetOrderDetails2(Dg_orden_pub_ap orden)
        {
            //Datos generales de la Cabecera
            string result = @"<div class='breakBefore'><div class='divImgCert add-mb-6'></div>
                              <p style='font-family: Roboto'><span style='font-weight: bold;'>Orden Publicitaria: </span>" + orden.Bitacora + "</p>" +
                            @"<p style='font-family: Roboto'><span style='font-weight: bold;'>Anunciante: </span ><span id='txtAnunciante'>" + orden.Anunciante_nombre + "</span></p>" +
                            @"<p class='add-mb-6' style='font-family: Roboto'><span style='font-weight: bold;'>Fecha: </span ><span id='txtFecha'>" + FormatFecha(orden.Fecha.ToString()) + " - " + FormatFecha(orden.Fecha_expiracion.ToString()) + "</span></p>";
            
            //Tabla de Detalles
            result = result + "<table id='detailsTable' class='table add-mb-6' style='font-family: Roboto; text-align: center;'>";
            result = result + "<tbody>";

            float totalImpresionesEmitidas = 0;
            float totalClicks = 0;
            float totalCTRImpreso = 0;

            foreach (Dg_orden_pub_as det in orden.Detalles)
            {
                //Cabecera para cada detalle
                result = result + " <thead><tr><th class='certTableHeader'> Detalle </th> <th class='sitio certTableHeader'> Sitio </th> <th class='fecha certTableHeader'> Fecha </th> <th class='pautado certTableHeader headerObjetivo'> Objetivo </th><th class='impreso certTableHeader'> Impresiones </th><th class='clicks certTableHeader'> Clicks </th><th class='ctr impreso certTableHeader'> CTR </th><th class='certTableHeader' width='1px'></th>";
                result = result + "</tr></thead>";

                //Datos generales del Detalle
                result += "<tr style='border: solid thin; border-color: #e7e9eb;'>";
                result += "<td style='font-weight: bold;'>" + det.Descripcion + "</td>";
                if (det.Medios.Count() > 1)
                {
                    result += "<td class='sitio' id='txtSitio' style='font-weight: bold;'>RON</td>";
                }
                else
                {
                    string med = Medio.getById(det.Medios[0].Id_medio).Desc_medio;
                    result += "<td class='sitio' id='txtSitio' style='font-weight: bold;'>" + med + "</td>";
                }

                result += "<td class='fecha' style='font-weight: bold;'>" + FormatFecha(det.Fecha_desde.ToString()) + " - " + FormatFecha(det.Fecha_hasta.ToString()) + "</td>";
                result += "</tr> ";

                float totalImpresionesEmitidasD = 0;
                float totalClicksD = 0;
                float totalCTRImpresoD = 0;

                if (det.Id_Google_Ad_Manager > 0)
                {
                    //apuntamos a la red adserver del detalle
                    Dg_red_GAM red = Dg_red_GAM.getById(det.Id_red);
                    CambiarRed(red.Codigo_red.ToString());

                    //se genera el reporte csv por d�a para este Detalle
                    string ruta = GoogleAdManager.printLineItemCSV(det.Id_Google_Ad_Manager, det.Fecha_desde, det.Fecha_hasta);
                    if ( ruta != "")
                    {
                        ////Datos generales del Detalle
                        //result += "<tr style='border: solid thin; border-color: #e7e9eb;'>";
                        //result += "<td style='font-weight: bold;'>" + det.Descripcion + "</td>";
                        //if (det.Medios.Count() > 1)
                        //{
                        //    result += "<td class='sitio' id='txtSitio' style='font-weight: bold;'>RON</td>";
                        //}
                        //else
                        //{
                        //    string med = Medio.getById(det.Medios[0].Id_medio).Desc_medio;
                        //    result += "<td class='sitio' id='txtSitio' style='font-weight: bold;'>" + med + "</td>";
                        //}

                        //result += "<td class='fecha' style='font-weight: bold;'>" + FormatFecha(det.Fecha_desde.ToString()) + " - " + FormatFecha(det.Fecha_hasta.ToString()) + "</td>";
                        //result += "</tr> ";

                        //extraemos y procesamos los datos del csv
                        string ubicacionArchivo = ruta;
                        System.IO.StreamReader archivo = new System.IO.StreamReader(ubicacionArchivo);
                        string separador = ",";
                        string linea;
                        List<string[]> tabla = new List<string[]>();

                        //pasamos el cvs a una tabla
                        archivo.ReadLine(); // Leer la primera l�nea pero descartarla porque es el encabezado
                        while ((linea = archivo.ReadLine()) != null)
                        {
                            string[] fila = linea.Split(separador);
                            tabla.Add(fila);
                        }
                        archivo.Close();

                        //eliminamos el archivo csv
                        File.Delete(ruta);

                        string cantPorDia = "";
                        if (det.Tipo_tarifa == 0)
                        {
                            TimeSpan difFechas = (TimeSpan)(det.Fecha_hasta - det.Fecha_desde);
                            int dias = difFechas.Days;
                            cantPorDia = (det.Cantidad / dias).ToString();
                        }
                        else
                        {
                            cantPorDia = " - ";                        
                        }

                        //Datos del Detalle por d�a
                        for (int i = 0; i < (tabla.Count); i++)
                        {
                            totalImpresionesEmitidasD += float.Parse(tabla[i][1]);
                            totalClicksD += float.Parse(tabla[i][2]);
                            float ctr = (float.Parse(tabla[i][3])) / 100;

                            result += "<tr style='border-left: solid thin; border-right: solid thin; border-color: #e7e9eb;' class='detalledias'>";
                            result += "<td></td>";
                            result += "<td></td>";
                            result += "<td class='fecha'>" + FormatFecha2(tabla[i][0]) + "</td>";
                            //result += "<td class='pautado'>" + cantPorDia + "</td>";
                            result += "<td class='pautado'></td>";
                            result += "<td class='impreso'>" + tabla[i][1] + "</td>";
                            result += "<td>" + tabla[i][2] + "</td>";
                            result += "<td class='ctr impreso'>" + ctr + "%</td>";
                            result += "<td></td></tr>";
                        }

                        totalCTRImpresoD = (totalClicksD / totalImpresionesEmitidasD) * 100;

                        if (tabla.Count == 0)
                        {
                            totalImpresionesEmitidasD = 0;
                            totalClicksD = 0;
                            totalCTRImpresoD = 0;
                        }
                    }
                }
                result += "<tr style='border-left: solid thin; border-right: solid thin; border-color: #e7e9eb; border-bottom-left-radius: .5rem; border-bottom-right-radius: .5rem;'>";
                result += "<td style='font-weight: bold;background-color:#f7f7f7;' colspan='3' class='totales colspan2'> Totales Detalle </td>";
                if (det.Tipo_tarifa == 0)
                {
                    result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales pautado'>" + det.Cantidad + "</td>";

                }
                else
                {
                    result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales pautado'> - </td>";
                }
                result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales impreso'>" + totalImpresionesEmitidasD.ToString() + "</td>";
                result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='totales clicks' >" + totalClicksD.ToString() + "</td>";
                result += "<td style='font-weight: bold;background-color:#f7f7f7;' class='ctr impreso totales'>" + Math.Round(totalCTRImpresoD, 2).ToString() + "%</td>";
                result += "<td class='totales'></td></tr>";
                result += "<tr><td></td><td></td><td></td><td></td><td></td></tr>"; //separador de Detalles

                totalImpresionesEmitidas += totalImpresionesEmitidasD;
                totalClicks += totalClicksD;
            }

            if (totalImpresionesEmitidas != 0)
            {
                totalCTRImpreso = (totalClicks / totalImpresionesEmitidas) * 100;
            }

            //Totales OP
            result += "<tr>";
            result += "<td style='font-weight: bold;background-color:#f79191a6;' colspan='3'> Totales OP </td>";
            result += "<td style='font-weight: bold;background-color:#f79191a6;' class='pautado'>" + orden.Total_Impresiones.ToString() + "</td>";
            result += "<td style='font-weight: bold;background-color:#f79191a6;' class='impreso'>" + totalImpresionesEmitidas.ToString() + "</td>";
            result += "<td style='font-weight: bold;background-color:#f79191a6;' class='clicks'>" + totalClicks.ToString() + "</td>";
            result += "<td style='font-weight: bold;background-color:#f79191a6;' class='ctr impreso'>" + Math.Round(totalCTRImpreso, 2).ToString() + "%</td>";
            result += "<td style='background-color:#f79191a6;></td>";
            result += "<td style='background-color:#f79191a6;></td></tr>";
            result = result + "</tbody></table></div>";

            result.Replace("\r", string.Empty).Replace("\n", string.Empty);
            result += "<div id='divAdjuntos'></div>";

            return result;
        }

        public List<long> GetLineItemCreatives(long lineItemId)
        {
            return GoogleAdManager.GetLineItemCreatives(lineItemId);
        }


        public IEnumerable<Dg_emplazamientos> GetEmplazamientos(string codRed)
        {
            CambiarRed(codRed);
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

        public IEnumerable<Dg_medidas> GetMedidasVideoTodasRedes(List<Parametro> parametros)
        {
            return GoogleAdManager.GetMedidasVideoTodasRedes(parametros);
        }

        public long ArchivarLineItems(Dg_orden_pub_ap op)
        {
            int result = 0;
            //si obtiene el param sinc auto para ver si se archivan o no las l�neas eliminadas
            int paramSinc = int.Parse(Dg_parametro.getById(2).Valor);
            if(paramSinc == 1 || paramSinc == 4 || paramSinc == 6 || paramSinc == 7)
            {
                foreach (Dg_orden_pub_as det in op.Detalles)
                {
                    Dg_red_GAM red = Dg_red_GAM.getById(det.Id_red);
                    CambiarRed(red.Codigo_red.ToString());
                    result = GoogleAdManager.ArchivarLineItem(det.Id_Google_Ad_Manager);
                    if (result > 0)
                    {
                        //enviar notificaci�n por mail
                        //string estadoLinea = "";
                        //if (result == 1)
                        //{
                        //    estadoLinea = "PAUSED";
                        //}
                        //else
                        //{
                        //    estadoLinea = "ARCHIVED";
                        //}
                        int paramEnviarMail = int.Parse(Dg_parametro.getById(3).Valor);
                        if (paramEnviarMail == 1 || paramEnviarMail == 4 || paramEnviarMail == 6 || paramEnviarMail == 7)
                        {
                            string asunto = "MMASS Online - Eliminaci�n de L�nea de Pedido";
                            string msj = @"Se ha eliminado la L�nea de Pedido <b>" + det.Descripcion + "</b>, " +
                                          "ID: <b>" + det.Id_Google_Ad_Manager + "</b> perteneciente al Pedido <b>" +
                                          op.Bitacora + "</b>, ID: <b>" + op.Id_Google_Ad_Manager + "</b><br>" +
                                          "N�mero de Orden MMASS: <b>" + op.Anio + "-" + op.Mes + "-" + op.Nro_orden + "</b><br>" +
                                          "La L�nea de Pedido ha quedado en el estado: <b>ARCHIVADA</b><br>" +
                                          "Usuario MMASS Online: <b>" + op.UsuarioSesion + "</b><br>" +
                                          "--------------------------------------------------------------------" +
                                          "---------------------------------------------------------------<br>" +
                                          "<font size=1>No responder este mensaje</font><br>" +
                                          "<H5>Sistema de Notificaciones MMASS Online</H5>";
                            enviarMail(asunto, msj);
                        }
                    }
                }
            }         
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

        public IEnumerable<Dg_orden_pub_ap> FiltrarPedidos(List<Parametro> parametros)
        {
            //apuntamos a la red adserver del detalle
            Dg_red_GAM red = Dg_red_GAM.getById(int.Parse(parametros[2].Value));
            CambiarRed(red.Codigo_red.ToString());
            //al idContacto lo convertimos en idContactoDigital
            Contacto anun = new Contacto();
            anun = Contacto.getContactoByIdyRed(int.Parse(parametros[3].Value), int.Parse(parametros[2].Value));
            parametros[3].Value = anun.IdContactoDigital;
            List<Order> ordenesGAM = GoogleAdManager.FilterOrders(parametros);
            List<Dg_orden_pub_ap> ordenesNuevas = new List<Dg_orden_pub_ap>();

            if (ordenesGAM.Count == 0)
            {
                return ordenesNuevas;
            }

            else
            {
                foreach (Order order in ordenesGAM)
                {
                        //Pasamos los datos del Pedido a la OP
                        Dg_orden_pub_ap ordenNueva = new Dg_orden_pub_ap();

                        //ordenNueva.Id_red = idRed;
                        ordenNueva.Id_Google_Ad_Manager = order.id;
                        ordenNueva.Observ = order.name;
                        ordenNueva.Anunciante_nombre = anun.Nombre_com;
                        //anun.IdContactoDigital = order.advertiserId.ToString();
                        //ordenNueva.anunciante = anun;
                        ordenNueva.Seg_neto = (order.totalBudget.microAmount) / 1000000;
                        string creatD = DateTimeUtilities.ToString(order.lastModifiedDateTime, "yyyy/MM/dd");
                        if (creatD != "0")
                        {
                            ordenNueva.Fecha_alta = System.DateTime.Parse(creatD);
                        }
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
                return ordenesNuevas;
            }
        }

        //COMENTAR SI NO SE USA
        public IEnumerable<Dg_orden_pub_ap> GetOpNuevas(List<Parametro> parametros)
        {
            List<Order> ordenesGAM = GoogleAdManager.FilterOrders(parametros);
            List<Dg_orden_pub_ap> ordenesNuevas = new List<Dg_orden_pub_ap>();

            if (ordenesGAM.Count == 0)
            {
                return ordenesNuevas;
            }

            else
            {
                int idRed = 0;
                foreach (Parametro p in parametros)
                {
                    if (p.ParameterName == "red")
                    {
                        idRed = int.Parse(p.Value);
                        break;
                    }
                }
                List<Dg_orden_pub_ap> opExistentes = Dg_orden_pub_ap.getAll2();

                foreach (Order order in ordenesGAM)
                {
                    bool existeOp = false;
                    foreach (Dg_orden_pub_ap OpExis in opExistentes)
                    {
                        //Si encontramos el Pedido dentro de las OP, se sale del for y buscamos el pr�ximo Pedido
                        if (order.id == OpExis.Id_Google_Ad_Manager && idRed == OpExis.Id_red)
                        {
                            existeOp = true;
                            break;
                        }
                    }

                    //Si no se encuentra el Pedido dentro de las OP existentes..
                    if (!existeOp)
                    {
                        //Pasamos los datos del Pedido a la OP
                        Dg_orden_pub_ap ordenNueva = new Dg_orden_pub_ap();
                        Contacto anun = new Contacto();

                        ordenNueva.Id_red = idRed;
                        ordenNueva.Id_Google_Ad_Manager = order.id;
                        ordenNueva.Observ = order.name;
                        ordenNueva.Anunciante_nombre = GoogleAdManager.GetAnunciantePorId(order.advertiserId);
                        anun.IdContactoDigital = order.advertiserId.ToString();
                        ordenNueva.anunciante = anun;
                        ordenNueva.Seg_neto = (order.totalBudget.microAmount) / 1000000;
                        string creatD = DateTimeUtilities.ToString(order.lastModifiedDateTime, "yyyy/MM/dd");
                        if (creatD != "0")
                        {
                            ordenNueva.Fecha_alta = System.DateTime.Parse(creatD);
                        }
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
        }

        //public ListaParametro ComprobarModificaciones(Dg_orden_pub_ap order)
        //{
        //    CambiarRed(order.Codigo_red.ToString());
        //    ListaParametro ordYdet = new ListaParametro();
        //    ordYdet.Parametros = new List<Parametro>();
        //    ordYdet.ListaDetalles = new List<int>();
        //    Order ordenGam = new Order();
        //    ordenGam = GoogleAdManager.GetOrderById(order.Id_Google_Ad_Manager);
        //    Dg_orden_pub_ap orden = new Dg_orden_pub_ap();
        //    orden = Dg_orden_pub_ap.getById(order.Id_op_dg);
        //    List<LineItem> lineasGAM = new List<LineItem>();
        //    lineasGAM = GoogleAdManager.getLineItemsByOrder(ordenGam.id);

        //    //buscar los detalles por orderId

        //    //se buscan diferencias entre la orden gam y la orden ap; si se encuentran, se devuelve la lista de cambios

        //    //if (orden.Bitacora != ordenGam.name)
        //    //{
        //    //    Parametro cambioNom = new Parametro();
        //    //    cambioNom.ParameterName = "Descripci�n";
        //    //    cambioNom.Value = orden.Bitacora + "@@@" + ordenGam.name;
        //    //    ordYdet.Parametros.Add(cambioNom);
        //    //}

        //    //if (System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.startDateTime, "yyyy/MM/dd")) != orden.Fecha)
        //    //{
        //    //    Parametro cambioDesde = new Parametro();
        //    //    cambioDesde.ParameterName = "Vigencia Desde";
        //    //    cambioDesde.Value = DateTimeUtilities.ToString(DateTimeUtilities.FromDateTime((System.DateTime)orden.Fecha, "America/Argentina/Buenos_Aires"), "dd/MM/yyyy") + "@@@" + DateTimeUtilities.ToString(ordenGam.startDateTime, "dd/MM/yyyy");
        //    //    ordYdet.Parametros.Add(cambioDesde);
        //    //}

        //    //if (System.DateTime.Parse(DateTimeUtilities.ToString(ordenGam.endDateTime, "yyyy/MM/dd")) != orden.Fecha_expiracion)
        //    //{
        //    //    Parametro cambioHasta = new Parametro();
        //    //    cambioHasta.ParameterName = "Vigencia Hasta";
        //    //    cambioHasta.Value = DateTimeUtilities.ToString(DateTimeUtilities.FromDateTime((System.DateTime)orden.Fecha_expiracion, "America/Argentina/Buenos_Aires"), "dd/MM/yyyy") + "@@@" + DateTimeUtilities.ToString(ordenGam.endDateTime, "dd/MM/yyyy");
        //    //    ordYdet.Parametros.Add(cambioHasta);
        //    //}

        //    //Se comparan las Lineas de Pedido

        //    foreach (Dg_orden_pub_as detalle in orden.Detalles)
        //    {
        //        if (detalle.Id_pedido_Google_Ad_Manager > 0)
        //        {
        //            CambiarRed(detalle.Codigo_red.ToString());
        //            LineItem linea = GoogleAdManager.GetLineItemById(detalle.Id_pedido_Google_Ad_Manager);
        //            if (linea != null)
        //            {

        //            }
        //            else { }
        //        }
        //        foreach (LineItem linea in lineasGAM)
        //        {
        //            if (linea.id == detalle.Id_Google_Ad_Manager)
        //            {
        //                bool difTipoTar = false;
        //                bool difEmplaza = false;

        //                switch (detalle.Tipo_tarifa)
        //                {
        //                    case 0:
        //                        if (linea.costType != CostType.CPM)
        //                        {
        //                            difTipoTar = true;
        //                        }
        //                        break;
        //                    case 1:
        //                        if (linea.costType != CostType.CPD)
        //                        {
        //                            difTipoTar = true;
        //                        }
        //                        break;
        //                    case 2:
        //                        {
        //                            difTipoTar = true;
        //                        }
        //                        break;
        //                    case 3:
        //                        if (linea.costType != CostType.CPC)
        //                        {
        //                            difTipoTar = true;
        //                        }
        //                        break;
        //                    case 4:
        //                        if (linea.costType != CostType.CPA)
        //                        {
        //                            difTipoTar = true;
        //                        }
        //                        break;
        //                }

        //                if (difTipoTar == true)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                    break;
        //                }

        //                else if (linea.name != detalle.Descripcion)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else if ((linea.costPerUnit.microAmount / 1000000.0) != detalle.Importe_unitario)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else if ((float)linea.discount != detalle.Porc_dto)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else if (linea.costType != CostType.CPD && (int)linea.primaryGoal.units != detalle.Cantidad)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                }

        //                else
        //                {
        //                    //Se comparan emplazamientos
        //                    int cantEmpGam = 0;

        //                    if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
        //                    {
        //                        cantEmpGam = linea.targeting.inventoryTargeting.targetedPlacementIds.Length;
        //                    }

        //                    if (cantEmpGam != detalle.Emplazamientos.Count)
        //                    {
        //                        difEmplaza = true;
        //                    }

        //                    else
        //                    {
        //                        foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
        //                        {
        //                            bool existeEmp = false;
        //                            foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
        //                            {
        //                                if (idEmpla == emp.Codigo_emplazamiento)
        //                                {
        //                                    existeEmp = true;
        //                                }
        //                            }
        //                            if (existeEmp == false)
        //                            {
        //                                difEmplaza = true;
        //                            }
        //                            break;
        //                        }
        //                    }
        //                }

        //                if (difEmplaza == true)
        //                {
        //                    ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                    break;
        //                }
        //                else
        //                {
        //                    //Se comparan medidas
        //                    if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
        //                    {
        //                        ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                    }
        //                    else
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
        //                                ordYdet.ListaDetalles.Add(detalle.Id_detalle);
        //                            }
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return ordYdet;
        //}

        private Dg_orden_pub_ap OrderGamAOrdenAp(Order ordenGam)
        {
            Dg_orden_pub_ap ordenNueva = new Dg_orden_pub_ap();
            long codRed = GetRedActual();
            Dg_red_GAM red = Dg_red_GAM.getByCodigo(codRed);
            List<Dg_orden_pub_ejecutivos> ejecutivos = new List<Dg_orden_pub_ejecutivos>();
            List<Dg_orden_pub_pagos> formasPago = new List<Dg_orden_pub_pagos>();
            List<Dg_orden_pub_as> detalles = new List<Dg_orden_pub_as>();

            ordenNueva.Id_red = red.Id_red;
            ordenNueva.Id_Google_Ad_Manager = ordenGam.id;
            ordenNueva.Bitacora = ordenGam.name;
            ordenNueva.anunciante = Contacto.getContactoByIdGAMyRed(ordenGam.advertiserId.ToString(), red.Id_red);
            ordenNueva.Seg_neto = (float)(ordenGam.totalBudget.microAmount / 1000000.0);
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
            int contId = 1;

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
                    foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                    {
                        Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                        emplaza.Codigo_emplazamiento = idEmpla;
                        emplaza.Id_emplazamiento = Dg_emplazamientos.getByCodigo2(idEmpla, red.Id_red).Id_emplazamiento;
                        emplazamientos.Add(emplaza);
                    }
                    detalle.Emplazamientos = emplazamientos;
                }
                if (linea.creativePlaceholders != null)
                {
                    foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                    {
                        Dg_orden_pub_medidas medida = new Dg_orden_pub_medidas();
                        string desc = "";
                        if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                        {
                            desc = cph.size.width.ToString() + "x" + cph.size.height.ToString() + "v";
                        }
                        else
                        {
                            desc = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                        }
                        medida.Id_medidadigital = Dg_medidas.getByDescripcion(desc).Id_medidadigital;
                        medida.Ancho = cph.size.width;
                        medida.Alto = cph.size.height;
                        medidas.Add(medida);
                    }
                    detalle.Medidas = medidas;
                }
                if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                {
                    detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Video");
                }
                else
                {
                    detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Banner");
                }
                detalle.Tarifa_manual = 1;
                detalle.Importe_unitario = (float)(linea.costPerUnit.microAmount / 1000000.0);
                detalle.Porc_dto = (float)linea.discount;
                detalle.Cantidad = (int)linea.primaryGoal.units;
                detalle.Monto_neto = (float)(linea.budget.microAmount / 1000000.0);
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

        public Parametro ImprimirMedidas(List<Dg_orden_pub_medidas> medidasBD, CreativePlaceholder[] medidasGam, string tipoLinea)
        {
            Parametro cambioLMed = new Parametro();
            cambioLMed.ParameterName = "Medidas";
            string medBD = "";
            string medGam = "";

            if (tipoLinea == "VIDEO_PLAYER")
            {
                foreach (Dg_orden_pub_medidas med in medidasBD)
                {
                    if (medBD == "")
                    {
                        medBD = med.Ancho.ToString() + "x" + med.Alto.ToString() + "v";
                    }
                    else
                    {
                        medBD += ", " + med.Ancho.ToString() + "x" + med.Alto.ToString() + "v";
                    }
                }
                foreach (CreativePlaceholder cph in medidasGam)
                {
                    if (medGam == "")
                    {
                        medGam = cph.size.width.ToString() + "x" + cph.size.height.ToString() + "v";
                    }
                    else
                    {
                        medGam += ", " + cph.size.width.ToString() + "x" + cph.size.height.ToString() + "v";
                    }
                }
            }
            else
            {
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
            }

            cambioLMed.Value = medBD + "@@@" + medGam;
            return cambioLMed;
        }

        public string FormatFecha(string fecha)
        {
            string fechaFormat = "";
            string[] arrFecha = fecha.Split(" ");
            fechaFormat = arrFecha[0];
            return fechaFormat;
        }

        public static string FormatFecha2(string fecha)
        {
            string[] arr = fecha.Split("-");
            string formatDate = "";
            formatDate = arr[2] + "-" + arr[1] + "-" + arr[0];
            return formatDate;
        }

        public List<ListaParametro> ComprobarModificacionesD(Dg_orden_pub_ap order)
        {
            List<ListaParametro> detallesCambios = new List<ListaParametro>();
            foreach(Dg_orden_pub_as detalle in order.Detalles)
            {
                if (detalle.Id_Google_Ad_Manager > 0)
                {
                    //apuntamos a la red adserver del detalle
                    Dg_red_GAM red = Dg_red_GAM.getById(detalle.Id_red);
                    CambiarRed(red.Codigo_red.ToString());

                    ListaParametro cambiosL = new ListaParametro();
                    cambiosL.Parametros = new List<Parametro>();
                    LineItem linea = GoogleAdManager.GetLineItemById(detalle.Id_Google_Ad_Manager);

                    //informar datos del Detalle
                    Parametro idDet = new Parametro();
                    idDet.ParameterName = "datosDet";
                    idDet.Value = detalle.Id_detalle.ToString() + "@@@" + detalle.Id_Google_Ad_Manager.ToString() + "@@@" + detalle.Id_red.ToString();
                    cambiosL.Parametros.Add(idDet);

                    if (linea.id > 0)
                    {
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
                        long[] emplazasLinea = { };

                        if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
                        {
                            cantEmpGam = linea.targeting.inventoryTargeting.targetedPlacementIds.Length;
                            emplazasLinea = linea.targeting.inventoryTargeting.targetedPlacementIds;
                        }

                        if (cantEmpGam != detalle.Emplazamientos.Count)
                        {
                            cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, emplazasLinea, detalle.Id_red));
                        }

                        else
                        {
                            foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
                            {
                                bool existeEmp = false;
                                foreach (long idEmpla in emplazasLinea)
                                {
                                    if (idEmpla == emp.Codigo_emplazamiento)
                                    {
                                        existeEmp = true;
                                        break;
                                    }
                                }
                                if (existeEmp == false)
                                {
                                    cambiosL.Parametros.Add(ImprimirEmplazas(detalle.Emplazamientos, emplazasLinea, detalle.Id_red));
                                }
                            }
                        }

                        //Se comparan medidas
                        if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
                        {
                            cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders, linea.environmentType.ToString()));
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
                                    cambiosL.Parametros.Add(ImprimirMedidas(detalle.Medidas, linea.creativePlaceholders, linea.environmentType.ToString()));
                                }
                            }
                        }

                        if ((linea.costPerUnit.microAmount / 1000000.0) != detalle.Importe_unitario)
                        {
                            Parametro cambioLImpUni = new Parametro();
                            cambioLImpUni.ParameterName = "Precio Unitario";
                            cambioLImpUni.Value = detalle.Importe_unitario.ToString() + "@@@" + (linea.costPerUnit.microAmount / 1000000.0).ToString();
                            cambiosL.Parametros.Add(cambioLImpUni);
                        }

                        if ((float)linea.discount != detalle.Porc_dto)
                        {
                            Parametro cambioLDesc = new Parametro();
                            cambioLDesc.ParameterName = "Descuento";
                            cambioLDesc.Value = detalle.Porc_dto.ToString() + "@@@" + ((float)linea.discount).ToString();
                            cambiosL.Parametros.Add(cambioLDesc);
                        }

                        if (linea.costType != CostType.CPD && (int)linea.primaryGoal.units != detalle.Cantidad)
                        {
                            Parametro cambioLCant = new Parametro();
                            cambioLCant.ParameterName = "Cantidad";
                            cambioLCant.Value = detalle.Cantidad.ToString() + "@@@" + ((int)linea.primaryGoal.units).ToString();
                            cambiosL.Parametros.Add(cambioLCant);
                        }

                        if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
                        {
                            Parametro cambioLDesde = new Parametro();
                            cambioLDesde.ParameterName = "Vigencia Desde";
                            cambioLDesde.Value = DateTimeUtilities.ToString(DateTimeUtilities.FromDateTime((System.DateTime)detalle.Fecha_desde, "America/Argentina/Buenos_Aires"), "dd/MM/yyyy") + "@@@" + DateTimeUtilities.ToString(linea.startDateTime, "dd/MM/yyyy");
                            cambiosL.Parametros.Add(cambioLDesde);
                        }

                        if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
                        {
                            Parametro cambioLHasta = new Parametro();
                            cambioLHasta.ParameterName = "Vigencia Hasta";
                            cambioLHasta.Value = DateTimeUtilities.ToString(DateTimeUtilities.FromDateTime((System.DateTime)detalle.Fecha_hasta, "America/Argentina/Buenos_Aires"), "dd/MM/yyyy") + "@@@" + DateTimeUtilities.ToString(linea.endDateTime, "dd/MM/yyyy");
                            cambiosL.Parametros.Add(cambioLHasta);
                        }

                        if (linea.orderName != detalle.Nombre_pedido_Google_Ad_Manager)
                        {
                            Parametro cambioLNomb = new Parametro();
                            cambioLNomb.ParameterName = "Nombre Pedido";
                            cambioLNomb.Value = detalle.Nombre_pedido_Google_Ad_Manager + "@@@" + linea.orderName;
                            cambiosL.Parametros.Add(cambioLNomb);
                        }
                    }
                    else
                    {
                        //informar que la linea fue archivada
                        Parametro lineaArchivada = new Parametro();
                        lineaArchivada.ParameterName = "Estado";
                        lineaArchivada.Value = "Activa" + "@@@" + "Archivada";
                        cambiosL.Parametros.Add(lineaArchivada);
                    }

                    detallesCambios.Add(cambiosL);
                }
            }

            return detallesCambios;
        }

        public List<Dg_orden_pub_as> ComprobarModificacionesDSincro(List<Parametro> parametros)
        {
            List<Dg_orden_pub_as> detallesMod = new List<Dg_orden_pub_as>();
            Dg_orden_pub_ap order = new Dg_orden_pub_ap();
            order.Detalles = Dg_orden_pub_as.filter(parametros);

            foreach (Dg_orden_pub_as detalle in order.Detalles)
            {
                    //apuntamos a la red adserver del detalle
                    Dg_red_GAM red = Dg_red_GAM.getById(detalle.Id_red);
                    CambiarRed(red.Codigo_red.ToString());

                bool modificado = false;
                    LineItem linea = GoogleAdManager.GetLineItemById(detalle.Id_Google_Ad_Manager);

                    //informar datos del Detalle
                    //Parametro idDet = new Parametro();
                    //idDet.ParameterName = "datosDet";
                    //idDet.Value = detalle.Id_detalle.ToString() + "@@@" + detalle.Id_Google_Ad_Manager.ToString() + "@@@" + detalle.Id_red.ToString();
                    //cambiosL.Parametros.Add(idDet);

                if (linea.id > 0)
                {
                        //se buscan diferencias entre la orden gam y la orden ap; si se encuentran, se devuelve la lista de cambios
                        switch (detalle.Tipo_tarifa)
                        {
                            case 0:
                                if (linea.costType != CostType.CPM)
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                                break;
                            case 1:
                                if (linea.costType != CostType.CPD)
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                                break;
                            case 2:
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                                break;
                            case 3:
                                if (linea.costType != CostType.CPC)
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                                break;
                            case 4:
                                if (linea.costType != CostType.CPA)
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                                break;
                        }

                        if (linea.name != detalle.Descripcion)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        //Se comparan emplazamientos
                        int cantEmpGam = 0;
                        long[] emplazasLinea = { };

                        if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
                        {
                            cantEmpGam = linea.targeting.inventoryTargeting.targetedPlacementIds.Length;
                            emplazasLinea = linea.targeting.inventoryTargeting.targetedPlacementIds;
                        }

                        if (cantEmpGam != detalle.Emplazamientos.Count)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        else
                        {
                            foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
                            {
                                bool existeEmp = false;
                                foreach (long idEmpla in emplazasLinea)
                                {
                                    if (idEmpla == emp.Codigo_emplazamiento)
                                    {
                                        existeEmp = true;
                                    break;
                                    }
                                }
                                if (existeEmp == false)
                                {
                                detallesMod.Add(detalle);
                                continue;
                            }
                            }
                        }

                        //Se comparan medidas
                        if (linea.creativePlaceholders.Length != detalle.Medidas.Count)
                        {
                        detallesMod.Add(detalle);
                        continue;
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
                                detallesMod.Add(detalle);
                                continue;
                            }
                            }
                        }

                        if ((linea.costPerUnit.microAmount / 1000000.0) != detalle.Importe_unitario)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        if ((float)linea.discount != detalle.Porc_dto)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        if (linea.costType != CostType.CPD && (int)linea.primaryGoal.units != detalle.Cantidad)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.startDateTime, "yyyy/MM/dd")) != detalle.Fecha_desde)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        if (System.DateTime.Parse(DateTimeUtilities.ToString(linea.endDateTime, "yyyy/MM/dd")) != detalle.Fecha_hasta)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }

                        if (linea.orderName != detalle.Nombre_pedido_Google_Ad_Manager)
                        {
                        detallesMod.Add(detalle);
                        continue;
                    }
                }
                else
                {
                    detallesMod.Add(detalle);
                    continue;
                }
                //if (modificado == true)
                //{
                //    detallesMod.Add(detalle);
                //}            
            }

            return detallesMod;
        }

        public List<Parametro> obtenerProgresoLineasGam(Dg_orden_pub_ap order)
        {
            List<Parametro> parametros = new List<Parametro>();

            foreach (Dg_orden_pub_as detalle in order.Detalles)
            {
                if (detalle.Id_Google_Ad_Manager > 0)
                {
                    Parametro parametro = new Parametro();

                    //apuntamos a la red adserver del detalle
                    Dg_red_GAM red = Dg_red_GAM.getById(detalle.Id_red);
                    CambiarRed(red.Codigo_red.ToString());

                    LineItem linea = GoogleAdManager.GetLineItemById(detalle.Id_Google_Ad_Manager);

                    if (linea.lineItemType != LineItemType.SPONSORSHIP)
                    {
                        double porcentaje;
                        if (linea.deliveryIndicator != null)
                        {
                            porcentaje = Math.Floor((linea.deliveryIndicator.actualDeliveryPercentage / linea.deliveryIndicator.expectedDeliveryPercentage) * 100);
                        }
                        else
                        {
                            porcentaje = 0;
                        }
                        parametro.ParameterName = detalle.Id_detalle.ToString();
                        parametro.Value = porcentaje.ToString();
                        parametros.Add(parametro);
                    }
                }
            }

            return parametros;

            //List<LineItem> lineasGAM = GoogleAdManager.getLineItemsByOrder(order.Id_Google_Ad_Manager);

            //foreach (LineItem linea in lineasGAM)
            //{
            //    Parametro parametro=new Parametro();
            //    foreach (Dg_orden_pub_as det in order.Detalles)
            //    {
            //        if (linea.id == det.Id_Google_Ad_Manager && linea.lineItemType != LineItemType.SPONSORSHIP)
            //        {
            //            double porcentaje;
            //            if (linea.deliveryIndicator != null)
            //            {
            //                porcentaje = Math.Floor((linea.deliveryIndicator.actualDeliveryPercentage / linea.deliveryIndicator.expectedDeliveryPercentage) * 100);
            //            }
            //            else
            //            {
            //                porcentaje = 0;
            //            }
            //            parametro.ParameterName=det.Id_detalle.ToString();
            //            parametro.Value = porcentaje.ToString();
            //            parametros.Add(parametro);
            //        }
            //    }
            //}          
            //return parametros;
        }

        public IEnumerable<Dg_orden_pub_as> comprobarNuevosDetalles(Dg_orden_pub_ap order)
        {            
            List<LineItem> lineasGAM = GoogleAdManager.getLineItemsByOrder(order.Id_Google_Ad_Manager);
            List<Dg_orden_pub_as> nuevosDetalles = new List<Dg_orden_pub_as>();

            if (lineasGAM.Count == 0)
            {
                return nuevosDetalles;
            }

            else
            {
                List<Dg_orden_pub_as> detalles = Dg_orden_pub_as.getByIdOp(order.Id_op_dg);

                //Se comparan Lineas de Pedido y Detalles
                foreach (LineItem linea in lineasGAM)
                {
                    bool existeDet = false;

                    foreach (Dg_orden_pub_as detalle in detalles)
                    {
                        if (linea.id == detalle.Id_Google_Ad_Manager)
                        {
                            existeDet = true;
                        }
                    }

                    if (existeDet == false)
                    {
                        //paso los datos de la linea al detalle
                        Dg_orden_pub_as detalle = new Dg_orden_pub_as();
                        List<Dg_orden_pub_emplazamientos> emplazamientos = new List<Dg_orden_pub_emplazamientos>();
                        List<Dg_orden_pub_medidas> medidas = new List<Dg_orden_pub_medidas>();

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
                            foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                            {
                                Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                                emplaza.Codigo_emplazamiento = idEmpla;
                                emplaza.Id_emplazamiento = Dg_emplazamientos.getByCodigo2(idEmpla, order.Id_red).Id_emplazamiento;
                                emplazamientos.Add(emplaza);
                            }
                            detalle.Emplazamientos = emplazamientos;
                        }
                        if (linea.creativePlaceholders != null)
                        {
                            foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                            {
                                Dg_orden_pub_medidas medida = new Dg_orden_pub_medidas();
                                string desc = "";
                                if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                                {
                                    desc = cph.size.width.ToString() + "x" + cph.size.height.ToString() + "v";
                                }
                                else
                                {
                                    desc = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                                }
                                medida.Id_medidadigital = Dg_medidas.getByDescripcion(desc).Id_medidadigital;
                                medida.Ancho = cph.size.width;
                                medida.Alto = cph.size.height;
                                medidas.Add(medida);
                            }
                            detalle.Medidas = medidas;
                        }
                        if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                        {
                            detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Video");
                        }
                        else
                        {
                            detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Banner");
                        }
                        detalle.Tarifa_manual = 1;
                        detalle.Importe_unitario = (float)(linea.costPerUnit.microAmount / 1000000.0);
                        detalle.Porc_dto = (float)linea.discount;
                        detalle.Cantidad = (int)linea.primaryGoal.units;
                        detalle.Monto_neto = (float)(linea.budget.microAmount / 1000000.0);
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

                        nuevosDetalles.Add(detalle);
                    }
                }
                return nuevosDetalles;
            }       
        }

        public IEnumerable<Dg_orden_pub_as> GetDetNuevos(List<Parametro> parametros)
        {
            List<Dg_orden_pub_as> detallesNuevos = new List<Dg_orden_pub_as>();
            int idRed = int.Parse(parametros[2].Value);
            //apuntamos a la red adserver seleccionada
            Dg_red_GAM red = Dg_red_GAM.getById(idRed);
            CambiarRed(red.Codigo_red.ToString());

            //al idContacto lo convertimos en idContactoDigital
            Contacto anun = new Contacto();
            anun = Contacto.getContactoByIdyRed(int.Parse(parametros[5].Value), idRed);
            parametros[5].Value = anun.IdContactoDigital;

            string listaOP = GoogleAdManager.GetIdOrderByFilters(parametros);
            if (listaOP != "")
            {
                List<LineItem> detallesGAM = GoogleAdManager.FilterLineItems(parametros, listaOP);

                if (detallesGAM.Count == 0)
                {
                    return detallesNuevos;
                }

                else
                {
                    List<Dg_orden_pub_as> detallesExistentes = Dg_orden_pub_as.getByAnunRedFecha(anun.Id_contacto, idRed, parametros[6].Value, parametros[7].Value);

                    //Recorremos las lineas de pedido y las vamos comparando con los detalles existentes
                    foreach (LineItem linea in detallesGAM)
                    {
                        bool existeDetalle = false;
                        foreach (Dg_orden_pub_as detExis in detallesExistentes)
                        {
                            //Si encontramos la linea dentro de los detalles, se sale del for y buscamos la pr�xima linea
                            if (linea.id == detExis.Id_Google_Ad_Manager && idRed == detExis.Id_red)
                            {
                                existeDetalle = true;
                                break;
                            }
                        }
                        //Si no se encuentra la linea de pedido dentro de los detalles existentes..
                        if (!existeDetalle)
                        {
                            //Pasamos los datos de la linea al detalle
                            Dg_orden_pub_as detalle = new Dg_orden_pub_as();
                            List<Dg_orden_pub_emplazamientos> emplazamientos = new List<Dg_orden_pub_emplazamientos>();
                            List<Dg_orden_pub_medidas> medidas = new List<Dg_orden_pub_medidas>();

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
                            
                            string creatD = DateTimeUtilities.ToString(linea.creationDateTime, "yyyy/MM/dd");
                            if (creatD != "0")
                            {
                                detalle.Fecha_creacion = System.DateTime.Parse(creatD);
                            }
                            detalle.Id_red = idRed;
                            detalle.Id_pedido_Google_Ad_Manager = linea.orderId;
                            detalle.Nombre_pedido_Google_Ad_Manager = linea.orderName;
                            detalle.Id_Google_Ad_Manager = linea.id;
                            detalle.Descripcion = linea.name;
                            detalle.Tarifa_manual = 1;
                            detalle.Importe_unitario = (float)(linea.costPerUnit.microAmount / 1000000.0);
                            detalle.Porc_dto = (float)linea.discount;
                            detalle.Cantidad = (int)linea.primaryGoal.units;
                            detalle.Monto_neto = (float)(linea.budget.microAmount / 1000000.0);
                            if (linea.targeting.geoTargeting != null)
                            {
                                Dg_areas_geo area = new Dg_areas_geo();
                                area.Id_area = Dg_areas_geo.getByCodigo(linea.targeting.geoTargeting.targetedLocations[0].id).Id_area;
                                detalle.areaGeo = area;
                            }
                            if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                            {
                                detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Video");
                            }
                            else
                            {
                                detalle.tipo_aviso_dg = Dg_tipos_avisos.getByDesc("Banner");
                            }
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

                            if (linea.targeting.inventoryTargeting.targetedPlacementIds != null)
                            {
                                foreach (long idEmpla in linea.targeting.inventoryTargeting.targetedPlacementIds)
                                {
                                    Dg_orden_pub_emplazamientos emplaza = new Dg_orden_pub_emplazamientos();
                                    emplaza.Codigo_emplazamiento = idEmpla;
                                    emplaza.Id_emplazamiento = Dg_emplazamientos.getByCodigo2(idEmpla, idRed).Id_emplazamiento;
                                    emplazamientos.Add(emplaza);
                                }
                                detalle.Emplazamientos = emplazamientos;
                            }
                            if (linea.creativePlaceholders != null)
                            {
                                foreach (CreativePlaceholder cph in linea.creativePlaceholders)
                                {
                                    Dg_orden_pub_medidas medida = new Dg_orden_pub_medidas();
                                    string desc = "";
                                    if (linea.environmentType.ToString() == "VIDEO_PLAYER")
                                    {
                                        desc = cph.size.width.ToString() + "x" + cph.size.height.ToString() + "v";
                                    }
                                    else
                                    {
                                        desc = cph.size.width.ToString() + "x" + cph.size.height.ToString();
                                    }
                                    medida.Id_medidadigital = Dg_medidas.getByDescripcion(desc).Id_medidadigital;
                                    medida.Ancho = cph.size.width;
                                    medida.Alto = cph.size.height;
                                    medidas.Add(medida);
                                }
                                detalle.Medidas = medidas;
                            }

                            detallesNuevos.Add(detalle);
                        }
                    }
                }
            }
            return detallesNuevos;
        }

        public bool saveMail(Mail mail)
        {
            mail.Pass = UserService.Encriptar(mail.Pass, "silverblue");
            return mail.saveMailCta();
        }

        public Mail getMailCta()
        {
            Mail mail = Mail.getMailCta();
            if (mail.Pass == null)
            {
                mail.Pass = "4toQaorWHkvy2xqYBmLyBA==";
            }
            mail.Pass = UserService.Desencriptar(mail.Pass, "silverblue");

            return mail;
        }

        public void enviarMail(string asunto, string mensaje)
        {
            Mail mail = getMailCta();
            List <Usuario> listaMails = Usuario.getListaAlertas();
            MailMessage correo = new MailMessage();

            correo.From = new MailAddress(mail.DirMail, mail.Nombre, Encoding.UTF8);//Correo de salida
            foreach(Usuario u in listaMails)
            {
                correo.To.Add(u.Email); //Correo destino
            }
            correo.Subject = asunto; //Asunto
            correo.Body = mensaje; //Mensaje del correo
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = mail.Servidor; //Host del servidor de correo 
            smtp.Port = int.Parse(mail.Puerto); //Puerto de salida
            smtp.Credentials = new NetworkCredential(mail.DirMail, mail.Pass); //Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = false;//True si el servidor de correo permite ssl

            smtp.Send(correo);
        }

        public String pruebaMail()
        {
            string resp = "";
            string asunto = "MMASS Online - Mail de Prueba";
            string msj = @"Este es un Mail de Prueba <br>" +
                          "--------------------------------------------------------------------" +
                          "---------------------------------------------------------------<br>" +
                          "<font size=1>No responder este mensaje</font><br>" +
                          "<H5>Sistema de Notificaciones MMASS Online</H5>";
            try
            {
                enviarMail(asunto, msj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resp = ex.Message;
            }
            return resp;
        }

    }
}