using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BlazorAuth {
    public class CustomDashboardStorage : IEditableDashboardStorage {
        private string userName;
        private string dashboardStorageFolder;

        public CustomDashboardStorage(IWebHostEnvironment hostingEnvironment, string userName) {
            this.userName = userName;
            this.dashboardStorageFolder = hostingEnvironment.ContentRootFileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath;
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo() {
            var dashboardInfos = new List<DashboardInfo>();

            var files = Directory.GetFiles(dashboardStorageFolder, "*.xml");

            foreach (var item in files) {
                var name = Path.GetFileNameWithoutExtension(item);

                if (!string.IsNullOrEmpty(userName) && name.EndsWith(userName, System.StringComparison.InvariantCultureIgnoreCase))
                    dashboardInfos.Add(new DashboardInfo() { ID = name, Name = name });
            }

            return dashboardInfos;
        }

        public XDocument LoadDashboard(string dashboardID) {
            if (GetAvailableDashboardsInfo().Any(di => di.Name == dashboardID)) {
                var path = Path.Combine(dashboardStorageFolder, dashboardID + ".xml");
                var content = File.ReadAllText(path);
                return XDocument.Parse(content);
            } else {
                throw new System.ApplicationException("You are not authorized to view this dashboard.");
            }
        }

        public string AddDashboard(XDocument dashboard, string dashboardName) {
            if (string.IsNullOrEmpty(userName) || userName != "admin")
                throw new System.ApplicationException("You are not authorized to add dashboards.");

            var path = Path.Combine(dashboardStorageFolder, dashboardName + "_" + userName + ".xml");

            File.WriteAllText(path, dashboard.ToString());

            return Path.GetFileNameWithoutExtension(path);
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard) {
            if (string.IsNullOrEmpty(userName) || userName != "admin")
                throw new System.ApplicationException("You are not authorized to save dashboards.");

            var path = Path.Combine(dashboardStorageFolder, dashboardID + ".xml");

            File.WriteAllText(path, dashboard.ToString());
        }
    }
}