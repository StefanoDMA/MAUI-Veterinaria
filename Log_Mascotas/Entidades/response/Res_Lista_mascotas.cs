using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_mascotas: ResBase
    {
        public int id_Usuario { get; set; }
        public List<Registro_Mascota> ListaMascotas { get; set; } = new List<Registro_Mascota>();

    }
}
