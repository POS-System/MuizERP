
namespace Entities.Base.Utils
{
    public interface IKeyedFactory<TResult, TParam>
    {
        TResult Create(TParam param);
    }
}
