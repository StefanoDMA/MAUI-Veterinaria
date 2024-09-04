namespace FrontEndHealthPets.Paginas.tabpage;

public partial class Consultas_Principal : TabbedPage
{
	public Consultas_Principal()
	{
		InitializeComponent();

        var consultasBaheiros = new Consultas_Baheiros
        {
            Title = "Baños",
            IconImageSource = "dog.png"
        };
        var citasVeterinarias = new Consultas_citas_Veterinarias
        {
            Title = "Citas Veterinarias",
            IconImageSource = "medicalappointment.png"
        };

        var medicamentos = new Consultas_Medicamentos
        {
            Title = "Medicamentos",
            IconImageSource = "pillsbottle.png"
        };

        var vacunas = new Consultas_Vacunas
        {
            Title = "Vacunas",
            IconImageSource = "vaccine.png"
        };

        this.Children.Add(consultasBaheiros);
        this.Children.Add(citasVeterinarias);
        this.Children.Add(medicamentos);
        this.Children.Add(vacunas);
    }
}