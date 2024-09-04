using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Baheiro_Mascotas
    {
        public Res_Baheiro_Mascotas IngresarBaheriosMascotas(Req_Baheiro_Mascotas req)
        {
            Res_Baheiro_Mascotas res = new Res_Baheiro_Mascotas();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
            }
            else
            if (req.baheiroMascotas.Id_Mascota == default(int))
            {
                res.resultado = false;
                res.error = "No se recibió el ID de la mascota";
            }
            else
            if (req.baheiroMascotas.Id_Baheiro == 0)
            {
                res.resultado = false;
                res.error = "No se recibió el ID del baño";
            }
            else
             if (req.baheiroMascotas.Fecha_y_hora_Baheiro == default(DateTime))
            {
                res.resultado = false;
                res.error = "No se recibió la fecha del ultimo baño";
            }else
             if (req.baheiroMascotas.Fecha_y_hora_proximo_Baheiro == default(DateTime))
            {
                res.resultado = false;
                res.error = "No se recibió la fecha del proximo baño";
            }else          
            if (string.IsNullOrEmpty(req.baheiroMascotas.Notas))
            {
                req.baheiroMascotas.Notas = string.Empty;
            }


            else
            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_BAHEIROS_MASCOTAS(req.baheiroMascotas.Id_Mascota, req.baheiroMascotas.Id_Baheiro, 
                    req.baheiroMascotas.Fecha_y_hora_Baheiro, req.baheiroMascotas.Fecha_y_hora_proximo_Baheiro,
                    req.baheiroMascotas.Notas, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = true;
                        res.error = "Error al ingresar el baño: " + errorDescripcion;
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Registro exitoso todo salio bien";
                    }
                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = "Error al ingresar el baño: " + ex.Message;
                }
            }

            return res;
        }

        public Res_ListaBaheiros_Mascotas ListaBaheriosMascotas(int id_mascota)
        {
            Res_ListaBaheiros_Mascotas res = new Res_ListaBaheiros_Mascotas();
          
          try
            {
               


                ConexionDataContext Linq = new ConexionDataContext();
                

                List<SP_OBTENER_LISTA_BAHEIROS_MASCOTASResult> resultSet = new List<SP_OBTENER_LISTA_BAHEIROS_MASCOTASResult>();
                resultSet = Linq.SP_OBTENER_LISTA_BAHEIROS_MASCOTAS(id_mascota).ToList();



                foreach (SP_OBTENER_LISTA_BAHEIROS_MASCOTASResult cadaResultSet in resultSet)
                {
                    res.resultado = true;
                    res.ListarBaheirosMascotas.Add(this.fabricalistadebaños(cadaResultSet));
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
        private BaheiroMascotas fabricalistadebaños(SP_OBTENER_LISTA_BAHEIROS_MASCOTASResult bAHEIROS_MASCOTASlinq)
        {
            BaheiroMascotas unalistabaños = new BaheiroMascotas();
          
            unalistabaños.Id_Mascota = bAHEIROS_MASCOTASlinq.ID_MASCOTA ?? 0 ;
            unalistabaños.Nombre_Mascota = bAHEIROS_MASCOTASlinq.NOMBRE_MASCOTA;
            unalistabaños.Id_Baheiro = bAHEIROS_MASCOTASlinq.ID_BAHEIRO ?? 0;            
            unalistabaños.Nombre_Baheiro = bAHEIROS_MASCOTASlinq.NOMBRE_BAHEIRO;
            unalistabaños.Descripcion_Baheiro = bAHEIROS_MASCOTASlinq.DESCRIPCION_BAHEIRO;
            unalistabaños.Fecha_y_hora_Baheiro = bAHEIROS_MASCOTASlinq.FECHA_Y_HORA_BAHEIRO;
            unalistabaños.Fecha_y_hora_proximo_Baheiro = bAHEIROS_MASCOTASlinq.FECHA_Y_HORA_PROXIMO_BAHEIRO;
            unalistabaños.Notas = bAHEIROS_MASCOTASlinq.NOTAS;
             

            return unalistabaños;
        }

    }
}