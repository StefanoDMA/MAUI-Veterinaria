using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas
{
    public class Log_Agregar_Vacuna
    {

        public Res_Vacuna IngresarVacuna(Req_Vacunas req)
            {
               Res_Vacuna res = new Res_Vacuna();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";

            }
            else
             if (string.IsNullOrEmpty(req.Vacunas.Nombre))
            {
                res.resultado = false;
                res.error = "El nombre de la vacuna esta vacia";
            }
            else         
            if (string.IsNullOrEmpty(req.Vacunas.Decripcion))
            {
                req.Vacunas.Decripcion = string.Empty;
            }
            else
             if (req.Vacunas.FechaVencimiento == default(DateTime))
            {
                res.resultado = false;
                res.error = "La fecha de vencimiento de la vacuna esta vacia";
            }
            else

            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_INGRESAR_VACUNA(req.Vacunas.Nombre, req.Vacunas.Decripcion, req.Vacunas.FechaVencimiento, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al ingresar el usuario: " + errorDescripcion;
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
                    res.error = "Error al ingresar la vacuna: " + ex.Message;
                }
            }
            return res;
        }


        public Res_Lista_Vascunas ListaVacunas(Res_Lista_Vascunas req)
        {
            Res_Lista_Vascunas res = new Res_Lista_Vascunas();


            List<SP_OBTENER_LISTA_VACUNASResult> Results = new List<SP_OBTENER_LISTA_VACUNASResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_VACUNAS().ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_VACUNASResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.Lista_Vacunas.Add(this.fabricaListaVacunas(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private Vacunas fabricaListaVacunas(SP_OBTENER_LISTA_VACUNASResult lISTA_VACUNASlINQ)
        {
            Vacunas UnaListaVacunas = new Vacunas();

            UnaListaVacunas.Id_Vacuna = lISTA_VACUNASlINQ.ID_VACUNA;
            UnaListaVacunas.Nombre = lISTA_VACUNASlINQ.NOMBRE;
            UnaListaVacunas.Decripcion = lISTA_VACUNASlINQ.DESCRIPCION;
            UnaListaVacunas.FechaVencimiento = lISTA_VACUNASlINQ.FECHA_VENCIMIENTO;



            return UnaListaVacunas;

        }


    }
}

 