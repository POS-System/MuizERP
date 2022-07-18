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
        private string _caption;
        private string _description;
        private bool _isVisible;

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
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;
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

        [LoadParameter]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
