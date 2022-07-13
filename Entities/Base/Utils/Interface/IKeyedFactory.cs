
namespace Entities.Base.Utils.Interface
{
    public interface IKeyedFactory<TResult, TParam>
    {
        TResult Create(TParam param);
    }
}
