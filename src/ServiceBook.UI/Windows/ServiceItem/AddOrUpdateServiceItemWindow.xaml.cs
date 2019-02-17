using ServiceBook.Models.ViewModels.ServiceItem;
using ServiceBook.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Windows.ServiceItem
{
    /// <summary>
    /// Interaction logic for AddOrUpdateServiceItemWindow.xaml
    /// </summary>
    public partial class AddOrUpdateServiceItemWindow : Window
    {
        private readonly IItemService itemService;
        private Models.DatabaseModels.ServiceItem currentItem;
        private string interventionId;
        private bool isUpdate;

        public AddOrUpdateServiceItemWindow(IItemService itemService)
        {
            InitializeComponent();
            this.itemService = itemService;
        }

        internal async void SetRequiredData(string interventionId, ServiceItemListViewModel item = null)
        {
            this.interventionId = interventionId;

            if (item == null)
            {
                this.Title = "Add a new service item";

                return;
            }

            this.Title = "Update a service item";
            this.currentItem = await this.itemService.GetAsync(item.Id);
            this.isUpdate = true;
            this.txtPart.Text = item.Part;
            this.txtPricePerUnit.Text = item.PricePerUnit.ToString();
            this.txtUnits.Text = item.Units.ToString();
            this.txtCost.Text = item.TotalCost.ToString();
        }

        private void RefreshCost(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(this.txtPricePerUnit?.Text, out decimal pricePerUnit))
            {
                this.txtCost.Text = "Invalid price per unit value!";

                return;
            }

            if (!double.TryParse(this.txtUnits?.Text, out double units))
            {
                this.txtCost.Text = "Invalid units value!";

                return;
            }

            this.txtCost.Text = Math.Round(units * (double)pricePerUnit, 2).ToString();
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(this.txtPricePerUnit.Text, out decimal pricePerUnit))
            {
                this.txtCost.Text = "Invalid price per unit value!";

                return;
            }

            if (!double.TryParse(this.txtUnits.Text, out double units))
            {
                this.txtCost.Text = "Invalid units value!";

                return;
            }

            var part = this.txtPart.Text;
            var cost = (decimal)units * pricePerUnit;
            var result = default(IServiceOperationResult);

            if (this.isUpdate)
            {
                this.currentItem.Part = part;
                this.currentItem.PricePerUnit = pricePerUnit;
                this.currentItem.Units = units;

                result = await this.itemService.UpdateAsync(this.currentItem);
            }
            else
            {
                result = await this.itemService
                    .CreateAsync(part, pricePerUnit, units, this.interventionId);
            }

            var keyWord = this.isUpdate ? "updated" : "added";
            var msgWindow = new MessageWindow(result.Success
                ? $"You have successfully {keyWord} a service item."
                : result.ToString());

            msgWindow.ShowDialog();

            if (!result.Success)
            {
                return;
            }

            this.Close();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtPart.Text = string.Empty;
            this.txtPricePerUnit.Text = string.Empty;
            this.txtUnits.Text = string.Empty;
            this.txtCost.Text = string.Empty;
        }
    }
}
