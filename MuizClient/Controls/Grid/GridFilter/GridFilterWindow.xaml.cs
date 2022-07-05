using Entities.Base.Parameters;
using MuizClient.Controls.Grid.GridFilter.GridFilterControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuizClient.Controls.Grid.GridFilter
{
    /// <summary>
    /// Логика взаимодействия для GridFilterWindow.xaml
    /// </summary>
    public partial class GridFilterWindow : Window
    {
        Type _itemType;
        ObservableCollection<IBaseFilterControl> filterControls;

        public Dictionary<PropertyInfo, IBaseFilterControl> FilterControlsDict { get; set; }

        public GridFilterWindow()
        {
            InitializeComponent();

            filterControls = new ObservableCollection<IBaseFilterControl>();
            FilterControlsDict = new Dictionary<PropertyInfo, IBaseFilterControl>();
        }

        #region DP

        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register("ItemType", typeof(Type), typeof(GridFilterWindow));

        public Type ItemType
        {
            get { return (Type)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        #endregion

        #region Actions

        private void Accept_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var filterControl in filterControls)
            {
                filterControl.UpdateFilter();
            }

            DialogResult = true;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Other

        public void InitFilters(IEnumerable<GridColumnInfo> columnInfos)
        {
            foreach(var columnInfo in columnInfos)
            {
                IBaseFilterControl control = null;
                var property = columnInfo.PropInfo;


                if (property.PropertyType == typeof(int))
                    control = new IntFilterControl();
                else if (property.PropertyType == typeof(string))
                    control = new TextFilterControl();


                if (control != null)
                {
                    control.Init(columnInfo);
                    control.PropertyName = property.Name;

                    filterControls.Add(control);
                    FilterControlsDict[property] = control;
                }

                itemsControl.ItemsSource = filterControls;
            }
        }


        public Dictionary<BaseFilterControl<object, IBaseFilterControl>, bool> filters;

        //public void InitFilters(Type itemType)
        //{
        //    _itemType = itemType;

        //    var properties = _itemType.GetType().GetProperties();//.Where(x => x.);
        //    foreach (var property in properties)
        //    {
        //        if (property.PropertyType == typeof(int))
        //        {
        //            var item = new IntFilterControl();
        //            item.Init(property.Name);
        //            //item.CurrentFilterValue = 50;
        //            item.PropertyName = property.Name;

        //            filterControls.Add(item);

        //            FilterControlsDict[property] = item;

        //            //itemsControl.Items.Add(item);
        //        }

        //        itemsControl.ItemsSource = filterControls;

        //        //var lastName = property
        //        //    .GetCustomAttributes(false)
        //        //    .Select(p => p as TitleAttribute)
        //        //    .LastOrDefault(x => x != null);

        //        //var newColumn = new DataGridTextColumn()
        //        //{
        //        //    Header = lastName != null ? lastName.Title : property.Name,
        //        //    Binding = new Binding(property.Name)
        //        //};

        //        //grid.Columns.Add(newColumn);
        //    }
        //}

        //public ParametersContainer GetParametersContainer()
        //{
        //    var values = FilterControlsDict.Values.Select(x => x.GetValue()).ToList();

        //    var parameters = FilterControlsDict
        //        .Where(x => (int)x.Value.GetValue() != 0)
        //        .ToDictionary(x => x.Key.Name, x => x.Value.GetValue());
        //    var result = new ParametersContainer(parameters);

        //    return result;
        //}

        #endregion
    }
}
