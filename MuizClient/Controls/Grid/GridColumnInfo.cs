using Entities.Base.Parameters;
using System.Reflection;
using System.Windows.Controls;

namespace MuizClient.Controls.Grid
{
    public class GridColumnInfo
    {
        public DataGridColumn GridColumn { get; set; }
        public PropertyInfo PropInfo { get; set; }
        public GridColumnFilter Filter { get; set; }
        //public object FilterValue { get; set; }

        public GridColumnInfo(DataGridColumn gridColumn, PropertyInfo propInfo, object filterValue = null)
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

    public class GridColumnFilter
    {
        public object Value { get; set; }
        public bool IsActive { get; set; }

        public object GetValue() => IsActive ? Value : null;
    }

    public static class ParametersContainerExtension
    {
        public static void SetGridColumn(this ParametersContainer paramContainer, GridColumnInfo gridRow)
        {
            var propName = gridRow.PropInfo.Name;
            var filterValue = gridRow.Filter.GetValue();

            if (filterValue != null)
                paramContainer.Add(gridRow.PropInfo.Name, filterValue);
            else
                paramContainer.Remove(propName);
        }
    }
}
