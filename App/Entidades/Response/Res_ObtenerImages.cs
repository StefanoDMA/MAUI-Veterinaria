using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.response
{
    public class Res_ObtenerImages
    {
        [JsonProperty("message")]
        public string Image { get; set; }  // Cambiar de List<string> a string
        public object Message { get; set; }
    }
}

