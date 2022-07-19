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

        #endregion

        #region Properties

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

        #endregion
    }
}
