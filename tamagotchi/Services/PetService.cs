using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using tamagotchi.Models;
using tamagotchi.Repositories;

namespace tamagotchi.Services
{
    public class PetService
    {
        IPetRepository _repo;

        Pet _currentPet;
        public Pet CurrentPet { get { return _currentPet; } }

        public PetService(IPetRepository repo)
        {
            _repo = repo;
        }

        public async Task SetCurrentpet(int ownerId)
        {
            try
            {
                List<Pet> pets = await GetAllByOwnerIdAsync(ownerId);
                _currentPet = pets[0];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<List<Pet>> GetAllByOwnerIdAsync(int ownerId)
        {
            return await _repo.GetAllByOwnerIdAsync(ownerId);
        }
        public async Task<int> CreateAsync(string name, int typeId, int statusId, int colorId, int ownerId, int stageId)
        {
            return await _repo.CreateAsync(name, typeId, statusId, colorId, ownerId, stageId);
        }
        public async Task UpdateStatsAsync(int id, int hunger, int sleepiness, int happiness)
        {
            await _repo.UpdateStatsAsync(id, hunger, sleepiness, happiness);
        }
    }
}
