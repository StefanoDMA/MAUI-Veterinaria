using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Vascunas: ResBase
    {
        public List<Vacunas> Lista_Vacunas { get; set; }

        // Constructor para inicializar la lista
        public Res_Lista_Vascunas()
        {
            Lista_Vacunas = new List<Vacunas>();
        }
    }
}
