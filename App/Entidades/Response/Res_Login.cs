using FrontEndHealthPets.Entidades.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.Response
{
   public class Res_Login: Res_Base
    {

        public Registro_Usuario Registro_Usuario { get; set; }

    }
}
