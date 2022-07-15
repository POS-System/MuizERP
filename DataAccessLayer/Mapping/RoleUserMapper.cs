using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;

namespace DataAccessLayer.Mapping
{
    internal sealed class RoleUserMapper : IMapper<SqlDataReaderWithSchema, RoleUser>
    {
        private readonly IDataMapper _dataMapper;

        public RoleUserMapper(
            IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, RoleUser item)
        {
            var type = item.GetType();

            _dataMapper.Map(drd, item,
                name =>
                {
                    if (name == nameof(item.TimeStamp))
                        name = type.Name + name;
                });

            _dataMapper.Map(drd, item.User);
        }
    }
}
