using Newtonsoft.Json;
using ServiceBook.Data;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServiceBook.Services.Seed
{
    public class DatabaseSeeder
    {
        private readonly ServiceBookDbContext dbcContext;
        private readonly Random random;

        public DatabaseSeeder(ServiceBookDbContext dbcContext)
        {
            this.dbcContext = dbcContext;
            this.random = new Random();
        }

        public void SeedDatabase(
            int usersCount,
            int vehiclesPerUser,
            int interventionsPerVehicle,
            int itemsPerIntervention)
        {
            var users = this.CreateUsers(usersCount);
            var vehicles = this.CreateVehicles(vehiclesPerUser, users);
            var interventions = this.CreateInterventions(interventionsPerVehicle, vehicles);
            var serviceItems = this.CreateServiceItems(itemsPerIntervention, interventions);

            this.dbcContext.Users.AddRange(users);
            this.dbcContext.SaveChanges();

            this.dbcContext.Vehicles.AddRange(vehicles);
            this.dbcContext.SaveChanges();

            this.dbcContext.ServiceInterventions.AddRange(interventions);
            this.dbcContext.SaveChanges();

            this.dbcContext.ServiceItems.AddRange(serviceItems);
            this.dbcContext.SaveChanges();
        }

        private IList<ServiceItem> CreateServiceItems(
            int itemsPerIntervention, IList<ServiceIntervention> interventions)
        {
            var items = new List<ServiceItem>();

            foreach (var intervention in interventions)
            {
                var currentItems = Enumerable.Range(0, itemsPerIntervention)
                    .Select(index => new ServiceItem
                    {
                        Part = $"Part # {index + 1}",
                        PricePerUnit = (decimal)(this.random.NextDouble() * 100),
                        Units = (double)this.random.Next(1, 5),
                        ServiceInterventionId = intervention.Id
                    });

                items.AddRange(currentItems);
            }
            
            return items;
        }

        private IList<ServiceIntervention> CreateInterventions(
            int interventionsPerVehicle, IList<Vehicle> vehicles)
        {
            var interventions = new List<ServiceIntervention>();

            foreach (var vehicle in vehicles)
            {
                var currentInterventions = Enumerable.Range(0, interventionsPerVehicle)
                    .Select(index => new ServiceIntervention
                    {
                        Mileage = 100000 + (index * 10000),
                        Description = "Some loooooooooooooooooooooooong description",
                        DateTime = DateTime.Now.AddDays((interventionsPerVehicle - index) * (-700)),
                        NextServiceIntervalDate = DateTime.Now
                        .AddDays((interventionsPerVehicle - index) * (-700))
                        .AddMonths(12),
                        VehicleId = vehicle.Id
                    });

                interventions.AddRange(currentInterventions);
            }
            
            return interventions;
        }

        private IList<Vehicle> CreateVehicles(int vehiclesPerUser, IList<User> users)
        {
            var vehicles = new List<Vehicle>();
            var jsonVehicles = File.ReadAllText(@"..\..\..\ServiceBook.Data\Seed\vehicles-list.json");
            var vehicleDtos = JsonConvert.DeserializeObject<VehicleDto[]>(jsonVehicles);
            var registrationPlateNumber = 1000;

            foreach (var user in users)
            {
                for (int i = 0; i < vehiclesPerUser; i++)
                {
                    var randomVehicle = vehicleDtos[this.random.Next(0, vehicleDtos.Length)];
                    var randomModel = randomVehicle.Models[this.random.Next(0, randomVehicle.Models.Length)];

                    vehicles.Add(new Vehicle
                    {
                        Make = randomVehicle.Make,
                        Model = randomModel,
                        ExactModelName = string.Empty,
                        RegistrationPlate = $"C {registrationPlateNumber++} AA",
                        VIN = $"WDB{this.random.Next(1000000, 10000000)}",
                        OwnerId = user.Id
                    });
                }
            }

            return vehicles;
        }

        private IList<User> CreateUsers(int count)
        {
            var users = new List<User>();
            var jsonUsers = File.ReadAllText(@"..\..\..\ServiceBook.Data\Seed\users-list.json");
            var userDtos = JsonConvert.DeserializeObject<UserDto[]>(jsonUsers);

            for (int i = 0; i < count; i++)
            {
                var user = userDtos[i % userDtos.Length];

                users.Add(new User
                {
                    Email = user.Email,
                    DateCreated = DateTime.Now
                        .AddDays(this.random.Next(1, 700) * (-1)),
                    FirstName = user.UserName.Split('.').First(),
                    LastName = user.UserName.Split('.').Last(),
                    PhoneNumber = DateTime.Now.Ticks.ToString()
                });
            }

            return users;
        }
    }
}
