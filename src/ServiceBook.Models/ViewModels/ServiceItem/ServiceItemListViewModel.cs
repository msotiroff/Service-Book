using System;

namespace ServiceBook.Models.ViewModels.ServiceItem
{
    public class ServiceItemListViewModel
    {
        private decimal pricePerUnit;
        private double units;

        public string Id { get; set; }

        public string Part { get; set; }

        public decimal PricePerUnit
        {
            get => Math.Round(this.pricePerUnit, 2);
            set => this.pricePerUnit = value;
        }

        public double Units
        {
            get => Math.Round(this.units, 2);
            set => this.units = value;
        }

        public decimal TotalCost => Math.Round((decimal)Units * PricePerUnit, 2);
    }
}
