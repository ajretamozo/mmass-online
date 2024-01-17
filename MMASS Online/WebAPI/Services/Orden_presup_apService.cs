﻿using System;
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
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace WebApi.Services
{
    public class Orden_presup_apR
    {
        public Orden_presup_ap op { get; set; }
        public int result { get; set; }
        public string message { get; set; }
    }
    public interface IOrden_presup_apService
    {
        Orden_presup_apR save(Orden_presup_ap miobj);
        IEnumerable<Orden_presup_ap> getAll();
        Orden_presup_ap getById(int id);
        IEnumerable<Orden_presup_ap> filter(List<Parametro> parametros);
        bool existePresupNombre(int id, string nom);
        void grabarLog(List<Parametro> datosLog);
        //bool bloquearOP(Dg_orden_pub_bloqueo opb);
        //bool desbloquearOP(Dg_orden_pub_bloqueo opb);
        //bool desbloquearTodas(int id);
        //void grabarLog(List<Parametro> datosLog);
    }
    public class Orden_presup_apM : Orden_presup_ap
    {
        public int result { get; set; }
        public string message { get; set; }
    }
    public class Orden_presup_apService : IOrden_presup_apService
    {
        public IEnumerable<Orden_presup_ap> getAll()
        {
            return Orden_presup_ap.getAll();
        }

        public Orden_presup_apR save(Orden_presup_ap miobj)
        {
            Orden_presup_apR ret = new Orden_presup_apR();
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
                    ret.message = "El Presupuesto Nro: " + ret.op.Anio + "-" + ret.op.Mes + "-" + ret.op.Nro_presup + " se generó con éxito";

                    int paramEnviarMail = int.Parse(Dg_parametro.getById(7).Valor);
                    if (paramEnviarMail == 1 || paramEnviarMail == 2 || paramEnviarMail == 5)
                    {
                        string asunto = "MMASS Online - Presupuesto";
                        string msj = @"Se requiere acción por parte del usuario.<br>
                                Ingrese al siguiente link para Aprobar o Rechazar el presupuesto: <br>" +
                                        ret.op.LinkPresup + "< br >" +
                                        "--------------------------------------------------------------------" +
                                        "---------------------------------------------------------------<br>" +
                                        "<font size=1>No responder este mensaje</font><br>" +
                                        "<H5>Sistema de Notificaciones MMASS Online</H5>";
                        enviarMail(asunto, msj);
                    }
                }
                else
                {
                    ret.result = -1;
                    ret.message = "Ocurrió un error al intentar guardar el Presupuesto";
                }
            }
            return ret;
        }
        public Orden_presup_ap getById(int id)
        {
            return Orden_presup_ap.getById(id);
        }

        public IEnumerable<Orden_presup_ap> filter(List<Parametro> parametros)
        {
            return Orden_presup_ap.filter(parametros);
        }

        //public bool bloquearOP(Dg_orden_pub_bloqueo opb)
        //{
        //    return opb.bloquear();
        //}

        //public bool desbloquearOP(Dg_orden_pub_bloqueo opb)
        //{
        //    return opb.desbloquear();
        //}

        //public bool desbloquearTodas(int idUsuario)
        //{
        //    return Dg_orden_pub_bloqueo.desbloquearTodas(idUsuario);
        //}

        public bool existePresupNombre(int id, string nom)
        {
            return Orden_presup_ap.existePresupNombre(id, nom);
        }

        public void grabarLog(List<Parametro> datosLog)
        {
            Orden_presup_ap.grabarLog(datosLog);
        }

        public Mail getMailCta()
        {
            Mail mail = Mail.getMailCta();

            if (mail.Pass != null)
            {
                mail.Pass = UserService.Desencriptar(mail.Pass, "silverblue");
            }

            return mail;
        }

        public void enviarMail(string asunto, string mensaje)
        {
            Mail mail = getMailCta();
            List<Usuario> listaMails = Usuario.getListaAlertas();
            MailMessage correo = new MailMessage();

            correo.From = new MailAddress(mail.DirMail, mail.Nombre, Encoding.UTF8);//Correo de salida
            foreach (Usuario u in listaMails)
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

    }
}