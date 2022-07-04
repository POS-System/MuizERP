using DataAccessLayer;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.User;
using MuizClient.Controls;
using MuizClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using static MuizClient.Controls.GridControl;

namespace MuizClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitStyle();
            InitializeComponent();
            InitApp();
        }

        private void InitStyle()
        {
            Style = (Style)FindResource(typeof(Window));
        }

        public void InitApp()
        {
            ThemeChange(Themes.Light);

            InitData();
        }

        private void InitData()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString;
            var containerDAL = new DALContainer(connectionString);

            // test

            //var properties = containerDAL.GetType().GetProperties()
            //    .Where(x =>
            //    {
            //        var type = x.PropertyType;
            //        if (type.IsGenericType)
            //        {
            //            var genericType = type.GetGenericTypeDefinition();
            //            var result = genericType == typeof(IEntityDAL<>) || genericType.IsSubclassOf(typeof(IEntityDAL<>));
            //            return result;
            //        }

            //        return false;
            //    }
            //    ).ToList();

            //foreach (var property in properties)
            //{
            //    //var item = property.GetValue(containerDAL) is typeof(IEntityDAL<>);
            //    var test = property.GetValue(containerDAL);
            //    //var item2 = (test as IEntityDAL<IBaseEntity>).GetItems(new ParametersContainer());
            //    //grid.InitGridData( as IEntityDAL<BaseEntity>);

            //    //var item3 = item2;
            //}

            //var y = properties;

            // test



            var userDAL = containerDAL.UserDAL;
            grid.InitGridData(userDAL);
        }

        //private IEntityDAL<T> GetDAL<T>(ContainerDAL containerDAL, PropertyInfo property)
        //{
        //    var result = property.GetValue(containerDAL) as IEntityDAL<T>;
        //    return result;
        //}

        private void ThemeChange(Themes theme)
        {
            // определяем путь к файлу ресурсов
            var uri = new Uri($"Config/Styles/Theme{theme}.xaml", UriKind.Relative);
            //var uri = new Uri("ThemeLight.xaml", UriKind.RelativeOrAbsolute);

            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private enum Themes
        {
            Light,
            Dark
        }
    }
}
