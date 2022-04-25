using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class SelectLibraryViewModel : BaseViewModel
    {

        private readonly IMvxNavigationService navigationService;
        private readonly IRequestService requestService;
        private readonly IDepartmentService departmentService;

        private bool isLoading = true;
        public bool IsLoading
        {
            get => this.isLoading;
            set { SetProperty(ref this.isLoading, value); }
        }
        private bool canSubmit = false;
        public bool CanSubmit
        {
            get => this.canSubmit;
            set { SetProperty(ref this.canSubmit, value); }
        }

        private IMvxAsyncCommand<string> validateCommand;
        public IMvxAsyncCommand<string> ValidateCommand
        {
            get
            {
                if (validateCommand != null)
                {
                    return validateCommand;
                }
                validateCommand = new MvxAsyncCommand<string>(async (post) => await ValidateHandler(post));
                return validateCommand;
            }
        }

        public override Task Initialize()
        {
            IsLoading = false;
            return base.Initialize();

        }

        private Library librarie = new Library();
        public Library Librarie
        {
            get => this.librarie;
            set { SetProperty(ref this.librarie, value); }
        }
        public SelectLibraryViewModel(IDepartmentService departmentService,
                              IMvxNavigationService navigationService,
                              IRequestService requestService)
        {
            this.departmentService = departmentService;
            this.navigationService = navigationService;
            this.requestService = requestService;
        }


        public async Task ValidateHandler( string url)
        {
            this.IsLoading = true;

            try
            {
                this.Librarie = await this.departmentService.GetLibraries(url);
                if (this.Librarie is null)
                {
                    throw new NullReferenceException(message: "Librarie object is null");
                }
                try
                {
                    CookiesSave opt = new CookiesSave();
                    opt.Department = this.Librarie.DepartmentCode;
                    opt.Library = this.Librarie.Name;
                    opt.LibraryCode = this.Librarie.Code;
                    opt.LibraryUrl = this.Librarie.Config.BaseUri;
                    opt.DomainUrl = this.Librarie.Config.DomainUri;
                    opt.ForgetMdpUrl = this.Librarie.Config.ForgetMdpUri;
                    opt.EventsScenarioCode = this.Librarie.Config.EventsScenarioCode;
                    opt.SearchScenarioCode = this.Librarie.Config.SearchScenarioCode;
                    opt.IsEvent = this.Librarie.Config.IsEvent;
                    opt.RememberMe = this.Librarie.Config.RememberMe;
                    opt.IsKm = this.Librarie.Config.IsKm;
                    opt.CanDownload = this.Librarie.Config.CanDownload;
                    opt.BuildingInfos = JsonConvert.SerializeObject(this.Librarie.Config.BuildingInformations);
                    opt.LibraryJsonUrl = url;
                    List<StandartViewList> standartViewList = new List<StandartViewList>();
                    foreach (var item in this.Librarie.Config.StandardsViews)
                    {
                        var tempo = new StandartViewList();

                        tempo.ViewName = item.ViewName;
                        tempo.ViewIcone = item.ViewIcone;
                        tempo.ViewQuery = item.ViewQuery;
                        tempo.ViewScenarioCode = item.ViewScenarioCode;
                        tempo.Username = "";
                        tempo.Library = opt.Library;
                        standartViewList.Add(tempo);
                    }
                    this.IsLoading = false;
                    LoginParameters loginParameters = new LoginParameters(this.Librarie.Config.ListSSO, opt, standartViewList);
                    await this.navigationService.Navigate<LoginViewModel, LoginParameters>(loginParameters);
                }
                catch (Exception ex)
                {
                    this.IsLoading = false;
                    this.DisplayAlert("Erreur", "Une erreur est survenue lors de la recupération des données de votre établisment", "OK");
                    this.CanSubmit = false;
                }

            }
            catch (Exception ex)
            {
                this.IsLoading = false;
                this.DisplayAlert("Erreur", "Veuillez selectionner un QRcode ou une url valide", "OK");
                this.CanSubmit = false;
            }


            this.IsLoading = false;
        }
    }
}
