using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Baheiros: ResBase
    {
        public List<Baheiro> ListaBaheiros { get; set; }

        // Constructor para inicializar la lista
        public Res_Lista_Baheiros()
        {
            ListaBaheiros = new List<Baheiro>();
        }
    }
}
