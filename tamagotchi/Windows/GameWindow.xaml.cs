using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using tamagotchi.Models;
using tamagotchi.Services;
using tamagotchi.Views;

namespace tamagotchi.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Queue<string> _speechBubbles;
        DispatcherTimer gameTimer;
        User user;
        TextBlock currentSpeechBubble;
        Button currentSpeechBubbleButton;
        bool _isCurrentSpeechBubble = false;
        int _deathCountdown = 10;
        bool _isCountingDown = false;

        UserService _userService;
        PetService _petService;
        Application _app;

        bool _wasHungry = false;
        bool _wasSick = false;
        bool _wasSleepy = false;



        public GameWindow(Application app, UserService userService, PetService petService)
        {
            InitializeComponent();
            _app = app;
            _userService = userService;
            _petService = petService;
            _speechBubbles = new Queue<string>();

            gameTimer = new DispatcherTimer();

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            switch(_petService.CurrentPet.TypeId)
            {
                case 1:
                    bmp.UriSource = new Uri("pack://application:,,,/Assets/Images/Dogs/Dog_Brown.png");
                    break;
                case 2:
                    bmp.UriSource = new Uri("pack://application:,,,/Assets/Images/Cats/Cat_Dark.png");
                    break;
                default:
                    bmp.UriSource = new Uri("pack://application:,,,/Assets/Images/Dogs/Dog_Brown.png");
                    break;
            }
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.EndInit();
            bmp.Freeze();

            ImageTamagotchi.Source = bmp;

            PositionAtBottom();

            StartGameLoop();
        }
        private void PositionAtBottom()
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = 0;
            this.Top = workArea.Bottom - this.Height;
        }

        private void StartGameLoop()
        {
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += OnGameTick;
            gameTimer.Start();
            UpdateBars();
        }

        private void UpdateBars()
        {
            HungerBar.Value = _petService.CurrentPet.Hunger;
            HappinessBar.Value = _petService.CurrentPet.Happiness;
            EnergyBar.Value = _petService.CurrentPet.Sleepiness;
        }

        private void OnGameTick(object? sender, EventArgs e)
        {
            _petService.CurrentPet.Hunger = Math.Max(0, _petService.CurrentPet.Hunger - 2);
            _petService.CurrentPet.Happiness = Math.Max(0, _petService.CurrentPet.Happiness - 1);
            _petService.CurrentPet.Sleepiness = Math.Max(0, _petService.CurrentPet.Sleepiness - 1);

            Debug.WriteLine($"Happiness: {_petService.CurrentPet.Happiness}");
            Debug.WriteLine($"Energy: {_petService.CurrentPet.Sleepiness}");
            Debug.WriteLine($"Hungry: {_petService.CurrentPet.Hunger}");

            UpdateBars();

            if (_petService.CurrentPet.IsSick && !_wasSick)
            {
                // TODO: Sæt pet state til sick
                QueueSpeechBubble("I feel terrible...");
            }

            if (_petService.CurrentPet.IsHungry && !_wasHungry)
            {
                QueueSpeechBubble("Hey you! I'm hungry");
            }

            if (_petService.CurrentPet.IsSleepy && !_wasSleepy)
            {
                QueueSpeechBubble("ZzzZzz ZzzZzz");
            }

            _wasSick = _petService.CurrentPet.IsSick;
            _wasHungry = _petService.CurrentPet.IsHungry;
            _wasSleepy = _petService.CurrentPet.IsSleepy;

            bool isDying = _petService.CurrentPet.Hunger == 0 && _petService.CurrentPet.Happiness == 0 && _petService.CurrentPet.Sleepiness == 0;

            if (isDying)
            {
                if (!_isCountingDown)
                {
                    _isCountingDown = true;
                    _deathCountdown = 10;
                    QueueSpeechBubble("I'm dying... please help me!");
                }

                _deathCountdown--;
                QueueSpeechBubble($"Dying in {_deathCountdown}...");

                if (_deathCountdown <= 0)
                {
                    gameTimer.Stop();
                    GameOver();
                }
            }
            else
            {
                _isCountingDown = false;
                _deathCountdown = 10;
            }
        }

        private void GameOver()
        {
            Window window = new MainWindow(_app, MainWindowNavigation.Route.GameOver, _userService, _petService);

            // TODO: Sæt pet state til dead
            window.Show();
            this.Close();
        }

        private void DisposeSpeechBubble()
        {
            if (currentSpeechBubbleButton != null)
            {
                currentSpeechBubbleButton.Content = "";
            }
            SpeechBubbleStack.Children.Remove(currentSpeechBubbleButton);
            _isCurrentSpeechBubble = false;
            if (_speechBubbles.Count > 0)
            {
                _speechBubbles.Dequeue();
            }
            CheckSpeechBubbleQueue();
        }

        private void QueueSpeechBubble(string text)
        {
            _speechBubbles.Enqueue(text);
            CheckSpeechBubbleQueue();
        }

        private void CheckSpeechBubbleQueue()
        {
            if (_speechBubbles.Count > 0 && !_isCurrentSpeechBubble)
            {
                CreateSpeechBubble(_speechBubbles.First());
            }
            SetQueueCounter();
        }

        private void SetQueueCounter()
        {
            CountSpeechBubbles.Text = _speechBubbles.Count.ToString();
            if (_speechBubbles.Count == 0)
            {
                CountSpeechBubblesBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                CountSpeechBubblesBorder.Visibility = Visibility.Visible;
            }
        }

        private void CreateSpeechBubble(string text)
        {
            SolidColorBrush transparent = new SolidColorBrush(Colors.Transparent);
            
            Button button = new Button();

            button.Background = transparent;
            button.BorderBrush = transparent;
            button.Width = double.NaN;
            button.Click += SpeechBubbleButton_Click;
            SpeechBubbleStack.Children.Add(button);

            TextBlock speechBubble = new TextBlock();
            speechBubble.TextAlignment = TextAlignment.Center;
            speechBubble.TextWrapping = TextWrapping.WrapWithOverflow;
            speechBubble.MinHeight = 0;
            speechBubble.MaxHeight = 100;
            speechBubble.MinWidth = 100;
            speechBubble.MaxWidth = 200;
            speechBubble.Cursor = Cursors.Hand;
            speechBubble.Background = new SolidColorBrush(Colors.White);
            speechBubble.Foreground = new SolidColorBrush(Colors.Black);
            speechBubble.Padding = new Thickness(10, 5, 10, 5);
            speechBubble.Text = $"{text}";


            button.Content = speechBubble;
            button.Tag = speechBubble;

            currentSpeechBubble = speechBubble;
            currentSpeechBubbleButton = button;
            _isCurrentSpeechBubble = true;
        }

        private void RemoveFromSpeechQueue(params string[] keywords)
        {
            List<string> filtered = _speechBubbles.Where(msg => !keywords.Any(k => msg.Contains(k, StringComparison.OrdinalIgnoreCase))).ToList();

            _speechBubbles.Clear();
            foreach (var msg in filtered)
            {
                _speechBubbles.Enqueue(msg);
            }

            if (currentSpeechBubble != null && keywords.Any(k => currentSpeechBubble.Text.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                DisposeSpeechBubble();
            }
            SetQueueCounter();
        }

        private void CloseIngameMenu()
        {
            IngameMenu.Visibility = Visibility.Collapsed;
        }

        private void SpeechBubbleButton_Click(object sender, RoutedEventArgs e)
        {
            DisposeSpeechBubble();
        }

        private void TestSpeechButton_Click(object sender, RoutedEventArgs e)
        {
            QueueSpeechBubble("TEST");
        }
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void IngameMenuButton_Click(object sender, RoutedEventArgs e)
        {
            IngameMenu.Visibility = Visibility.Visible;
        }

        private void IngameMenuCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseIngameMenu();
        }

        private void EatButton_Click(object sender, RoutedEventArgs e)
        {
            _petService.CurrentPet.Hunger = 100;
            HungerBar.Value = 100;
            _ = _petService.UpdateStatsAsync(_petService.CurrentPet.Id, _petService.CurrentPet.Hunger, _petService.CurrentPet.Sleepiness, _petService.CurrentPet.Happiness);
            RemoveFromSpeechQueue("hungry", "terrible", "dying", "help me");
            CloseIngameMenu();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _petService.CurrentPet.Happiness = 100;
            HappinessBar.Value = 100;
            _ = _petService.UpdateStatsAsync(_petService.CurrentPet.Id, _petService.CurrentPet.Hunger, _petService.CurrentPet.Sleepiness, _petService.CurrentPet.Happiness);
            RemoveFromSpeechQueue("happy", "terrible", "dying", "help me");
            CloseIngameMenu();
        }

        private void SleepButton_Click(object sender, RoutedEventArgs e)
        {
            _petService.CurrentPet.Sleepiness = 100;
            EnergyBar.Value = 100;
            _ = _petService.UpdateStatsAsync(_petService.CurrentPet.Id, _petService.CurrentPet.Hunger, _petService.CurrentPet.Sleepiness, _petService.CurrentPet.Happiness);
            RemoveFromSpeechQueue("zzz", "dying", "help me");
            CloseIngameMenu();
        }

        private void WorkButton_Click(object sender, RoutedEventArgs e)
        {
            CloseIngameMenu();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            _ = _petService.UpdateStatsAsync(_petService.CurrentPet.Id, _petService.CurrentPet.Hunger, _petService.CurrentPet.Sleepiness, _petService.CurrentPet.Happiness);
            Window window = new MainWindow(_app, MainWindowNavigation.Route.MainMenu, _userService, _petService);
            window.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            gameTimer.Stop();
            base.OnClosed(e);
        }
    }
}