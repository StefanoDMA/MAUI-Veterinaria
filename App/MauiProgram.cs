using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui; // Asegúrate de tener esta referencia
using Microsoft.Maui.Controls.Hosting;

namespace FrontEndHealthPets
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit(); // Registrar la biblioteca CommunityToolkit

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}