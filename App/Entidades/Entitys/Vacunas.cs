using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class Vacunas
    {
            
        public int Id_Vacuna { get; set; }
        public string Nombre { get; set; }
      
        public string Decripcion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

    }
}
