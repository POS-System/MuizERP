namespace Entities.Base.Utils
{
    public interface IConverter<in TFrom, out TTo>
    {
        TTo Convert(TFrom fromValue);
    }
}
