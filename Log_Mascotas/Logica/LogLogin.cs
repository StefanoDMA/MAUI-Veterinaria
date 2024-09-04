using Log_Mascotas.AccesoDatos;
using Log_Mascotas.Entidades;
using Log_Mascotas.Entidades.Request;
using Log_Mascotas.Entidades.response;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Log_Mascotas.Logica
{
    public class LogLogin
    {
        public Res_login Ingresarlogin(Req_login req)
        {
            Res_login res = new Res_login();
            res.registro_Usuario = new Registro_Usuario();

            if (req == null)
            {
                res.resultado = false;
                res.error = "campo vacio";
            }
            else
            {
                if (String.IsNullOrEmpty(req.Correo_Electronico))
                {
                    res.resultado = false;
                    res.error = "correo vacio ";
                }
                else
                {
                    if (String.IsNullOrEmpty(req.Contrasena))
                    {
                        res.resultado = false;
                        res.error = "contrasena incorrecta ";
                    }
                    else
                    {
                        int? id_usuario = 0;
                        int? estado = 0;
                        string nombre = "";
                        string apellido = "";
                        string Correo_Electronico = "";
                        string Password = "";

                        try
                        {
                            ConexionDataContext Linq = new ConexionDataContext();
                            Linq.SP_LOGIN(req.Correo_Electronico, req.Contrasena, ref id_usuario, ref estado, ref nombre, ref apellido,
                            ref Correo_Electronico, ref Password);

                            if (id_usuario == 0)
                            {
                                res.resultado = false;
                                res.error = "el usuario o clave no existen o son incorrecto ";
                            }
                            else
                            { 
                                res.resultado = true;
                                res.registro_Usuario.Id_Usuario = id_usuario.Value;
                                res.registro_Usuario.Nombre = nombre;
                                res.registro_Usuario.Apellidos = apellido;
                                res.registro_Usuario.Correo_Electronico = Correo_Electronico;
                                res.registro_Usuario.Password = Password;
                                
                                res.error = "El usuario ingresó correctamente";
                            }
                        }
                        catch (SqlException ex)
                        {
                            res.resultado = false;
                            res.error = "Error de base de datos: " + ex.Message;
                        }
                    }
                }
            }

            return res;
        }
    }
}