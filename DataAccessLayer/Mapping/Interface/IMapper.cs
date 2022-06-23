
namespace DataAccessLayer.Mapping.Interface
{
    public interface IMapper<in TFrom, in TTo>
    {
        void Map(TFrom from, TTo to);
    }
}
