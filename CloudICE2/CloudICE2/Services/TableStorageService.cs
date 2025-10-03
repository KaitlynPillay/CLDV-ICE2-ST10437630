using Azure;
using Azure.Data.Tables;
using CloudICE2.Models;
using CloudICE2.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudICE2.Models;

namespace CloudICE2.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly string _tableName = "Customers";

        public TableStorageService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureTableStorage");
            _tableServiceClient = new TableServiceClient(connectionString);
        }

        public async Task<CustomerEntity> GetCustomerAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.CreateIfNotExistsAsync();

            return await tableClient.GetEntityAsync<CustomerEntity>(partitionKey, rowKey);
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.CreateIfNotExistsAsync();

            var customers = new List<CustomerEntity>();
            var query = tableClient.QueryAsync<CustomerEntity>();

            await foreach (var customer in query)
            {
                customers.Add(customer);
            }

            return customers;
        }

        public async Task<CustomerEntity> AddCustomerAsync(CustomerEntity customer)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.CreateIfNotExistsAsync();

            await tableClient.AddEntityAsync(customer);
            return customer;
        }

        public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.UpdateEntityAsync(customer, ETag.All, TableUpdateMode.Replace);
            return customer;
        }

        public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}