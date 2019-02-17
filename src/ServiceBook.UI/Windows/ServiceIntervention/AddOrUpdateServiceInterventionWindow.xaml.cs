using ServiceBook.Services.Interfaces;
using System;
using System.Windows;

namespace ServiceBook.UI.Windows.ServiceIntervention
{
    /// <summary>
    /// Interaction logic for AddOrUpdateServiceInterventionWindow.xaml
    /// </summary>
    public partial class AddOrUpdateServiceInterventionWindow : Window
    {
        private readonly IInterventionService interventionService;
        private Models.DatabaseModels.ServiceIntervention serviceIntervention;
        private string interventionId;
        private string vehicleId;
        private bool isUpdate;

        public AddOrUpdateServiceInterventionWindow(
            IInterventionService interventionService)
        {
            InitializeComponent();

            this.interventionService = interventionService;
        }

        public async void SetRequiredDataAsync(string vehicleId, string interventionId = default(string))
        {
            this.Title = "Add a new service intervention";
            this.vehicleId = vehicleId;
            this.interventionId = interventionId;
            this.isUpdate = interventionId != default(string);

            if (isUpdate)
            {
                this.serviceIntervention = await this.interventionService.GetAsync(interventionId);
                this.Title = "Update a service intervention";
                this.datePickerCurrentDate.SelectedDate = this.serviceIntervention.DateTime;
                this.datePickerNextDate.SelectedDate = this.serviceIntervention.NextServiceIntervalDate;
                this.txtMileage.Text = this.serviceIntervention.Mileage.ToString();
                this.txtDescription.Text = this.serviceIntervention.Description;
            }
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs args)
        {
            var currentDate = this.datePickerCurrentDate.SelectedDate ?? DateTime.UtcNow;
            var nextInterventionDate = this.datePickerNextDate.SelectedDate ?? DateTime.UtcNow.AddYears(1);
            var description = this.txtDescription.Text;
            
            if (!int.TryParse(this.txtMileage.Text, out int mileage))
            {
                return;
            }

            var result = default(IServiceOperationResult);

            if (this.isUpdate)
            {
                this.serviceIntervention.DateTime = currentDate;
                this.serviceIntervention.NextServiceIntervalDate = nextInterventionDate;
                this.serviceIntervention.Description = description;
                this.serviceIntervention.Mileage = mileage;

                result = await this.interventionService.UpdateAsync(this.serviceIntervention);
            }
            else
            {
                result = await this.interventionService
                    .CreateAsync(currentDate, nextInterventionDate, mileage, description, this.vehicleId);
            }

            var keyWord = this.isUpdate ? "updated" : "added";
            var msgWindow = new MessageWindow(result.Success 
                ? $"You have successfully {keyWord} a service intervention." 
                : result.ToString());

            msgWindow.ShowDialog();

            if (!result.Success)
            {
                return;
            }

            this.Close();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs args)
        {
            this.datePickerCurrentDate.SelectedDate = default(DateTime?);
            this.datePickerNextDate.SelectedDate = default(DateTime?);
            this.txtDescription.Text = string.Empty;
            this.txtMileage.Text = string.Empty;
        }
    }
}
