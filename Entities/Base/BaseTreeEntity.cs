using Entities.Base.Attributes;

namespace Entities.Base
{
    public class BaseTreeEntity<T> : BaseSortableEntity
        where T : BaseTreeEntity<T>
    {
        #region Fields

        private int? _parentID;
        private EntityCollection<T> _childs;

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

        public EntityCollection<T> Childs
        {
            get { return _childs; }
            set
            {
                if (_childs != value)
                {
                    _childs = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public BaseTreeEntity()
        {
            _childs = new EntityCollection<T>();
        }

        #endregion
    }
}
