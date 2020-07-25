using Azure_Todo_Functions.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Todo_Functions.Data
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<TaskItem>> GetItemsAsync(string query);
        Task<IEnumerable<User>> GetUsersAsync(string queryString);
        Task<TaskItem> GetItemAsync(string id);
        Task AddItemAsync(TaskItem item);
        Task UpdateItemAsync(string id, TaskItem item);
        Task DeleteItemAsync(string id);

        Task AddUserAsync(User item);
    }
}
