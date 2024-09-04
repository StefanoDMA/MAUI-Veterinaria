﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Entidades
{
    public class Registro_Mascota
    {
        public int Id_Mascota { get; set; }
        public int id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string especie { get; set; }     
        
        public string raza { get; set; }

        public DateTime Fecha_Nacimiento { get; set; }

        public DateTime Fecha_Proximo_Baheiro { get; set; }


    }
}
