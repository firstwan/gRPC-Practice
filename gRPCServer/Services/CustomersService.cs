using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using System.Collections.Generic;

namespace gRPCServer.Services
{
    public class CustomersService : Customer.CustomerBase 
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            var customer = new CustomerModel();

            switch (request.CustomerId)
            {
                case 1:
                    customer.FirstName = "Jack";
                    customer.LastName = "Smith";
                    break;
                case 2:
                    customer.FirstName = "James";
                    customer.LastName = "Lee";
                    break;
                default:
                    customer.FirstName = "Greg";
                    customer.LastName = "Thomas";
                    break;

            }


            return Task.FromResult(customer);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> newCustomers = new List<CustomerModel>(){
                new CustomerModel(){
                    FirstName = "First",
                    LastName = "Wan",
                    Age = 28,
                    IsAtive = true,
                    EmailAddress = "firstwan@hotmail.com"
                },
                new CustomerModel(){
                    FirstName = "Tim",
                    LastName = "Corey",
                    Age = 46,
                    IsAtive = true,
                    EmailAddress = "timcorey@hotmail.com"
                },
                new CustomerModel(){
                    FirstName = "James",
                    LastName = "Lee",
                    Age = 30,
                    IsAtive = false,
                    EmailAddress = "jameslee@hotmail.com"
                }
            };

            foreach (var customer in newCustomers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(customer);
            }
        }
    }
}