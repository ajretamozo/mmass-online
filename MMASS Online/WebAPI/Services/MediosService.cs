using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Google.Api.Ads.AdManager.v202105;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Entities;
using WebApi.Helpers;
using System.Configuration;


namespace WebApi.Services
{
    public interface IMedioService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Medio> getAll();
        IEnumerable<Medio> getPorEmpresa(int idEmp);
        //AGREGUE (getallm, getalla, getalle):
        IEnumerable<Dg_medidas> getAllM();
        IEnumerable<Dg_areas_geo> getAllA();
        IEnumerable<Dg_emplazamientos> getAllE(int redGAM);
        bool saveE(EmplazamientosList emplazamientos);
        bool saveMedidas(Dg_medidas miObj);
        IEnumerable<Convenios> getAllConvenios();
        Convenios getConvenioById(int Id);
        IEnumerable<Convenios> filterConvenios(List<Parametro> parametros);
        Conv_dg_detalle getDetConvenioById(int IdConv, int IdDet);
        IEnumerable<Conv_dg_detalle> getDetConveniosByIdConv(Conv_dg_detalle detConv);
        IEnumerable<Dg_red_GAM> getAllRedes();
        Dg_red_GAM getRedByCodigo(long netCode);
        Dg_red_GAM getRedById(int id);
        List<string> getCodigosRed();
        bool deleteRed(Dg_red_GAM miobj);
        int saveRed(Dg_red_GAM miobj);
        IEnumerable<Dg_red_GAM> filterRedes(List<Parametro> parametros);
        IEnumerable<Concepto_Negocio> getAllConceptos();
        Dg_emplazamientos getEmplazaByCodigo(long cod, int idRed);
        int getBD();
        String getConString();
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

        //AGREGUE (getallm, getalla, getalle):
        public IEnumerable<Dg_medidas> getAllM()
        {
            return Dg_medidas.getAll();
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

        public Conv_dg_detalle getDetConvenioById(int IdConv, int IdDet)
        {
            return Conv_dg_detalle.getById(IdConv, IdDet);
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

    }
}