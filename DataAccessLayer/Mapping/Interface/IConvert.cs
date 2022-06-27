
namespace DataAccessLayer.Mapping.Interface
{
    public interface IConvert<in TFrom, in TTo>
    {
        void Convert(TFrom from, TTo to);
    }
}
