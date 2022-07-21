
namespace Entities.Base.Utils.Interface
{
    public interface IKeyedProvider<in TParam, out TResult>
    {
        TResult GetByValue(TParam param);
    }
}
