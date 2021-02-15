
using System.Threading.Tasks;

namespace FactoryMediator
{
    public interface IAsyncFactory<TFactoryContext, TModel>
        where TFactoryContext : class, IFactoryContext, new()
        where TModel : class, new()
    {
        Task<TModel> CreateAsync(TFactoryContext context);
    }
}