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

namespace MuizClient.Controls.OtherControls
{
    /// <summary>
    /// Логика взаимодействия для CheckList.xaml
    /// </summary>
    public partial class CheckList : ItemsControl
    {
        public CheckList()
        {
            InitializeComponent();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return false;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ((ContentPresenter)element).ContentTemplate = ItemTemplate;
        }
    }
}
