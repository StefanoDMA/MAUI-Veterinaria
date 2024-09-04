using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Log_Mascotas.Logica
{
    public class Log_Actualizar_Usuario
    {
        public Res_Actualizar_Usuario Actualizar_Usuario(Req_Actualizar_Usuario req)
        {
            Res_Actualizar_Usuario res = new Res_Actualizar_Usuario();

            // Validación de entrada
            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
            }
            else if (req.Id_Usuario == 0)
            {
                res.resultado = false;
                res.error = "No se recibieron datos del usuario";
            }
             
            if (string.IsNullOrEmpty(req.Nombre))
                {
              req.Nombre = string.Empty;
            }
             if (string.IsNullOrEmpty(req.Apellidos))
            {
                req.Apellidos = string.Empty;
            }
             if (string.IsNullOrEmpty(req.Correo_Electronico))
            {
                req.Correo_Electronico = string.Empty;
            }
             if (string.IsNullOrEmpty(req.Confirmacion_Correo_Electronico))
            {
                req.Confirmacion_Correo_Electronico = string.Empty;
            }
             if (string.IsNullOrEmpty(req.Password))
            {
                req.Password = string.Empty;
            }
            if (string.IsNullOrEmpty(req.Confirmar_Password))
            {
                req.Confirmar_Password = string.Empty;
            }
            
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    // Crear la instancia del contexto de datos
                    ConexionDataContext LINQ = new ConexionDataContext();

                    // Llamar al procedimiento almacenado
                    LINQ.SP_ACTUALIZAR_USUARIO(
                        req.Id_Usuario,
                        req.Nombre,
                        req.Apellidos,
                        req.Correo_Electronico,
                        req.Confirmacion_Correo_Electronico,
                        req.Password,
                        req.Confirmar_Password,
                        ref idReturn,
                        ref idError,
                        ref errorDescripcion
                    );

                    // Evaluar el resultado del procedimiento almacenado
                    if (idError != 0)
                    {
                        res.resultado = false;
                        res.error = string.IsNullOrEmpty(errorDescripcion)
                                    ? "Error al actualizar el usuario"
                                    : "Error al actualizar el usuario: " + errorDescripcion;
                    }
                    else if (idReturn != 1)
                    {
                        res.resultado = false;
                        res.error = "No se actualizó ningún registro";
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Usuario actualizado exitosamente";
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "Se produjo un error al actualizar el usuario: " + ex.Message;
                }
            }

            return res;
        }
    }
}
