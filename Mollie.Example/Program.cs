using Mollie.Api;
using Mollie.Api.Models;

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

            Console.WriteLine("Loading payment methods ...");
            var methods = await mollieClient.GetPaymentMethods();
            foreach (var method in methods.data)
            {
                Console.WriteLine($"- {method.description}");
            }
            Console.WriteLine();

            Console.WriteLine("Loading iDeal issuers ...");
            var issuers = await mollieClient.ListIssuers();
            foreach (var issuer in issuers.data)
            {
                Console.WriteLine($"- {issuer.name}");
            }
            Console.WriteLine();

            Console.WriteLine("Starting payment ...");

            var payment = new Payment
            {
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/payment/completed/?orderId=1245",
                webhookUrl = "http://www.foxip.net/webhooks/mollie"
            };

            var status = await mollieClient.CreatePayment(payment);

            Console.WriteLine("The status of your payment is: " + status.status);
            Console.WriteLine();

            Console.WriteLine("Please follow this link to start the payment:");
            Console.WriteLine(status.links.paymentUrl);

            Console.WriteLine("Press [enter] to continue ...");
            Console.ReadLine();

            Console.WriteLine("Getting payment status ...");
            status = await mollieClient.GetPayment(status.id);
            Console.WriteLine("The status is now: " + status.status);
            Console.WriteLine();

            Console.WriteLine("Loading payments ...");
            var payments = await mollieClient.GetPayments();
            foreach (var paymentInfo in payments.data)
            {
                Console.WriteLine($"- {paymentInfo.id}");
            }
            Console.WriteLine();

            Console.WriteLine("Press [enter] to quit ...");
            Console.ReadLine();

        }
    }
}