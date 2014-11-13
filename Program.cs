using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Mollie.Api
{
	class Program
	{
		static void Main(string[] args)
		{
			MollieClient mollieClient = new MollieClient();
			mollieClient.setApiKey(ConfigurationManager.AppSettings["mollie_api_key"]);

			Console.WriteLine("Loading iDeal issuers ...");
			Issuers issuers = mollieClient.GetIssuers();
			foreach (Issuer issuer in issuers.data)
			{
				Console.WriteLine(issuer.name);
			}

			Console.WriteLine("Starting payment without method ...");

			Payment payment = new Payment 
			{ 
				amount = 99.99M, 
				description = "Test payment", 
				redirectUrl = "http://www.foxip.net/completed/?orderId=1245",
			};

			PaymentStatus status = mollieClient.StartPayment(payment);

			Console.WriteLine("The status is: " + status.status);
			Console.WriteLine("Please follow this link to start the payment:");
			Console.WriteLine(status.links.paymentUrl);

			Console.WriteLine("Press [enter] to continue ...");
			Console.ReadLine();

			Console.WriteLine("Getting status ...");
			status = mollieClient.GetStatus(status.id);
			Console.WriteLine("The status is now: " + status.status);

			//Refunds only for iDEAL, Bancontact/Mister Cash, SOFORT Banking, creditcard and banktransfer 
			Console.WriteLine("Refunding ...");
			try
			{
				RefundStatus refundStatus = mollieClient.Refund(status.id);
				Console.WriteLine("The status is now: " + refundStatus.payment.status);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
			}

			Console.WriteLine("Starting payment with a specific method ...");
			status = mollieClient.StartPayment(new Payment { 
				amount = 1.99M, 
				method = Method.mistercash,
				description = "Test payment", 
				redirectUrl = "http://www.foxip.net/completed/?orderId=12345" });
			Console.WriteLine("The status is: " + status.status);
			Console.WriteLine("Please follow this link to start the payment:");
			Console.WriteLine(status.links.paymentUrl);
			Console.WriteLine("Press [enter] to continue ...");
			Console.ReadLine();
		}
	}
}
