using Log_Mascotas.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades.response
{
    public class Res_Lista_Clinica_Veterinaria: ResBase
    {

        public List<Clinica_Veterinaria> Lista_Clinica_Veterinaria { get; set; } = new List<Clinica_Veterinaria>();

    }
}
