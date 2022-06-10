using SQLite;
using System;
using System.Collections.Generic;

namespace Syracuse.Mobitheque.Core.Models
{
    public class CookiesSave : ICloneable
    {
        public object Clone()
        {
            return new CookiesSave
            {
                ID = this.ID,
                Active = this.Active,
                Username = this.Username,
                searchValue = this.searchValue,
                DisplayName = this.DisplayName,
                Cookies = this.Cookies,
                Department = this.DisplayName,
                Library = this.Library,
                LibraryCode = this.LibraryCode,
                LibraryUrl = this.LibraryUrl,
                LibraryJsonUrl = this.LibraryJsonUrl,
                DomainUrl = this.DomainUrl,
                ForgetMdpUrl = this.ForgetMdpUrl,
                SearchScenarioCode = this.SearchScenarioCode,
                EventsScenarioCode = this.EventsScenarioCode,
                RememberMe = this.RememberMe,
                IsKm = this.IsKm,
                CanDownload = this.CanDownload,
                IsEvent = this.IsEvent,
                BuildingInfos = this.BuildingInfos,
                IsTutorial = this.IsTutorial,
                IsTutorialAddAcount = this.IsTutorialAddAcount,
            };
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public bool Active { get; set; } = true;

        public string Username { get; set; }

        private string searchValue { get; set; }
        public string SearchValue
        {
            get { return this.searchValue; }
            set
            {
                if (value != null && value != "")
                {
                    string[] val = value.Split(',');
                    if (val.Length > 20)
                    {
                        string[] res = new string[20];
                        for (int i = 0; i < 20; i++)
                        {
                            res[i] = val[i];
                        }
                        this.searchValue = string.Join(",", res);
                    }
                    else
                    {
                        this.searchValue = value;
                    }
                }
                else
                {
                    this.searchValue = value;
                }
            }
        }

        public string DisplayName { get; set; }

        public bool HaveDisplayName
        {
            get { return this.DisplayName != null && this.DisplayName != ""; }
        }

        public string Cookies { get; set; }

        public string Department { get; set; }

        public string Library { get; set; }

        public string LibraryCode { get; set; }

        public string LibraryUrl { get; set; }

        public string LibraryJsonUrl { get; set; }

        public string DomainUrl { get; set; }

        public string ForgetMdpUrl { get; set; }

        public string SearchScenarioCode { get; set; }

        public string EventsScenarioCode { get; set; }

        public bool RememberMe { get; set; }

        public bool IsKm { get; set; }

        public bool CanDownload { get; set; }

        public bool IsEvent { get; set; }

        public string BuildingInfos { get; set; }

        public bool IsTutorial { get; set; } = true;

        public bool IsTutorialAddAcount { get; set; } = true;
    }

    public class LoginParameters
    {
        public LoginParameters(List<SSO> listSSO, CookiesSave cookiesSave, List<StandartViewList> standartViewList)
        {
            ListSSO = listSSO;
            CookiesSave = cookiesSave;
            StandartViewList = standartViewList;
        }

        public List<SSO> ListSSO { get; set; }

        public CookiesSave CookiesSave { get; set; }

        public List<StandartViewList> StandartViewList { get; set; }
    }

    public class StandartViewList
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ViewName { get; set; }
        public string ViewIcone { get; set; }
        public string ViewQuery { get; set; }
        public string ViewScenarioCode { get; set; }
        public string Username { get; set; }
        public string Library { get; set; }
    }

}
