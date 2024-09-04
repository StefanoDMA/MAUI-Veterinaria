namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class Baheiros : ContentPage
{
	public Baheiros()
	{
		InitializeComponent();
	}

 

    private void BTAsignarBaheiro_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Asignar_Baheiro_Mascota());
    }
}