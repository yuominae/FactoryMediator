namespace FactoryMediator
{
    public interface IFactory<TFactoryContext, TModel>
        where TFactoryContext : class, IFactoryContext, new()
        where TModel : class, new()
    {
        public TModel Create(TFactoryContext context);
    }
}