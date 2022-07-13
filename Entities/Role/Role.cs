using Entities.Base;
using Entities.Base.Attributes;
using System.Collections.ObjectModel;

namespace Entities
{
    [SaveCommand]
    [LoadCommand]
    public class Role : BaseEntity
    {
        #region Fields

        private string _name;
        private EntityCollection<RoleUser> _roleUsers;

        #endregion

        #region Properties
        [SaveParameter]
        [LoadParameter]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public EntityCollection<RoleUser> RoleUsers
        {
            get
            {
                return _roleUsers;
            }

            set
            {
                _roleUsers = value;
                OnPropertyChanged("RoleUsers");
            }
        }
        public Role()
        {
            _roleUsers = new EntityCollection<RoleUser>();
        }
        #endregion
    }
}
