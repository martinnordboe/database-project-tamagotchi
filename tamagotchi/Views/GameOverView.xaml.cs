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
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOverView : UserControl
    {
        MainWindowNavigation _router;


        public GameOverView(MainWindowNavigation router)
        {
            InitializeComponent();

            _router = router;
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.MainMenu);
        }
    }
}
