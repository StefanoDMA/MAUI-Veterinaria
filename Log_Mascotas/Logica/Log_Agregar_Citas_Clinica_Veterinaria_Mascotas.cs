using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Agregar_Citas_Clinica_Veterinaria_Mascotas
    {

        public Res_Citas_Clinica_Veterinaria_Mascotas IngresarCitasClinicaVeterinaria(Req_Citas_Clinica_Veterinaria_Mascotas req)
        {
            Res_Citas_Clinica_Veterinaria_Mascotas res = new Res_Citas_Clinica_Veterinaria_Mascotas();

            if (req == null)
            {
                res.resultado = false;
                res.error = "no hay datos";
            }
            else
                if (req.citas_Clinica_Veterinaria_Mascotas.Id_Mascota == default(int))
            {
                res.resultado = false;
                res.error = "no hay id de mascota";
                Debug.WriteLine($"Error: ID de mascota no proporcionado. Valor: {req.citas_Clinica_Veterinaria_Mascotas.Id_Mascota}");
            }
            else
             if (req.citas_Clinica_Veterinaria_Mascotas.Id_Clinica == default(int))
            {
                res.resultado = false;
                res.error = "no hay id de clinica veterinaria";
                Debug.WriteLine($"Error: ID de clínica veterinaria no proporcionado. Valor: {req.citas_Clinica_Veterinaria_Mascotas.Id_Clinica}");
            }
            else
            if (req.citas_Clinica_Veterinaria_Mascotas.Id_Doctor == default(int))
            {
                res.resultado = false;
                res.error = "no hay id de doctor";
                Debug.WriteLine($"Error: ID de doctor no proporcionado. Valor: {req.citas_Clinica_Veterinaria_Mascotas.Id_Doctor}");
            }
            else
                if (req.citas_Clinica_Veterinaria_Mascotas.Fecha_y_hora_Cita == default(DateTime))
            {
                res.resultado = false;
                res.error = "no hay fecha de cita";
            }
            else
              if (string.IsNullOrEmpty(req.citas_Clinica_Veterinaria_Mascotas.Descripcion))
            {
                res.resultado = false;
                res.error = "no hay descripcion";
            }
            else

                if (string.IsNullOrEmpty(req.citas_Clinica_Veterinaria_Mascotas.Notas))
            {
                req.citas_Clinica_Veterinaria_Mascotas.Notas = string.Empty;
            }
            else
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext lINQ = new ConexionDataContext();
                    lINQ.SP_CITAS__CLINICA_VETERINARIA_MASCOTAS(req.citas_Clinica_Veterinaria_Mascotas.Id_Mascota,
                    req.citas_Clinica_Veterinaria_Mascotas.Id_Clinica,
                    req.citas_Clinica_Veterinaria_Mascotas.Id_Doctor, req.citas_Clinica_Veterinaria_Mascotas.Fecha_y_hora_Cita, req.citas_Clinica_Veterinaria_Mascotas.Descripcion,
                    req.citas_Clinica_Veterinaria_Mascotas.Notas, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = errorDescripcion;
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "todo salio bien " + ex.Message;
                }
            }

            return res;
        }

        public Res_Lista_Citas_Veterinaria_mascotas IngresarListaCitasVeterinarias(int id_mascota)
        {
            Res_Lista_Citas_Veterinaria_mascotas res = new Res_Lista_Citas_Veterinaria_mascotas();
            List<SP_OBTENER_LISTA_CITAS_CLINICA_VETERINARIAResult> Results = new List<SP_OBTENER_LISTA_CITAS_CLINICA_VETERINARIAResult>();

            try
            {
                Debug.WriteLine($"Consultando citas para ID_MASCOTA: {id_mascota}");

                Results = new ConexionDataContext().SP_OBTENER_LISTA_CITAS_CLINICA_VETERINARIA(id_mascota).ToList();

                Debug.WriteLine($"Número de citas encontradas: {Results.Count}");

                foreach (SP_OBTENER_LISTA_CITAS_CLINICA_VETERINARIAResult cadaResultSet in Results)
                {
                    Debug.WriteLine($"Procesando cita ID: {cadaResultSet.ID_CITA}, Fecha: {cadaResultSet.FECHA_Y_HORA}");

                    res.Lista_Citas_Veterinaria_Mascotas.Add(this.fabricaListaCitasVeterinarias(cadaResultSet));
                }
                 
                res.resultado = true;
                Debug.WriteLine("Procesamiento de citas completado exitosamente.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener citas: {ex.Message}");
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;
        }

        private Citas_Clinica_Veterinaria_Mascotas fabricaListaCitasVeterinarias(SP_OBTENER_LISTA_CITAS_CLINICA_VETERINARIAResult lISTA_CITAS_CLINICA_VETERINARIALINQ)
        {
            Debug.WriteLine("Iniciando la fabricación de Citas_Clinica_Veterinaria_Mascotas.");

            Citas_Clinica_Veterinaria_Mascotas unaListaCitas = new Citas_Clinica_Veterinaria_Mascotas();

            // Depuración de cada propiedad
            Debug.WriteLine($"ID_MASCOTA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_MASCOTA}");
            Debug.WriteLine($"NOMBRE_MASCOTA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_MASCOTA}");
            Debug.WriteLine($"ID_CLINICA_VETERINARIA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_CLINICA_VETERINARIA}");
            Debug.WriteLine($"NOMBRE_DE_LA_CLINICA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_DE_LA_CLINICA}");
            Debug.WriteLine($"DIRECCION_DE_LA_CLINICA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.DIRECCION_DE_LA_CLINICA}");
            Debug.WriteLine($"TELEFONO: {lISTA_CITAS_CLINICA_VETERINARIALINQ.TELEFONO}");
            Debug.WriteLine($"ID_DOCTOR: {lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_DOCTOR}");
            Debug.WriteLine($"NOMBRE_DEL_DOCTOR: {lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_DEL_DOCTOR}");
            Debug.WriteLine($"FECHA_Y_HORA: {lISTA_CITAS_CLINICA_VETERINARIALINQ.FECHA_Y_HORA}");
            Debug.WriteLine($"DESCRIPCION: {lISTA_CITAS_CLINICA_VETERINARIALINQ.DESCRIPCION}");
            Debug.WriteLine($"NOTAS: {lISTA_CITAS_CLINICA_VETERINARIALINQ.NOTAS}");

            // Asignación de propiedades al objeto destino
            unaListaCitas.Id_Mascota = lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_MASCOTA ?? 0;
            unaListaCitas.Nombre_Mascota = lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_MASCOTA;
            unaListaCitas.Id_Clinica = lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_CLINICA_VETERINARIA ?? 0;
            unaListaCitas.Nombre_Clinica = lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_DE_LA_CLINICA;
            unaListaCitas.Id_Doctor = lISTA_CITAS_CLINICA_VETERINARIALINQ.ID_DOCTOR ?? 0;
            unaListaCitas.Direccion = lISTA_CITAS_CLINICA_VETERINARIALINQ.DIRECCION_DE_LA_CLINICA;
            unaListaCitas.Telefono = lISTA_CITAS_CLINICA_VETERINARIALINQ.TELEFONO;
            unaListaCitas.Nombre_Doctor = lISTA_CITAS_CLINICA_VETERINARIALINQ.NOMBRE_DEL_DOCTOR;
            unaListaCitas.Fecha_y_hora_Cita = lISTA_CITAS_CLINICA_VETERINARIALINQ.FECHA_Y_HORA;
            unaListaCitas.Descripcion = lISTA_CITAS_CLINICA_VETERINARIALINQ.DESCRIPCION;
            unaListaCitas.Notas = lISTA_CITAS_CLINICA_VETERINARIALINQ.NOTAS;

            Debug.WriteLine("Finalizada la fabricación de Citas_Clinica_Veterinaria_Mascotas.");

            return unaListaCitas;
        }
    }
}
