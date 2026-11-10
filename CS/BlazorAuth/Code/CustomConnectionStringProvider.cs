using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;

namespace BlazorAuth {
    public class CustomConnectionStringProvider : IDataSourceWizardConnectionStringsProvider {
        readonly IDashboardUserProvider dashboardUserProvider;
        readonly Dictionary<string, string> connectionStrings = new Dictionary<string, string>();

        public CustomConnectionStringProvider(IDashboardUserProvider dashboardUserProvider) {
            this.dashboardUserProvider = dashboardUserProvider;
            connectionStrings.Add("NorthwindConnectionString", @"XpoProvider=SQLite; Data Source=App_Data/nwind.db;");
            connectionStrings.Add("CarsXtraSchedulingConnectionString", @"XpoProvider=SQLite;Data Source=App_Data/CarsDB.db;");
        }

        public Dictionary<string, string> GetConnectionDescriptions() {
            var userName = dashboardUserProvider.GetUserName();
            var connections = new Dictionary<string, string>();

            if (userName == "admin") {
                connections.Add("NorthwindConnectionString", "Northwind Connection");
                connections.Add("CarsXtraSchedulingConnectionString", "CarsXtraScheduling Connection");
            } else if (userName == "user") {
                connections.Add("CarsXtraSchedulingConnectionString", "CarsXtraScheduling Connection");
            }

            return connections;
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name) {
            if (GetConnectionDescriptions().ContainsKey(name)) {
                return new CustomStringConnectionParameters(connectionStrings[name]);
            } else {
                throw new System.ApplicationException("You are not authorized to use this connection.");
            }
        }
    }
}