using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Citas_Veterinaria_mascotas: Res_Base
    {
        public List<Citas_Clinica_Veterinaria_Mascotas> Lista_Citas_Veterinaria_Mascotas = new List<Citas_Clinica_Veterinaria_Mascotas>();

    }
}
