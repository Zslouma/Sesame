using SQLite;

namespace Syracuse.Mobitheque.Core.Models
{
    public class CookiesSave
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public bool Active { get; set; } = true;

        public string Username { get; set; }

        public string SearchValue { get; set; }

        public string DisplayName { get; set; }

        public string Cookies { get; set; }

        public string Department { get; set; }

        public string Library { get; set; }

        public string LibraryUrl { get; set; }

        public string DomainUrl { get; set; }  

        public string SearchScenarioCode { get; set; }

        public string EventsScenarioCode { get; set; }

        public bool RememberMe { get; set; }

        public bool IsKm { get; set; }

        public bool IsEvent { get; set; }

        public string BuildingInfos { get; set; }

        public bool IsTutorial { get; set; } = true;
    }
}
