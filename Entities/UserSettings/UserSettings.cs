using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    public class UserSettings : BaseEntity
    {
        #region Fields

        private int _userID;
        private int _themeID;
        private string _color;

        #endregion

        #region Properties

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
