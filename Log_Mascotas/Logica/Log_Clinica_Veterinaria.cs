using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.entitys;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Diagnostics; // Asegúrate de agregar esta referencia

namespace Log_Mascotas.Logica
{
    public class Log_Clinica_Veterinaria
    {
        private readonly Log_AgregarDoctor _logAgregarDoctor;

        public Log_Clinica_Veterinaria(Log_AgregarDoctor logAgregarDoctor)
        {
            _logAgregarDoctor = logAgregarDoctor;
        }

        public Res_Clinica_Veterinaria IngresarClinicaVeterinaria(Req_Clinica_Veterinaria req)
        { 
            Res_Clinica_Veterinaria res = new Res_Clinica_Veterinaria();

            // Validaciones básicas
            if (req == null)
            {
                res.resultado = false;
                res.error = "No hay datos";
                Debug.WriteLine("Error: " + res.error);
                return res;
            }

            if (string.IsNullOrEmpty(req.clinica_Veterinaria.Nombre_Clinica))
            {
                res.resultado = false;
                res.error = "No hay nombre de clínica";
                Debug.WriteLine("Error: " + res.error);
                return res;
            }

            if (req.clinica_Veterinaria.id_Doctor == default(int))
            {
                res.resultado = false;
                res.error = "No hay ID de doctor";
                Debug.WriteLine("Error: " + res.error);
                return res;
            }

            if (string.IsNullOrEmpty(req.clinica_Veterinaria.Direccion))
            {
                res.resultado = false;
                res.error = "No hay dirección";
                Debug.WriteLine("Error: " + res.error);
                return res;
            }

            if (string.IsNullOrEmpty(req.clinica_Veterinaria.Telefono))
            {
                res.resultado = false;
                res.error = "No hay teléfono";
                Debug.WriteLine("Error: " + res.error);
                return res;
            }

            // Verificar que el doctor existe
            if (!VerificarDoctorExistente(req.clinica_Veterinaria.id_Doctor))
            {
                res.resultado = false;
                res.error = "El doctor no existe";
                Debug.WriteLine("Error al verificar doctor: " + res.error);
                return res;
            }


            // Insertar la clínica veterinaria
            try
            {
                int? idReturn = 0;
                int? idError = 0;
                string errorDescripcion = null;

                Debug.WriteLine("Iniciando inserción de clínica veterinaria...");
                using (var lINQ = new ConexionDataContext())
                {
                    Debug.WriteLine("Llamando a SP_CLINICA_VETERINARIA con parámetros:");
                    Debug.WriteLine($"Nombre_Clinica: {req.clinica_Veterinaria.Nombre_Clinica}");
                    Debug.WriteLine($"id_Doctor: {req.clinica_Veterinaria.id_Doctor}");
                    Debug.WriteLine($"Direccion: {req.clinica_Veterinaria.Direccion}");
                    Debug.WriteLine($"Telefono: {req.clinica_Veterinaria.Telefono}");

                    lINQ.SP_CLINICA_VETERINARIA(
                        req.clinica_Veterinaria.Nombre_Clinica, 
                        req.clinica_Veterinaria.id_Doctor, 
                        req.clinica_Veterinaria.Direccion, 
                        req.clinica_Veterinaria.Telefono, 
                        ref idReturn, 
                        ref idError, 
                        ref errorDescripcion
                    );

                    Debug.WriteLine($"Resultado de SP_CLINICA_VETERINARIA - idReturn: {idReturn}");
                    Debug.WriteLine($"Error de SP_CLINICA_VETERINARIA - idError: {idError}");
                    Debug.WriteLine($"Descripción de error: {errorDescripcion}");

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = errorDescripcion;
                        Debug.WriteLine("Error en inserción de clínica veterinaria: " + res.error);

                        // Insertar la asociación entre clínica y doctor
                        Debug.WriteLine("Insertando asociación entre clínica y doctor...");
                        lINQ.SP_INSERTAR_CLINICA_DOCTOR((int)idReturn, req.clinica_Veterinaria.id_Doctor);
                        Debug.WriteLine("Asociación insertada correctamente.");
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Se ingresó correctamente";
                        Debug.WriteLine("Clínica veterinaria ingresada correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = "Error al ingresar la clínica veterinaria: " + ex.Message;
                Debug.WriteLine("Excepción: " + ex.Message);
            }

            return res;
        }

        private bool VerificarDoctorExistente(int idDoctor)
        {
            using (var lINQ = new ConexionDataContext())
            {
                // Agrega un mensaje de depuración antes de llamar al procedimiento almacenado
                Debug.WriteLine($"Verificando existencia del doctor con ID: {idDoctor}");

                // Llamar al procedimiento almacenado y pasar el parámetro
                var resultado = lINQ.SP_OBTENER_LISTA_DOCTOR(idDoctor).FirstOrDefault();

                // Agrega mensajes de depuración para verificar el resultado
                if (resultado != null)
                {
                    Debug.WriteLine($"Doctor encontrado: ID = {resultado.ID_DOCTOR}, Nombre = {resultado.NOMBRE}");
                    return true;
                }
                else
                {
                    Debug.WriteLine("Doctor no encontrado.");
                    return false;
                }
            }
        }


        public Res_Lista_Clinica_Veterinaria ListaClinicasVeterinarias(Res_Lista_Clinica_Veterinaria req
            )
        {
            Res_Lista_Clinica_Veterinaria res = new Res_Lista_Clinica_Veterinaria();


            List<SP_OBTENER_LISTA_CLINICASResult> Results = new List<SP_OBTENER_LISTA_CLINICASResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_CLINICAS().ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_CLINICASResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.Lista_Clinica_Veterinaria.Add(this.fabricaListaCLinicas(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private Clinica_Veterinaria fabricaListaCLinicas(SP_OBTENER_LISTA_CLINICASResult lISTA_CLINICA_VETERINARIALINQ)
        {
            Clinica_Veterinaria unaListaclinicas = new Clinica_Veterinaria();

             unaListaclinicas.Id_Clinica_Veterinaria = lISTA_CLINICA_VETERINARIALINQ.ID_CLINICA_VETERINARIA;
            unaListaclinicas.Nombre_Clinica = lISTA_CLINICA_VETERINARIALINQ.NOMBRE_CLINICA_VETERINARIA;
           
            unaListaclinicas.Direccion = lISTA_CLINICA_VETERINARIALINQ.DIRECCION;
            unaListaclinicas.Telefono = lISTA_CLINICA_VETERINARIALINQ.TELEFONO;
                

            return unaListaclinicas;

        }


    }
}