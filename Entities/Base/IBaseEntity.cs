using System;

namespace Entities.Base
{
    public interface IBaseEntity
    {
        int ID { get; }
        int CompanyID { get; }
        DateTime CreatedDate { get; }
        int CreatedByUserID { get; }
        DateTime LastModifiedDate { get; }
        int ModifiedByUserID { get; }
    }
}
