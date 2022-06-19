using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Metadata;
using System;
using Grpc.Net.Client;
using gRPCServer;

namespace gRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            /**************Hellow world*********************/
            // var client = new Greeter.GreeterClient(channel);

            // var input = new HelloRequest()
            // {
            //     Name = "From client"
            // };

            // var reply = await client.SayHelloAsync(input);

            // Console.WriteLine(reply.Message);


            /**************Lookup customer*********************/
            // var customerClient = new Customer.CustomerClient(channel);
            
            // var input = new CustomerLookupModel() { CustomerId = 3 };

            // var response = await customerClient.GetCustomerInfoAsync(input);

            // Console.WriteLine($"{response.FirstName} {response.LastName}");


            /**************Get new customer*********************/
            var customerClient = new Customer.CustomerClient(channel);
            
            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                var source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                while (await call.ResponseStream.MoveNext(token))
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }
            }
        }
    }
}
