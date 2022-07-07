﻿using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.User;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository : IGetItems<User>, ISave<User>
    {
    }
}