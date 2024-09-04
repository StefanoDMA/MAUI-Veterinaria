using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Baheiros: Res_Base
    {
        public List<Baheiro> ListaBaheiros { get; set; }

        // Constructor para inicializar la lista
        public Res_Lista_Baheiros()
        {
            ListaBaheiros = new List<Baheiro>();
        }
    }
}
