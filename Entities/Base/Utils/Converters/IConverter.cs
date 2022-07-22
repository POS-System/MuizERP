
namespace Entities.Base.Utils.Converters
{
    public interface IConverter<in TFrom, out TTo>
    {
        TTo Convert(TFrom fromValue);
    }
}
