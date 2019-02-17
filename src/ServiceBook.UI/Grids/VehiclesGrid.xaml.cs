using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Config;
using ServiceBook.Models.Enums;
using ServiceBook.Models.ViewModels.Vehicle;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.Vehicle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Grids
{
    /// <summary>
    /// Interaction logic for VehiclesGrid.xaml
    /// </summary>
    public partial class VehiclesGrid : UserControl
    {
        private readonly IUserService userService;
        private readonly IVehicleService vehicleService;
        private readonly IServiceProvider serviceProvider;

        private int currentPageIndex;
        private string currentOrderMember;
        private OrderDirection currentOrderDirection;
        private ObservableCollection<VehicleListViewModel> vehiclesGridData;

        public VehiclesGrid()
        {
            InitializeComponent();

            this.serviceProvider = (IServiceProvider)Application
                .Current
                .Resources[UIConstants.ServiceProviderResourceKey];

            this.userService = this.serviceProvider.GetRequiredService<IUserService>();
            this.vehicleService = this.serviceProvider.GetRequiredService<IVehicleService>();

            this.currentPageIndex = 1;
            this.currentOrderMember = nameof(Models.DatabaseModels.Vehicle.RegistrationPlate);
            this.currentOrderDirection = OrderDirection.Ascending;

            this.SetGridDataAsync().GetAwaiter().GetResult();
        }

        public async Task SetGridDataAsync(IEnumerable<VehicleListViewModel> vehicles = null)
        {
            if (vehicles != null)
            {
                this.DataGrid.ItemsSource = 
                    new ObservableCollection<VehicleListViewModel>(vehicles);

                return;
            }

            var allVehicles = await this.vehicleService.AllAsync(
                    this.currentOrderMember,
                    this.currentOrderDirection,
                    this.currentPageIndex,
                    UIConstants.ItemsPerPage);

            this.vehiclesGridData = new ObservableCollection<VehicleListViewModel>(allVehicles);
            this.DataGrid.ItemsSource = this.vehiclesGridData;
        }

        internal void ResetPageIndex()
        {
            this.currentPageIndex = 1;
        }

        private async void BtnEditVehicle_Clicked(object sender, RoutedEventArgs e)
        {
            var vehicleModel = (VehicleListViewModel)((Button)sender).DataContext;
            var vehicle = await this.vehicleService.GetAsync(vehicleModel.Id);
            var updateVehicleWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateVehicleWindow>();

            updateVehicleWindow.Title = $"Update vehicle \"{vehicle.RegistrationPlate}\"";
            updateVehicleWindow.SetVehicleData(vehicle);
            updateVehicleWindow.ShowDialog();

            await this.SetGridDataAsync();
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
        
        private async void VehiclesGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = e.OriginalSource as ScrollViewer;

            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                this.currentPageIndex += 1;

                var currentOffset = scrollViewer.VerticalOffset;
                var newData = await this.vehicleService.AllAsync(
                    this.currentOrderMember,
                    this.currentOrderDirection,
                    this.currentPageIndex,
                    UIConstants.ItemsPerPage);

                if (newData.Count() < UIConstants.ItemsPerPage)
                {
                    this.currentPageIndex -= 1;
                }

                foreach (var item in newData)
                {
                    this.vehiclesGridData.Add(item);
                }

                scrollViewer.ScrollToVerticalOffset(currentOffset);
            }
        }

        private void VehiclesGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var column = e.Column;
            this.currentOrderMember = column.SortMemberPath;
        }
    }
}
