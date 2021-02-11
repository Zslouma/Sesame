using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class SelectLibraryViewModel : BaseViewModel
    {
        private readonly IDepartmentService departmentService;
        private readonly IGeolocationService geolocationService;
        private readonly IMvxNavigationService navigationService;
        private Library[] librariesSelected;
        private string[] departmentsPickerSource;
        public string[] DepartmentsPickerSource
        {
            get => this.departmentsPickerSource;
            set => SetProperty(ref this.departmentsPickerSource, value);
        }

        private bool isAnyLibrarySelected;
        public bool IsAnyLibrarySelected
        {
            get => this.isAnyLibrarySelected;
            set => SetProperty(ref this.isAnyLibrarySelected, value);
        }
        //

        private int departmentsPickerIndex = -1;
        public int DepartmentsPickerIndex
        {
            get => this.departmentsPickerIndex;
            set {
                SetProperty(ref this.departmentsPickerIndex, value);
                this.RaisePropertyChanged(nameof(ButtonColor));
                Task.Run(async () => await this.ChangeLibraries());
            }
        }

        private string[] librariesPickerSource;
        public string[] LibrariesPickerSource
        {
            get => this.librariesPickerSource;
            set => SetProperty(ref this.librariesPickerSource, value);
        }

        private int librariesPickerIndex = -1;
        public int LibrariesPickerIndex
        {
            get => this.librariesPickerIndex;
            set
            {
                SetProperty(ref this.librariesPickerIndex, value);
                this.RaisePropertyChanged(nameof(ButtonColor));
                this.RaisePropertyChanged(nameof(TextColor));
                Console.WriteLine("librariesPickerIndex = {0}", value);
            }
        }

        private IMvxAsyncCommand validateCommand;
        public IMvxAsyncCommand ValidateCommand
        {
            get
            {
                this.validateCommand = this.validateCommand ?? new MvxAsyncCommand(ValidateHandler);
                return this.validateCommand;
            }
        }

        private async Task ValidateHandler()
        {
            if (this.LibrariesPickerIndex == -1 || this.DepartmentsPickerIndex == -1)
            {
                this.DisplayAlert("Erreur", "Veuillez selectionner un département et une librarie valide", "OK");
                return;
            }
            CookiesSave opt = new CookiesSave();

            opt.Department = this.DepartmentsPickerSource[this.departmentsPickerIndex];
            opt.Library = this.librariesPickerSource[this.LibrariesPickerIndex];
            opt.LibraryCode = this.librariesSelected[this.librariesPickerIndex].Code;
            opt.LibraryUrl = this.librariesSelected[this.librariesPickerIndex].Config.BaseUri;
            opt.DomainUrl = this.librariesSelected[this.librariesPickerIndex].Config.DomainUri;
            opt.EventsScenarioCode = this.librariesSelected[this.librariesPickerIndex].Config.EventsScenarioCode;
            opt.SearchScenarioCode = this.librariesSelected[this.librariesPickerIndex].Config.SearchScenarioCode;
            opt.IsEvent = this.librariesSelected[this.librariesPickerIndex].Config.IsEvent;
            opt.RememberMe = this.librariesSelected[this.librariesPickerIndex].Config.RememberMe;
            opt.IsKm = this.librariesSelected[this.librariesPickerIndex].Config.IsKm;
            opt.BuildingInfos = JsonConvert.SerializeObject(this.librariesSelected[this.librariesPickerIndex].Config.BuildingInformations);

            await this.navigationService.Navigate<LoginViewModel, CookiesSave>(opt);
        }

        private async Task ChangeLibraries()
        {
            Library[] libraries = await this.departmentService.GetLibraries();
            Department[] departments = await this.departmentService.GetDepartments();

            if (departments == null)
            {
                Exception exception = new Exception("Unable to check departments.");
                throw exception;
            }

            if (libraries == null)
            {
                Exception exception1 = new Exception("Unable to find departments.");
                throw exception1;
            }

            Library[] library = Array.FindAll(libraries,
                element => element.DepartmentCode == departments[this.departmentsPickerIndex].Code);

            if (library == null)
            {
                this.IsAnyLibrarySelected = false;
                this.LibrariesPickerIndex = -1;
                Exception exception = new Exception("Il n'y a pas de librarie dans ce département.");
                throw exception;
            }
            var PickerSource = new string[library.Length];

            for (int i = 0; i < library.Length; i++)
            {
                PickerSource[i] = library[i].Name;
            }
            this.LibrariesPickerSource = PickerSource;
            this.librariesSelected = library;
            this.IsAnyLibrarySelected = true;
            await this.RaiseAllPropertiesChanged();
            if (library.Length == 1)
            {
                this.LibrariesPickerIndex = 0;
            }
            await this.RaiseAllPropertiesChanged();
        }

        public string ButtonColor => this.LibrariesPickerIndex != -1 && this.DepartmentsPickerIndex != -1 ? "#0066ff" : "#EAEAEA";

        public string TextColor => this.LibrariesPickerIndex != -1 && this.DepartmentsPickerIndex != -1 ? "White" : "#0066ff";

        private readonly IRequestService requestService;
            
        public SelectLibraryViewModel(IGeolocationService geolocationService,
                                      IDepartmentService departmentService,
                                      IMvxNavigationService navigationService,
                                      IRequestService requestService)
        {
            this.departmentService = departmentService;
            this.geolocationService = geolocationService;
            this.navigationService = navigationService;
            this.requestService = requestService;
            this.isAnyLibrarySelected = false;

        }

        public override Task Initialize()
        {
            this.ManageDepartments();
            return base.Initialize();
        }
        private async Task ManageDepartments()
        {
            Department[] departments = await this.departmentService.GetDepartments();

            if (departments == null) {
                Exception exception = new Exception("Unable to find departments.");
                throw exception;
            }

            var PickerSource = new string[departments.Count()];

            for (int i = 0; i < departments.Count(); i++)
            {
                PickerSource[i] = $"{departments[i].Code} - {departments[i].Name}";
            }
            this.DepartmentsPickerSource = PickerSource;
        }

    }
}
