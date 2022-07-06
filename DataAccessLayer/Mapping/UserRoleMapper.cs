using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using Entities;
using System;

namespace DataAccessLayer.Mapping
{
    internal sealed class UserRoleMapper : IMapper<SqlDataReaderWithSchema, UserRole>
    {
        private readonly IMapper<SqlDataReaderWithSchema, Role> _roleMapper;

        public UserRoleMapper(IMapper<SqlDataReaderWithSchema, Role> roleMapper)
        {
            _roleMapper = roleMapper;
        }

        public void Map(SqlDataReaderWithSchema drd, UserRole item)
        {
            item.ID = Convert.ToInt32(drd["UserRoleID"]);
            item.UserID = Convert.ToInt32(drd["UserID"]);

            _roleMapper.Map(drd, item.Role);
        }
    }
}
