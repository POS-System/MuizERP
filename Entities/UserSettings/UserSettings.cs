using Entities.Base;
using Entities.Base.Attributes;

namespace Entities.UserSettings
{
    [LoadCommand]
    public class UserSettings : BaseEntity
    {
        #region Fields

        private User _user;
        private int _themeID;
        private string _color;

        #endregion

        #region Properties

        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
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

        #region Constructor

        public UserSettings()
        {
            _user = new User();
        }

        #endregion
    }
}
