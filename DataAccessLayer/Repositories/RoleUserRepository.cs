using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.ParametersContainers;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal class RoleUserRepository : IRoleUserRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IMapper<SqlDataReaderWithSchema, RoleUser> _roleUserMapper;

        public RoleUserRepository(
            DataRepository dataRepository,
            IMapper<SqlDataReaderWithSchema, RoleUser> roleUserMapper)
        {
            _dataRepository = dataRepository;

            _roleUserMapper = roleUserMapper;            
        }

        public EntityCollection<RoleUser> GetCollection(IParametersContainer parameters)
        {
            var result = new EntityCollection<RoleUser>();

            _dataRepository.ReadCollectionWithSchema<RoleUser>(
                cmd => cmd.AddParameters(parameters),
                drd =>
                {
                    var item = new RoleUser();
                    _roleUserMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(RoleUser item, SqlConnection conn)
        {
            _dataRepository.SaveBaseItem(item, conn);            
        }
    }
}

