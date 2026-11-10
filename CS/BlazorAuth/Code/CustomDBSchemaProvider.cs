using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;

namespace BlazorAuth {
    public class CustomDBSchemaProvider : DBSchemaProviderEx {
        private readonly IDashboardUserProvider dashboardUserProvider;

        public CustomDBSchemaProvider(IDashboardUserProvider dashboardUserProvider) : base() {
            this.dashboardUserProvider = dashboardUserProvider;
        }

        public override DBTable[] GetTables(SqlDataConnection connection, params string[] tableList) {
            var result = base.GetTables(connection, tableList);
            var userName = dashboardUserProvider.GetUserName();

            if (userName == "admin") {
                return result;
            } else if (userName == "user") {
                return result.Where(t => t.Name == "Cars").ToArray();
            } else {
                return [];
            }
        }
    }
}