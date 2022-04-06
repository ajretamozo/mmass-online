using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class ListaParametro
    {
        public List<Parametro> Parametros { get; set; }
        public List<int> ListaDetalles { get; set; }
    }

    public class Parametro
    {
        public string ParameterName { get; set; }
        public string Value { get; set; }
    }
}
