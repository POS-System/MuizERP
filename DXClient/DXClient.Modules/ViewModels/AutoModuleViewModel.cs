using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DXClient.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DXClient.Modules.ViewModels
{
    public class AutoModuleViewModel<T> : IDocumentModule, ISupportState<Info>
    {
        public string Caption { get; private set; }
        public virtual bool IsActive { get; set; }
        public ObservableCollection<T> Items { get; private set; }

        public static AutoModuleViewModel<T> Create(string caption)
        {
            return ViewModelSource.Create(() => new AutoModuleViewModel<T>()
            {
                Caption = caption,
            });
        }
        protected AutoModuleViewModel()
        {
            //Items = new ObservableCollection<T>();
            //Enumerable.Range(0, 100)
            //    .Select(x => new DataItem() { Id = x, Value = "Item #" + x.ToString() })
            //    .ToList()
            //    .ForEach(x => Items.Add(x));
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
}

