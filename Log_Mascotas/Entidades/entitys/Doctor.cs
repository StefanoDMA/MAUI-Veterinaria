using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.entitys
{
    
    public class Doctor
    {
        public int Id_Clinica_Veterinaria { get; set; }
        public int Id_Doctor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo_Electronico { get; set; }
     

    }
}
