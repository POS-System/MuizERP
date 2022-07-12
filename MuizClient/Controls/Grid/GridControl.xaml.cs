using DataAccessLayer;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;
using Entities.Base;
using Entities.Base.Attributes;
using Entities.Base.Parameters;
using MuizClient.Controls.Grid;
using MuizClient.Controls.Grid.GridFilter;
using MuizClient.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace MuizClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для GridControl.xaml
    /// </summary>
    public partial class GridControl : UserControl, INotifyPropertyChanged
    {
        Type _itemType;
        CollectionViewSource _collectionVS;
        ObservableCollection<IBaseEntity> _collection;

        private ParametersContainer _parametersContainer = new ParametersContainer();
        Action updateGridData;
        
        public delegate void DSaveGridData(BaseEntity baseEntity);
        DSaveGridData saveGridData;

        public GridControl()
        {
            InitializeComponent();
            //phonesList = new ObservableCollection<Phone>();

            //Phone phone = new Phone();
            //phone.Title = "iPhone" + (phonesList.Count + 1).ToString();
            //phone.Company = "Company" + (phonesList.Count + 1).ToString();
            //phone.Price = (phonesList.Count + 1) * 100;
            //phonesList.Add(phone);
        }

        //private void InitData()
        //{
        //    var test = _entities.GetType();
        //    var test1 = _entities.GetType().GetGenericArguments();

        //    ItemType = _entities.GetType().GetGenericArguments().SingleOrDefault();


        //    collectionVS = new CollectionViewSource();
        //    collectionVS.Source = _entities;
        //    collectionVS.Filter += new FilterEventHandler(CollectionViewSource_Filter);
        //    collectionVS.IsLiveFilteringRequested = true;

        //    grid.IsSynchronizedWithCurrentItem = true;
        //    grid.ItemsSource = collectionVS.View;
        //}

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            //Phone item = e.Item as Phone;

            //e.Accepted = !(item?.Price / 10 % 2 == 1);
        }

        private void Add_Button_Click()
        {
            if (_itemType != null)
            {
                var yo = Activator.CreateInstance(_itemType) as User;

                yo.FirstName = "fds";

                _collection.Add(yo);

                saveGridData(yo);

                //grid.ItemsSource.Add(yo);
            }


            //Phone phone = new Phone();
            //phone.Title = "iPhone" + (phonesList.Count + 1).ToString();
            //phone.Company = "Company" + (phonesList.Count + 1).ToString();
            //phone.Price = phonesList.Count * 100;
            //phonesList.Add(phone);
        }

        public void InitGridData<T, V>(T repository) 
            where T : IGetItems<V>, ISave<V> 
            where V : BaseEntity
        {
            _itemType = typeof(V);

            if (grid?.Columns?.Count < 1) GenerateColumns<V>();

            updateGridData = () => SetGridData(repository.GetItems(_parametersContainer));
            saveGridData = (item) => repository.SaveItem(item as V);

            updateGridData();
        }

        //public void InitGridData<T>(IGetItems<T> iGetItems, ISave<T> iSave) where T : BaseEntity
        //{
        //    _itemType = typeof(T);

        //    if (grid?.Columns?.Count < 1) GenerateColumns<T>();

        //    updateGridData = () => SetGridData(iGetItems.GetItems(_parametersContainer));
            
        //    saveGridData = (item) => iSave.SaveItem(item as T); 

        //    updateGridData();
        //}

        private void Edit_Button_Click()
        {
            //Phone phone = phonesGrid.SelectedItem as Phone;

            //if (phone != null)
            //    phone.Price = phone.Price + 10;
        }

        private void Remove_Button_Click()
        {
            var selectedItem = grid.SelectedItem as IBaseEntity;

            if (selectedItem != null)
                _collection.Remove(selectedItem);
        }


        private bool _isHasSelected;
        public bool IsHasSelected 
        {
            get => _isHasSelected;

            set
            {
                _isHasSelected = value;
                OnPropertyChanged(nameof(IsHasSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Filter_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var _gridFilterWindow = new GridFilterWindow();
            _gridFilterWindow.InitFilters(_columns);

            if (_gridFilterWindow.ShowDialog() == true)
            {
                FilterGridData();
            }
            else
            {
                ClearFilterGridData();
            }
        }

        private void Refresh_Button_Click()
        {
            updateGridData();
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((DataGrid)sender).SelectedItem;
            IsHasSelected = selectedItem != null;
        }


        #region Data        
        

        private void FilterGridData()
        {
            foreach (var column in _columns)
            {
                _parametersContainer.SetGridColumn(column);
            }

            updateGridData();
        }

        private void ClearFilterGridData()
        {
            _parametersContainer.Clear();
            updateGridData();
        }


        /// <summary>
        /// Присвоение таблице данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Данные для таблицы</param>
        public void SetGridData<T>(ObservableCollection<T> collection)
        {
            _collection = new ObservableCollection<IBaseEntity>(collection.Cast<IBaseEntity>());
            grid.ItemsSource = _collection;
        }


        /// <summary>
        /// Автоматическое создание колонок таблицы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void GenerateColumns<T>()
        {
            if (grid != null)
            {
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    var lastName = property.GetCustomAttribute<TitleAttribute>();
                        //.GetCustomAttributes(false)
                        //.Select(p => p as TitleAttribute)
                        //.LastOrDefault(x => x != null);

                    var newColumn = new DataGridTextColumn()
                    {
                        Header = lastName != null ? lastName.Title : property.Name,
                        Binding = new Binding(property.Name)
                    };

                    grid.Columns.Add(newColumn);

                    _columns.Add(new GridColumnInfo(newColumn, property));
                }
            }
        }

        public List<GridColumnInfo> _columns = new List<GridColumnInfo>();

        #endregion
    }
}
