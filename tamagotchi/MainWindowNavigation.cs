using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using tamagotchi.Services;
using tamagotchi.Views;

namespace tamagotchi
{
    public class MainWindowNavigation
    {
        Route currentRoute;
        Application _app;
        Window _window;
        ContentControl _mainContent;

        UserService _userService;
        PetService _petService;

        public MainWindowNavigation(Application app, Window window, ContentControl contentControl, UserService userService, PetService petService )
        {
            this._mainContent = contentControl;
            this._app = app;
            this._window = window;
            this._userService = userService;
            this._petService = petService;
        }

        public void Navigate(Route route)
        {
            switch (route)
            {
                case Route.MainMenu:
                    _mainContent.Content = new MainMenu(_app, _window, this, _userService);
                    break;
                case Route.GameOver:
                    _mainContent.Content = new GameOverView(this);
                    break;
                case Route.History:
                    _mainContent.Content = new LoadGameView(_app, _window, this, _userService, this._petService);
                    break;
                case Route.EnterUserName:
                    _mainContent.Content = new StartGameUserNameView(_app, _window, this, _userService);
                    break;
                case Route.ChoosePetType:
                    _mainContent.Content = new ChoosePetTypeView(_app, _window, this, _userService);
                    break;
                case Route.ChoosePetVariant:
                    _mainContent.Content = new ChoosePetVariantView(_app, _window, this, _userService, _petService);
                    break;
                default:
                    _mainContent.Content = new MainMenu(_app, _window, this, _userService);
                    break;
            }
        }

        public enum Route
        {
            MainMenu,
            GameOver,
            History,
            EnterUserName,
            ChoosePetType,
            ChoosePetVariant
        }
    }
}
