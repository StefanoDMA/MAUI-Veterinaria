using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Entidades.Response
{
    public class ImageResponse
    {
        public List<string> Message { get; set; } // Cambia a 'List<string>' si la API devuelve una lista de URLs
        public string Status { get; set; }
    }
}