using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserRoleMapper : IMapper<SqlDataReaderWithSchema, UserRole>
    {
        private readonly IDataMapper _dataMapper;

        public UserRoleMapper(
            IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, UserRole item)
        {
            var type = item.GetType();

            _dataMapper.Map(drd, item,
                name =>
                {
                    if (name == nameof(item.TimeStamp))
                        name = type.Name + name;
                });

            _dataMapper.Map(drd, item.Role);
        }
    }
}
