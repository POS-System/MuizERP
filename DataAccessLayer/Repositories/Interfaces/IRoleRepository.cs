﻿using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoleRepository : ISave<Role>, IGetItems<Role>
    {        
    }
}
