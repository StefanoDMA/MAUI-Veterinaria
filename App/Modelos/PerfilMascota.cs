using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace FrontEndHealthPets.Modelos
{
    public class PerfilMascota
    {
        public int Id_Mascota { get; set; } // Propiedad Id_Mascota
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageSource ImageSource { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }

        public DateTime Fecha_Proximo_Baheiro { get; set; }
    }
}