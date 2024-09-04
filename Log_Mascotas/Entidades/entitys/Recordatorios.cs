using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades
{
    public class Recordatorios
    {

        public int Id_Mascota { get; set; }

        public int Id_Vacuna { get; set; }

        public int Id_Medicamentos { get; set; }
        public int Id_Cita { get; set; }
        public int Id_Baheiro { get; set; }

        public DateTime? Fecha { get; set; }

        public TimeSpan? Hora { get; set; }

        public string Descripcion { get; set; }

        public string Nombre { get; set; }

        public string Tipo { get; set; }


    }
}
