using Entities.Base;
using Entities.Base.Attributes;
using System;
using System.Collections.ObjectModel;

namespace Entities.User
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
		private byte _roleID;
		private string _phone;
		private string _email;
		private bool _active;
		private string _login;
		private string _password;
		private DateTime _birthDay;
		private byte _genderID;
		private byte _themeID;
		private string _color;

        private ObservableCollection<UserRole> _userRoles;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter]
        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string FirstName
        {
            get { return _firstName; }
            set 
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string SecondName
        {
            get { return _secondName; }
            set 
            {
                _secondName = value;
                OnPropertyChanged("SecondName");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string INN
        {
            get { return _inn; }
            set
            {
                _inn = value;
                OnPropertyChanged("INN");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public byte RoleID
        {
            get { return _roleID; }
            set
            {
                _roleID = value;
                OnPropertyChanged("RoleID");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged("Active");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                _birthDay = value;
                OnPropertyChanged("BirthDay");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public byte GenderID
        {
            get { return _genderID; }
            set
            {
                _genderID = value;
                OnPropertyChanged("GenderID");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public byte ThemeID
        {
            get { return _themeID; }
            set
            {
                _themeID = value;
                OnPropertyChanged("ThemeID");
            }
        }

        [LoadParameter]
        [SaveParameter]
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        public ObservableCollection<UserRole> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                OnPropertyChanged("Roles");
            }
        }

        #endregion

        #region Constructor

        public User()
        {
            _userRoles = new ObservableCollection<UserRole>();
        }

        #endregion
    }
}
