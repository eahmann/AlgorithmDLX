using AlgorithmDLX.Sudoku;
using AlgorithmDLX.Core;
using Microsoft.Extensions.Logging;

namespace AlgorithmDLX.MauiBlazorHybridApp
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<IDLXSolver, DLXSolver>();
            builder.Services.AddSingleton<IDLXMatrixBuilder, DLXMatrixBuilder>();
            builder.Services.AddSingleton<IDLXController, DLXController>();
            builder.Services.AddSingleton<ISudokuTools, SudokuTools>();

            return builder.Build();
        }
    }
}
