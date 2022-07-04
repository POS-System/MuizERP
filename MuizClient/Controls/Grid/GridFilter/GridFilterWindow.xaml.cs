using MuizClient.Controls.Grid.GridFilter.GridFilterControls;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public GridFilterWindow()
        {
            InitializeComponent();
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
            DialogResult = true;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Other

        public Dictionary<BaseFilterControl<object>, bool> filters;

        public void InitFilter(Type itemType)
        {
            _itemType = itemType;

            var properties = _itemType.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(int))
                {
                    var item = new IntFilterControl();
                    item.Init(property.Name);
                    item.CurrentFilterValue = 50;
                    item.PropertyName = property.Name;

                    itemsControl.Items.Add(item);

                    //filters[item] = екгу
                    ////item.Init("Yo");
                    //filtersPanel.Children.Add(item);
                }
                    

                //var lastName = property
                //    .GetCustomAttributes(false)
                //    .Select(p => p as TitleAttribute)
                //    .LastOrDefault(x => x != null);

                //var newColumn = new DataGridTextColumn()
                //{
                //    Header = lastName != null ? lastName.Title : property.Name,
                //    Binding = new Binding(property.Name)
                //};

                //grid.Columns.Add(newColumn);
            }
        }

        #endregion
    }
}
