using Azure;
using CloudICE2.Models;
using CloudICE2.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CloudICE2.Models;
using CloudICE2.Services;

namespace CloudICE2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ITableStorageService _tableStorageService;

        public CustomersController(ITableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableStorageService.GetAllCustomersAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new CustomerEntity
                {
                    PartitionKey = "customer",
                    RowKey = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone
                };

                await _tableStorageService.AddCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var customer = await _tableStorageService.GetCustomerAsync(partitionKey, rowKey);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerViewModel
            {
                PartitionKey = customer.PartitionKey,
                RowKey = customer.RowKey,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new CustomerEntity
                {
                    PartitionKey = model.PartitionKey,
                    RowKey = model.RowKey,
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    ETag = ETag.All
                };

                await _tableStorageService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var customer = await _tableStorageService.GetCustomerAsync(partitionKey, rowKey);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableStorageService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}