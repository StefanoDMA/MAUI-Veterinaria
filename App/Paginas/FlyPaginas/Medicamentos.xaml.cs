using System.Diagnostics;

namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class Medicamentos : ContentPage
{
    public Medicamentos()
    {
        InitializeComponent();
    }

    private void BtGuardarMedicamento_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AgregarMedicamento());
    }

    private async void BtRecetarMedicamento_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Agregar_Medicamentos_Mascota());
        }
        catch (Exception ex)
        {
            // Maneja la excepción o muestra un mensaje de error
            Debug.WriteLine($"Error al navegar: {ex.Message}");
        }
    }
}