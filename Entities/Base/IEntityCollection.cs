using System.Collections.Generic;

namespace Entities.Base
{
    public interface IEntityCollection
    {
        bool IsModified { get; }
        void FixValues();
        void ResetState();
    }

    public interface IEntityCollection<T> : IEntityCollection, IEnumerable<T>
        where T : BaseEntity
    {

    }
}
