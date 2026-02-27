using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace tamagotchi.Models
{
    public class User
    {
        private int id;
        private string name = string.Empty;

        private string createdAt = string.Empty;
        private string updatedAt = string.Empty;    

        public int Id { get { return id; } set { id = value; } }

        public string Name { get { return name; } set { name = value; } }

        public string CreatedAt { get { return createdAt; } set { createdAt = value; } }
        public string UpdatedAt { get {return updatedAt; } set { updatedAt = value; } }

    }

}
