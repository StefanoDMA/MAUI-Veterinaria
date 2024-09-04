using FrontEndHealthPets.Entidades;
using Newtonsoft.Json;

namespace FrontEndHealthPets.Paginas.FlyPaginas;

public partial class AgregarVacunas : ContentPage
{
    string LaURL = "https://localhost:44348/api";
	public AgregarVacunas()
	{
		InitializeComponent();
	}

    private async void BtVacuna_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrEmpty(NombreVacuna.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
                        if (string.IsNullOrEmpty(DescripcionVacuna.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
             if (FechaVencimiento.Date < DateTime.Today)
            {
                await DisplayAlert("Error", "Seleccione una fecha de baño válida", "Aceptar");
                
                return;
            }

            DateTime fechaYhoravencimiento = FechaVencimiento.Date.Add(TimeFechaVencimiento.Time);
            

            Req_Vacunas req = new Req_Vacunas();

            req.Vacunas = new Entidades.Vacunas();

            req.Vacunas.Nombre = NombreVacuna.Text;
            req.Vacunas.Decripcion = DescripcionVacuna.Text;
            req.Vacunas.FechaVencimiento = fechaYhoravencimiento;


            var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(LaURL + "/Ingresar_Vacunas/ObtenerIngresar_Vacunas", jsoncontent);
            if (response.IsSuccessStatusCode)// saber si el api esta vivo
            {
                // si conecto

                var responsecontent = await response.Content.ReadAsStringAsync();
                Res_Vacuna res = new Res_Vacuna();

                res = JsonConvert.DeserializeObject<Res_Vacuna>(responsecontent);

                if (res.resultado)
                {
                    await DisplayAlert("Exito", "Vacuna ingresada correctamente", "Aceptar");
                    await Navigation.PushAsync(new VacunasPage());

                }
                else
                {
                    DisplayAlert("Error", res.error, "Aceptar");// Error en backend", "Login incorrecto!", "Aceptar" agregar luego

                }
            }
            else
            {
                //No conectó
                DisplayAlert("Error de conexión", "Ocurrió un error de conexión", "Aceptar");

            }
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }
}
