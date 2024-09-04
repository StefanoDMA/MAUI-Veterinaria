using Log_Mascotas.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_de_Doctores_Clinica: ResBase
    {
        public int Id_Clinica_Veterinaria { get; set; }
        public List<Doctor> listaDoctoresporclinica = new List<Doctor>();
 
    }
}
