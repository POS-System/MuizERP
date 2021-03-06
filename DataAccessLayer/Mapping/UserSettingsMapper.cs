using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserSettingsMapper : IMapper<SqlDataReaderWithSchema, UserSettings>
    {
        private readonly IDataMapper _dataMapper;

        public UserSettingsMapper(
            IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, UserSettings item)
        {
            var type = item.GetType();

            _dataMapper.Map(drd, item);
        }
    }
}
