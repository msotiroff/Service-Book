namespace ServiceBook.Models.ViewModels.Vehicle
{
    public class VehicleListViewModel
    {
        public string Id { get; set; }

        public string Make { get; set; }
        
        public string Model { get; set; }

        public string ExactModelName { get; set; }
        
        public string RegistrationPlate { get; set; }
        
        public string OwnerFullName { get; set; }

        public int ServiceInterventionsCount { get; set; }
    }
}
