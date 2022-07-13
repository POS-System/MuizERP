using Entities.Base.Attributes;

namespace Entities.Base
{
    public class BaseTreeEntity : BaseSortableEntity
    {
        #region Fields

        private int? _parentID;

        #endregion

        #region Properties

        [LoadParameter]
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

        #endregion
    }
}
