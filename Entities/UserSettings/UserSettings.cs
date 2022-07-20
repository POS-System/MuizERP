using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    [SaveCommand(IgnoreProperties = new[] { "CompanyID" })]
    public class UserSettings : BaseEntity
    {
        #region Fields

        private int _userID;
        private int _themeID;
        private string _color;
        private string _uiStateVersion;
        private string _uiLogicalState;
        private string _uiVisualState;

        #endregion

        #region Properties

        [LoadParameter(Required = false)]
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

        [LoadParameter]
        [SaveParameter]
        public int UserID
        {
            get { return _userID; }
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public int ThemeID
        {
            get { return _themeID; }
            set
            {
                if (_themeID != value)
                {
                    _themeID = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string UiStateVersion
        {
            get { return _uiStateVersion; }
            set
            {
                if (_uiStateVersion != value)
                {
                    _uiStateVersion = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string UiLogicalState
        {
            get { return _uiLogicalState; }
            set
            {
                if (_uiLogicalState != value)
                {
                    _uiLogicalState = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string UiVisualState
        {
            get { return _uiVisualState; }
            set
            {
                if (_uiVisualState != value)
                {
                    _uiVisualState = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
