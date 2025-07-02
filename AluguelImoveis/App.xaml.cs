using AluguelImoveis.Services;
using AluguelImoveis.Services.Interfaces;
using AluguelImoveis.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;
using System.Windows;

namespace AluguelImoveis
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Configurar HttpClient
                    services.AddHttpClient("ApiClient", client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:5287/api/");
                    });

                    services.AddTransient<IHttpService, HttpService>();
                    services.AddTransient<IImovelHttpService, ImovelHttpService>();
                    services.AddTransient<ILocatarioHttpService, LocatarioHttpService>();
                    services.AddTransient<IAluguelHttpService, AluguelHttpService>();

                    services.AddTransient<MainWindow>();
                    services.AddTransient<CreateImovelView>();
                    services.AddTransient<UpdateImovelView>();
                    services.AddTransient<CreateLocatarioView>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}