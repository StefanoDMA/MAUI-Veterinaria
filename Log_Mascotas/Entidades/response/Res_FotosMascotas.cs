using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas
{
    public class Res_FotosMascotas : ResBase
    {

        public List<string> fotos { get; set; } // o cualquier otro tipo que desees para almacenar las fotos

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

    }
}
