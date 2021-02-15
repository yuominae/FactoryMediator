using System.Threading.Tasks;

using FactoryMediator;

using Xunit;

namespace FactoryMediatorTests
{
    public class FactoriesTest
    {
        [Fact]
        public async Task CreateBasicModelViaAsyncFactory()
        {
            var basicModelFactoryContext = new BasicModelAsyncFactoryContext();

            var expected = new BasicModel
            {
                BasicProperty = basicModelFactoryContext.BasicPropertyValue
            };

            var factoryProvider = new DefaultFactoryMediator();

            factoryProvider.Init();

            var basicModel = await factoryProvider.CreateModelAsync<BasicModelAsyncFactoryContext, BasicModel>(basicModelFactoryContext);

            Assert.NotNull(basicModel);
        }

        [Fact]
        public void CreateBasicModelViaFactory()
        {
            var basicModelFactoryContext = new BasicModelFactoryContext();

            var expected = new BasicModel
            {
                BasicProperty = basicModelFactoryContext.BasicPropertyValue
            };

            var factoryProvider = new DefaultFactoryMediator();

            factoryProvider.Init();

            var basicModel = factoryProvider.CreateModel<BasicModelFactoryContext, BasicModel>(basicModelFactoryContext);

            Assert.NotNull(basicModel);
        }

        private class BasicModelAsyncFactory
            : IAsyncFactory<BasicModelAsyncFactoryContext, BasicModel>
        {
            public Task<BasicModel> CreateAsync(BasicModelAsyncFactoryContext context)
                => Task.FromResult(new BasicModel
                {
                    BasicProperty = context.BasicPropertyValue
                });
        }

        private class BasicModelFactory
            : IFactory<BasicModelFactoryContext, BasicModel>
        {
            public BasicModel Create(BasicModelFactoryContext context)
                => new BasicModel
                {
                    BasicProperty = context.BasicPropertyValue
                };
        }

        private class BasicModelAsyncFactoryContext
            : IFactoryContext
        {
            public string BasicPropertyValue { get; set; } = "This is a test";
        }

        private class BasicModelFactoryContext
            : IFactoryContext
        {
            public string BasicPropertyValue { get; set; } = "This is a test";
        }

        private class BasicModel
        {
            public string BasicProperty { get; set; }
        }
    }
}