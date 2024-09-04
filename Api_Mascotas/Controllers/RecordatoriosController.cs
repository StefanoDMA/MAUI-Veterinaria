using Log_Mascotas.Entidades;
using Log_Mascotas.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Api_Mascotas.Controllers
{
    public class RecordatoriosController : ApiController
    {

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Agregar_Recordatorio/Ingresar_Recordatorios")]

        public Res_Ingresar_recordatorio IngresarRecordatorio(Req_Ingresar_Recordatorio req)
        {
            return new Log_Ingresar_Recordatorio ().IngresarRecordatorio(req);
        }

    }
}