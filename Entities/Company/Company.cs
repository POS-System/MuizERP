using Entities.Base;
using Entities.Base.Attributes;
using MuizEnums;

namespace Entities
{
    [LoadCommand]
    [HierarhyCommnad(EHierarchyDirection.Up)]
    [SaveCommand(IgnoreProperties = new[] { "CompanyID" })]
    public class Company : BaseSortableEntity
    {
        #region Fields

        private int? _parentID;
        private string _name;
        private string _inn;

        #endregion

        #region Properties

        [LoadParameter(Required = false)]
        [SaveParameter]
        public override int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                OnPropertyChanged("CompanyID");
            }
        }

        [LoadParameter(Nullable = true)]
        [SaveParameter]
        public int? ParentID
        {
            get { return _parentID; }
            set
            {
                _parentID = value;
                OnPropertyChanged("ParentID");
            }
        }
        
        [LoadParameter]
        [SaveParameter]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("ParentID");
            }
        }
        
        [LoadParameter]
        [SaveParameter]
        public string INN
        {
            get { return _inn; }
            set
            {
                _inn = value;
                OnPropertyChanged("INN");
            }
        }

        #endregion
    }
}
