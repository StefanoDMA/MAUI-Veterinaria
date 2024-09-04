using Log_Mascotas.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Doctores
    {
        public int id_Doctor { get; set; }
        public List<Doctor> Lista_Doctores { get; set; } = new List<Doctor>();

    }
}
