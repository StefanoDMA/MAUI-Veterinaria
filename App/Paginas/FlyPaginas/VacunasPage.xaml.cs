using System;
using Microsoft.Maui.Controls;

namespace FrontEndHealthPets.Paginas.FlyPaginas
{
    public partial class VacunasPage : ContentPage
    {
        public VacunasPage()
        {
            InitializeComponent();
        }

        private async void OnAddVaccineClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarVacunas());
        }

        private void OnConsultVaccineClicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new Asignar_Mascota_Vacuna());
        }
    }
}
