
namespace DataAccessLayer.Mapping.Interface
{
    internal interface IMapper<in TFrom, in TTo>
    {
        void Map(TFrom drd, TTo item);
    }
}
