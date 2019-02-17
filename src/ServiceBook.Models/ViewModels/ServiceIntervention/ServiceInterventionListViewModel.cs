using System;

namespace ServiceBook.Models.ViewModels.ServiceIntervention
{
    public class ServiceInterventionListViewModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public int Mileage { get; set; }
        
        public string ShortDescription { get; set; }

        public decimal TotalCost { get; set; }
    }
}
