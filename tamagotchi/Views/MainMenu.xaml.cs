using System;
using System.Collections.Generic;
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
using tamagotchi.Services;
using tamagotchi.Windows;

namespace tamagotchi.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        UserService _userService;
        Application _app;
        Window _window;
        MainWindowNavigation _router;

        public MainMenu(Application app, Window window, MainWindowNavigation router, UserService userService)
        {
            InitializeComponent();

            _userService = userService;
            _app = app;
            _window = window;
            _router = router;
        }


        private void StartNewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            //Window window = new GameWindow(_app, _userService);
            //window.Show();
            //_window.Close();
            _router.Navigate(MainWindowNavigation.Route.EnterUserName);
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            _app.Shutdown();
            //_window.Close();
        }

        private void LoadGame_Button_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.History);
        }
    }
}
