using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas
{
    public class Log_MedicamentosMascotas
    {
       
            public Res_MedicamentosMascotas AgregarMedicamentosMascotas(Req_MedicamentosMascotas req)
            {
                Res_MedicamentosMascotas res = new Res_MedicamentosMascotas();

            if (req == null)
            {
                res.resultado = false;
                res.error = "No se recibieron datos";

            }
            else

            if (req.medicamentosMascotas.Id_Mascota == 0)
            {
                res.resultado = false;
                res.error = "No se recibió el nombre del medicamento";

            }
            else

            if (req.medicamentosMascotas.id_medicamento == 0)
            {
                res.resultado = false;
                res.error = "No se recibió la descripción del medicamento";

            }
            else
            if (string.IsNullOrEmpty(req.medicamentosMascotas.Modo_De_Administracion))
            {
                res.resultado = false;
                res.error = "No se recibió el modo de administración";
            }else
                if (req.medicamentosMascotas.Fecha_Inicio == default(DateTime))
                {
                    res.resultado = false;
                    res.error = "No se recibió la fecha de inicio";
                 
                }else

                if (req.medicamentosMascotas.Fecha_Fin == default(DateTime))
                {
                    res.resultado = false;
                    res.error = "No se recibió la fecha de fin";
                  
                }else

                if (req.medicamentosMascotas.Hora_De_Ingesta == default(TimeSpan))
                {
                    res.resultado = false;
                    res.error = "No se recibió la hora de ingesta";
                   
                }else

                if (string.IsNullOrEmpty(req.medicamentosMascotas.Notas))
                {
                    req.medicamentosMascotas.Notas = string.Empty;
                }

                else
                {
                    int? idReturn = 0;
                    int? idError = 0;
                    string errorDescripcion = null;

                      TimeSpan horaActual = DateTime.Now.TimeOfDay;

                try
                    {
                        ConexionDataContext Linq = new ConexionDataContext();
                        Linq.SP_INGRESAR_MASCOTA_MEDICAMENTO(req.medicamentosMascotas.Id_Mascota,
                        req.medicamentosMascotas.id_medicamento, req.medicamentosMascotas.Modo_De_Administracion, 
                        req.medicamentosMascotas.Fecha_Inicio, req.medicamentosMascotas.Fecha_Fin, req.medicamentosMascotas.Hora_De_Ingesta,
                        req.medicamentosMascotas.Notas, ref idReturn, ref idError, ref errorDescripcion);

                        if (idReturn == -1)
                        {
                            res.resultado = false;
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
                        res.error = ex.Message;
                    }
                }

                return res;
            }

        public Res_Lista_Medicamentos_Mascotas ListaMedicamentosMascotas(int id_mascota)
        {
            Res_Lista_Medicamentos_Mascotas res = new Res_Lista_Medicamentos_Mascotas();

            try
            {
                ConexionDataContext linq = new ConexionDataContext();
                
                List<SP_OBTENER_LISTA_MEDICAMENTOS_MASCOTASResult> results = new List<SP_OBTENER_LISTA_MEDICAMENTOS_MASCOTASResult>();
                results = linq.SP_OBTENER_LISTA_MEDICAMENTOS_MASCOTAS(id_mascota).ToList();

                foreach (SP_OBTENER_LISTA_MEDICAMENTOS_MASCOTASResult cadaresults in results)
                { 
                    res.resultado = true;
                    res.ListaMedicamentosMascotas.Add(FabricaListaMedicamentosMascotas(cadaresults));
                
                }
                               
            }catch(Exception ex)
            {
                res.resultado = false;
            
                res.error = $"Error en backend: {ex.Message}";
            }

            return res;
        }

        private MedicamentosMascotas FabricaListaMedicamentosMascotas(SP_OBTENER_LISTA_MEDICAMENTOS_MASCOTASResult MedicamentosMascotasLinq)
        { 
        
             MedicamentosMascotas UnalistamedicamentosMascotas = new MedicamentosMascotas();

            UnalistamedicamentosMascotas.Id_Mascota = MedicamentosMascotasLinq.ID_MASCOTA ??0;
            UnalistamedicamentosMascotas.Nombre_Mascota = MedicamentosMascotasLinq.NOMBRE_MASCOTA;
            UnalistamedicamentosMascotas.id_medicamento = MedicamentosMascotasLinq.ID_MEDICAMENTO ??0;
            UnalistamedicamentosMascotas.Nombre_Medicamento = MedicamentosMascotasLinq.NOMBRE_MEDICAMENTO;
            UnalistamedicamentosMascotas.categoria = MedicamentosMascotasLinq.CATEGORIA;
            UnalistamedicamentosMascotas.Modo_De_Administracion = MedicamentosMascotasLinq.MODO_DE_ADMINISTRACION;
            UnalistamedicamentosMascotas.Hora_De_Ingesta = MedicamentosMascotasLinq.HORA_DE_INGESTA;
            UnalistamedicamentosMascotas.Fecha_Inicio = MedicamentosMascotasLinq.FECHA_INICIO;
            UnalistamedicamentosMascotas.Fecha_Fin = MedicamentosMascotasLinq.FECHA_FIN;           
            UnalistamedicamentosMascotas.Notas = MedicamentosMascotasLinq.NOTAS;

            return UnalistamedicamentosMascotas;
        
        }

        }
    }


     
    




