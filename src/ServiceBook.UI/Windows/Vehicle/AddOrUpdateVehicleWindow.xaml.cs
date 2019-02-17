using ServiceBook.Services.Interfaces;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ServiceBook.UI.Windows.Vehicle
{
    public partial class AddOrUpdateVehicleWindow : Window
    {
        private readonly IVehicleService vehicleService;
        private readonly IUserService userService;
        private Models.DatabaseModels.Vehicle vehicleToBeUpdated;
        private bool isUpdate;

        public AddOrUpdateVehicleWindow(
            IVehicleService vehicleService,
            IUserService userService)
        {
            InitializeComponent();

            this.vehicleService = vehicleService;
            this.userService = userService;

            this.InitializeUsersAsync();
        }

        public void SetVehicleData(Models.DatabaseModels.Vehicle vehicle)
        {
            this.isUpdate = true;
            this.vehicleToBeUpdated = vehicle;
            this.txtMake.Text = vehicle.Make;
            this.txtModel.Text = vehicle.Model;
            this.txtExactModelNAme.Text = vehicle.ExactModelName;
            this.txtVIN.Text = vehicle.VIN;
            this.txtRegPlate.Text = vehicle.RegistrationPlate;

            foreach (ComboBoxItem item in this.comboBoxOwner.Items)
            {
                if ((string)item.DataContext == vehicle.OwnerId)
                {
                    this.comboBoxOwner.SelectedItem = item;
                    break;
                }
            }
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var make = this.txtMake.Text;
            var model = this.txtModel.Text;
            var exactModelName = this.txtExactModelNAme.Text;
            var vin = this.txtVIN.Text;
            var regPlate = this.txtRegPlate.Text;
            var ownerId = ((ComboBoxItem)this.comboBoxOwner.SelectedItem).DataContext as string;

            var result = default(IServiceOperationResult);

            if (this.isUpdate)
            {
                this.vehicleToBeUpdated.Make = make;
                this.vehicleToBeUpdated.Model = model;
                this.vehicleToBeUpdated.ExactModelName = exactModelName;
                this.vehicleToBeUpdated.VIN = vin;
                this.vehicleToBeUpdated.RegistrationPlate = regPlate;
                this.vehicleToBeUpdated.OwnerId = ownerId;

                result = await this.vehicleService.UpdateAsync(this.vehicleToBeUpdated);
            }
            else
            {
                result = await this.vehicleService
                    .CreateAsync(make, model, exactModelName, vin, regPlate, ownerId);
            }

            if (result.Success)
            {
                var keyword = this.isUpdate ? "updated" : "created";
                var msgWindow = new MessageWindow(
                    $"Vehicle '{make} {model}' {keyword} successfully.");

                msgWindow.ShowDialog();

                this.Close();
            }
            else
            {
                var msgWindow = new MessageWindow(result.ToString());

                msgWindow.ShowDialog();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtMake.Text = string.Empty;
            this.txtModel.Text = string.Empty;
            this.txtExactModelNAme.Text = string.Empty;
            this.txtVIN.Text = string.Empty;
            this.txtRegPlate.Text = string.Empty;

            if (this.comboBoxOwner.Items.Count > 0)
            {
                this.comboBoxOwner.SelectedIndex = 0;
            }
        }

        private async void InitializeUsersAsync()
        {
            var allUsers = (await this.userService.AllAsync(nameof(Models.DatabaseModels.User.FirstName)))
                .OrderBy(u => u.FullName);

            foreach (var user in allUsers)
            {
                this.comboBoxOwner.Items.Add(new ComboBoxItem
                {
                    Content = user.FullName,
                    DataContext = user.Id
                });
            }

            if (allUsers.Any())
            {
                this.comboBoxOwner.SelectedIndex = 0;
            }
        }
    }
}
