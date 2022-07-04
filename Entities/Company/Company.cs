using Entities.Base;
using Entities.Base.Attributes;
using MuizEnums;

namespace Entities.Company
{
    [LoadCommand]
    [HierarhyCommnad(HierarhyDirection.Up)]
    [SaveCommand(IgnoreProperties = new[] { "CompanyID" })]
    public class Company : BaseSortableEntity
    {
        #region Fields

        private int? _parentID;
        private string _name;
        private string _inn;

        #endregion

        #region Properties

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
