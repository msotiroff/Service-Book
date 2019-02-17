using System;
using System.IO;

namespace ServiceBook.Config
{
    public class DbConstants
    {
        /// Database tables names:
        public const string UsersTableName = "Users";
        public const string VehiclesTableName = "Vehicles";
        public const string ServiceItemsTableName = "ServiceItems";
        public const string ServiceInterventionsTableName = "ServiceInterventions";

        /// Defines the database path:
        private const string DatabaseName = "ServiceBook.db";
        public static readonly string DbFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            "Service Book");
        public static readonly string DatabaseFileFullPath = Path.Combine(DbFolderPath, DatabaseName);
        public static readonly string SQLiteConnectionString = $@"Data Source={DatabaseFileFullPath};";

        /// Indexes names:
        public const string ServiceInterventionVehicleIdIndexName = "IX_ServiceIntervention_VehicleId";
        public const string ServiceItemServiceInterventionIdIndexName = "IX_ServiceItem_ServiceInterventionId";
    }
}
