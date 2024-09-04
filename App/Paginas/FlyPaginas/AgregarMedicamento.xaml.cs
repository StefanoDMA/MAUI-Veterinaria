using FrontEndHealthPets.Entidades;
using FrontEndHealthPets.Entidades.Request;
using FrontEndHealthPets.Entidades.response;
using Newtonsoft.Json;

namespace FrontEndHealthPets.Paginas.FlyPaginas;


public partial class AgregarMedicamento : ContentPage
{
    string LaURL = "https://localhost:44348/api";
    public AgregarMedicamento()
    {
        


        InitializeComponent();
    }

    private async void Btmedicamento_Clicked(object sender, EventArgs e)
    {

        try
        {

            if (string.IsNullOrEmpty(Nombre.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
             if (Categoria.SelectedItem == null)
            {
                DisplayAlert("Error", "Seleccione una categoría", "Aceptar");
                return;
            }
            else
             if (string.IsNullOrEmpty(Descripcion.Text))
            {
                DisplayAlert("Error", "Faltan datos", "Aceptar");
                return;
            }
            else
             if (FechaVencimiento.Date < DateTime.Today)
            {
                DisplayAlert("Error", "Seleccione una fecha de vencimiento válida", "Aceptar");
                return;
            }

            Req_Medicamentos req = new Req_Medicamentos();

            req.Medicamentos = new Entidades.Medicamentos();

            req.Medicamentos.Nombre = Nombre.Text;
            req.Medicamentos.Categoria = Categoria.SelectedItem.ToString();
            req.Medicamentos.Decripcion = Descripcion.Text;
            req.Medicamentos.FechaDeVencimiento = FechaVencimiento.Date;
            

            var jsoncontent = new StringContent(JsonConvert.SerializeObject(req), System.Text.Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(LaURL + "/Ingresar_Medicamentos/Ingresar_Medicamentos", jsoncontent);
            if (response.IsSuccessStatusCode)// saber si el api esta vivo
            {
                // si conecto

                var responsecontent = await response.Content.ReadAsStringAsync();
                Res_Usuario res = new Res_Usuario();

                res = JsonConvert.DeserializeObject<Res_Usuario>(responsecontent);

                if (res.resultado)
                {
                    await DisplayAlert("Éxito", "Medicamento ingresado correctamente", "Aceptar");
                    Navigation.PushAsync(new Medicamentos());

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