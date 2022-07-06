using Entities.Base;
using Entities.Base.Attributes;

namespace Entities
{
    [LoadCommand]
    [SaveCommand(IgnoreProperties = new string[] { "CompanyID" })]
    public class UserRole : BaseEntity
    {
        #region Fields

        private int _userID;
        private Role _role;

        #endregion

        #region Properties

        [LoadParameter]
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

        [SaveParameter]
        public int RoleID
        {
            get { return _role.ID; }
        }

        public Role Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged("Role");
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
