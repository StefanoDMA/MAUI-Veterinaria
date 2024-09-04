using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_ObtenerFotos : ResBase
    {



        public byte[] Foto { get; set; }
          public string Descripcion { get; set; }

    }
}
