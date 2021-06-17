using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Database;
using Syracuse.Mobitheque.Core.ViewModels;
using Syracuse.Mobitheque.Core.ViewModels.Sorts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core
{
    public class App : MvxApplication
    {
        static CookiesDatabase database;

        public static CookiesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new CookiesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CookiesDatabase.db3"));
                }
                return database;
            }
        }

        static DocumentsDatabase docDatabase;

        public static DocumentsDatabase DocDatabase
        {
            get
            {
                if (docDatabase == null)
                {
                    docDatabase = new DocumentsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocumentsDatabase.db3"));
                }
                return docDatabase;
            }
        }

        static ApplicationState appState;

        public static ApplicationState AppState
        {
            get
            {
                if (appState == null)
                {
                    appState = new ApplicationState(false);
                }
                return appState;
            }
        }
        public override async void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            SortAlgorithmFactory.RegisterAlgorithms();

             CookiesSave user = await Database.GetActiveUser();
            if (user != null)
            {
                if (user.RememberMe)
                {
                    RegisterAppStart<MasterDetailViewModel>();
                }
                else
                {
                    RegisterAppStart<SelectLibraryViewModel>();
                }
            }
            else { 
                RegisterAppStart<SelectLibraryViewModel>();
            }
        }
    }
}
