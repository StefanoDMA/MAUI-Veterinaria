using FrontEndHealthPets.Entidades.entitys;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_de_Doctores_Clinica: Res_Base
    {
        public int Id_Clinica_Veterinaria { get; set; }
        public List<Doctor> listaDoctoresporclinica = new List<Doctor>();
 
    }
}
