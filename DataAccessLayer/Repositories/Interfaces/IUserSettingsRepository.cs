using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.UserSettings;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserSettingsRepository : IGetItems<UserSettings>
    {
    }
}
