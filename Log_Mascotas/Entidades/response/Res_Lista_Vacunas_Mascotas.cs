using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades
{
    public class Res_Lista_Vacunas_Mascotas:ResBase
    {

       public List<Vacunas_Mascotas> ListarVacunasMascotas = new List<Vacunas_Mascotas>();

    }
}
