using Azure_Todo_Functions.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Todo_Functions.Data
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        private Container _identityContainer;

        public CosmosDbService(CosmosClient dbClient, string dbName, string containerName)
        {
            this._container = dbClient.GetContainer(dbName, containerName);
            this._identityContainer = dbClient.GetContainer(dbName, "Identity");
        }

        public async Task AddUserAsync(User item)
        {
            await this._identityContainer.CreateItemAsync(item, new PartitionKey(item.id.ToString()));
        }

        public async Task AddItemAsync(TaskItem item)
        {
            await this._container.CreateItemAsync<TaskItem>(item, new PartitionKey(item.id.ToString()));
        }      

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<TaskItem>(id, new PartitionKey(id));
        }

        public async Task<TaskItem> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<TaskItem> response = await this._container.ReadItemAsync<TaskItem>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<TaskItem>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<TaskItem>(new QueryDefinition(queryString));
            List<TaskItem> results = new List<TaskItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string queryString)
        {
            var query = this._identityContainer.GetItemQueryIterator<User>(new QueryDefinition(queryString));
            List<User> results = new List<User>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, TaskItem item)
        {
            await this._container.UpsertItemAsync<TaskItem>(item, new PartitionKey(id));
        }        
    }
}
