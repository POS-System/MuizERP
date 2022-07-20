using DataAccessLayer;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Repositories.Interfaces.Base;
using DevExpress.Mvvm.ModuleInjection;
using DXClient.Modules.ViewModels;
using DXClient.Modules.Views;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXClient.Main
{

    // TODO: надо Рефакторить!
    public static class MenuModules
    {
        private static readonly DALContainer _dal = new DALContainer(ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString);
        //private static readonly ParametersContainer _parametersContainer = new ParametersContainer();

        private static Dictionary<string, (Func<MenuItem, object> ViewModelFactory, Type ViewType)> MenuItemsInfo = new Dictionary<string, (Func<MenuItem, object>, Type)>()
        {
            { "Пользователи", ((menuItem) => AutoGridViewModel<User, IUserRepository>.Create(menuItem.Caption, _dal.UserRepository), typeof(AutoGridView)) },
            { "Роли", ((menuItem) => AutoGridViewModel<Role, IRoleRepository>.Create(menuItem.Caption, _dal.RoleRepository), typeof(AutoGridView)) },
            { "Тест1", ((menuItem) => ModuleViewModel.Create(menuItem.Caption), typeof(ModuleView)) }
        };


        public static Module GetModule(MenuItem menuItem)
        {
            Module result = null;
            if (MenuItemsInfo.ContainsKey(menuItem.Caption))
            {
                var moduleInfo = MenuItemsInfo[menuItem.Caption];
                result = new Module(menuItem.Caption, () => moduleInfo.ViewModelFactory(menuItem), moduleInfo.ViewType);
            }

            return result;
        }


        // TODO: Рефакторить!!!
        public static ObservableCollection<string> WatchHistories { get; set; } = new ObservableCollection<string>() { "yooo", "12312" };
    }
}
