using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    [SaveCommand]
    public class UserRole : BaseEntity
    {
        #region Fields
        private int _userID;
        private Role _role;
        private bool _isChecked;
        #endregion

        #region Properties
        [SaveParameter]
        public int UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                OnPropertyChanged("UserID");
            }
        }

        public Role Role
        {
            get 
            { 
                return _role; 
            }
            set
            {
                _role = value;
                OnPropertyChanged("Role");
            }
        }

        [SaveParameter]
        public int RoleID
        {
            get { return _role.ID; }
        }

        [SaveParameter]
        [LoadParameter]
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public override int CompanyID
        {
            get { return _companyID; }
            set
            {
                _companyID = value;
                OnPropertyChanged("CompanyID");
            }
        }
        #endregion

        #region Constructor

        public UserRole()
        {
            _role = new Role();          
        }
        #endregion
    }
}
