using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using QuickClerkShared.Data;

using System.Windows;
using System.Windows.Threading;

namespace QuickClerk;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        await host.StartAsync().ConfigureAwait(true);

        //*** Items requiring hostbuilder configuration cannot come before this ***

        //!!! This is going to hang our db while the app is created !!!
        // When using a remote db, Migrations will be performed by DevOps, not the app
        // We can currently get away with this because we are using a very small sqlite db
        using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        using (CashRegisterContext context = scope.ServiceProvider.GetRequiredService<CashRegisterContext>())
        {
            context.Database.Migrate();
        }

        // Configuration is complete, launch the app startup & window
        App app = new();
        app.InitializeComponent();
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();

        // If our Run method returns unexpectedly, call the shutdown actions
        await host.StopAsync().ConfigureAwait(true);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
            => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
        .ConfigureServices((hostContext, services) =>
        {
            //wire up the UI classes
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            //messenger class to broadcast/request action messages from the dashboard buttons
            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

            //capture the UI thread
            services.AddSingleton(_ => Current.Dispatcher);

            //local db
            services.AddDbContext<CashRegisterContext>();

            //display errors to user
            services.AddTransient<ISnackbarMessageQueue>(provider =>
            {
                Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
            });
        });
}
