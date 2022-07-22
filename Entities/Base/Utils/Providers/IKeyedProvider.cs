
namespace Entities.Base.Utils.Providers
{
    public interface IKeyedProvider<in TParam, out TResult>
    {
        TResult GetByValue(TParam param);
    }
}
