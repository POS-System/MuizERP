using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DXClient.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DXClient.Modules.ViewModels
{
    public class ModuleViewModel : IDocumentModule, ISupportState<Info>
    {
        public string Caption { get; private set; }
        public virtual bool IsActive { get; set; }
        public ObservableCollection<DataItem> Items { get; private set; }

        public static ModuleViewModel Create(string caption)
        {
            return ViewModelSource.Create(() => new ModuleViewModel()
            {
                Caption = caption,
            });
        }
        protected ModuleViewModel()
        {
            Items = new ObservableCollection<DataItem>();
            Enumerable.Range(0, 100)
                .Select(x => new DataItem() { Id = x, Value = "Item #" + x.ToString() })
                .ToList()
                .ForEach(x => Items.Add(x));
        }

        #region Serialization
        Info ISupportState<Info>.SaveState()
        {
            return new Info()
            {
                Caption = this.Caption,
            };
        }
        void ISupportState<Info>.RestoreState(Info state)
        {
            this.Caption = state.Caption;
        }
        #endregion
    }
    public class DataItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class Info
    {
        public string Caption { get; set; }
    }
}
