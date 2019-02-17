using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Models.ViewModels.Vehicle;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.Vehicle;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Windows.User
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IVehicleService vehicleService;
        private Models.DatabaseModels.User user;

        public UserDetailsWindow(IServiceProvider serviceProvider, IVehicleService vehicleService)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.vehicleService = vehicleService;
        }

        public void SetRequiredData(Models.DatabaseModels.User user)
        {
            this.user = user;
            this.FillVehiclesAsync();
        }

        private async void BtnDetailsVehicle_Clicked(object sender, RoutedEventArgs e)
        {
            var vehicle = (VehicleListViewModel)((Button)sender).DataContext;
            var vehicleDetailsWindow = this.serviceProvider
                .GetRequiredService<VehicleDetailsWindow>();

            vehicleDetailsWindow.Title = $"Details for vehicle \"{vehicle.RegistrationPlate}\"";

            await vehicleDetailsWindow.SetRequiredDataAsync(vehicle.Id);

            vehicleDetailsWindow.ShowDialog();
        }

        private async void FillVehiclesAsync()
        {
            var vehicles = await this.vehicleService.GetByUserIdAsync(this.user.Id);

            this.UserVehiclesGrid.ItemsSource = vehicles;
        }
    }
}
