using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Ingresar_Recordatorio
    {

        public Res_Ingresar_recordatorio IngresarRecordatorio(Req_Ingresar_Recordatorio req)
        {
            Res_Ingresar_recordatorio res = new Res_Ingresar_recordatorio();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
                return res;
            }
            else if (req.recordatorio.Id_Mascota == 0)
            {
                res.resultado = false;
                res.error = "Debe seleccionar una mascota";
                return res;
            }
            else
            {
                // Validación para asegurarse de que solo se pase un ID de una de las opciones
                int contador = 0;
                if (req.recordatorio.Id_Vacuna != 0) contador++;
                if (req.recordatorio.Id_Medicamentos != 0) contador++;
                if (req.recordatorio.Id_Baheiro != 0) contador++;
                if (req.recordatorio.Id_Cita != 0) contador++;

                if (contador != 1)
                {
                    res.resultado = false;
                    res.error = "Debe seleccionar solo una de las opciones (Vacuna, Cita, Medicamento o Baheiro)";
                    return res;
                }
                else
                {
                    // Asignar valores en blanco
                    req.recordatorio.Fecha = DateTime.MinValue;
                    req.recordatorio.Hora = TimeSpan.Zero;
                    req.recordatorio.Tipo = string.Empty;
                    req.recordatorio.Nombre = string.Empty;

                    try
                    {
                        int? idReturn = 0;
                        int? idError = 0;
                        string errorDescripcion = null;

                        ConexionDataContext Linq = new ConexionDataContext();
                        Linq.SP_INSERTAR_RECORDATORIO(req.recordatorio.Id_Mascota, req.recordatorio.Fecha,
                        req.recordatorio.Hora, req.recordatorio.Descripcion, req.recordatorio.Tipo, req.recordatorio.Nombre,
                        req.recordatorio.Id_Vacuna, req.recordatorio.Id_Medicamentos, req.recordatorio.Id_Cita,
                        req.recordatorio.Id_Baheiro, ref idReturn, ref idError, ref errorDescripcion);

                        if (idReturn == -1)
                        {
                            res.resultado = false;
                            res.error = "Error al ingresar el recordatorio: " + errorDescripcion;
                        }
                        else
                        {
                            res.resultado = true;
                            res.error = "Registro exitoso";
                        }

                    }
                    catch (Exception ex)
                    {
                        res.resultado = false;
                        res.error = "Fallo en base de datos " + ex.Message;
                    }

                    return res;
                }
            }
        }
    }
}