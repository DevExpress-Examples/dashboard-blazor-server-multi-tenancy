using System.Xml.Linq;
using DevExpress.DashboardWeb;
using DevExpress.Security;

namespace BlazorAuth {
    public class CustomDashboardStorage : IEditableDashboardStorage {
        readonly IDashboardUserProvider dashboardUserProvider;
        string DashboardStorageFolder { get; }

        public CustomDashboardStorage(IWebHostEnvironment hostingEnvironment, IDashboardUserProvider dashboardUserProvider) {
            this.dashboardUserProvider = dashboardUserProvider;
            this.DashboardStorageFolder = hostingEnvironment.ContentRootFileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath!
                ?? throw new InvalidOperationException("Dashboard storage dashboardPath is not available.");
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo() {
            var userName = dashboardUserProvider.GetUserName();
            var dashboardInfos = new List<DashboardInfo>();

            var files = Directory.GetFiles(DashboardStorageFolder, "*.xml");

            foreach(var item in files) {
                var name = Path.GetFileNameWithoutExtension(item);

                if(!string.IsNullOrEmpty(userName) && name.EndsWith(userName, System.StringComparison.InvariantCultureIgnoreCase))
                    dashboardInfos.Add(new DashboardInfo() { ID = name, Name = name });
            }

            return dashboardInfos;
        }

        public XDocument LoadDashboard(string dashboardID) {
            if(GetAvailableDashboardsInfo().Any(di => di.Name == dashboardID)) {
                var dashboardPath = Path.GetFullPath(Path.Combine(DashboardStorageFolder, dashboardID + ".xml"));
                ValidateDashboardPath(dashboardPath);

                var content = File.ReadAllText(dashboardPath);
                return XDocument.Parse(content);
            } else {
                throw new System.ApplicationException("You are not authorized to view this dashboard.");
            }
        }

        public string AddDashboard(XDocument dashboard, string dashboardName) {
            var userName = dashboardUserProvider.GetUserName();

            if(string.IsNullOrEmpty(userName) || userName != "admin")
                throw new System.ApplicationException("You are not authorized to add dashboards.");

            var dashboardPath = Path.GetFullPath(Path.Combine(DashboardStorageFolder, dashboardName + "_" + userName + ".xml"));
            ValidateDashboardPath(dashboardPath);

            File.WriteAllText(dashboardPath, dashboard.ToString());

            return Path.GetFileNameWithoutExtension(dashboardPath);
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard) {
            var userName = dashboardUserProvider.GetUserName();

            if(string.IsNullOrEmpty(userName) || userName != "admin")
                throw new System.ApplicationException("You are not authorized to save dashboards.");

            var dashboardPath = Path.GetFullPath(Path.Combine(DashboardStorageFolder, dashboardID + ".xml"));
            ValidateDashboardPath(dashboardPath);

            File.WriteAllText(dashboardPath, dashboard.ToString());
        }

        void ValidateDashboardPath(string dashboardPath) {
            if(!SafePath.IsRooted(DashboardStorageFolder, dashboardPath))
                throw new InvalidOperationException("Invalid dashboard folder dashboardPath");
        }
    }
}