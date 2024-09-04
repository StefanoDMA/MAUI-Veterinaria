using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class BaheiroMascotas
    {
        public int Id_Mascota { get; set; }
        public string Nombre_Mascota { get; set; }
        public int Id_Baheiro { get; set; }
        public string Nombre_Baheiro { get; set; }

        public string Descripcion_Baheiro { get; set; }
        public DateTime? Fecha_y_hora_Baheiro { get; set; }
        public DateTime? Fecha_y_hora_proximo_Baheiro { get; set; }
        
        public string Notas { get; set; }
    }
}
