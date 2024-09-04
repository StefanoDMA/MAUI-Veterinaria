using FrontEndHealthPets.Modelos;

namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class DetallesMascotaPage : ContentPage
{
    public PerfilMascota Mascota { get; set; }

    public DetallesMascotaPage(PerfilMascota mascota)
    {
        InitializeComponent();
        Mascota = mascota;
        BindingContext = this;
    }
}
