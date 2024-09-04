using Log_Mascotas.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades
{
    public class Req_EliminarUsuario
    {
        public int Id_Usuario { get; set; }
        public eliminar_usuario eliminar_usuario { get; set; }

    }
}
