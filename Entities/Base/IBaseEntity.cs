using MuizEnums;
using System;

namespace Entities.Base
{
    public interface IBaseEntity
    {
        int ID { get; set; }
        EState State { get; set; }
        int CompanyID { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifyDate { get; set; }
        int CreateByUserID { get; set; }
        int ModifyByUserID { get; set; }
        byte[] TimeStamp { get; set; }
        bool IsModified { get; }
        void FixValues();
        void ResetState();
    }
}
