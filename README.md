mollie-api-csharp
=================

Mollie API client for C#

## How to use the API client ##

Initializing the Mollie API client, and setting your API key.

```c#
MollieClient mollieClient = new MollieClient();
mollieClient.setApiKey("your_api_key_here");
```

Loading iDeal issuers

```c#
Issuers issuers = mollieClient.GetIssuers();
foreach (Issuer issuer in issuers.data)
{
  Console.WriteLine(issuer.name);
}
```

Creating a new payment.

```c#
Payment payment = new Payment 
{ 
	amount = 99.99M, 
	description = "Test payment", 
	redirectUrl = "http://www.myshop.net/completed/?orderId=1245",
};
PaymentStatus paymentStatus = mollieClient.StartPayment(payment);
```

Getting payment status

```c#
PaymentStatus paymentStatus = mollieClient.GetStatus("id12345");
```

Refunds

```c#
RefundStatus refundStatus = mollieClient.Refund("id12345");
```
