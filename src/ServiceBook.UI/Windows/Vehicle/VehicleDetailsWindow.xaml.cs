using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Models.ViewModels.ServiceIntervention;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.ServiceIntervention;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Windows.Vehicle
{
    /// <summary>
    /// Interaction logic for VehicleDetailsWindow.xaml
    /// </summary>
    public partial class VehicleDetailsWindow : Window
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IVehicleService vehicleService;
        private readonly IInterventionService interventionService;
        private string vehicleId;

        public VehicleDetailsWindow(
            IServiceProvider serviceProvider,
            IVehicleService vehicleService,
            IInterventionService interventionService)
        {
            InitializeComponent();

            this.serviceProvider = serviceProvider;
            this.vehicleService = vehicleService;
            this.interventionService = interventionService;
        }

        public async Task SetRequiredDataAsync(string vehicleId)
        {
            this.vehicleId = vehicleId;

            var interventions = (await this.interventionService
                .GetByVehicleIdAsync(vehicleId))
                .OrderByDescending(i => i.Date);
            this.txtTotalSum.Text = Math.Round(interventions.Sum(i => i.TotalCost), 2).ToString();
            this.InterventionsGrid.ItemsSource = interventions;
        }

        private async void BtnEditIntervention_Clicked(object sender, RoutedEventArgs args)
        {
            var intervention = (ServiceInterventionListViewModel)((Button)sender).DataContext;

            var updateInterventionWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateServiceInterventionWindow>();

            updateInterventionWindow.SetRequiredDataAsync(this.vehicleId, intervention.Id);
            updateInterventionWindow.ShowDialog();

            await this.SetRequiredDataAsync(this.vehicleId);
        }

        private async void BtnDeleteIntervention_Clicked(object sender, RoutedEventArgs args)
        {
            var intervention = (ServiceInterventionListViewModel)((Button)sender).DataContext;
            var questionBox = new QuestionWindow(
                "Are you sure you want to delete this service intervention?");

            questionBox.ShowDialog();

            if (!questionBox.Confirmed)
            {
                return;
            }

            await this.interventionService.RemoveAsync(intervention.Id);
            await this.SetRequiredDataAsync(this.vehicleId);
        }

        private async void BtnDetailsIntervention_Clicked(object sender, RoutedEventArgs args)
        {
            var intervention = (ServiceInterventionListViewModel)((Button)sender).DataContext;
            var interventionDetailsWindow = this.serviceProvider
                .GetRequiredService<InterventionDetailsWindow>();

            await interventionDetailsWindow.SetRequiredDataAsync(intervention.Id);

            interventionDetailsWindow.ShowDialog();

            await this.SetRequiredDataAsync(this.vehicleId);
        }

        private async void BtnAddIntervention_Clicked(object sender, RoutedEventArgs e)
        {
            var addInterventionWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateServiceInterventionWindow>();

            addInterventionWindow.SetRequiredDataAsync(this.vehicleId);
            addInterventionWindow.ShowDialog();

            await this.SetRequiredDataAsync(this.vehicleId);
        }
    }
}
