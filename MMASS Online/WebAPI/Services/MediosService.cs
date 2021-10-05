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

namespace WebApi.Services
{
    public interface IMedioService
    {
        //Contacto GetAgencia(string username, string password);
        IEnumerable<Medio> getAll();
        //AGREGUE (getallm, getalla, getalle):
        IEnumerable<Dg_medidas> getAllM();
        IEnumerable<Dg_areas_geo> getAllA();
        IEnumerable<Dg_emplazamientos> getAllE(int redGAM);
        bool saveE(EmplazamientosList emplazamientos);
        bool saveMedidas(Dg_medidas miObj);
        IEnumerable<Convenios> getAllConvenios();
        Convenios getConvenioById(int Id);
        IEnumerable<Convenios> filter(List<Parametro> parametros);
        Conv_dg_detalle getDetConvenioById(int IdConv, int IdDet);
        IEnumerable<Conv_dg_detalle> getDetConveniosByIdConv(int IdConv);
        IEnumerable<Dg_red_GAM> getAllRedes();
        Dg_red_GAM getRedByCodigo(long netCode);
        Dg_red_GAM getRedById(int id);
        List<string> getCodigosRed();
        bool deleteRed(Dg_red_GAM miobj);
        bool saveRed(Dg_red_GAM miobj);
        IEnumerable<Dg_red_GAM> filterRedes(List<Parametro> parametros);
    }

    public class MedioService : IMedioService
    {

        public IEnumerable<Medio> getAll()
        {
            return Medio.getAll();
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
            return Convenios.getById(Id);
        }
        public IEnumerable<Convenios> filter(List<Parametro> parametros)
        {
            return Convenios.filter(parametros);
        }

        public Conv_dg_detalle getDetConvenioById(int IdConv, int IdDet)
        {
            return Conv_dg_detalle.getById(IdConv, IdDet);
        }

        public IEnumerable<Conv_dg_detalle> getDetConveniosByIdConv(int IdConv)
        {
            return Conv_dg_detalle.getByIdConv(IdConv);
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

        public bool saveRed(Dg_red_GAM miobj)
        {
            return miobj.save();
        }

        public IEnumerable<Dg_red_GAM> filterRedes(List<Parametro> parametros)
        {
            return Dg_red_GAM.filter(parametros);
        }

    }
}