using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DXClient.Main.ViewModels
{
    public class MainViewModel
    {
        AccMenuItem _selectedItem;
        Func<string> _selectedItemChanged;

        public Menu AppMenu { get; set; }
        public AccMenuItem SelectedItem 
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedItemChanged?.Invoke("");
            }
        }

        public MainViewModel()
        {
        }

        public MainViewModel(ObservableCollection<MenuItem> menuItems, Func<string> selectedItemChanged) : base()
        {
            _selectedItemChanged = selectedItemChanged;

            InitMenuItems(menuItems);
        }

        private void InitMenuItems(ObservableCollection<MenuItem> menuItems)
        {
            AppMenu = new Menu();
            AppMenu.MenuItems = menuItems.Select(x => ToAccMenuItem(x)).ToList();
            //SelectedItem = AppMenu.MenuItems[0];
        }

        //public ObservableCollection<AccMenuItem> MenuItems { get; private set; }

        public static MainViewModel Create(ObservableCollection<MenuItem> menuItems)
        {
            return ViewModelSource.Create(() => new MainViewModel(menuItems));
        }

        public AccMenuItem ToAccMenuItem(MenuItem menuItem)
        {
            return new AccMenuItem()
            {
                Caption = menuItem.Caption,
                SubItems = menuItem.Childs.Select(x => ToAccMenuItem(x as MenuItem)).ToList()
            };
        }
    }


    public class Menu
    {
        public List<AccMenuItem> MenuItems { get; set; }
        public Menu()
        {
            //MenuItems = GetMenuItems();
        }

        //private static List<MenuItem> GetMenuItems()
        //{
        //    List<MenuItem> items = new List<MenuItem>();
        //    List<MenuItem> subitems = new List<MenuItem>();
        //    subitems.Add(new MenuItem() { Caption = "SubItem3" });
        //    items.Add(new MenuItem()
        //    {
        //        Caption = "Item1",
        //        SubItems = new List<MenuItem>() { new MenuItem() { Caption = "SubItem1" },
        //            new MenuItem() { Caption = "SubItem2", SubItems=subitems }
        //        }
        //    });
        //    items.Add(new MenuItem()
        //    {
        //        Caption = "Item2",
        //        SubItems = new List<AccMenuItem>() { new MenuItem() { Caption = "SubItem1" },
        //            new MenuItem() { Caption = "SubItem2" }
        //        }
        //    });
        //    return items;
        //}
    }

    public class AccMenuItem
    {
        string _caption;

        public string Caption { 
            get => _caption; 
            set
            {
                _caption = value;
                RegionStr = value;
            }
        }
        public List<AccMenuItem> SubItems { get; set; }

        public string RegionStr { get; set; }
    }

    
}
