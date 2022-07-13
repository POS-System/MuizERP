using Entities.Base;
using Entities.Base.Attributes;
using System;

namespace Entities
{
    [LoadCommand]
    public class MenuItem : BaseTreeEntity
    {
        #region Fields

        private Type _entityType;
        private bool _isFolder;
        private string _name;
        private string _description;

        private EntityCollection<MenuItem> _childs;

        #endregion

        #region Properties

        public Type EntityType
        {
            get { return _entityType; }
            set
            {
                if (_entityType != value)
                {
                    _entityType = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        public bool IsFolder
        {
            get { return _isFolder; }
            set
            {
                if (_isFolder != value)
                {
                    _isFolder = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
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
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public EntityCollection<MenuItem> Childs
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

        public MenuItem()
        {
            _childs = new EntityCollection<MenuItem>();
        }

        #endregion
    }
}
