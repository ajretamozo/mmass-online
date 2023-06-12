using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing.Drawing2D;


namespace WebApi.Services
{
    public interface IMedioService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Medio> getAll();
        IEnumerable<Medio> getPorEmpresa(int idEmp);
        IEnumerable<Dg_medidas> getMedidas(int tipo);
        IEnumerable<Dg_areas_geo> getAllA();
        IEnumerable<Dg_emplazamientos> getAllE(int redGAM);
        bool saveE(EmplazamientosList emplazamientos);
        bool saveMedidas(Dg_medidas miObj);
        bool saveMedidasV(Dg_medidas miObj);
        IEnumerable<Convenios> getAllConvenios();
        Convenios getConvenioById(int Id);
        IEnumerable<Convenios> filterConvenios(List<Parametro> parametros);
        Conv_dg_detalle getDetConvenioById(int IdDetConv);
        IEnumerable<Conv_dg_detalle> getDetConveniosByIdConv(Conv_dg_detalle detConv);
        IEnumerable<Dg_red_GAM> getAllRedes();
        Dg_red_GAM getRedByCodigo(long netCode);
        Dg_red_GAM getRedById(int id);
        List<string> getCodigosRed();
        bool deleteRed(Dg_red_GAM miobj);
        int saveRed(Dg_red_GAM miobj);
        IEnumerable<Dg_red_GAM> filterRedes(List<Parametro> parametros);
        IEnumerable<Concepto_Negocio> getAllConceptos();
        IEnumerable<Clasificacion_op> getAllClasificacion();
        Dg_emplazamientos getEmplazaByCodigo(long cod, int idRed);
        int getBD();
        String getConString();
        IEnumerable<Plazos_Pagos> GetAllPlazos();
        bool cambiarParam(Dg_parametro param);
        Dg_parametro getParamById(int id);
        List<Conv_dg_detalle> checkLinConv(Dg_orden_pub_ap op);
        List<Moneda> getAllMonedas();
        double getCambioActual(List<Parametro> parametros);
        List<Moneda> getCotizaciones();
    }

    public class MedioService : IMedioService
    {

        public IEnumerable<Medio> getAll()
        {
            return Medio.getAll();
        }

        public IEnumerable<Medio> getPorEmpresa(int idEmp)
        {
            return Medio.getPorEmpresa(idEmp);
        }

        public IEnumerable<Dg_medidas> getMedidas(int tipo)
        {
            return Dg_medidas.getMedidas(tipo);
        }

        public IEnumerable<Dg_areas_geo> getAllA()
        {
            return Dg_areas_geo.getAll();
        }

        public IEnumerable<Dg_emplazamientos> getAllE(int redGAM)
        {
            return Dg_emplazamientos.getByIdRed(redGAM);
        }

        public bool saveE(EmplazamientosList miObj)
        {
            return miObj.save();
        }

        public bool saveMedidas(Dg_medidas miObj)
        {
            return miObj.saveMedidas();
        }

        public bool saveMedidasV(Dg_medidas miObj)
        {
            return miObj.saveMedidasV();
        }

        public IEnumerable<Convenios> getAllConvenios()
        {
            return Convenios.getAll();
        }

        public Convenios getConvenioById(int Id)
        {
            int BD = getBD();
            if (BD == 1)
            {
                return Convenios.getByIdC9(Id);
            }
            else
            {
                return Convenios.getByIdC5N(Id);
            }
        }
        public IEnumerable<Convenios> filterConvenios(List<Parametro> parametros)
        {
            return Convenios.filter(parametros);
        }

        public Conv_dg_detalle getDetConvenioById(int IdDetConv)
        {
            return Conv_dg_detalle.getById(IdDetConv);
        }

        public IEnumerable<Conv_dg_detalle> getDetConveniosByIdConv(Conv_dg_detalle detConv)
        {
            return detConv.getByIdConv();
        }

        public IEnumerable<Dg_red_GAM> getAllRedes()
        {
            return Dg_red_GAM.getAll();
        }

        public Dg_red_GAM getRedByCodigo(long netCode)
        {
            return Dg_red_GAM.getByCodigo(netCode);
        }

        public Dg_red_GAM getRedById(int id)
        {
            return Dg_red_GAM.getById(id);
        }

        public List<string> getCodigosRed()
        {
            return Dg_red_GAM.getCodigos();
        }

        public bool deleteRed(Dg_red_GAM miobj)
        {
            return miobj.deleteRed();
        }

        public int saveRed(Dg_red_GAM miobj)
        {
            return miobj.save();
        }

        public IEnumerable<Dg_red_GAM> filterRedes(List<Parametro> parametros)
        {
            return Dg_red_GAM.filter(parametros);
        }

        public IEnumerable<Concepto_Negocio> getAllConceptos()
        {
            return Concepto_Negocio.getAll();
        }

        public IEnumerable<Clasificacion_op> getAllClasificacion()
        {
            return Clasificacion_op.getAll();
        }

        public Dg_emplazamientos getEmplazaByCodigo(long cod, int idRed)
        {
            return Dg_emplazamientos.getByCodigo2(cod, idRed);
        }

        public int getBD()
        {
            return int.Parse(ConfigurationManager.AppSettings["Base"]);
        }

        public String getConString()
        {
            string csEdit = "";
            string[] arrCs = DB.conexion.ConnectionString.Split(";");
            string[] arrIp = arrCs[0].Split("=");
            string[] arrBd = arrCs[1].Split("=");
            csEdit = arrIp[1] + " - " + arrBd[1];

            return csEdit;
        }

        public IEnumerable<Plazos_Pagos> GetAllPlazos()
        {
            return Plazos_Pagos.getAll();
        }

        public bool cambiarParam(Dg_parametro param)
        {
            return param.updateParam();
        }

        public Dg_parametro getParamById(int id)
        {
            return Dg_parametro.getById(id);
        }

        public List<Conv_dg_detalle> checkLinConv(Dg_orden_pub_ap op)
        {
            List<Conv_dg_detalle> detallesConvValidos = new List<Conv_dg_detalle>();
            Conv_dg_detalle detConv = new Conv_dg_detalle();
            detConv.Id_convenio = op.Id_convenio;
            detConv.Id_empresa = op.empresa.Id_empresa;
            List<Conv_dg_detalle> detallesConv = detConv.getByIdConv();
            Dg_orden_pub_as detalle = op.Detalles[0];
            //si la linea viene con area null, le creamos una para poder compararla
            if (detalle.areaGeo == null)
            {
                Dg_areas_geo area = new Dg_areas_geo();
                area.Id_area = 0;
                detalle.areaGeo = area;
            }

            foreach (Conv_dg_detalle detConvenio in detallesConv)
            {
                //se chequea si los datos de la linea concuerdan con algun det convenio

                if (detalle.Tipo_tarifa != detConvenio.Forma_uso.Id)
                {
                    continue;
                }

                //Se comparan emplazamientos
                if (detConvenio.Emplazamientos.Count < detalle.Emplazamientos.Count)
                {
                    continue;
                }

                else
                {
                    foreach (Dg_orden_pub_emplazamientos emp in detalle.Emplazamientos)
                    {
                        bool existeEmp = false;
                        foreach (Dg_emplazamientos empConv in detConvenio.Emplazamientos)
                        {
                            if (empConv.Codigo_emplazamiento == emp.Codigo_emplazamiento)
                            {
                                existeEmp = true;
                                break;
                            }
                        }
                        if (existeEmp == false)
                        {
                            continue;
                        }
                    }
                }

                //Se comparan medidas
                if (detConvenio.Medidas.Count < detalle.Medidas.Count)
                {
                    continue;
                }

                else
                {
                    foreach (Dg_orden_pub_medidas med in detalle.Medidas)
                    {
                        bool existe = false;
                        foreach (Dg_medidas medConv in detConvenio.Medidas)
                        {
                            if (medConv.Id_medidadigital == med.Id_medidadigital)
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (existe == false)
                        {
                            continue;
                        }
                    }
                }

                if (detConvenio.Precio_unitario != detalle.Importe_unitario)
                {
                    continue;
                }

                if (detConvenio.Porc_desc != detalle.Porc_dto)
                {
                    continue;
                }

                if (detalle.Fecha_desde < detConvenio.Fecha_desde)
                {
                    continue;
                }

                if (detalle.Fecha_hasta > detConvenio.Fecha_hasta)
                {
                    continue;
                }

                if (detalle.tipo_aviso_dg.Tipo_aviso_ads != detConvenio.Tipos_aviso[0].Tipo_aviso_ads)
                {
                    continue;
                }

                if (detalle.areaGeo.Id_area != detConvenio.Id_area)
                {
                    continue;
                }

                detallesConvValidos.Add(detConvenio);
            }
            return detallesConvValidos;
        }

        public List<Moneda> getAllMonedas()
        {
            return Moneda.getAll();
        }

        public double getCambioActual(List<Parametro> parametros)
        {
            return Moneda.getCambioActual(int.Parse(parametros[0].Value), int.Parse(parametros[1].Value));
        }

        public List<Moneda> getCotizaciones()
        {
            return Moneda.getCotizaciones();
        }
    }
}