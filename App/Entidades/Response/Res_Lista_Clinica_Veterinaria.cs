using FrontEndHealthPets.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndHealthPets.Entidades.Response;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.Entitys;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Clinica_Veterinaria: Res_Base
    {

        public List<Clinica_Veterinaria> Lista_Clinica_Veterinaria { get; set; } = new List<Clinica_Veterinaria>();

    }
}
