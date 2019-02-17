using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBook.Models.DatabaseModels
{
    public class User : BaseEntity
    {
        public User()
        {
            this.DateCreated = DateTime.UtcNow;
        }

        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string PhoneNumber { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public IEnumerable<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}