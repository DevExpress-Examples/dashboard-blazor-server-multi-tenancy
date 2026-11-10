using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;

namespace BlazorAuth {
    public class MultiTenantDashboardConfigurator : DashboardConfigurator {
        public MultiTenantDashboardConfigurator(
            IDashboardUserProvider dashboardUserProvider,
            IDataSourceWizardConnectionStringsProvider connectionStringsProvider,
            IDataSourceStorage dataSourceStorage,
            IEditableDashboardStorage dashboardStorage,
            IDBSchemaProviderEx dbSchemaProvider) {

            SetConnectionStringsProvider(connectionStringsProvider);
            SetDataSourceStorage(dataSourceStorage);
            SetDashboardStorage(dashboardStorage);
            SetDBSchemaProvider(dbSchemaProvider);

            VerifyClientTrustLevel += (s, e) => {
                var userName = dashboardUserProvider.GetUserName();

                if (string.IsNullOrEmpty(userName) || userName == "guest")
                    e.ClientTrustLevel = ClientTrustLevel.Restricted;
            };
        }
    }
}