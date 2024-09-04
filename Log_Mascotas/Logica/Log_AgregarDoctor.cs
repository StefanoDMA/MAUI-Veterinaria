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
    public class Log_AgregarDoctor
    {

        public Res_AgregarDoctor IngresarDoctor(Req_AgregarDoctor req)
        {
            Res_AgregarDoctor res = new Res_AgregarDoctor();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
            }else
                if(string.IsNullOrEmpty(req.Doctor.Nombre))
                {
                   res.resultado=false;
                    res.error = "No se recibieron datos";

            }else
                if(string.IsNullOrEmpty(req.Doctor.Telefono))
            {
                res.resultado = false;
                res.error = "No se recibieron datos";

            }
            else
                if(string.IsNullOrEmpty(req.Doctor.Correo_Electronico))
            {
                res.resultado = false;
                res.error = "No se recibieron datos";

            }
            else


                

            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;
                   

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_INGRESAR_DOCTOR(req.Doctor.Nombre, req.Doctor.Telefono, req.Doctor.Correo_Electronico, 
                    ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al ingresar el Baheiro: " + errorDescripcion;
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "registro exitoso";
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "fallo en base de datos " + ex.Message;
                }
            }



            return res;
        }

    }
}



