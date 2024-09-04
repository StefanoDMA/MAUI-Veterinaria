using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class FotosMascota
    {
       
        public byte[] Foto { get; set; } // varbinary(max) en la tabla
        public int Id_Mascota { get; set; }
        public string Descripcion { get; set; }
    }
}