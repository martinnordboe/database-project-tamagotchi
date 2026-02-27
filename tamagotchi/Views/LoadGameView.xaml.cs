using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
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
using tamagotchi.Models;
using tamagotchi.Services;
using tamagotchi.Windows;

namespace tamagotchi.Views
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class LoadGameView : UserControl
    {
        Application _app;
        Window _window;
        MainWindowNavigation _router;
        UserService _userService;
        PetService _petService;

        ObservableCollection<User> users;

        public LoadGameView(Application app, Window window, MainWindowNavigation router, UserService userService, PetService petService)
        {
            InitializeComponent();
            _app = app;
            _window = window;
            _router = router;
            _userService = userService;
            _petService = petService;
            users = new ObservableCollection<User>();


            _ = LoadUsers();

            //ListUsers
        }

        async Task LoadUsers()
        {
            users = await _userService.GetAll();
            foreach(User user in users)
            {
                CreateUserLoadButton(user);
            }
        }

        void CreateUserLoadButton(User user)
        {
            Button button = new Button();
            button.Width = 300;
            button.Height = 100;
            button.Background = new SolidColorBrush(Colors.Red);
            button.Click += UserButton_Click;
            button.Padding = new Thickness(10);
            button.Margin = new Thickness(10);
            button.Tag = user;
            ListUsers.Children.Add(button);

            Grid grid = new Grid();
            button.Content = grid;
            grid.Width = 300;
            grid.Height = 100;

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            TextBlock userTitle = new TextBlock();
            userTitle.HorizontalAlignment = HorizontalAlignment.Left;
            userTitle.Text = "User";
            userTitle.FontSize = 14;
            TextBlock userName = new TextBlock();
            userName.HorizontalAlignment = HorizontalAlignment.Left;
            userName.Text = user.Name;

            TextBlock lastSavedTitle = new TextBlock();
            lastSavedTitle.HorizontalAlignment = HorizontalAlignment.Right;
            lastSavedTitle.Text = "Last save";
            lastSavedTitle.FontSize = 14;
            TextBlock lastSavedDetail = new TextBlock();
            lastSavedDetail.HorizontalAlignment = HorizontalAlignment.Right;
            lastSavedDetail.Text = user.UpdatedAt;



            Grid.SetRow(userTitle, 0);
            Grid.SetColumn(userTitle, 0);

            Grid.SetRow(userName, 1);
            Grid.SetColumn(userName, 0);

            Grid.SetRow(lastSavedTitle, 0);
            Grid.SetColumn(lastSavedTitle, 1);

            Grid.SetRow(lastSavedDetail, 1);
            Grid.SetColumn(lastSavedDetail, 1);


            grid.Children.Add(userTitle);
            grid.Children.Add(userName);
            grid.Children.Add(lastSavedTitle);
            grid.Children.Add(lastSavedDetail);
        }

        private async void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            User user = (User)button.Tag;

            _userService.CurrentUser = user;
            await _petService.SetCurrentpet(user.Id);

            GameWindow gameWindow = new GameWindow(_app, _userService, _petService);
            gameWindow.Show();
            _window.Close();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _router.Navigate(MainWindowNavigation.Route.MainMenu);
        }
    }
}
