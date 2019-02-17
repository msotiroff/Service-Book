using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Config;
using ServiceBook.Models.Enums;
using ServiceBook.Services.Interfaces;
using ServiceBook.UI.Windows.User;
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
    /// Interaction logic for UsersGrid.xaml
    /// </summary>
    public partial class UsersGrid : UserControl
    {
        private readonly IUserService userService;
        private readonly IServiceProvider serviceProvider;

        private int currentPageIndex;
        private string currentOrderMember;
        private OrderDirection currentOrderDirection;
        private ObservableCollection<Models.DatabaseModels.User> usersGridData;

        public UsersGrid()
        {
            InitializeComponent();

            this.currentOrderDirection = OrderDirection.Ascending;
            this.currentOrderMember = nameof(Models.DatabaseModels.User.Email);
            this.currentPageIndex = 1;

            this.serviceProvider = (IServiceProvider)Application
                .Current
                .Resources[UIConstants.ServiceProviderResourceKey];
            this.userService = this.serviceProvider.GetRequiredService<IUserService>();

            this.SetGridDataAsync().GetAwaiter().GetResult();
        }

        public async Task SetGridDataAsync(
            IEnumerable<Models.DatabaseModels.User> users =
                default(IEnumerable<Models.DatabaseModels.User>))
        {
            if (users != null)
            {
                this.DataGrid.ItemsSource = 
                    new ObservableCollection<Models.DatabaseModels.User>(users);

                return;
            }

            var allUsers = await this.userService.AllAsync(
                    this.currentOrderMember,
                    this.currentOrderDirection,
                    this.currentPageIndex,
                    UIConstants.ItemsPerPage);

            this.usersGridData = new ObservableCollection<Models.DatabaseModels.User>(allUsers);
            this.DataGrid.ItemsSource = this.usersGridData;
        }

        internal void ResetPageIndex()
        {
            this.currentPageIndex = 1;
        }

        private async void BtnEditUser_Clicked(object sender, RoutedEventArgs e)
        {
            var userId = (((Button)sender).DataContext as Models.DatabaseModels.User)?.Id;
            var user = await this.userService.GetAsync(userId);
            var updateUserWindow = this.serviceProvider.GetRequiredService<AddOrUpdateUserWindow>();

            updateUserWindow.Title = $"Update user \"{user.FullName}\"";
            updateUserWindow.SetUserData(user);
            updateUserWindow.ShowDialog();

            await this.SetGridDataAsync();
        }

        private void BtnDetailsUser_Clicked(object sender, RoutedEventArgs e)
        {
           var user = (Models.DatabaseModels.User)((Button)sender).DataContext;
            var userDetailsWindow = this.serviceProvider
                .GetRequiredService<UserDetailsWindow>();

            userDetailsWindow.SetRequiredData(user);
            userDetailsWindow.Title = $"Details for user \"{user.FullName}\"";
            userDetailsWindow.lblInnerTitle.Content = $"{user.FullName}'s vehicles";
            userDetailsWindow.ShowDialog();
        }

        private async void UsersGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = e.OriginalSource as ScrollViewer;

            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                this.currentPageIndex += 1;

                var currentOffset = scrollViewer.VerticalOffset;
                var newData = await this.userService.AllAsync(
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
                    this.usersGridData.Add(item);
                }

                scrollViewer.ScrollToVerticalOffset(currentOffset);
            }
        }

        private void UsersGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var column = e.Column;
            this.currentOrderMember = column.SortMemberPath;
        }
    }
}
