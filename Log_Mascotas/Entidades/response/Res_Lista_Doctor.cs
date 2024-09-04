using Log_Mascotas.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Doctor : ResBase
    {

        public List<Doctor> ListaDoctor { get; set; }

    }
}
