using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Ingresar_Vacunas_Mascotas
    {

        public Res_Vacunas_Mascotas IngresarVAcunasMascotas(Req_Vacunas_Mascotas req)
        {
            Res_Vacunas_Mascotas res = new Res_Vacunas_Mascotas();
            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibió la información necesaria para ingresar la vacuna";
            }
            else if (req.vacunas_Mascotas.Id_Mascota == 0)
            {
                res.resultado = false;
                res.error = "No se recibió el id de la mascota";
            }
            else if (req.vacunas_Mascotas.Id_Vacuna == 0)
            {
                res.resultado = false;
                res.error = "No se recibió el id de la vacuna";
            }else
            if (req.vacunas_Mascotas.Dosis < 0)
            {
                res.resultado = false;
                res.error = "la dosis no puede ser menor que 0";
            }
            else if (req.vacunas_Mascotas.Fecha_y_Hora_Aplicacion == default(DateTime))
            {
                res.resultado = false;
                res.error = "No se recibió la fecha de aplicación de la vacuna";
            }
            else if (req.vacunas_Mascotas.Fecha_y_Hora_Proxima_Aplicacion == default(DateTime))
            {
                res.resultado = false;
                res.error = "No se recibió la fecha de la próxima aplicación de la vacuna";
            }            
            else if (string.IsNullOrEmpty(req.vacunas_Mascotas.notas))
            {
                req.vacunas_Mascotas.notas = string.Empty;
            }
            else
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;
                   
                    {
                        ConexionDataContext LINQ = new ConexionDataContext();
                        LINQ.SP_INGRESAR_MASCOTAS_VACUNAS(req.vacunas_Mascotas.Id_Mascota, req.vacunas_Mascotas.Id_Vacuna,
                            req.vacunas_Mascotas.Dosis, req.vacunas_Mascotas.Fecha_y_Hora_Aplicacion, req.vacunas_Mascotas.Fecha_y_Hora_Proxima_Aplicacion,
                            req.vacunas_Mascotas.notas, ref idReturn, ref idError, ref errorDescripcion);

                        if (idReturn == -1)
                        {
                            res.resultado = false;
                            res.error = "Error al ingresar la vacuna: " + errorDescripcion;
                        }
                        else
                        {
                            res.resultado = true;
                            res.error = "Vacuna ingresada exitosamente";
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "Se produjo un error al ingresar la vacuna: " + ex.Message;
                }
            }
            return res;
        }
        public Res_Lista_Vacunas_Mascotas ListaVacunasMascotas(int id_mascota)
        {
            Res_Lista_Vacunas_Mascotas res = new Res_Lista_Vacunas_Mascotas();

            try
            {

                ConexionDataContext Linq = new ConexionDataContext();


                List<SP_OBTENER_LISTA_VACUNAS_MASCOTASResult > resultSet = new List<SP_OBTENER_LISTA_VACUNAS_MASCOTASResult>();
                resultSet = Linq.SP_OBTENER_LISTA_VACUNAS_MASCOTAS(id_mascota).ToList();


                foreach (SP_OBTENER_LISTA_VACUNAS_MASCOTASResult cadaResultSet in resultSet)
                {
                    res.resultado = true;
                    res.ListarVacunasMascotas.Add(this.fabricalistadeVacunasMascotas(cadaResultSet));
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = "Error en backend: " + ex.Message;
                res.error = "estoy callendo en el caht";
                res.error = $"Error en backend: {ex.Message}";

            }
            return res;
        }
        private Vacunas_Mascotas fabricalistadeVacunasMascotas(SP_OBTENER_LISTA_VACUNAS_MASCOTASResult Mascotas_VacunasLiqn)
        {
            Vacunas_Mascotas UnalistvacunasMascotas = new Vacunas_Mascotas();

            UnalistvacunasMascotas.Id_Mascota = Mascotas_VacunasLiqn.IDMASCOTA;
            UnalistvacunasMascotas.Nombre_Mascota = Mascotas_VacunasLiqn.NOMBRE_MASCOTA;
            UnalistvacunasMascotas.Id_Vacuna = Mascotas_VacunasLiqn.IDVACUNA ;
            UnalistvacunasMascotas.Nombre_Vacuna = Mascotas_VacunasLiqn.NOMBRE_VACUNA;
            UnalistvacunasMascotas.Descripcion = Mascotas_VacunasLiqn.DESCRIPCION_VACUNA;
            UnalistvacunasMascotas.Dosis = Mascotas_VacunasLiqn.DOSIS;
            UnalistvacunasMascotas.Fecha_y_Hora_Aplicacion = Mascotas_VacunasLiqn.FECHA_Y_HORA_PROXIMAAPLICACION;
            UnalistvacunasMascotas.Fecha_y_Hora_Proxima_Aplicacion = Mascotas_VacunasLiqn.FECHA_Y_HORA_PROXIMAAPLICACION;
            UnalistvacunasMascotas.notas = Mascotas_VacunasLiqn.COMENTARIOS;

            return UnalistvacunasMascotas;
        }

    }
}