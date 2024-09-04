using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class Registro_Usuario
    {
        public Int64 Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo_Electronico { get; set; }

        public string Confirmacion_Correo_Electronico { get; set; }
        public string Password { get; set; }

        public string Confirmar_Password { get; set; }

        public string numero_verificacion { get; set; }

        public string token { get; set; }
    }
}
