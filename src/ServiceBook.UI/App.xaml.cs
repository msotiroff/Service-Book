using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceBook.Config;
using ServiceBook.DAL;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Data;
using ServiceBook.Models.Factories;
using ServiceBook.Models.Interfaces;
using ServiceBook.Services;
using ServiceBook.Services.Interfaces;
using ServiceBook.Services.Seed;
using ServiceBook.UI.Windows;
using ServiceBook.UI.Windows.ServiceIntervention;
using ServiceBook.UI.Windows.ServiceItem;
using ServiceBook.UI.Windows.User;
using ServiceBook.UI.Windows.Vehicle;
using System;
using System.Text;
using System.Windows;

namespace ServiceBook.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.EnsureDatabasePathExist();

            IServiceProvider serviceProvider = this.BuildServiceProvider();

            this.EnsureDatabaseUpToDate(serviceProvider);

            //this.SeedDatabase(serviceProvider);

            Current.Resources[UIConstants.ServiceProviderResourceKey] = serviceProvider;
            AppDomain.CurrentDomain.UnhandledException += this.LoggingExceptionHandler;

            var homeWindow = serviceProvider.GetRequiredService<HomeWindow>();
            homeWindow.Show();
        }

        private void LoggingExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            var dbDirectory = DbConstants.DbFolderPath;
            var logFileName = "ExceptionsLog.log";
            var fullPath = System.IO.Path.Combine(dbDirectory, logFileName);
            var builder = new StringBuilder()
                .AppendLine("Unhandled exception:")
                .AppendLine($"Error occured at {DateTime.Now.ToString()}")
                .AppendLine($"Message: {exception.Message}")
                .AppendLine($"Source: {exception.Source}")
                .AppendLine($"Stack trace: {exception.StackTrace}")
                .AppendLine()
                .AppendLine(new string('-', 20))
                .AppendLine();

            System.IO.File.AppendAllText(fullPath, builder.ToString());

            var msgWindow = new MessageWindow(
                $"An error occured while executing the action. See the log file: {fullPath}");

            msgWindow.ShowDialog();

            Environment.Exit(1);
        }

        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Database dependencies:
            var databasePath = DbConstants.DatabaseFileFullPath;
            services.AddDbContext<ServiceBookDbContext>();
            services.AddTransient<DbContext, ServiceBookDbContext>();

            // UI dependencies:
            services.AddTransient<HomeWindow>();
            services.AddTransient<AddOrUpdateUserWindow>();
            services.AddTransient<AddOrUpdateVehicleWindow>();
            services.AddTransient<VehicleDetailsWindow>();
            services.AddTransient<UserDetailsWindow>();
            services.AddTransient<AddOrUpdateServiceInterventionWindow>();
            services.AddTransient<InterventionDetailsWindow>();
            services.AddTransient<AddOrUpdateServiceItemWindow>();

            // Validation dependencies:
            services.AddTransient<IModelValidator, ModelValidator>();

            // File processing dependencies:
            services.AddTransient<IFileProcessor, FileProcessor>();

            // User dependencies:
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserFactory, UserFactory>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Vehicle dependencies:
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IVehicleFactory, VehicleFactory>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();

            // ServiceIntervention dependencies:
            services.AddTransient<IInterventionService, InterventionService>();
            services.AddTransient<IServiceInterventionFactory, ServiceInterventionFactory>();
            services.AddTransient<IServiceInterventionRepository, ServiceInterventionRepository>();

            // ServiceItem dependencies:
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IServiceItemFactory, ServiceItemFactory>();
            services.AddTransient<IServiceItemRepository, ServiceItemRepository>();

            return services.BuildServiceProvider();
        }

        private void EnsureDatabasePathExist()
        {
            if (!System.IO.Directory.Exists(DbConstants.DbFolderPath))
            {
                System.IO.Directory.CreateDirectory(DbConstants.DbFolderPath);
            }
        }

        private void EnsureDatabaseUpToDate(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<ServiceBookDbContext>();

            context.Database.Migrate();
        }

        private void SeedDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ServiceBookDbContext>();
            var seeder = new DatabaseSeeder(context);

            seeder.SeedDatabase(100, 10, 10, 4);
        }
    }
}
