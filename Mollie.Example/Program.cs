using System.Text.Json;
using Mollie.Api;
using Mollie.Api.Enums;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Customer;

namespace Mollie.Example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Add your api key as argument");
                return;
            }

            MollieClient mollieClient = new MollieClient(args[0]);

            var input = "";

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            while (input?.ToLower() != "q") {

                Console.WriteLine("1. ListPaymentMethods");
                Console.WriteLine("2. GetPaymentMethod");
                Console.WriteLine("3. CreatePayment");
                Console.WriteLine("4. ListPayments");
                Console.WriteLine("5. GetPayment");
                Console.WriteLine("6. CreateRefund");
                Console.WriteLine("7. ListRefunds");
                Console.WriteLine("8. CreateCustomer");
                Console.WriteLine("9. GetCustomer");
                Console.WriteLine("10. ListCustomers");
                Console.WriteLine("11. ListMandates");
                Console.WriteLine();
                Console.WriteLine("Q. Quit");

                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var methods = await mollieClient.ListPaymentMethods();
                        Console.WriteLine(JsonSerializer.Serialize(methods, options));
                        break;
                    case "2":
                        var issuers = await mollieClient.GetPaymentMethod(Method.ideal);
                        Console.WriteLine(JsonSerializer.Serialize(issuers, options));
                        break;
                    case "3":
                        var status = await mollieClient.CreatePayment(new CreatePayment
                        {
                            amount = new Amount { currency = "EUR", value = "99.99" },
                            description = "Test payment",
                            method = Method.ideal,
                            redirectUrl = "http://www.example.com/payment/completed/?orderId=1245",
                            webhookUrl = "http://www.example.com/webhooks/mollie"
                        });
                        Console.WriteLine(JsonSerializer.Serialize(status, options));
                        break;
                    case "4":
                        var payments = await mollieClient.ListPayments();
                        Console.WriteLine(JsonSerializer.Serialize(payments, options));
                        break;
                    case "5":
                        payments = await mollieClient.ListPayments();
                        var paymentId = payments._embedded.payments[0].id;
                        var paymentStatus = await mollieClient.GetPayment(paymentId);
                        Console.WriteLine(JsonSerializer.Serialize(paymentStatus, options));
                        break;
                    case "6":
                        payments = await mollieClient.ListPayments();
                        paymentId = payments._embedded.payments[0].id;
                        var refund = new Refund { amount = new Amount { currency = "EUR", value = "2" } };
                        var refundStatus = await mollieClient.CreateRefund(paymentId, refund);
                        Console.WriteLine(JsonSerializer.Serialize(refundStatus, options));
                        break;
                    case "7":
                        payments = await mollieClient.ListPayments();
                        paymentId = payments._embedded.payments[0].id;
                        var refunds = await mollieClient.ListRefunds(paymentId);
                        Console.WriteLine(JsonSerializer.Serialize(refunds, options));
                        break;
                    case "8":
                        var customer = await mollieClient.CreateCustomer(new CreateCustomer
                        {
                            email = "president@whitehouse.gov",
                            name = "President"
                        });
                        Console.WriteLine(JsonSerializer.Serialize(customer, options));
                        break;
                    case "9":
                        var customers = await mollieClient.ListCustomers();
                        var customerId = customers._embedded.customers[0].id;
                        customer = await mollieClient.GetCustomer(customerId);
                        Console.WriteLine(JsonSerializer.Serialize(customer, options));
                        break;
                    case "10":
                        customers = await mollieClient.ListCustomers();
                        Console.WriteLine(JsonSerializer.Serialize(customers, options));
                        break;
                    case "11":
                        customers = await mollieClient.ListCustomers();
                        customerId = customers._embedded.customers[0].id;
                        var mandates = await mollieClient.ListMandates(customerId);
                        Console.WriteLine(JsonSerializer.Serialize(customers, options));
                        break;
                }
            }
        }
    }
}