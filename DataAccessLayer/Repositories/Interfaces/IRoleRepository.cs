using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;
using Entities.Base.Parameters;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoleRepository : ISave<Role>, IGetItems<Role>
    {
        ObservableCollection<UserRole> GetUserRoles(IParametersContainer parametersContainer);

        void SaveUserRole(UserRole userRole, SqlConnection conn);
    }
}
