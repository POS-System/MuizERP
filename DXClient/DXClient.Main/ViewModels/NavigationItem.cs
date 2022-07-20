using DevExpress.Mvvm;
using DevExpress.Xpf.Accordion;
using DXClient.Common;
using System;

namespace DXClient.Main.ViewModels
{
    [Serializable]
    public class NavigationItem : INavigationItem, ISupportState<NavigationItem>
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
        public string RegionStr { get; set; }
        public NavigationItem() { }
        public NavigationItem(string caption)
        {
            Caption = caption;
        }

        #region Serialization
        NavigationItem ISupportState<NavigationItem>.SaveState()
        {
            return this;
        }
        void ISupportState<NavigationItem>.RestoreState(NavigationItem state)
        {
            Caption = state.Caption;
        }
        #endregion
    }
}
