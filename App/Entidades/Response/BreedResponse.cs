using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.Response
{
    public class BreedResponse
    {
        public Dictionary<string, List<string>> Message { get; set; }
        public string Status { get; set; }
    }
}