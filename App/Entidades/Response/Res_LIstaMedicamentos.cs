using FrontEndHealthPets.Entidades.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_LIstaMedicamentos:Res_Base
    {

        public List<Medicamentos> ListarMedicamentos { get; set; } = new List<Medicamentos>();

    }
}
