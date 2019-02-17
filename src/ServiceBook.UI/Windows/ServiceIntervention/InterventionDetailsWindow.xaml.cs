using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Models.ViewModels.ServiceIntervention;
using ServiceBook.Models.ViewModels.ServiceItem;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.ServiceItem;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Windows.ServiceIntervention
{
    /// <summary>
    /// Interaction logic for InterventionDetailsWindow.xaml
    /// </summary>
    public partial class InterventionDetailsWindow : Window
    {
        private readonly IItemService itemService;
        private readonly IInterventionService interventionService;
        private readonly IServiceProvider serviceProvider;
        private Models.DatabaseModels.ServiceIntervention currentIntervention;

        public InterventionDetailsWindow(
            IItemService itemService,
            IInterventionService interventionService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.itemService = itemService;
            this.interventionService = interventionService;
            this.serviceProvider = serviceProvider;
        }

        internal async Task SetRequiredDataAsync(string interventionId)
        {
            this.currentIntervention = await this.interventionService.GetAsync(interventionId);
            var allItems = await this.itemService
                .GetByInterventionIdAsync(this.currentIntervention.Id);
            var total = Math.Round(allItems.Sum(i => i.TotalCost), 2);
            this.txtDescription.Text = this.currentIntervention.Description;
            this.ItemsGrid.ItemsSource = allItems;
            this.txtTotalCost.Text = total.ToString();
        }

        private async void BtnAddItem_Clicked(object sender, RoutedEventArgs e)
        {
            var addItemWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateServiceItemWindow>();

            addItemWindow.SetRequiredData(this.currentIntervention.Id);

            addItemWindow.ShowDialog();

            await this.SetRequiredDataAsync(this.currentIntervention.Id);
        }

        private async void BtnEditItem_Clicked(object sender, RoutedEventArgs e)
        {
            var item = (ServiceItemListViewModel)((Button)sender).DataContext;
            var editItemWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateServiceItemWindow>();

            editItemWindow.SetRequiredData(this.currentIntervention.Id, item);

            editItemWindow.ShowDialog();

            await this.SetRequiredDataAsync(this.currentIntervention.Id);
        }

        private async void BtnDeleteItem_Clicked(object sender, RoutedEventArgs e)
        {
            var item = (ServiceItemListViewModel)((Button)sender).DataContext;
            var questionBox = new QuestionWindow(
                "Are you sure you want to delete this service item?");

            questionBox.ShowDialog();

            if (!questionBox.Confirmed)
            {
                return;
            }

            await this.itemService.RemoveAsync(item.Id);
            await this.SetRequiredDataAsync(this.currentIntervention.Id);
        }
    }
}
