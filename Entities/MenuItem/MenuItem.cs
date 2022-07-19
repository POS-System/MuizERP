using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    public class MenuItem : BaseTreeEntity<MenuItem>
    {
        #region Fields

        private bool _isFolder;
        private string _caption;
        private string _description;
        private string _alias;
        private bool _isVisible;

        #endregion

        #region Properties

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
        public string Alias
        {
            get { return _alias; }
            set
            {
                if (_alias != value)
                {
                    _alias = value;
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
