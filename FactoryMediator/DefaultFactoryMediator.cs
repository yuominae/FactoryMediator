using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FactoryMediator
{
    public class DefaultFactoryMediator
        : IFactoryMediator
    {
        private readonly List<FactoryInfo> factoryInfos = new List<FactoryInfo>();

        public void Init()
        {
            var assembly = Assembly.GetCallingAssembly();

            this.factoryInfos.AddRange(this.GetFactoriesInAssembly(assembly));
        }

        public void Init(Type profileAssemblyMarkerType)
        {
            var assembly = profileAssemblyMarkerType.GetTypeInfo().Assembly;

            this.factoryInfos.AddRange(this.GetFactoriesInAssembly(assembly));
        }

        public void AddFactory<TFactoryContext, TModel>(IFactory<TFactoryContext, TModel> factory)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
            => this.factoryInfos.Add(new FactoryInfo
            {
                FactoryContextType = typeof(TFactoryContext),
                FactoryType = factory.GetType(),
                IsAsync = false
            });

        public void AddAsyncFactory<TFactoryContext, TModel>(IAsyncFactory<TFactoryContext, TModel> factory)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
            => this.factoryInfos.Add(new FactoryInfo
            {
                FactoryContextType = typeof(TFactoryContext),
                FactoryType = factory.GetType(),
                IsAsync = true
            });

        public IFactory<TFactoryContext, TModel> GetFactory<TFactoryContext, TModel>()
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
        {
            var factoryType = this.GetFactoryType<TFactoryContext>();

            var factory = (IFactory<TFactoryContext, TModel>)Activator.CreateInstance(factoryType);

            return factory;
        }

        public IAsyncFactory<TFactoryContext, TModel> GetAsyncFactory<TFactoryContext, TModel>()
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
        {
            var factoryType = this.GetFactoryType<TFactoryContext>();

            var factory = (IAsyncFactory<TFactoryContext, TModel>)Activator.CreateInstance(factoryType);

            return factory;
        }

        public TModel CreateModel<TFactoryContext, TModel>(TFactoryContext context)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
        {
            var factory = this.GetFactory<TFactoryContext, TModel>();

            return factory.Create(context);
        }

        public Task<TModel> CreateModelAsync<TFactoryContext, TModel>(TFactoryContext context)
            where TFactoryContext : class, IFactoryContext, new()
            where TModel : class, new()
        {
            var factory = this.GetAsyncFactory<TFactoryContext, TModel>();

            return factory.CreateAsync(context);
        }

        private IEnumerable<FactoryInfo> GetFactoriesInAssembly(Assembly assembly)
            => assembly.GetTypes()
                .Select(t => new
                {
                    Type = t,
                    GenericInterfaces = t.GetInterfaces().Where(i => i.IsGenericType)
                })
                .Where(o => o.GenericInterfaces.Any())
                .Select(o => new
                {
                    o.Type,
                    FactoryInterface = o.GenericInterfaces.FirstOrDefault(i =>
                    {
                        var genericTypeDefinition = i.GetGenericTypeDefinition();

                        return genericTypeDefinition == typeof(IFactory<,>)
                            || genericTypeDefinition == typeof(IAsyncFactory<,>);
                    })
                })
                .Where(o => o.FactoryInterface != null)
                .Select(o => new FactoryInfo
                {
                    FactoryType = o.Type,
                    FactoryContextType = o.FactoryInterface.GetGenericArguments().First(),
                    IsAsync = o.FactoryInterface.GetGenericTypeDefinition() == typeof(IAsyncFactory<,>)
                });

        private Type GetFactoryType<TFactoryContext>()
        {
            var factoryInfo = this.factoryInfos.FirstOrDefault(f => f.FactoryContextType.Name == typeof(TFactoryContext).Name);

            return factoryInfo.FactoryType;
        }

        private class FactoryInfo
        {
            public Type FactoryType { get; set; }

            public Type FactoryContextType { get; set; }

            public bool IsAsync { get; set; }
        }
    }
}