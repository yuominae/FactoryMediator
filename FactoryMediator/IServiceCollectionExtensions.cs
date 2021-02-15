using System;

using Microsoft.Extensions.DependencyInjection;

namespace FactoryMediator
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultFactoryProvider(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton<IFactoryMediator, DefaultFactoryMediator>(p =>
            {
                var factoryProvider = new DefaultFactoryMediator();

                factoryProvider.Init();

                return factoryProvider;
            });

        public static IServiceCollection AddDefaultFactoryProvider(this IServiceCollection serviceCollection, Type profileAssemblyMarkerType)
            => serviceCollection.AddSingleton<IFactoryMediator, DefaultFactoryMediator>(p =>
            {
                var factoryProvider = new DefaultFactoryMediator();

                factoryProvider.Init(profileAssemblyMarkerType);

                return factoryProvider;
            });
    }
}