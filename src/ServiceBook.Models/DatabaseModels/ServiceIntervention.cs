using ServiceBook.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceBook.Models.DatabaseModels
{
    public class ServiceIntervention : BaseEntity
    {
        [Display(Name = "Intervention date")]
        public DateTime DateTime { get; set; }

        public int Mileage { get; set; }

        [DateAfter(nameof(DateTime))]
        [Display(Name = "Next service date")]
        public DateTime NextServiceIntervalDate { get; set; }

        public string Description { get; set; }

        public string VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }
        
        public IEnumerable<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();
    }
}