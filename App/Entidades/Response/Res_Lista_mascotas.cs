using FrontEndHealthPets.Entidades.Response;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_mascotas: Res_Base
    {
        
        public List<Registro_Mascota> ListaMascotas { get; set; } = new List<Registro_Mascota>();

    }
}
