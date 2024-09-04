using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FrontEndHealthPets.Entidades
{
    public class Vacunas_Mascotas
    {
        public int Id_Mascota { get; set; }
        public string Nombre_Mascota { get; set; }
        public int Id_Vacuna { get; set; }
        public string Nombre_Vacuna { get; set; }

        public string Descripcion { get; set; }
        
        public int Dosis { get; set; }
        public DateTime? Fecha_y_Hora_Aplicacion { get; set; }
        public DateTime? Fecha_y_Hora_Proxima_Aplicacion { get; set; }    
        public string notas { get; set; }
    }
}
