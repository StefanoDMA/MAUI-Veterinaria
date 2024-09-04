using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Fotos : Res_Base
    {
        public List<FotosMascota> Lista_Fotos { get; set; } = new List<FotosMascota>();

    }
}
