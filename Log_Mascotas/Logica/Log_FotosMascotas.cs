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
    public class Log_FotosMascotas
    {
        public Res_FotosMascotas IngresarFotos(Req_FotosMascota req)
        {
            Res_FotosMascotas res = new Res_FotosMascotas();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";
            }
            else
            if (req.FotosMascotas.Foto == null || req.FotosMascotas.Foto.Length == 0)
            {
                res.resultado = false;
                res.error = "No se recibió la foto";
            }
            else
            if (req.FotosMascotas.Id_Mascota == default(int))
            {
                res.resultado = false;
                res.error = "No se recibió el ID de la mascota";
            }
            else
            {
                // No se verifica si la descripción es nula o vacía
                // Si no se recibe la descripción, se asigna un valor predeterminado
                if (string.IsNullOrEmpty(req.FotosMascotas.Descripcion))
                {
                    req.FotosMascotas.Descripcion = string.Empty; // o cualquier otro valor predeterminado
                }

                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_INGRESAR_FOTOS(req.FotosMascotas.Foto, req.FotosMascotas.Id_Mascota, req.FotosMascotas.Descripcion,
                    ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = "Error al ingresar la foto: " + errorDescripcion;
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
        public Res_Lista_Fotos ListaFotos(int id_mascota)
        {
            Res_Lista_Fotos res = new Res_Lista_Fotos ();


            List<SP_OBTENER_LISTA_FOTOS_MASCOTASResult> Results = new List<SP_OBTENER_LISTA_FOTOS_MASCOTASResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_FOTOS_MASCOTAS(id_mascota).ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_FOTOS_MASCOTASResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.Lista_Fotos.Add(this.fabricaListaVacunas(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private FotosMascota fabricaListaVacunas(SP_OBTENER_LISTA_FOTOS_MASCOTASResult lISTA_FOTOS_MASCOTASlINQ)
        {
            FotosMascota UnaListaFotos = new FotosMascota();

            UnaListaFotos.Id_Foto = lISTA_FOTOS_MASCOTASlINQ.ID_FOTO;
            UnaListaFotos.Foto = ConvertBinaryToByteArray(lISTA_FOTOS_MASCOTASlINQ.FOTO);
            UnaListaFotos.Descripcion = lISTA_FOTOS_MASCOTASlINQ.DESCRIPCION;



            return UnaListaFotos;

        }
        // Método auxiliar para convertir System.Data.Linq.Binary a byte[]
        private byte[] ConvertBinaryToByteArray(System.Data.Linq.Binary binary)
        {
            return binary.ToArray();
        }

    }
}