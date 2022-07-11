using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BlazorAuth {
    public class CustomDBSchemaProvider : DBSchemaProviderEx {
        private string userName;

        public CustomDBSchemaProvider(string userName) : base() {
            this.userName = userName;
        }

        public override DBTable[] GetTables(SqlDataConnection connection, params string[] tableList) {
            var result = base.GetTables(connection, tableList);

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