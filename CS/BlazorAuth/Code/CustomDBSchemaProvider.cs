using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuth {
    public class CustomDBSchemaProvider : DBSchemaProviderEx {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public CustomDBSchemaProvider(AuthenticationStateProvider authenticationStateProvider) : base() {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public override DBTable[] GetTables(SqlDataConnection connection, params string[] tableList) {
            var result = base.GetTables(connection, tableList);
            var userName = AuthHelper.GetUserName(authenticationStateProvider);

            if (userName == "admin") {
                return result;
            } else if (userName == "user") {
                return result.Where(t => t.Name == "Cars").ToArray();
            } else {
                return new DBTable[0];
            }
        }
    }
}