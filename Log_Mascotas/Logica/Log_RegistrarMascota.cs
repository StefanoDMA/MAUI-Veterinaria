using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_RegistrarMascota
    {
        public Res_Mascota RegistrarMascota(Req_Mascota req)
        {
            Res_Mascota res = new Res_Mascota();

            // Registro de datos recibidos
            Debug.WriteLine("Datos recibidos en RegistrarMascota:");
            Debug.WriteLine($"Nombre: {req?.Registro_Mascota?.Nombre}");
            Debug.WriteLine($"Especie: {req?.Registro_Mascota?.especie}");
            Debug.WriteLine($"Raza: {req?.Registro_Mascota?.raza}");
            Debug.WriteLine($"Fecha de Nacimiento: {req?.Registro_Mascota?.Fecha_Nacimiento}");
            Debug.WriteLine($"ID Usuario: {req?.Registro_Mascota?.id_Usuario}");

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
                Debug.WriteLine("Error: No se recibieron datos");
            }
            else if (req.Registro_Mascota.id_Usuario == default(int))
            {
                res.resultado = false;
                res.error = "No se recibió el id del usuario";
                Debug.WriteLine("Error: No se recibió el id del usuario");
            }
            else if (string.IsNullOrEmpty(req.Registro_Mascota.Nombre))
            {
                res.resultado = false;
                res.error = "No se recibió el nombre de la mascota";
                Debug.WriteLine("Error: No se recibió el nombre de la mascota");
            }
            else if (string.IsNullOrEmpty(req.Registro_Mascota.especie))
            {
                res.resultado = false;
                res.error = "No se recibió la especie de la mascota";
                Debug.WriteLine("Error: No se recibió la especie de la mascota");
            }
            else if (string.IsNullOrEmpty(req.Registro_Mascota.raza))
            {
                res.resultado = false;
                res.error = "No se recibió la raza de la mascota";
                Debug.WriteLine("Error: No se recibió la raza de la mascota");
            }
            else if (req.Registro_Mascota.Fecha_Nacimiento == default(DateTime))
            {
                res.resultado = false;
                res.error = "No se recibió la fecha de nacimiento de la mascota";
                Debug.WriteLine("Error: No se recibió la fecha de nacimiento de la mascota");
            }
            else
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;
                    int? idmascota = 0;

                    // Registra los valores de los parámetros que se pasan al SP
                    Debug.WriteLine("Llamada al SP con los siguientes parámetros:");
                    Debug.WriteLine($"ID Usuario: {req.Registro_Mascota.id_Usuario}");
                    Debug.WriteLine($"Nombre: {req.Registro_Mascota.Nombre}");
                    Debug.WriteLine($"Especie: {req.Registro_Mascota.especie}");
                    Debug.WriteLine($"Raza: {req.Registro_Mascota.raza}");
                    Debug.WriteLine($"Fecha de Nacimiento: {req.Registro_Mascota.Fecha_Nacimiento}");

                    ConexionDataContext LINQ = new ConexionDataContext();
                    LINQ.SP_INGRESAR_MASCOTA( req.Registro_Mascota.id_Usuario, req.Registro_Mascota.Nombre, req.Registro_Mascota.especie, req.Registro_Mascota.raza,
                    req.Registro_Mascota.Fecha_Nacimiento, ref idmascota, ref idReturn, ref idError, ref errorDescripcion);

                    // Registra el resultado y los detalles de cualquier error
                    Debug.WriteLine($"ID Return: {idReturn}");
                    Debug.WriteLine($"ID Error: {idError}");
                    Debug.WriteLine($"Descripción del Error: {errorDescripcion}");
                    Debug.WriteLine($"ID Mascota recuperado: {idmascota}");

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al ingresar la mascota: " + errorDescripcion;
                        Debug.WriteLine("Error al ingresar la mascota: " + errorDescripcion);
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Registro exitoso";
                        res.Id_Mascota = idmascota ?? 0; // Asigna el ID de la mascota a la respuesta
                        Debug.WriteLine("Registro exitoso");
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "Se produjo un error: " + ex.Message;
                    Debug.WriteLine("Excepción: " + ex.Message);
                }
            }

            return res;
       
        }

        public Res_Lista_mascotas ListaMascotas(int id_usuario)
        {
           Res_Lista_mascotas res = new Res_Lista_mascotas();


            List<SP_OBTENER_LISTA_MASCOTASResult> Results = new List<SP_OBTENER_LISTA_MASCOTASResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_MASCOTAS(id_usuario).ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_MASCOTASResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.ListaMascotas.Add(this.fabricaListaCitasVeterinarias(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private Registro_Mascota fabricaListaCitasVeterinarias(SP_OBTENER_LISTA_MASCOTASResult lISTA_MASCOTASLinq)
        {
            Registro_Mascota UnaListamascotas = new Registro_Mascota();

             UnaListamascotas.Id_Mascota = lISTA_MASCOTASLinq.ID_MASCOTA;
            UnaListamascotas.id_Usuario = lISTA_MASCOTASLinq.ID_USUARIO;
            UnaListamascotas.Nombre = lISTA_MASCOTASLinq.NOMBRE;
            UnaListamascotas.especie = lISTA_MASCOTASLinq.ESPECIE;
            UnaListamascotas.raza = lISTA_MASCOTASLinq.RAZA;
            UnaListamascotas.Fecha_Nacimiento = lISTA_MASCOTASLinq.FECHA_NACIMIENTO;
            UnaListamascotas.Fecha_Proximo_Baheiro = lISTA_MASCOTASLinq.FECHA_Y_HORA_PROXIMO_BAHEIRO ?? DateTime.MinValue;






            return UnaListamascotas;

        }

    }
}