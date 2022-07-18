using Entities;
using DataAccessLayer;
using DataAccessLayer.Repositories.Interfaces;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DXClient.Common;
using DXClient.Main.Properties;
using DXClient.Main.ViewModels;
using DXClient.Main.Views;
using DXClient.Modules.ViewModels;
using DXClient.Modules.Views;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using AppModules = DXClient.Common.Modules;
using Entities.Base.Utils;
using System.Collections.ObjectModel;
using System.Linq;

namespace DXClient.Main
{
    public partial class App : Application
    {
        public App()
        {
            ApplicationThemeHelper.UpdateApplicationThemeName();
            SplashScreenManager.CreateThemed().ShowOnStartup();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper.Run();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            ApplicationThemeHelper.SaveApplicationThemeName();
            base.OnExit(e);
        }
    }
    public class Bootstrapper
    {
        public static Bootstrapper Default { get; protected set; }
        public static void Run()
        {
            Default = new Bootstrapper();
            Default.RunCore();
        }
        protected Bootstrapper() { }

        const string StateVersion = "1.0";
        public virtual void RunCore()
        {
            ConfigureTypeLocators();
            RegisterModules();
            if (!RestoreState())
                InjectModules();
            ConfigureNavigation();
            ShowMainWindow();
        }

        protected IModuleManager Manager { get { return ModuleManager.DefaultManager; } }
        protected virtual void ConfigureTypeLocators()
        {
            var mainAssembly = typeof(MainViewModel).Assembly;
            var modulesAssembly = typeof(ModuleViewModel).Assembly;
            var assemblies = new[] { mainAssembly, modulesAssembly };
            ViewModelLocator.Default = new ViewModelLocator(assemblies);
            ViewLocator.Default = new ViewLocator(assemblies);
        }

        private void RegisterInitModule()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString;
            var containerDAL = new DALContainer(connectionString);
            var paramContainer = new ParametersContainer();
            var menuItems = containerDAL.MainMenuRepository.GetItems(paramContainer);

            Manager.Register(Regions.MainWindow, new Module(AppModules.Main, () => MainViewModel.Create(menuItems), typeof(MainView)));

            // TODO: change logic init Modules
            InitMyModules(menuItems);
            //foreach (var menuItem in menuItems)
            //{
            //    Manager.Register(Regions.Documents, new Module(menuItem.Caption, () => ModuleViewModel.Create(menuItem.Caption), typeof(ModuleView)));
            //}
        }

        private void InitMyModules(ObservableCollection<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                Manager.Register(Regions.Documents, new Module(menuItem.Caption, () => ModuleViewModel.Create(menuItem.Caption), typeof(ModuleView)));

                InitMyModules(new ObservableCollection<MenuItem>(menuItem.Childs.Cast<MenuItem>()));
            }
        }


        protected virtual void RegisterModules()
        {
            Manager.GetRegion(Regions.Documents).VisualSerializationMode = VisualSerializationMode.PerKey;

            RegisterInitModule();

            Manager.Register(Regions.Navigation, new Module(AppModules.Module1, () => new NavigationItem("Module1")));
            Manager.Register(Regions.Navigation, new Module(AppModules.Module2, () => new NavigationItem("Module2")));
            Manager.Register("Region1", new Module("Module3", () => new NavigationItem("Module3")));
            //Manager.Register(Regions.Documents, new Module(AppModules.Module1, () => ModuleViewModel.Create("Module1"), typeof(ModuleView)));
            //Manager.Register(Regions.Documents, new Module(AppModules.Module2, () => ModuleViewModel.Create("Module2"), typeof(ModuleView)));
            //Manager.Register(Regions.Documents, new Module("Module3", () => ModuleViewModel.Create("Module3"), typeof(ModuleView)));
        }

        protected virtual bool RestoreState()
        {
#if !DEBUG
            if (Settings.Default.StateVersion != StateVersion) return false;
            return Manager.Restore(Settings.Default.LogicalState, Settings.Default.VisualState);
#else
            return false;
#endif
        }
        protected virtual void InjectModules()
        {
            Manager.Inject(Regions.MainWindow, AppModules.Main);
            //Manager.Inject(Regions.Navigation, AppModules.Module1);
            //Manager.Inject(Regions.Navigation, AppModules.Module2);
            Manager.Inject("Region1", "Module3");
        }
        protected virtual void ConfigureNavigation()
        {
            //Manager.GetEvents(Regions.Navigation).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Documents).Navigation += OnDocumentsNavigation;
            //Manager.GetEvents("Region1").Navigation += OnNavigation;
        }
        protected virtual void ShowMainWindow()
        {
            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.Show();
            App.Current.MainWindow.Closing += OnClosing;
        }
        void OnNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NewViewModelKey == null) return;
            Manager.InjectOrNavigate(Regions.Documents, e.NewViewModelKey);
        }
        void OnDocumentsNavigation(object sender, NavigationEventArgs e)
        {
            //Manager.Navigate(Regions.Navigation, e.NewViewModelKey);
        }
        void OnClosing(object sender, CancelEventArgs e)
        {
            string logicalState;
            string visualState;
            Manager.Save(out logicalState, out visualState);
            Settings.Default.StateVersion = StateVersion;
            Settings.Default.LogicalState = logicalState;
            Settings.Default.VisualState = visualState;
            Settings.Default.Save();
        }
    }
}