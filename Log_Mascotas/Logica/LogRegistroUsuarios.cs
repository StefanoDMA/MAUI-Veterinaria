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
    public class LogIngresarUsuario
    {
        public Res_Usuario IngresarUsuario(Req_Usuario req)
        {
            Res_Usuario res = new Res_Usuario();


            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibió la información necesaria para ingresar el usuario";

            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Nombre))

            {
                res.resultado = false;
                res.error = "No se recibió el nombre del usuario";

            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Apellidos))
            {
                res.resultado = false;
                res.error = "No se recibió el apellido del usuario";
            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Correo_Electronico))

            {
                res.resultado = false;
                res.error = "No se recibió el correo electrónico del usuario";

            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Confirmacion_Correo_Electronico))
            {
                res.resultado = false;
                res.error = "No se recibió la confirmación del correo electrónico del usuario";
            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Password))
            {
                res.resultado = false;
                res.error = "No se recibió la contraseña del usuario";

            }
            else
            if (String.IsNullOrEmpty(req.registro_Usuario.Confirmar_Password))
            {
                res.resultado = false;
                res.error = "No se recibió la confirmación de la contraseña del usuario";
            }
            else
            if (req.registro_Usuario.numero_verificacion == null)
            {
                res.resultado = false;
                res.error = "No se recibió el número de verificación";
            }
            else
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext LINQ = new ConexionDataContext();
                    LINQ.SP_INGRESAR_USUARIO(req.registro_Usuario.Nombre, req.registro_Usuario.Apellidos, req.registro_Usuario.Correo_Electronico,
req.registro_Usuario.Confirmacion_Correo_Electronico, req.registro_Usuario.Confirmar_Password, req.registro_Usuario.Password,
req.registro_Usuario.numero_verificacion, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al ingresar el usuario: " + errorDescripcion;
                    }
                    else
                    {
                        // Llama al método ActivarUsuario después de registrar al usuario
                        var activarRes = ActivarUsuario(req.registro_Usuario.Correo_Electronico, req.registro_Usuario.numero_verificacion);

                        if (activarRes.resultado)
                        {
                            res.resultado = true;
                            res.error = "Registro y activación exitosos";
                        }
                        else
                        {
                            res.resultado = false;
                            res.error = "Registro exitoso pero no se pudo activar el usuario: " + activarRes.error;
                        }
                    }


                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "se cayo la bd no se por que: " + ex.Message;
                }

                

            }

            return res;
        }

        private ResActivarUsuario ActivarUsuario(string correoElectronico, string numeroVerificacion)
        {
            var res = new ResActivarUsuario();

            try
            {
                int? idReturn = 0;
                int? errorId = 0;
                string errorDescripcion = null;
                int? filasActualizadas = 0;

                ConexionDataContext LINQ = new ConexionDataContext();
                LINQ.SP_ACTIVAR_USUARIO(correoElectronico, numeroVerificacion, ref idReturn, ref errorId, ref errorDescripcion, ref filasActualizadas);


                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al activar el usuario: " + errorDescripcion;
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Usuario activado exitosamente";
                    }
                
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = "Se cayó la BD, no se sabe por qué: " + ex.Message;
            }

            return res;
        }
    }

    public class ResActivarUsuario
    {
        public bool resultado { get; set; }
        public string error { get; set; }
    }
}

   