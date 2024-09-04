using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class Citas_Clinica_Veterinaria_Mascotas
    {
        public int Id_Mascota { get; set; }

        public string Nombre_Mascota { get; set; }
        public int Id_Clinica { get; set; }
        public string Nombre_Clinica { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public int id_doctor { get; set; }

        public string Nombre_Doctor { get; set; }
        public DateTime? Fecha_y_hora_Cita { get; set; }

        public string Descripcion { get; set; } 
        public string Notas { get; set; }

    }
}
