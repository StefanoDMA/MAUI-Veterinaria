using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades
{
    public class FotosMascota
    {
        public int Id_Foto { get; set; }
        public byte[] Foto { get; set; } // varbinary(max) en la tabla
        public int Id_Mascota { get; set; }
        public string Descripcion { get; set; }
    }
}