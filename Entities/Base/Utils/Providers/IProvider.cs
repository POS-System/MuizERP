
namespace Entities.Base.Utils.Interface
{
    public interface IProvider<out TResult>
    {
        TResult Get();
    }
}
