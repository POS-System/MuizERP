using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities.MenuUserHistory;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserMenuItemMapper : IMapper<SqlDataReaderWithSchema, UserMenuItem>
    {
        private readonly IDataMapper _dataMapper;

        public UserMenuItemMapper(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, UserMenuItem item)
        {
            _dataMapper.Map(drd, item);
            _dataMapper.Map(drd, item.MenuItem);
        }
    }
}
