using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas
{
    public class Log_Agregar_Medicamentos
    {

        public Res_Medicamentos AgregarMedicamentos(Req_Medicamentos req)
             {
            Res_Medicamentos res = new Res_Medicamentos();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se ingreso ningun dato";
            }
            else
            if (string.IsNullOrEmpty(req.Medicamentos.Nombre))
            {
                res.resultado = false;
                res.error = "El nombre del medicamento es requerido";
            }else
            if (string.IsNullOrEmpty(req.Medicamentos.Categoria))
            {
                res.resultado = false;
                res.error = "La categoria del medicamento es requerida";
            }
            else
            if (string.IsNullOrEmpty(req.Medicamentos.Decripcion))
            {
                res.resultado = false;
                res.error = "La descripcion del medicamento es requerida";
            }
            else
                if (req.Medicamentos.FechaDeVencimiento == default(DateTime))
            {
                res.resultado = false;
                res.error = "La fecha de vencimiento del medicamento es requerida";

            }else

                {
                try
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                    ConexionDataContext Linq = new ConexionDataContext();
                    Linq.SP_INGRESAR_MEDICAMENTO(req.Medicamentos.Nombre, req.Medicamentos.Categoria, req.Medicamentos.Decripcion, req.Medicamentos.FechaDeVencimiento, ref idReturn, ref idError, ref errorDescripcion);

                    if (idReturn == -1)
                    {
                        res.resultado = false;
                        res.error = errorDescripcion;
                    }
                    else
                    {
                        res.resultado = true;
                        res.error = "Medicamento ingresado correctamente";
                    }

                }
                catch (Exception ex)
                {
                    res.resultado = false;
                    res.error = ex.Message;
                }
            }



            return res;
        }

        public Res_LIstaMedicamentos ListaMedicamentos(Res_LIstaMedicamentos req)
        {
            Res_LIstaMedicamentos res = new Res_LIstaMedicamentos();


            List<SP_OBTENER_LISTA_MEDICAMENTOSResult> Results = new List<SP_OBTENER_LISTA_MEDICAMENTOSResult>();
            Results = new ConexionDataContext().SP_OBTENER_LISTA_MEDICAMENTOS().ToList();

            try
            {
                foreach (SP_OBTENER_LISTA_MEDICAMENTOSResult cadaResultSet in Results)
                {
                    res.resultado = true;
                    res.ListarMedicamentos.Add(this.fabricaListaCitasVeterinarias(cadaResultSet));

                }

            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = ex.Message;
            }

            return res;

        }

        private Medicamentos fabricaListaCitasVeterinarias(SP_OBTENER_LISTA_MEDICAMENTOSResult lISTA_MEDICAMENTOSlINQ)
        {
            Medicamentos UnaListaMedicamentos = new Medicamentos();

            UnaListaMedicamentos.Id_Medicamento = lISTA_MEDICAMENTOSlINQ.ID_MEDICAMENTO;
            UnaListaMedicamentos.Nombre = lISTA_MEDICAMENTOSlINQ.NOMBRE;
            UnaListaMedicamentos.Categoria = lISTA_MEDICAMENTOSlINQ.CATEGORIA;
            UnaListaMedicamentos.Decripcion = lISTA_MEDICAMENTOSlINQ.DESCRIPCION;
            UnaListaMedicamentos.FechaDeVencimiento = lISTA_MEDICAMENTOSlINQ.FECHA_VENCIMIENTO ?? default(DateTime);

            return UnaListaMedicamentos;

        }


    }
}
