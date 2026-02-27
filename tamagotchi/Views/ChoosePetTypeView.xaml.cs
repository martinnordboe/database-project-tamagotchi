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

namespace tamagotchi.Views
{
    /// <summary>
    /// Interaction logic for ChoosePetTypeView.xaml
    /// </summary>
    public partial class ChoosePetTypeView : UserControl
    {

        Application _app;
        Window _window;
        MainWindowNavigation _router;
        UserService _userService;

        public ChoosePetTypeView(Application app, Window window, MainWindowNavigation router, UserService userService)
        {
            InitializeComponent();

            _app = app;
            _window = window;
            _router = router;
            _userService = userService;

            SelectPet("Dog");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.ChoosePetVariant);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.EnterUserName);
        }

        private void DogBorder_Click(object sender, MouseButtonEventArgs e)
        {
            SelectPet("Dog");
        }

        private void CatBorder_Click(object sender, MouseButtonEventArgs e)
        {
            SelectPet("Cat");
        }

        private void SelectPet(string pet)
        {
            DogBorder.Background = new SolidColorBrush(Colors.Transparent);
            CatBorder.Background = new SolidColorBrush(Colors.Transparent);
            DogImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Outlines/Dog_Outline_Grey.png"));
            CatImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Outlines/Cat_Outline_Grey.png"));

            if (pet == "Dog")
            {
                _userService.petTypeMenu = 1;
                DogBorder.Background = new SolidColorBrush(Colors.LightSeaGreen);
                DogImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Outlines/Dog_Outline_Highlight.png"));
            }
            else
            {
                _userService.petTypeMenu = 2;
                CatBorder.Background = new SolidColorBrush(Colors.LightSeaGreen);
                CatImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Outlines/Cat_Outline_Highlight.png"));
            }
        }
    }
}
