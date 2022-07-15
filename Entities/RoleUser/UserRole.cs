using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    [SaveCommand("xp_SaveUserRole")]
    public class RoleUser : BaseEntity
    {
        #region Fields
        private int _roleID;
        private User _user;
        private bool _isChecked;
        #endregion

        #region Properties
        [SaveParameter]
        public int RoleID
        {
            get { return _roleID; }
            set
            {
                _roleID = value;
                OnPropertyChanged("RoleID");
            }
        }

        public User User
        {
            get 
            { 
                return _user; 
            }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        [SaveParameter]
        public int UserID
        {
            get { return _user.ID; }
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

        public RoleUser()
        {
            _user = new User();            
        }
        #endregion
    }
}
