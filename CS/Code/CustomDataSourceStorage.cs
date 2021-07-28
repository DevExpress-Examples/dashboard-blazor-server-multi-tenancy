using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BlazorAuth {
    public class CustomDataSourceStorage : IDataSourceStorage {
        private string userName;
        private Dictionary<string, XDocument> documents = new Dictionary<string, XDocument>();

        private const string sqlDataSourceId1 = "SQL Data Source (Northwind)";
        private const string sqlDataSourceId2 = "SQL Data Source (CarsXtraScheduling)";
        private const string jsonDataSourceId = "JSON Data Source";

        public CustomDataSourceStorage(string userName) {
            this.userName = userName;

            DashboardSqlDataSource sqlDataSource1 = new DashboardSqlDataSource(sqlDataSourceId1, "NorthwindConnectionString");
            SelectQuery query1 = SelectQueryFluentBuilder
                .AddTable("Categories")
                .SelectAllColumnsFromTable()
                .Build("Categories");
            sqlDataSource1.Queries.Add(query1);
            SelectQuery query2 = SelectQueryFluentBuilder
                .AddTable("Products")
                .SelectAllColumnsFromTable()
                .Build("Products");
            sqlDataSource1.Queries.Add(query2);

            DashboardSqlDataSource sqlDataSource2 = new DashboardSqlDataSource(sqlDataSourceId2, "CarsXtraSchedulingConnectionString");
            SelectQuery query = SelectQueryFluentBuilder
                .AddTable("Cars")
                .SelectAllColumnsFromTable()
                .Build("Cars");
            sqlDataSource2.Queries.Add(query);

            DashboardJsonDataSource jsonDataSource = new DashboardJsonDataSource(jsonDataSourceId);
            jsonDataSource.JsonSource = new UriJsonSource(new System.Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
            jsonDataSource.RootElement = "Customers";

            documents[sqlDataSourceId1] = new XDocument(sqlDataSource1.SaveToXml());
            documents[sqlDataSourceId2] = new XDocument(sqlDataSource2.SaveToXml());
            documents[jsonDataSourceId] = new XDocument(jsonDataSource.SaveToXml());
        }

        public XDocument GetDataSource(string dataSourceID) {
            if (GetDataSourcesID().Contains(dataSourceID)) {
                return documents[dataSourceID];
            } else {
                throw new System.ApplicationException("You are not authorized to use this datasource.");
            }
        }

        public IEnumerable<string> GetDataSourcesID() {
            if (userName == "admin") {
                return documents.Keys;
            } else if (userName == "user") {
                return new string[] { sqlDataSourceId2 };
            } else {
                return new string[0];
            }
        }
    }
}