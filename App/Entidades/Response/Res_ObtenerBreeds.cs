using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_ObtenerBreeds
    {

        public Dictionary<string, List<string>> Breeds { get; set; }
        public string Status { get; set; }

    }
}
