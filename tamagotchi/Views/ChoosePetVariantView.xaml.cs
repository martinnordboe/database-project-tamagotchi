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
using tamagotchi.Models;

namespace tamagotchi.Views
{
    /// <summary>
    /// Interaction logic for ChoosePetVariantView.xaml
    /// </summary>
    public partial class ChoosePetVariantView : UserControl
    {

        Application _app;
        Window _window;
        MainWindowNavigation _router;
        UserService _userService;
        PetService _petService;

        public ChoosePetVariantView(Application app, Window window, MainWindowNavigation router, UserService userService, PetService petService)
        {
            InitializeComponent();

            _app = app;
            _window = window;
            _router = router;
            _userService = userService;
            _petService = petService;

            if (_userService.petTypeMenu == 1)
            {
                PetImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Dogs/Dog_Brown.png"));
            }
            else if (_userService.petTypeMenu == 2)
            {
                PetImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/Cats/Cat_Dark.png"));
            }
        }




        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string petName = PetNameInput.Text.Trim();

            if (string.IsNullOrEmpty(petName))
            {
                MessageBox.Show("Please enter a pet name.");
                return;
            }

            _ = CreatePetAndStart(petName);
        }

        private async Task<int> CreateUser()
        {
            return await _userService.CreateAsync(_userService.usernameMenu);
        }

        private async Task CreatePetAndStart(string petName)
        {
            int userId = await CreateUser();
            int petId = await _petService.CreateAsync(petName, _userService.petTypeMenu, 1, 1, userId, 1);
            await _petService.SetCurrentpet(userId);

            GameWindow gameWindow = new GameWindow(_app, _userService, _petService);
            gameWindow.Show();
            _window.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.ChoosePetType);
        }
    }
}
