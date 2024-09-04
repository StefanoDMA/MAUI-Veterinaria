using FrontEndHealthPets.Entidades.entitys;
using FrontEndHealthPets.Paginas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndHealthPets.Entidades.Response;
using FrontEndHealthPets.Entidades.Entitys;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_Lista_Doctor : Res_Base
    {

        public List<Doctor> ListaDoctor { get; set; }

    }
}
