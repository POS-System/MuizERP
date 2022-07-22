using Entities.Base.Utils;
using Entities.Base.Utils.ParametersContainers;
using MuizClient.Helpers.FilterValue;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MuizClient.Controls.Grid
{
    public class GridColumnInfo
    {
        public DataGridColumn GridColumn { get; set; }
        public PropertyInfo PropInfo { get; set; }
        public GridColumnFilter Filter { get; set; }
        //public object FilterValue { get; set; }

        public GridColumnInfo(DataGridColumn gridColumn, PropertyInfo propInfo, IFilterValue filterValue = null)
        {
            GridColumn = gridColumn;
            PropInfo = propInfo;
            Filter = new GridColumnFilter()
            {
                IsActive = filterValue != null,
                Value = filterValue
            };
        }
    }

    public class GridColumnFilter : INotifyPropertyChanged
    {
        private bool isActive;

        public IFilterValue Value { get; set; }
        //public object Value { get; set; }
        public bool IsActive 
        {
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, object> GetValue(string propName) => IsActive ? Value.GetPropertyFilterValues(propName) : null;


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public static class ParametersContainerExtension
    {
        public static void AddGridColumnFilter(this ParametersContainer paramContainer, GridColumnInfo gridRow)
        {
            var propName = gridRow.PropInfo.Name;
            var filterValue = gridRow.Filter.GetValue(propName);

            if (filterValue != null)
                paramContainer.AddRange(filterValue);
        }
    }
}
