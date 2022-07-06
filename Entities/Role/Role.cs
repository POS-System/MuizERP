using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    public class Role : BaseEntity
    {
        #region Fields

        private string _name;

        #endregion

        #region Properties

        [LoadParameter]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion
    }
}
