using System;

namespace ServiceBook.Models.DatabaseModels
{
    public class ServiceItem : BaseEntity
    {
        public string Part { get; set; }

        public decimal PricePerUnit { get; set; }

        public double Units { get; set; }

        public decimal TotalCost => Math.Round((decimal)Units * PricePerUnit, 2);
        
        public string ServiceInterventionId { get; set; }

        public ServiceIntervention ServiceIntervention { get; set; }
    }
}