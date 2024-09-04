using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.entitys
{
    public class Clinica_Veterinaria
    {
        
       public int  Id_Clinica_Veterinaria { get; set; }
        public string Nombre_Clinica { get; set; }

            public string Nombre_Doctor { get; set; }
        public int id_Doctor { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
      
       
        
    }
}
