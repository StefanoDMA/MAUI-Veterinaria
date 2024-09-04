using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_RecuperarPassword
    {

        public Res_RecuperarPassword RecuperarPassword(Req_RecuperarPassword req)
        {
            Res_RecuperarPassword res = new Res_RecuperarPassword();

            if (req == null)
            {
                res.resultado = false;
                res.error = "campos vacios";
            }
            else
            {
                if (String.IsNullOrEmpty(req.recuperarPassword.CorreoElectronico))
                {
                    res.resultado = false;
                    res.error = "Correo no existente";
                }
                else
                {
                    if (String.IsNullOrEmpty(req.recuperarPassword.NuevoPassword))
                    {
                        res.resultado = false;
                        res.error = "nueva contraseña vacia";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(req.recuperarPassword.ConfirmarPassword))
                        {
                            res.resultado = false;
                            res.error = "confirmacion de contrasena vacia";
                        }
                        
                        else
                        {
                            try
                            {
                                ConexionDataContext Linq = new ConexionDataContext();
                                Linq.SP_RESTABLECER_PASSWORD(req.recuperarPassword.CorreoElectronico, req.recuperarPassword.NuevoPassword, req.recuperarPassword.ConfirmarPassword);

                                res.resultado = true;
                                res.error = "contraseña restablecida con éxito";
                            }
                            catch (Exception ex)
                            {
                                res.resultado = false;
                                res.error = "fallo en base de datos " + ex.Message;
                            }
                        }
                    }
                }
            }

            return res;
        }
    }
}
