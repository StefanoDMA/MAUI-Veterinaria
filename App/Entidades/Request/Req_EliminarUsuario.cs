using FrontEndHealthPets.Entidades.entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades
{
    public class Req_EliminarUsuario
    {

        public int Id_Usuario { get; set; }
        public string correoElectronico { get; set; }
        }
    }
