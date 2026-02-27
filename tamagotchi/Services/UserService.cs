using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using tamagotchi.Models;
using tamagotchi.Repositories;

namespace tamagotchi.Services
{
    public class UserService
    {
        IUserRepository _repo;
        User _currentUser;
        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; } }
        public int petTypeMenu = 1;
        public string usernameMenu = string.Empty;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(string name)
        {
            return await _repo.CreateAsync(name);
        }
        public async Task<ObservableCollection<User>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}
