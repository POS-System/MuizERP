using MuizEnums;
using System;

namespace Entities.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HierarhyCommnadAttribute : Attribute
    {
        #region Свойства

        public HierarhyDirection Direction { get; private set; }

        #endregion

        #region Конструктор

        public HierarhyCommnadAttribute(HierarhyDirection direction)
        {
            Direction = direction;
        }

        #endregion
    }
}
