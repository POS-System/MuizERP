using Entities.Base;
using Entities.Base.Attributes;

namespace Entities.MenuUserHistory
{
    /// <summary>
    /// Связка пользователя с пунктом меню.
    /// Для истории, избранного и т.д.
    /// </summary>
    [LoadCommand]
    [SaveCommand(IgnoreProperties = new string[] { "CompanyID" })]
    public class UserMenuItem : BaseEntity
    {
        #region Fields

        private int _userID;
        private MenuItem _menuItem;

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

        [SaveParameter]
        public int MenuItemID
        {
            get { return _menuItem.ID; }
        }

        public MenuItem MenuItem
        {
            get { return _menuItem; }
            set
            {
                if (_menuItem != value)
                {
                    _menuItem = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public UserMenuItem()
        {
            _menuItem = new MenuItem();
        }

        #endregion
    }
}
