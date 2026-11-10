using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuth {
    public class MultiTenantDashboardConfigurator : DashboardConfigurator {
        public MultiTenantDashboardConfigurator(
            AuthenticationStateProvider authenticationStateProvider,
            CustomConnectionStringProvider connectionStrings,
            CustomDataSourceStorage dataSources,
            CustomDashboardStorage dashboards,
            CustomDBSchemaProvider schema) {

            SetConnectionStringsProvider(connectionStrings);
            SetDataSourceStorage(dataSources);
            SetDashboardStorage(dashboards);
            SetDBSchemaProvider(schema);

            VerifyClientTrustLevel += (s, e) => {
                var userName = AuthHelper.GetUserName(authenticationStateProvider);

                if (string.IsNullOrEmpty(userName) || userName == "guest")
                    e.ClientTrustLevel = ClientTrustLevel.Restricted;
            };
        }
    }
}