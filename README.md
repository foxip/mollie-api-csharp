mollie-api-csharp
=================

Mollie API client for C#

## How to use the API client ##

Initializing the Mollie API client, and setting your API key.

```c#
var mollieClient = new MollieClient("your_api_key_here");
```

Loading iDeal issuers

```c#
var issuers = await mollieClient.GetIssuers();
foreach (var issuer in issuers.data)
{
   Console.WriteLine(issuer.name);
}
```

Creating a new payment.

```c#
var payment = new Payment 
{ 
   amount = 99.99M, 
   description = "Test payment", 
   redirectUrl = "http://www.myshop.net/payments/completed/?orderId=1245",
   webhookUrl = "http://www.myshop.net/webhooks/mollie/",
};
var paymentStatus = await mollieClient.StartPayment(payment);
var molliePaymentId = paymentStatus.id;
Response.Redirect(paymentStatus.links.paymentUrl);
```

Getting payment status

```c#
var paymentStatus = await mollieClient.GetStatus(molliePaymentId);
if (paymentStatus.status == Status.paid)
{
   Console.WriteLine("Your order is paid");
}
```
