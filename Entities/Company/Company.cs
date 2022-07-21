using Entities.Base;
using Entities.Base.Attributes;
using MuizEnums;

namespace Entities
{
    [LoadCommand]
    [HierarhyCommnad(EHierarchyDirection.Down)]
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
                if (_companyID != value)
                {
                    _companyID = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter(Nullable = true)]
        [SaveParameter]
        public int? ParentID
        {
            get { return _parentID; }
            set
            {
                if (_parentID != value)
                {
                    _parentID = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string INN
        {
            get { return _inn; }
            set
            {
                if (_inn != value)
                {
                    _inn = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter(Required = false)]
        public override byte[] TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                if (_timeStamp != value)
                {
                    _timeStamp = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
