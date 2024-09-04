using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades.entitys;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class Log_Lista_Doctores
    {
        public Res_Lista_Doctor ListaDoctores(int id_Doctor )
        {
            Res_Lista_Doctor res = new Res_Lista_Doctor();

            res.ListaDoctor = new List<Doctor>();

            List<SP_OBTENER_LISTA_DOCTORResult> Results = new List<SP_OBTENER_LISTA_DOCTORResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_DOCTOR(id_Doctor).ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_DOCTORResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.ListaDoctor.Add(this.fabricaListaDoctores(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private Doctor fabricaListaDoctores(SP_OBTENER_LISTA_DOCTORResult lISTA_CLINICA_DOCTORLINQ)
        {
            Doctor unaListacdoctores = new Doctor();

            unaListacdoctores.Id_Doctor = lISTA_CLINICA_DOCTORLINQ.ID_DOCTOR;
            unaListacdoctores.Nombre = lISTA_CLINICA_DOCTORLINQ.NOMBRE;
            unaListacdoctores.Telefono = lISTA_CLINICA_DOCTORLINQ.TELEFONO;
            unaListacdoctores.Correo_Electronico = lISTA_CLINICA_DOCTORLINQ.CORREO_ELECTRONICO;
                


            return unaListacdoctores;

        }


    }
}

   