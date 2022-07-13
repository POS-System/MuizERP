namespace Entities.Base.Utils.Interface
{
    public interface IConverter<in TFrom, out TTo>
    {
        TTo Convert(TFrom fromValue);
    }
}
