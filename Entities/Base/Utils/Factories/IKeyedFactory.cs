
namespace Entities.Base.Utils.Factories
{
    public interface IKeyedFactory<in TParam, out TResult>
    {
        TResult Create(TParam param);
    }

    public interface IKeyedFactory<in TParam1, in TParam2, out TResult>
    {
        TResult Create(TParam1 param1, TParam2 param2);
    }
}
