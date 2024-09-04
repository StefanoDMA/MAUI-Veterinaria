using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class MedicamentosMascotas
    {

       public int Id_Mascota { get; set; }

        public string Nombre_Mascota { get; set; }
        public int id_medicamento { get; set; }

        public string Modo_De_Administracion { get; set; }

        public string Nombre_Medicamento { get; set; }

        public string categoria { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public DateTime? Fecha_Fin { get; set; }
        public TimeSpan? Hora_De_Ingesta { get; set; }
        public string Notas { get; set; }

    }
}
