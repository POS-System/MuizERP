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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    /// <summary>
    /// Логика взаимодействия для IntFilterControl.xaml
    /// </summary>
    public partial class IntFilterControl : BaseFilterControl<int, IntFilterControl>
    {
        public IntFilterControl()
        {
            InitializeComponent();

            slider.Minimum = 0; // int.MinValue;
            slider.Maximum = 10000; // int.MaxValue;
        }
    }
}
