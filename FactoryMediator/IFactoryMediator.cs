
using System.Threading.Tasks;

namespace FactoryMediator
{
    public interface IFactoryMediator
    {
        TModel CreateModel<TFactoryContext, TModel>(TFactoryContext context)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new();

        Task<TModel> CreateModelAsync<TFactoryContext, TModel>(TFactoryContext context)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new();

        IAsyncFactory<TFactoryContext, TModel> GetAsyncFactory<TFactoryContext, TModel>()
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new();

        IFactory<TFactoryContext, TModel> GetFactory<TFactoryContext, TModel>()
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new();
    }
}