using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlazorAuth {
    public class MultiTenantDashboardConfigurator : DashboardConfigurator {
        private string userName;

        public MultiTenantDashboardConfigurator(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor) {
            var identityName = contextAccessor.HttpContext.User.Identity.Name;
            userName = identityName?.Substring(0, identityName.IndexOf("@"));

            SetConnectionStringsProvider(new CustomConnectionStringProvider(userName));
            SetDataSourceStorage(new CustomDataSourceStorage(userName));
            SetDashboardStorage(new CustomDashboardStorage(hostingEnvironment, userName));
            SetDBSchemaProvider(new CustomDBSchemaProvider(userName));

            VerifyClientTrustLevel += MultiTenantDashboardConfigurator_VerifyClientTrustLevel;
        }
        private void MultiTenantDashboardConfigurator_VerifyClientTrustLevel(object sender, VerifyClientTrustLevelEventArgs e) {
            if (string.IsNullOrEmpty(userName) || userName == "guest")
                e.ClientTrustLevel = ClientTrustLevel.Restricted;
        }
    }
}