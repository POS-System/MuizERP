﻿using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoleRepository : ISaveItem<Role>, IGetCollection<Role>
    {        
    }
}
