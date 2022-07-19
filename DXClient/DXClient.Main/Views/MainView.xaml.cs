using DevExpress.Mvvm.ModuleInjection;
using DXClient.Common;
using DXClient.Main.ViewModels;
using System.Windows.Controls;

namespace DXClient.Main.Views
{
    public partial class MainView : UserControl
    {
        protected IModuleManager Manager { get => ModuleManager.DefaultManager; }

        public MainView()
        {
            InitializeComponent();
        }

        private void Accordion_SelectedItemChanged(object sender, DevExpress.Xpf.Accordion.AccordionSelectedItemChangedEventArgs e)
        {
            if (e.OldItem != e.NewItem)
            {
                var moduleName = (e.NewItem as AccMenuItem)?.RegionStr;
                var module = Manager.GetModule(Regions.Documents, moduleName);

                if (module != null)
                    Manager.InjectOrNavigate(Regions.Documents, moduleName);
            }
        }
    }
}
