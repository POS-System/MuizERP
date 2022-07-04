using MuizEnums;
using System;

namespace Entities.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HierarhyCommnadAttribute : Attribute
    {
        #region Свойства

        public EHierarchyDirection Direction { get; private set; }

        #endregion

        #region Конструктор

        public HierarhyCommnadAttribute(EHierarchyDirection direction)
        {
            Direction = direction;
        }

        #endregion
    }
}
