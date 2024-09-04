using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class Medicamentos
    {
        public int Id_Medicamento { get; set; }

        public string Nombre  { get; set; }

        public string Categoria { get; set; }
        public string Decripcion { get; set; }

        public DateTime FechaDeVencimiento { get; set; }

    }
}
