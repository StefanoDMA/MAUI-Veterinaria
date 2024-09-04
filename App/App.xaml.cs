using FrontEndHealthPets.Paginas;

namespace FrontEndHealthPets
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
