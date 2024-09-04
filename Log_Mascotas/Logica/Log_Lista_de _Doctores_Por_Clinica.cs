using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades.response;
using Log_Mascotas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log_Mascotas.Entidades.entitys;

namespace Log_Mascotas.Logica
{
    public class Log_Lista_de_Doctores_Por_Clinica
    {
        public Res_Lista_de_Doctores_Clinica ObtenerDoctoresPorClinica(int Id_Clinica_Veterinaria)
        {
            Res_Lista_de_Doctores_Clinica res = new Res_Lista_de_Doctores_Clinica();

            try
            {
                ConexionDataContext Linq = new ConexionDataContext();

                List<SP_OBTENER_LISTA_DE_DOCTORES_POR_CLINICAResult> resultSet = Linq
                    .SP_OBTENER_LISTA_DE_DOCTORES_POR_CLINICA(Id_Clinica_Veterinaria)
                    .ToList();

                foreach (var cadaResultSet in resultSet)
                {
                    res.resultado = true;
                    res.listaDoctoresporclinica.Add(FabricarDoctoresPorClinica(cadaResultSet));
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error = $"Error en backend: {ex.Message}";
            }

            return res;
        }

        private Doctor FabricarDoctoresPorClinica(SP_OBTENER_LISTA_DE_DOCTORES_POR_CLINICAResult listadoDoctoresPorClinicaLinq)
        {
            Doctor doctorporClinica = new Doctor();

            doctorporClinica.Id_Clinica_Veterinaria = listadoDoctoresPorClinicaLinq.ID_CLINICA_VETERINARIA;
            doctorporClinica.Id_Doctor = listadoDoctoresPorClinicaLinq.ID_DOCTOR;
            doctorporClinica.Nombre = listadoDoctoresPorClinicaLinq.NOMBRE_DOCTOR;
            doctorporClinica.Telefono = listadoDoctoresPorClinicaLinq.TELEFONO;
            doctorporClinica.Correo_Electronico= listadoDoctoresPorClinicaLinq.CORREO_ELECTRONICO;
        




            return doctorporClinica;
        }
    }
}
