using Entities.Base;
using Entities.Base.Attributes;
using Entities.MenuUserHistory;
using System;

namespace Entities
{
    [LoadCommand]
    [SaveCommand]
    public class User : BaseEntity
    {
        #region Fields

        private int _number;
		private string _firstName;
		private string _lastName;
		private string _secondName;
		private string _inn;
		private string _phone;
		private string _email;
		private bool _active;
		private string _login;
		private string _password;
		private DateTime _birthDay;
		private byte _genderID;
		private byte _themeID;
		private string _color;

        private UserSettings _userSettings;
        private EntityCollection<UserRole> _userRoles;
        private EntityCollection<UserMenuItem> _menuHistory;
        private EntityCollection<UserMenuItem> _menuFavorites;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter]
        public int Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged();
                }
            }
        }

        [Title("Тест!!!!")]
        [LoadParameter]
        [SaveParameter]
        public string FirstName
        {
            get { return _firstName; }
            set 
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string SecondName
        {
            get { return _secondName; }
            set 
            {
                if (_secondName != value)
                {
                    _secondName = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string INN
        {
            get { return _inn; }
            set
            {
                if (_inn != value)
                {
                    _inn = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                if (_birthDay != value)
                {
                    _birthDay = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public byte GenderID
        {
            get { return _genderID; }
            set
            {
                if (_genderID != value)
                {
                    _genderID = value;
                    OnPropertyChanged();
                }
            }
        }

        [LoadParameter]
        [SaveParameter]
        public byte ThemeID
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

        public UserSettings UserSettings
        {
            get { return _userSettings; }
            set
            {
                if (_userSettings != value)
                {
                    _userSettings = value;
                    OnPropertyChanged();
                }
            }
        }

        public EntityCollection<UserRole> UserRoles
        {
            get { return _userRoles; }
            set
            {
                if (_userRoles != value)
                {
                    _userRoles = value;
                    OnPropertyChanged();
                }
            }
        }

        public EntityCollection<UserMenuItem> MenuHistory
        {
            get { return _menuHistory; }
            set
            {
                if (_menuHistory != value)
                {
                    _menuHistory = value;
                    OnPropertyChanged();
                }
            }
        }

        public EntityCollection<UserMenuItem> MenuFavorites
        {
            get { return _menuFavorites; }
            set
            {
                if (_menuFavorites != value)
                {
                    _menuFavorites = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public User()
        {
            _userSettings = new UserSettings();
            _userRoles = new EntityCollection<UserRole>();
            _menuFavorites = new EntityCollection<UserMenuItem>();
            _menuHistory = new EntityCollection<UserMenuItem>();
        }

        #endregion
    }
}
