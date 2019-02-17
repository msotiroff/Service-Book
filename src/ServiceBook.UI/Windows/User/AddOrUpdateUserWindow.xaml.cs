using ServiceBook.Services.Interfaces;
using System.Windows;

namespace ServiceBook.UI.Windows.User
{
    /// <summary>
    /// Interaction logic for AddOrUpdateUser.xaml
    /// </summary>
    public partial class AddOrUpdateUserWindow : Window
    {
        private readonly IUserService userService;
        private Models.DatabaseModels.User userToBeUpdated;
        private bool isUpdate;

        public AddOrUpdateUserWindow(IUserService userService)
        {
            InitializeComponent();

            this.userService = userService;
        }

        public void SetUserData(Models.DatabaseModels.User user)
        {
            this.txtEmail.Text = user.Email;
            this.txtFirstName.Text = user.FirstName;
            this.txtLastName.Text = user.LastName;
            this.txtPhoneNumber.Text = user.PhoneNumber;

            this.isUpdate = true;
            this.userToBeUpdated = user;
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var email = this.txtEmail.Text;
            var firstName = this.txtFirstName.Text;
            var lastName = this.txtLastName.Text;
            var phoneNumber = this.txtPhoneNumber.Text;

            var result = default(IServiceOperationResult);

            if (this.isUpdate)
            {
                this.userToBeUpdated.Email = email;
                this.userToBeUpdated.FirstName = firstName;
                this.userToBeUpdated.LastName = lastName;
                this.userToBeUpdated.PhoneNumber = phoneNumber;

                result = await this.userService.UpdateAsync(this.userToBeUpdated);
            }
            else
            {
                result = await this.userService
                    .CreateAsync(email, firstName, lastName, phoneNumber);
            }

            if (result.Success)
            {
                var keyword = this.isUpdate ? "updated" : "created";
                var msgWindow = new MessageWindow(
                    $"User '{firstName} {lastName}' {keyword} successfully.");

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
            this.txtEmail.Text = string.Empty;
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtPhoneNumber.Text = string.Empty;
        }
    }
}
