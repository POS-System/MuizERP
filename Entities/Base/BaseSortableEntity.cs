using Entities.Base.Attributes;

namespace Entities.Base
{
    public class BaseSortableEntity : BaseEntity
    {
        #region Fields

        private int _orderBy;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter]
        public int OrderBy
        {
            get { return _orderBy; }
            set
            {
                if (_orderBy != value)
                {
                    _orderBy = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
