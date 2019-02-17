using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBook.Models.DatabaseModels
{
    public class Vehicle : BaseEntity
    {
        public Vehicle()
        {
            this.ServiceInterventions = new List<ServiceIntervention>();
        }

        [Required(ErrorMessage = "Make is required.")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        public string ExactModelName { get; set; }

        public string VIN { get; set; }
        
        [Required(ErrorMessage = "Registration plate is required.")]
        public string RegistrationPlate { get; set; }
        
        [Required(ErrorMessage = "Owner is required.")]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public IEnumerable<ServiceIntervention> ServiceInterventions { get; set; }
    }
}
