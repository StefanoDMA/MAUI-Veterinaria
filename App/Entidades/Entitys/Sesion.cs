using System;

namespace FrontEndHealthPets.Entidades.Entitys
{
    public static class Sesion
    {
        public static long Id_mascota { get; set; }
        public static long id_usuario { get; set; }
        public static string nombre { get; set; }
        public static string apellidos { get; set; }
        public static string Correo_Electronico { get; set; }
        public static string Password { get; set; }
        public static string token { get; set; }

        // Valida si la sesión está activa comprobando el token
        public static bool ValidarSesion()
        {
            return !string.IsNullOrEmpty(token);  // Simplificación del método
        }

        // Resetea todos los valores de la sesión
        public static void CerrarSesion()
        {
            Id_mascota = 0;
            id_usuario = 0;
            nombre = string.Empty;  // Cambiado a la forma más común en C#
            apellidos = string.Empty;
            Correo_Electronico = string.Empty;
            Password = string.Empty;
            token = string.Empty;
        }
    }
}
