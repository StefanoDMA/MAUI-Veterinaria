using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.Request
{
    public class Req_ObtenerBreeds
    {
        public Dictionary<string, List<string>> Breeds { get; set; }
    }
}
