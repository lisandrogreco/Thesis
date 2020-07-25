using Azure_Todo_Functions.Data;
using Azure_Todo_Functions.Services;
using Legacy_Todo.Services;
using Microsoft.AspNetCore.Identity;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Azure_Todo_Functions.Startup))]
namespace Azure_Todo_Functions
{
    public class Startup : FunctionsStartup
    {
        public static string JwtSecret = "REMOVED_JWT_SECRET";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var cosmosDbUrl = Environment.GetEnvironmentVariable("CosmosDbUrl");
            var cosmosDbKey = Environment.GetEnvironmentVariable("CosmosDbKey");

            builder.Services.AddScoped<TaskService>();
            builder.Services.AddSingleton<IAuthService>(new AuthService(JwtSecret, 2592000));
            builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(cosmosDbUrl, cosmosDbKey).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(string url, string key)
        {
            string databaseName = Environment.GetEnvironmentVariable("CosmosDbName");
            string containerName = Environment.GetEnvironmentVariable("CosmosDbContainerName");

            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(url, key);
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            await database.Database.CreateContainerIfNotExistsAsync("Identity", "/id");

            return cosmosDbService;
        }
    }
}
