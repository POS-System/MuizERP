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
        //Type _itemType;
        ObservableCollection<IBaseFilterControl> filterControls;

        public Dictionary<PropertyInfo, IBaseFilterControl> FilterControlsDict { get; set; }
        //public Action AnyFilterChanged;

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

        public void InitFilters(IEnumerable<GridColumnInfo> columnInfos, Action anyFilterChanged = null)
        {
            foreach(var columnInfo in columnInfos)
            {
                IBaseFilterControl control = null;
                var property = columnInfo.PropInfo;

                if (property.PropertyType == typeof(string))
                    control = new StringFilterControl();
                else if (property.PropertyType == typeof(int))
                    control = new IntFilterControl();
                else if (property.PropertyType == typeof(double))
                    control = new DoubleFilterControl();
                else if (property.PropertyType == typeof(bool))
                    control = new BooleanFilterControl();

                if (control != null)
                {
                    control.Init(columnInfo);
                    control.PropertyName = property.Name;
                    control.AnyFilterChanged += anyFilterChanged;

                    filterControls.Add(control);
                    FilterControlsDict[property] = control;
                }

                itemsControl.ItemsSource = filterControls;
            }
        }

        #endregion
    }
}
