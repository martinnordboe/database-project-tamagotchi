using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace tamagotchi.Views
{
    /// <summary>
    /// Interaction logic for StartGameUserNameView.xaml
    /// </summary>
    public partial class StartGameUserNameView : UserControl
    {
        Application _app;
        Window _window;
        MainWindowNavigation _router;
        UserService _userService;

        public StartGameUserNameView(Application app, Window window, MainWindowNavigation router, UserService userService)
        {
            InitializeComponent();

            _app = app;
            _window = window;
            _router = router;
            _userService = userService;
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _userService.usernameMenu = usernameTextbox.Text.Trim();
            Debug.WriteLine(_userService.usernameMenu);
            _router.Navigate(MainWindowNavigation.Route.ChoosePetType);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.MainMenu);
        }
    }
}
