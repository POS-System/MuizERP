using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DXClient.Main.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<string> WatchHistories { get => MenuModules.WatchHistories; set => MenuModules.WatchHistories = value; }

        public Menu AppMenu { get; set; }
        public AccMenuItem SelectedItem { get; set; }

        protected IModuleManager Manager { get => ModuleManager.DefaultManager; }

        public MainViewModel()
        { }

        public MainViewModel(ObservableCollection<MenuItem> menuItems) : base()
        {
            InitMenuItems(menuItems);
        }

        private void InitMenuItems(ObservableCollection<MenuItem> menuItems)
        {
            AppMenu = new Menu();
            AppMenu.MenuItems = menuItems.Select(x => ToAccMenuItem(x)).ToList();
            //SelectedItem = AppMenu.MenuItems[0];
        }

        public static MainViewModel Create(ObservableCollection<MenuItem> menuItems)
        {
            return ViewModelSource.Create(() => new MainViewModel(menuItems));
        }

        public AccMenuItem ToAccMenuItem(MenuItem menuItem)
        {
            return new AccMenuItem()
            {
                Caption = menuItem.Caption,
                SubItems = menuItem.Childs.Select(x => ToAccMenuItem(x)).ToList()
            };
        }
    }

    public class Menu
    {
        public List<AccMenuItem> MenuItems { get; set; }
        public Menu()
        { }
    }

    public class AccMenuItem
    {
        string _caption;

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                RegionStr = value;
            }
        }
        public List<AccMenuItem> SubItems { get; set; }

        public Uri ImageUri { get; set; } = DXImageHelper.GetImageUri("SvgImages/Actions/ClearTableStyle.svg");

        public string RegionStr { get; set; }
    }
}
