using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.Request
{
    public class Req_Actualizar_Usuario
    {
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo_Electronico { get; set; }

        public string Confirmacion_Correo_Electronico { get; set; }
        public string Password { get; set; }

        public string Confirmar_Password { get; set; }

    }
}