using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using JobOfferService.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JobOfferService.IntegrationTests
{
    public class IntegrationWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
        where TProgram : class
    {

        public readonly string DATABASE_NAME = "test_db";
        public readonly string COLLECTION_NAME = "jobOffer";


        public readonly TestcontainerDatabase mongoContainer;
        public readonly TestcontainersContainer natsContainer;

        public IntegrationWebApplicationFactory()
        {
            mongoContainer = new TestcontainersBuilder<MongoDbTestcontainer>()
                .WithDatabase(new MongoDbTestcontainerConfiguration
                {
                    Database = DATABASE_NAME,
                    Username = "mongoadmin", 
                    Password = "secret"
                })
                .WithImage("mongo:5")
                .WithName("mongo")
                .WithCleanUp(true)
                .Build();

            natsContainer = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("nats:2")
                .WithName("nats")
                .WithCleanUp(true)
                .WithPortBinding(4222)
                .WithPortBinding(8222)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(MongoDbSettings));
                if (descriptor != null) services.Remove(descriptor);
                descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMongoDbSettings));
                if (descriptor != null) services.Remove(descriptor);

                MongoDbSettings mongoSettings = new() { ConnectionString = mongoContainer.ConnectionString, DatabaseName = mongoContainer.Database };
                services.AddSingleton<IMongoDbSettings>(mongoSettings);
            });
        }

        public async Task InitializeAsync()
        {
            await mongoContainer.StartAsync();
            await natsContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await mongoContainer.DisposeAsync();
            await natsContainer.DisposeAsync();
        }

    }
}
