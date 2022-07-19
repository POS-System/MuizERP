using DataAccessLayer;
using DataAccessLayer.Repositories.Interfaces.Base;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Grid;
using DXClient.Common;
using Entities.Base;
using Entities.Base.Attributes;
using Entities.Base.Utils;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace DXClient.Modules.ViewModels
{
    public class AutoGridViewModel<EntityType, RepositoryType> : IDocumentModule, ISupportState<Info>
        where EntityType : BaseEntity
        where RepositoryType : IGetItems<EntityType>, ISave<EntityType>
    {
        public string Caption { get; private set; }
        public virtual bool IsActive { get; set; }
        public ObservableCollection<EntityType> Items { get; private set; }
        public ObservableCollection<GridColumn> Columns { get; private set; }

        public static AutoGridViewModel<EntityType, RepositoryType> Create(string caption, RepositoryType repository)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["ConnectionERP"].ConnectionString;
            //var containerDAL = new DALContainer(connectionString);

            var paramContainer = new ParametersContainer();
            var items = repository.GetItems(paramContainer);
            //containerDAL.

            //updateGridData = () => SetGridData(repository.GetItems(_parametersContainer));
            //saveGridData = (item) => repository.SaveItem(item as V);


            return ViewModelSource.Create(() => new AutoGridViewModel<EntityType, RepositoryType>()
            {
                Caption = caption,
                Items = items
            });;
        }

        protected AutoGridViewModel()
        {
            GenerateColumns<EntityType>();
        }


        /// <summary>
        /// Автоматическое создание колонок таблицы
        /// </summary>
        /// <typeparam name="EntityType"></typeparam>
        public void GenerateColumns<EntityType>() 
            where EntityType : BaseEntity
        {
            var properties = typeof(EntityType).GetProperties();
            Columns = new ObservableCollection<GridColumn>();


            foreach (var property in properties)
            {
                var lastName = property.GetCustomAttribute<TitleAttribute>();
                var newColumn = new GridColumn()
                {
                    Header = lastName != null ? lastName.Title : property.Name,
                    FieldName = property.Name
                    //Binding = new Binding(property.Name)
                };

                Columns.Add(newColumn);
            }
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

