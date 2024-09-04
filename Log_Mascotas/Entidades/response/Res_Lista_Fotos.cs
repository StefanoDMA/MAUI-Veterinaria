using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Fotos : ResBase
    {
        public List<FotosMascota> Lista_Fotos { get; set; } = new List<FotosMascota>();

    }
}
