using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using System;

namespace Log_Mascotas.Logica
{
    public class Log_EliminarUsuario
    {
        public Res_EliminarUsuario EliminarUsuario(int idUsuario, string correoElectronico)
        {
            var res = new Res_EliminarUsuario();

            if (idUsuario <= 0)
            {
                res.resultado = false;
                res.error = "ID de usuario inválido.";
                return res;
            }

            if (string.IsNullOrEmpty(correoElectronico))
            {
                res.resultado = false;
                res.error = "El correo electrónico es obligatorio.";
                return res;
            }

            try
            {
                int? idReturn = 0;
                int? idError = 0;
                string errorDescripcion = null;

                using (var LINQ = new ConexionDataContext())
                {
                    // Llamada al procedimiento almacenado para eliminar el usuario
                    LINQ.SP_ELIMINAR_USUARIO(idUsuario, correoElectronico, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn.HasValue && idReturn.Value == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al eliminar el usuario: " + (errorDescripcion ?? "Descripción de error no disponible.");
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Usuario eliminado exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = "Se produjo un error al eliminar el usuario: " + ex.Message;
            }

            return res;
        }
    }
}