using System.IO;
using System.Windows;
using System.Windows.Controls;
using tamagotchi.Data;
using tamagotchi.Models;
using tamagotchi.Repositories;
using tamagotchi.Services;
using tamagotchi.Views;
using tamagotchi.Windows;
using tamagotchi;

namespace tamagotchi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //string dbPath = $"Data Source=Data/tamagotchi.db";
            string dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "tamagotchi.db");
            string connectionString = $"Data Source={dbPath}";
            IDbConnectionFactory connection = new SqliteConnectionFactory(connectionString);


            IUserRepository userRepo = new UserRepository(connection);
            IPetRepository petRepo = new PetRepository(connection);

            UserService userService = new UserService(userRepo);
            PetService petService = new PetService(petRepo);

            Window window = new MainWindow(this, MainWindowNavigation.Route.MainMenu, userService, petService);
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

    }

}
