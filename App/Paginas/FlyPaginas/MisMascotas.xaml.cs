using FrontEndHealthPets.Modelos;
using Microsoft.Maui.Controls;

namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class MisMascotas : ContentPage
{
    private MascotasViewModel ViewModel => (MascotasViewModel)BindingContext;

    public MisMascotas()
    {
        InitializeComponent();
        BindingContext = new MascotasViewModel(); // Establece el BindingContext
    }

    protected override  void OnAppearing()
    {
        base.OnAppearing();
        // Llama al método para cargar las mascotas
         ViewModel.CargarMascotasRegistradas();
    }



    private async void btNuevaMascota_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new IngresarMascotas());
    }
}
