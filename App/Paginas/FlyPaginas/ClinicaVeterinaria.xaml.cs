using FrontEndHealthPets.Entidades.Request;
using Newtonsoft.Json;
using FrontEndHealthPets.Entidades.Entitys;
using FrontEndHealthPets.Entidades.Response;

using System.Net.Http;

namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class ClinicaVeterinaria : ContentPage
{


    public ClinicaVeterinaria()
    {
        InitializeComponent();

    }

    private void BtGuardarCita_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarCita());
    }

   
    }



