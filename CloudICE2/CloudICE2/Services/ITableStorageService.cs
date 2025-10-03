using CloudICE2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudICE2.Models;

namespace CloudICE2.Services
{
    public interface ITableStorageService
    {
        Task<CustomerEntity> GetCustomerAsync(string partitionKey, string rowKey);
        Task<List<CustomerEntity>> GetAllCustomersAsync();
        Task<CustomerEntity> AddCustomerAsync(CustomerEntity customer);
        Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customer);
        Task DeleteCustomerAsync(string partitionKey, string rowKey);
    }
}