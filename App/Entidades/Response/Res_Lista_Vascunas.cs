using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Vascunas: Res_Base
    {
        public List<Vacunas> Lista_Vacunas { get; set; }

        // Constructor para inicializar la lista
        public Res_Lista_Vascunas()
        {
            Lista_Vacunas = new List<Vacunas>();
        }
    }
}
