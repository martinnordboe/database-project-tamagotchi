using System;
using System.Collections.Generic;
using System.Text;

namespace tamagotchi.Models
{
    public class Pet
    {
        private int id;
        private string name = string.Empty;
        private int hunger;
        private int happiness;
        private int sleepiness;
        private string createdAt = string.Empty;
        private string updatedAt = string.Empty;

        public int Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        
        public int TypeId { get; set; }
        public int ColorId { get; set; }
        public int StatusId { get; set; }
        public int StageId { get; set; }
        public int OwnerId { get; set; }

        public bool IsHungry => Hunger < 30;
        public bool IsSick => Hunger < 20 && Happiness < 20;
        public bool IsSleepy => Sleepiness < 20;
        public bool IsIdle => !IsHungry && !IsSick && !IsSleepy;

        public int Hunger { get { return hunger; } set { hunger = value; } }
        public int Happiness { get { return happiness; } set { happiness = value; } }
        public int Sleepiness { get { return sleepiness; } set { sleepiness = value; } }


        public string CreatedAt { get { return createdAt; } set { createdAt = value; } }
        public string UpdatedAt { get { return updatedAt; } set { updatedAt = value; } }

    }
}
