using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tamagotchi.Data;
using tamagotchi.Repositories;
using tamagotchi.Services;
using tamagotchi.Views;
using Microsoft.Data.Sqlite;



namespace tamagotchi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //PetService petService;
        UserService _userService;
        PetService _petService;
        Application _app;
        MainWindowNavigation.Route _route;

        MainWindowNavigation _router;

        public MainWindow(Application app, MainWindowNavigation.Route route, UserService userService, PetService petService)
        {
            InitializeComponent();

            _app = app;
            _userService = userService;
            _petService = petService;

            _route = route;
            _router = new MainWindowNavigation(_app, this, viewContent, _userService, _petService);

            _router.Navigate(_route);
        }
    }
}