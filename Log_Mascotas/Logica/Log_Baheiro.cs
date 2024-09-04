using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Baheiro
    {

        public Res_Baheiro AgregarBaherio(Req_Baheiro req)
        {
            Res_Baheiro res = new Res_Baheiro();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";

            }
            else
             if (string.IsNullOrEmpty(req.baheiro.TipoDe_Baheiro))
            {
                res.resultado = false;
                res.error = "no hay datos de baheiro";
            }
            else
                if (String.IsNullOrEmpty(req.baheiro.Descripcion))
            {
                req.baheiro.Descripcion = string.Empty;
            }

            else

            {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_AGREGAR_BAHEIRO(req.baheiro.TipoDe_Baheiro, req.baheiro.Descripcion, ref idReturn, ref idError, ref errorDescripcion);

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

        public Res_Lista_Baheiros ListaBaheiros(Res_Lista_Baheiros req)
        {
            Res_Lista_Baheiros res = new Res_Lista_Baheiros();

            try
            {
                Debug.WriteLine("Iniciando la obtención de la lista de Baheiros...");

                List<SP_OBTENER_LISTA_BAHEIROSResult> Results = new ConexionDataContext().SP_OBTENER_LISTA_BAHEIROS().ToList();
                Debug.WriteLine("Cantidad de resultados obtenidos: " + Results.Count);

                foreach (SP_OBTENER_LISTA_BAHEIROSResult cadaResultSet in Results)
                {
                    Debug.WriteLine("Procesando ID_BAHEIRO: " + cadaResultSet.ID_BAHEIRO);
                    res.resultado = true;
                    res.ListaBaheiros.Add(this.fabricaListaBaheiros(cadaResultSet));
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
                Debug.WriteLine("Error: " + ex.Message);
            }

            return res;
        }

        private Baheiro fabricaListaBaheiros(SP_OBTENER_LISTA_BAHEIROSResult LISTA_BAHEIROlINQ)
        {
            Baheiro UnaListaBaheiros = new Baheiro();

            try
            {
                Debug.WriteLine("Asignando valores a Baheiro...");
                UnaListaBaheiros.Id_Baheiro = LISTA_BAHEIROlINQ.ID_BAHEIRO;
                UnaListaBaheiros.TipoDe_Baheiro = LISTA_BAHEIROlINQ.TIPO_DE_BAHEIRO;
                UnaListaBaheiros.Descripcion = LISTA_BAHEIROlINQ.DESCRIPCION;
                Debug.WriteLine("Valores asignados: Id_Baheiro = " + UnaListaBaheiros.Id_Baheiro +
                                ", TipoDe_Baheiro = " + UnaListaBaheiros.TipoDe_Baheiro +
                                ", Descripcion = " + UnaListaBaheiros.Descripcion);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al asignar valores a Baheiro: " + ex.Message);
                throw;
            }

            return UnaListaBaheiros;
        }
    }
}


