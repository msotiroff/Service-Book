using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ServiceBook.Config;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.User;
using ServiceBook.UI.Windows.Vehicle;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiceBook.UI.Windows
{
    public partial class HomeWindow : Window
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserService userService;
        private readonly IVehicleService vehicleService;
        private readonly IFileProcessor fileProcessor;

        public HomeWindow(
            IServiceProvider serviceProvider,
            IUserService userService,
            IVehicleService vehicleService,
            IFileProcessor fileProcessor)
        {
            InitializeComponent();

            this.serviceProvider = serviceProvider;
            this.userService = userService;
            this.vehicleService = vehicleService;
            this.fileProcessor = fileProcessor;
        }

        private async void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var searchTerm = this.txtSearch.Text;

            if (searchTerm.Length == 0)
            {
                this.imgLoading.Visibility = Visibility.Visible;
                this.UsersGrid.ResetPageIndex();
                this.VehiclesGrid.ResetPageIndex();
                await this.UsersGrid.SetGridDataAsync();
                await this.VehiclesGrid.SetGridDataAsync();
                this.imgLoading.Visibility = Visibility.Hidden;

                return;
            }

            if (e.Key != Key.Enter)
            {
                return;
            }

            if (this.tabItemUsers.IsSelected)
            {
                this.imgLoading.Visibility = Visibility.Visible;
                var users = await this.userService.FilterAsync(searchTerm);

                await this.UsersGrid.SetGridDataAsync(users);
                this.imgLoading.Visibility = Visibility.Hidden;
            }
            else if (this.tabItemVehicles.IsSelected)
            {
                this.imgLoading.Visibility = Visibility.Visible;
                var vehicles = await this.vehicleService.FilterAsync(searchTerm);

                await this.VehiclesGrid.SetGridDataAsync(vehicles);
                this.imgLoading.Visibility = Visibility.Hidden;
            }
        }

        private async void TabItem_Changed(object sender, MouseButtonEventArgs e)
        {
            this.txtSearch.Text = string.Empty;

            if (this.tabItemUsers.IsSelected)
            {
                await this.UsersGrid.SetGridDataAsync();
            }
            else if (this.tabItemVehicles.IsSelected)
            {
                await this.VehiclesGrid.SetGridDataAsync();
            }
        }

        private void BtnAddVehicle_Clicked(object sender, RoutedEventArgs e)
        {
            var addVehicleWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateVehicleWindow>();
            addVehicleWindow.ShowDialog();
        }

        private async void BtnAddUser_Clicked(object sender, RoutedEventArgs e)
        {
            var addUserWindow = this.serviceProvider
                .GetRequiredService<AddOrUpdateUserWindow>();

            addUserWindow.ShowDialog();
            await this.UsersGrid.SetGridDataAsync();
        }

        private async void BtnBackUp_Clicked(object sender, RoutedEventArgs e)
        {
            var folderPicker = new System.Windows.Forms.FolderBrowserDialog
            {
                Description =
                "Please, select a folder that should be the back-up file saved to."
            };
            var result = folderPicker.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            var fileName = $"ServiceBook-BackUp-{Guid.NewGuid().ToString("N")}.db";
            var destinationFullPath = Path.Combine(folderPicker.SelectedPath, fileName);

            await this.fileProcessor
                .CopyAsync(DbConstants.DatabaseFileFullPath, destinationFullPath);
        }

        private void BtnRestore_Clicked(object sender, RoutedEventArgs e)
        {
            var questionWindow = new QuestionWindow(
                "You are about to restore the database, " +
                $"this will overwrite the existing database.{Environment.NewLine}" +
                $"Do you want to make a back-up of the existing database?");

            questionWindow.ShowDialog();

            if (questionWindow.Confirmed)
            {
                this.BtnBackUp_Clicked(sender, e);
            }

            var filePicker = new System.Windows.Forms.OpenFileDialog();
            var result = filePicker.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            Task.Run(() => 
            {
                this.fileProcessor.CopyAsync(
                    filePicker.FileName, DbConstants.DatabaseFileFullPath, overwrite: true);
            })
            .Wait();
        }
    }
}
