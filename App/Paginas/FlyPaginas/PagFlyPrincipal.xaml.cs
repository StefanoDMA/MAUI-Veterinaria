using FrontEndHealthPets.Datos;
using System.Diagnostics;

namespace FrontEndHealthPets.Paginas;

public partial class PagFlyPrincipal : FlyoutPage
{
	public PagFlyPrincipal()
	{
		InitializeComponent();
		flyoutPage.collectionView.SelectionChanged += CollectionView_SelectionChanged;
	}

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var item = e.CurrentSelection.FirstOrDefault() as Pagina_Flyout_Items;

            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                await Task.Delay(100);  // Prueba con un retraso más corto

                IsPresented = false;
            }
        }
        catch (Exception ex)
        {
            // Aquí puedes capturar y manejar la excepción
            Debug.WriteLine($"Error: {ex.Message}");
            // Mostrar un mensaje o registrar el error
        }
    }
    }